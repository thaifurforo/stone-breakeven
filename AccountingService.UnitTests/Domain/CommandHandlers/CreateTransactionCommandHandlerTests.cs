using AccountingService.Domain.CommandHandlers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Domain.Notifications;
using AccountingService.Domain.Validators.Commands;
using AccountingService.Repository.Contexts;
using AccountingService.Repository.Repositories;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccountingService.UnitTests.Domain.CommandHandlers;

public class CreateTransactionCommandHandlerTests : IAsyncLifetime
{
    // Given
    private readonly Fixture _fixture = new();
    private readonly IAccountRepository _accountRepository;
    private readonly Mock<ITransactionRepository> _repository = new();
    private readonly Mock<IMediator> _mediator = new();
    private readonly CreateTransactionCommandHandler _handler;
    private readonly IValidator<CreateTransactionCommand> _validator;

    private static readonly int? GenericNullInt = null;
    private const string DepositTransactionType = "deposit";
    private static Account _account1;
    private static Account _account2;
    
    private readonly CreateTransactionCommand _command;

    public CreateTransactionCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ReadModelSqlContext>()
            .UseInMemoryDatabase(databaseName: "FakeDatabase")
            .Options;
        var readModelSqlContext = new ReadModelSqlContext(options);
        _accountRepository = new AccountSqlRepository(readModelSqlContext);

        _validator = new CreateTransactionCommandValidator();
        _handler = new(_mediator.Object, _repository.Object, _accountRepository, _validator);
        
        _account1 = _fixture.Build<Account>().With(x => x.IsActive, true).Create();
        _account2 = _fixture.Build<Account>().With(x => x.IsActive, true).Create();
        
        _command = _fixture.Build<CreateTransactionCommand>()
            .With(x => x.TransactionType, DepositTransactionType)
            .With(x => x.CreditAccountId, _account1.Id)
            .With(x => x.DebitAccountId, GenericNullInt)
            .Create();
    }
    
    public async Task InitializeAsync()
    {
        await _accountRepository.AddAccount(_account1);
        await _accountRepository.AddAccount(_account2);
        await _accountRepository.Save();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async void Handle_GivenAValidCommand_ShouldCreateTransactionAndSaveChanges()
    {
        // When
        await _handler.Handle(_command, CancellationToken.None);
        
        // Then
        _repository.Verify(x => x.Save(), Times.Once); 
    }
    
    [Fact]
    public async void CreateTransactionCommandHandlerTest()
    {
        // When
        await _handler.Handle(_command, CancellationToken.None);
        
        // Then
        _mediator.Verify(x => x.Publish(It.IsAny<CreatedTransactionEvent>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // Given
    public static readonly object[][] TheoryData =
    {
        new object[] { 1, 2, "transfer", true, 1},
        new object[] { 1, 1, "transfer", false, 0},
        new object[] { 1, 0, "deposit", true, 1},
        new object[] { 0, 2, "withdraw", true, 1},
        new object[] { 1, 2, "deposit", false, 0},
        new object[] { 1, 2, "withdraw", false, 0},
        new object[] { 0, 2, "deposit", false, 0},
        new object[] { 1, 0, "withdraw", false, 0},
    };
    [Theory, MemberData(nameof(TheoryData))]
    public async void CreateTransactionCommandHandler_GivenRequest_ShouldReturnExpected(int? creditAccountId, int? debitAccountId, string transactionType,
        bool expectedValidation, int expectedPublish)
    {
        // Given
        if (creditAccountId == 1)
            creditAccountId = _account1.Id;
        else if (creditAccountId == 0)
            creditAccountId = null;
        
        if (debitAccountId == 1)
            debitAccountId = _account1.Id;
        else if (debitAccountId == 2)
            debitAccountId = _account2.Id;
        else if (debitAccountId == 0)
            debitAccountId = null;
        
        var command = new CreateTransactionCommand()
            { TransactionType = transactionType, CreditAccountId = creditAccountId, DebitAccountId = debitAccountId };
    
        // When
        var validationResult = await _validator.ValidateAsync
        (command,
            opt => opt.IncludeAllRuleSets(),
            It.IsAny<CancellationToken>());
        await _handler.Handle(command, CancellationToken.None);
        
        // Then
        _mediator.Verify(x => x.Publish(It.IsAny<CreatedTransactionEvent>(), 
            It.IsAny<CancellationToken>()), Times.Exactly(expectedPublish));
        validationResult.IsValid.Should().Be(expectedValidation);
    }
}