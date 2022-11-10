using MediatR;

namespace AccountingService.Domain.Notifications;

public class DeactivatedAccountEvent : INotification
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Agency { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public string Document { get; set; }
    public bool IsDeactivated { get; set; }
}