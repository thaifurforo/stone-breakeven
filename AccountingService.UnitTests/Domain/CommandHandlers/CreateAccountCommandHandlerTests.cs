using AccountingService.Domain.CommandHandlers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Notifications;
using FluentValidation;
using MediatR;
using Moq;

namespace AccountingService.UnitTests.Domain.CommandHandlers;

public class CreateAccountCommandHandlerTests
{
    
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<IAccountRepository> _accountRepository = new();
    private readonly Mock<IValidator<CreateAccountCommand>> _validator = new();


    [Theory]
    [InlineData("001", "12345678909", 1)]
    [InlineData("001", "12345678000942", 1)]
    [InlineData("001", "12345678900", 0)]
    [InlineData("001", "12345678000900", 0)]
    [InlineData("001", "1234567890123456", 0)]
    [InlineData("abc", "12345678909", 0)]
    public async void CreateAccountCommandHandler_GivenRequest_ShouldReturnExpected(string agency, string document, int expected)
    {
        var handler = new CreateAccountCommandHandler(_mediator.Object, _accountRepository.Object, _validator.Object);

        await handler.Handle(new CreateAccountCommand(){Agency = agency, Document = document}, CancellationToken.None);
        
        _mediator.Verify(x => x.Publish(It.IsAny<CreatedAccountEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(expected));
    }

    [Fact]
    public async void CreateAccountCommandHandlerTest()
    {
        var handler = new CreateAccountCommandHandler(_mediator.Object, _accountRepository.Object, _validator.Object);

        await handler.Handle(new CreateAccountCommand(), CancellationToken.None);
        
        _mediator.Verify(x => x.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    
     
    
}