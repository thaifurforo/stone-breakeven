using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Controllers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Repository.Contexts;
using AccountingService.Repository.Repositories;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccountingService.UnitTests.Api.Controllers;

public class TransactionControllerTests : IAsyncLifetime
{
    // Given
    private readonly TransactionController _transactionController;
    private readonly ITransactionRepository _transactionRepository;
    private readonly Mock<IMediator> _mediator = new();

    private readonly Account _creditAccount;
    private readonly Account _debitAccount;
    private readonly Transaction _transaction;
    
    public TransactionControllerTests()
    {
        var options = new DbContextOptionsBuilder<ReadModelSqlContext>()
            .UseInMemoryDatabase(databaseName: "FakeDatabase")
            .Options;
        var readModelSqlContext = new ReadModelSqlContext(options);
        _transactionRepository = new TransactionSqlRepository(readModelSqlContext);
        _transactionController = new TransactionController(_transactionRepository, _mediator.Object);
        
        var fixture = new Fixture();
        _creditAccount = fixture.Build<Account>().Create();
        _debitAccount = fixture.Build<Account>().Create();
        _transaction = fixture.Build<Transaction>()
            .With(x => x.CreditAccountId, _creditAccount.Id)
            .With(x => x.DebitAccountId, _debitAccount.Id)
            .Create();
    }
    
    public async Task InitializeAsync()
    {
        await _transactionRepository.AddTransaction(_transaction);
        await _transactionRepository.Save();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    [Fact]
    public async void CreateTransactionTest()
    {
        // When
        await _transactionController.CreateTransaction(It.IsAny<CreateTransactionCommand>());
        
        // Then
        _mediator.Verify(x => x.Send(It.IsAny<CreateTransactionCommand>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async void GetAllTransactionsTest()
    {
        // When
        var result = await _transactionController.GetAllTransactions();

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void GetTransactionByIdTest()
    {
        // When
        var request = new GetByTransactionId() { Id = _transaction.TransactionId.ToString() };
        var result = await _transactionController.GetTransactionById(request);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void GetTransactionsByAccountTest()
    {
        // When
        var request = new GetByAccountId() { Id = _creditAccount.Id };
        var result = await _transactionController.GetTransactionsByAccount(request);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void GetCreditTransactionsByAccountTest()
    {
        // When
        var request = new GetByAccountId() { Id = _creditAccount.Id };
        var result = await _transactionController.GetCreditTransactionsByAccount(request);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void GetDebitTransactionsByAccountTest()
    {
        // When
        var request = new GetByAccountId() { Id = _debitAccount.Id };
        var result = await _transactionController.GetDebitTransactionsByAccount(request);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
}

