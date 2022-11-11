using AccountingService.Domain.CommandHandlers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Notifications;
using FluentValidation;
using MediatR;
using Moq;

namespace AccountingService.UnitTests.Domain.CommandHandlers;

public class DeactivateAccountCommandHandlerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<IAccountRepository> _accountRepository = new();

    public static readonly object[][] TheoryData =
    {
        new object[] { DateTime.Now, 0, 1},
        new object[] { DateTime.Now.AddDays(1), 0, 0},
        new object[] { DateTime.Now, 1, 0},
        new object[] { DateTime.Now, -1, 0},
    };

    [Theory, MemberData(nameof(TheoryData))]
    public async void DeactivateAccountCommandHandler_GivenRequest_ShouldReturnExpected(DateTime closingDate, decimal balance, int expected)
    {
        var handler = new DeactivateAccountCommandHandler(_mediator.Object, _accountRepository.Object);

        await handler.Handle(new DeactivateAccountCommand(), CancellationToken.None);
        
        _mediator.Verify(x => x.Publish(It.IsAny<DeactivatedAccountEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(expected));
    }
    
    [Fact]
    public async void DeactivateAccountCommandHandlerTest()
    {
        var handler = new DeactivateAccountCommandHandler(_mediator.Object, _accountRepository.Object);

        await handler.Handle(new DeactivateAccountCommand(), CancellationToken.None);
        
        _mediator.Verify(x => x.Publish(It.IsAny<DeactivatedAccountEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}