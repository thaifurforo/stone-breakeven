using MediatR;

namespace AccountManagementService.Domain.Notifications;

public class ErrorEvent : INotification
{
    public string Exception { get; set; }
    public string ErrorPile { get; set; }
}