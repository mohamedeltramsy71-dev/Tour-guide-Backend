using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TourGuide.Application.DTOs.Auth;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IEmailService emailService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request, string confirmationBaseUrl)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            throw new ConflictException("Email already registered.");

        var role = request.Role is "Guide" or "Tourist" ? request.Role : "Tourist";

        var user = new ApplicationUser
        {
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new BusinessRuleException(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, role);

        if (role == "Guide")
        {
            await _unitOfWork.Repository<GuideProfile>().AddAsync(new GuideProfile { UserId = user.Id });
            await _unitOfWork.SaveChangesAsync();
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = Uri.EscapeDataString(token);
        var confirmationLink = $"{confirmationBaseUrl}?userId={user.Id}&token={encodedToken}";
        await _emailService.SendConfirmationEmailAsync(user.Email!, user.FullName, confirmationLink);

        return new LoginResponse
        {
            UserId = user.Id,
            AccessToken = string.Empty,
            RefreshToken = string.Empty,
            Role = role,
            FullName = user.FullName,
            Email = user.Email!,
            AvatarUrl = user.AvatarUrl
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.Users
            .Include(u => u.GuideProfile)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            throw new UnauthorizedException("Invalid email or password.");

        if (!user.EmailConfirmed)
            throw new UnauthorizedException("Please confirm your email before logging in.");

        if (user.IsBanned)
            throw new UnauthorizedException("Your account has been banned.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Tourist";

        if (role == "Guide" && user.GuideProfile is not null && !user.GuideProfile.IsApproved)
            throw new UnauthorizedException("Your guide account is pending admin approval.");

        var accessToken = await _jwtService.GenerateAccessTokenAsync(user);
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);

        await _unitOfWork.Repository<RefreshToken>().AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResponse
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            Role = role,
            FullName = user.FullName,
            Email = user.Email!,
            AvatarUrl = user.AvatarUrl
        };
    }

    public async Task<LoginResponse> GoogleLoginAsync(GoogleAuthRequest request)
    {
        var clientId = _configuration["Google:ClientId"]!;
        GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken,
                new GoogleJsonWebSignature.ValidationSettings { Audience = [clientId] });
        }
        catch
        {
            throw new UnauthorizedException("Invalid Google token.");
        }

        var user = await _userManager.Users
            .Include(u => u.GuideProfile)
            .FirstOrDefaultAsync(u => u.Email == payload.Email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                FullName = payload.Name,
                Email = payload.Email,
                UserName = payload.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                throw new BusinessRuleException(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Tourist");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Tourist";

        var accessToken = await _jwtService.GenerateAccessTokenAsync(user);
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);

        await _unitOfWork.Repository<RefreshToken>().AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResponse
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            Role = role,
            FullName = user.FullName,
            Email = user.Email!,
            AvatarUrl = user.AvatarUrl
        };
    }

    public async Task ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");

        var result = await _userManager.ConfirmEmailAsync(user, Uri.UnescapeDataString(token));
        if (!result.Succeeded)
            throw new BusinessRuleException("Invalid or expired confirmation token.");
    }

    public async Task ForgetPasswordAsync(ForgetPasswordRequest request, string resetBaseUrl)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null) return;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Uri.EscapeDataString(token);
        var resetLink = $"{resetBaseUrl}?email={user.Email}&token={encodedToken}";

        await _emailService.SendPasswordResetEmailAsync(user.Email!, user.FullName, resetLink);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException("User not found.");

        var result = await _userManager.ResetPasswordAsync(
            user, Uri.UnescapeDataString(request.Token), request.NewPassword);

        if (!result.Succeeded)
            throw new BusinessRuleException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var tokens = await _unitOfWork.Repository<RefreshToken>()
            .FindAsync(r => r.Token == request.RefreshToken);

        var storedToken = tokens.FirstOrDefault()
            ?? throw new UnauthorizedException("Invalid refresh token.");

        if (storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedException("Refresh token expired or revoked.");

        storedToken.IsRevoked = true;
        _unitOfWork.Repository<RefreshToken>().Update(storedToken);

        var newRefreshToken = _jwtService.GenerateRefreshToken(storedToken.UserId);
        await _unitOfWork.Repository<RefreshToken>().AddAsync(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

        var user = await _userManager.Users
            .Include(u => u.GuideProfile)
            .FirstOrDefaultAsync(u => u.Id == storedToken.UserId)
            ?? throw new NotFoundException("User not found.");

        var accessToken = await _jwtService.GenerateAccessTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        return new LoginResponse
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
            Role = roles.FirstOrDefault() ?? "Tourist",
            FullName = user.FullName,
            Email = user.Email!,
            AvatarUrl = user.AvatarUrl
        };
    }

    public async Task LogoutAsync(string userId, string refreshToken)
    {
        var tokens = await _unitOfWork.Repository<RefreshToken>()
            .FindAsync(r => r.UserId == userId && r.Token == refreshToken);

        var token = tokens.FirstOrDefault();
        if (token is not null)
        {
            token.IsRevoked = true;
            _unitOfWork.Repository<RefreshToken>().Update(token);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");

        var result = await _userManager.ChangePasswordAsync(
            user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
            throw new BusinessRuleException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}