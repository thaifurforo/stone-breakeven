using AccountingService.Domain.Notifications;
using MediatR;

namespace AccountingService.Domain.NotificationHandlers;

public class LogEventHandler :
    INotificationHandler<CreatedAccountEvent>,
    INotificationHandler<DeactivatedAccountEvent>,
    INotificationHandler<ErrorEvent>
{
    public Task Handle(CreatedAccountEvent @event, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            Console.WriteLine($"CREATED: '{@event.Id} - " +
                              $"{@event.Number} - " +
                              $"{@event.Agency} - " +
                              $"{@event.Balance} - " +
                              $"{@event.IsActive} - " +
                              $"{@event.OpeningDate} - " +
                              $"{@event.ClosingDate} - " +
                              $"{@event.IsActive} - " +
                              $"{@event.Document}'");
        });
    }

    public Task Handle(DeactivatedAccountEvent @event, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            Console.WriteLine($"DEACTIVATED: '{@event.IsDeactivated} - " +
                              $"{@event.Id} - " +
                              $"{@event.Number} - " +
                              $"{@event.Agency} - " +
                              $"{@event.Balance} - " +
                              $"{@event.IsActive} - " +
                              $"{@event.OpeningDate} - " +
                              $"{@event.ClosingDate} - " +
                              $"{@event.IsActive} - " +
                              $"{@event.Document}'");
        });
    }
    
    public Task Handle(ErrorEvent @event, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
                {
                    Console.WriteLine($"ERRO: '{@event.Exception} \n {@event.ErrorPile}'");        
                } 
            );
    }
    
}