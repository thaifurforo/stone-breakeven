using AccountingService.Domain.CommandHandlers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Notifications;
using AccountingService.Domain.Validators.Commands;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AccountingService.UnitTests.Domain.CommandHandlers;

public class CreateAccountCommandHandlerTests
{
    // Given
    private readonly Fixture _fixture = new();
    private readonly Mock<IAccountRepository> _repository = new();
    private readonly Mock<IMediator> _mediator = new();
    private readonly CreateAccountCommandHandler _handler;
    private readonly IValidator<CreateAccountCommand> _validator;
    private readonly CreateAccountCommand _command;
    
    private const string GenericValidAgency = "001";
    private const string GenericValidDocument = "12345678909";

    public CreateAccountCommandHandlerTests()
    {
        _validator = new CreateAccountCommandValidator();
        _handler = new(_mediator.Object, _repository.Object, _validator);
        _command = new CreateAccountCommand() { Agency = GenericValidAgency, Document = GenericValidDocument };

    }
    
    [Theory]
    [InlineData("001", "12345678909", true, 1)]
    [InlineData("001", "12345678000942", true, 1)]
    [InlineData("001", "12345678900", false, 0)]
    [InlineData("001", "12345678000900", false, 0)]
    [InlineData("001", "1234567890123456", false, 0)]
    [InlineData("abc", "12345678909", false, 0)]
    public async void CreateAccountCommandHandler_GivenAndValidatedRequest_ShouldReturnExpected(string agency, string document,
        bool expectedValidation, int expectedPublish)
    {
        var command = new CreateAccountCommand() { Agency = agency, Document = document };

        // When
        var validationResult = await _validator.ValidateAsync
        (command,
            opt => opt.IncludeAllRuleSets(),
            It.IsAny<CancellationToken>());
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (Exception ex)
        {
            ex.Should().BeOfType<BadHttpRequestException>();
            Assert.False(expectedValidation);
        }

        // Then
        _mediator.Verify(x => x.Publish(It.IsAny<CreatedAccountEvent>(), 
            It.IsAny<CancellationToken>()), Times.Exactly(expectedPublish));
        validationResult.IsValid.Should().Be(expectedValidation);
    }

    [Fact]
    public async void CreateAccountCommandHandlerTest()
    {
        // When
        await _handler.Handle(_command, CancellationToken.None);
        
        // Then
        _mediator.Verify(x => x.Publish(It.IsAny<CreatedAccountEvent>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void Handle_GivenAValidCommand_ShouldCreateAccountAndSaveChanges()
    {
        // When
        var command = _fixture.Build<CreateAccountCommand>()
            .With(x => x.Agency, GenericValidAgency)
            .With(x => x.Document, GenericValidDocument)
            .Create();

        await _handler.Handle(command, CancellationToken.None);
        
        // Then
        _repository.Verify(x => x.Save(), Times.Exactly(2)); }
}
    
     
    
