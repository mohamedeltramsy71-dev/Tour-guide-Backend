using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using TourGuide.Application.Interfaces;

namespace TourGuide.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    private async Task SendAsync(string toEmail, string toName, string subject, string htmlBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Tour Guide", _settings.From));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }

    public Task SendConfirmationEmailAsync(string toEmail, string toName, string confirmationLink)
    {
        var html = $"""
            <h2>Welcome to Tour Guide, {toName}!</h2>
            <p>Please confirm your email address by clicking the button below:</p>
            <a href="{confirmationLink}" 
               style="background:#2563eb;color:white;padding:12px 24px;
                      text-decoration:none;border-radius:6px;display:inline-block">
                Confirm Email
            </a>
            <p>This link expires in 24 hours.</p>
            """;

        return SendAsync(toEmail, toName, "Confirm Your Email — Tour Guide", html);
    }

    public Task SendPasswordResetEmailAsync(string toEmail, string toName, string resetLink)
    {
        var html = $"""
            <h2>Reset Your Password</h2>
            <p>Hi {toName}, click the button below to reset your password:</p>
            <a href="{resetLink}"
               style="background:#dc2626;color:white;padding:12px 24px;
                      text-decoration:none;border-radius:6px;display:inline-block">
                Reset Password
            </a>
            <p>This link expires in 1 hour. If you didn't request this, ignore this email.</p>
            """;

        return SendAsync(toEmail, toName, "Reset Your Password — Tour Guide", html);
    }

    public Task SendGuideRejectionEmailAsync(string toEmail, string toName, string reason)
    {
        var html = $"""
            <h2>Guide Application Update</h2>
            <p>Hi {toName}, unfortunately your guide application was not approved.</p>
            <p><strong>Reason:</strong> {reason}</p>
            <p>You can contact us for more information.</p>
            """;

        return SendAsync(toEmail, toName, "Guide Application Status — Tour Guide", html);
    }

    public Task SendGuideApprovalEmailAsync(string toEmail, string toName)
    {
        var html = $"""
            <h2>Congratulations, {toName}!</h2>
            <p>Your guide account has been approved. You can now login and start creating packages.</p>
            <p>Welcome to the Tour Guide family!</p>
            """;

        return SendAsync(toEmail, toName, "Guide Account Approved — Tour Guide", html);
    }
}