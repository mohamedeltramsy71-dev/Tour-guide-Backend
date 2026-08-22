using TourGuide.Domain.Enums;

namespace TourGuide.Application.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string email, string fullName, string confirmationLink);
    Task SendPasswordResetEmailAsync(string email, string fullName, string resetLink);
    Task SendGuideRejectionEmailAsync(string email, string fullName, string reason);
    Task SendGuideApprovalEmailAsync(string email, string fullName);
    Task SendNotificationEmailAsync(string email, string fullName, string message, NotificationType type);
}