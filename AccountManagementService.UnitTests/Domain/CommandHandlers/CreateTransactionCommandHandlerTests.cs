using AccountManagementService.Domain.CommandHandlers;
using AccountManagementService.Domain.Commands;
using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.Models;
using AccountManagementService.Domain.Notifications;
using AccountManagementService.Domain.Validators.Commands;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AccountManagementService.UnitTests.Domain.CommandHandlers;

public class CreateTransactionCommandHandlerTests
{
    // Given
    private readonly Fixture _fixture = new();
    private readonly Mock<IAccountRepository> _accountRepository = new();
    private readonly Mock<ITransactionRepository> _repository = new();
    private readonly Mock<IMediator> _mediator = new();
    private readonly CreateTransactionCommandHandler _handler;
    private readonly IValidator<CreateTransactionCommand> _validator;

    private static readonly int? GenericNullInt = null;
    private const string DepositTransactionType = "deposit";
    private static readonly Account  Account1 = new() { IsActive = true , Id = 1};
    private static readonly Account Account2 = new() { IsActive = true , Id = 2};
    private static readonly Account Account3 = new() { IsActive = false , Id = 3};
    
    private readonly CreateTransactionCommand _command;

    public CreateTransactionCommandHandlerTests()
    {
        _validator = new CreateTransactionCommandValidator();
        _handler = new(_mediator.Object, _repository.Object, _accountRepository.Object, _validator);
        
        _command = _fixture.Build<CreateTransactionCommand>()
            .With(x => x.TransactionType, DepositTransactionType)
            .With(x => x.CreditAccountId, Account1.Id)
            .With(x => x.DebitAccountId, GenericNullInt)
            .Create();
        }
    
    [Fact]
    public async void Handle_GivenAValidCommand_ShouldSave()
    {
        // When
        _accountRepository.Setup(x => x.GetAccountById(It.IsAny<int>())).ReturnsAsync(Account1);
        await _handler.Handle(_command, CancellationToken.None);
        
        // Then
        _repository.Verify(x => x.Save(), Times.Once); 
    }
    
    [Fact]
    public async void CreateTransactionCommandHandler_GivenCommand_ShouldPublishEvent()
    {
        // When
        _accountRepository.Setup(x => x.GetAccountById(It.IsAny<int>())).ReturnsAsync(Account1);
        await _handler.Handle(_command, CancellationToken.None);
        
        // Then
        _mediator.Verify(x => x.Publish(It.IsAny<CreatedTransactionEvent>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void CreateTransactionCommandHandler_GivenCommand_ShouldMakeChangesToRepository()
    {
        // When
        _accountRepository.Setup(x => x.GetAccountById(It.IsAny<int>())).ReturnsAsync(Account1);
        await _handler.Handle(_command, CancellationToken.None);
        
        // Then
        _repository.Verify(x => x.AddTransaction(It.IsAny<Transaction>()), Times.Once);
        _repository.Verify(x => x.Save(), Times.Once);
    }
    
    // Given
    public static List<object?[]> TheoryData => new()
    {
        new object[] { Account1.Id, Account2.Id, "transfer", true, 1, 0},
        new object[] { Account1.Id, Account1.Id, "transfer", false, 0, 1},
        new object?[] { Account1.Id, null, "deposit", true, 1, 0},
        new object?[] { null, Account2.Id, "withdraw", true, 1, 0},
        new object[] { Account1.Id, Account2.Id, "deposit", false, 0, 1},
        new object[] { Account1.Id, Account2.Id, "withdraw", false, 0, 1},
        new object?[] { null, Account2.Id, "deposit", false, 0, 1},
        new object?[] { Account1.Id, null, "withdraw", false, 0, 1},
    };
    [Theory, MemberData(nameof(TheoryData))]
    public async void CreateTransactionCommandHandler_GivenAndValidatedRequest_ShouldReturnExpected(int? creditAccountId, int? debitAccountId, string transactionType,
        bool expectedValidation, int expectedCreatedEvent, int expectedErrorEvent)
    {
        // Given
        _accountRepository.Setup(x => x.GetAccountById(Account1.Id)).ReturnsAsync(Account1);
        _accountRepository.Setup(x => x.GetAccountById(Account2.Id)).ReturnsAsync(Account2);
        _accountRepository.Setup(x => x.GetAccountById(Account3.Id)).ReturnsAsync(Account3);
        var command = new CreateTransactionCommand()
            { TransactionType = transactionType, CreditAccountId = creditAccountId, DebitAccountId = debitAccountId };
    
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
        _mediator.Verify(x => x.Publish(It.IsAny<CreatedTransactionEvent>(), 
            It.IsAny<CancellationToken>()), Times.Exactly(expectedCreatedEvent));
        _mediator.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), 
            It.IsAny<CancellationToken>()), Times.Exactly(expectedErrorEvent));
        validationResult.IsValid.Should().Be(expectedValidation);
    }
    
    // Given
    public static List<object?[]> TheoryData2 => new()
    {
        new object[] { Account1.Id, Account1.Id, "transfer"},
        new object[] { Account1.Id, Account2.Id, "deposit"},
        new object[] { Account1.Id, Account2.Id, "withdraw"},
        new object?[] { null, Account2.Id, "deposit"},
        new object?[] { Account1.Id, null, "withdraw"},
        new object?[] { Account3.Id, null, "deposit"},
    };
    [Theory, MemberData(nameof(TheoryData2))]
    public async void CreateTransactionCommandHandler_GivenInvalidRequest_ShouldReturnExpected(int? creditAccountId, int? debitAccountId, string transactionType)
    {
        // Given
        _accountRepository.Setup(x => x.GetAccountById(Account1.Id)).ReturnsAsync(Account1);
        _accountRepository.Setup(x => x.GetAccountById(Account2.Id)).ReturnsAsync(Account2);
        _accountRepository.Setup(x => x.GetAccountById(Account3.Id)).ReturnsAsync(Account3);
        var command = new CreateTransactionCommand()
            { TransactionType = transactionType, CreditAccountId = creditAccountId, DebitAccountId = debitAccountId };
    
        // When
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (Exception ex)
        {
            // Then
            ex.Should().BeOfType<BadHttpRequestException>();
        }
        
        // Then
        _mediator.Verify(x => x.Publish(It.IsAny<CreatedTransactionEvent>(), 
            It.IsAny<CancellationToken>()), Times.Never);
    }
}