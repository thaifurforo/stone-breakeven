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

public class TransactionControllerTests
{
    // Given
    private readonly TransactionController _transactionController;
    private readonly Mock<ITransactionRepository> _transactionRepository = new();
    private readonly Mock<IMediator> _mediator = new();

    private readonly Account _creditAccount;
    private readonly Account _debitAccount;
    private readonly Transaction _transaction;
    private readonly List<Transaction> _transactions = new();
    private readonly GetByTransactionId _getByTransactionIdRequest;
    private readonly GetByAccountId _getByAccountIdRequest;

    private readonly CreateTransactionCommand _createTransactionCommand;
    
    public TransactionControllerTests()
    {
      
        _transactionController = new TransactionController(_transactionRepository.Object, _mediator.Object);
        
        var fixture = new Fixture();
        _creditAccount = fixture.Build<Account>().Create();
        _debitAccount = fixture.Build<Account>().Create();
        _transaction = fixture.Build<Transaction>().Create();
        _transactions.Add(_transaction);
        _getByTransactionIdRequest = fixture.Build<GetByTransactionId>()
            .With(x => x.Id, _transaction.TransactionId.ToString)
            .Create();
        _getByAccountIdRequest = fixture.Build<GetByAccountId>().Create();
        _createTransactionCommand = fixture.Build<CreateTransactionCommand>().Create();
    }
    
    [Fact]
    public async void CreateTransaction_GivenCommand_ShouldSendCommand()
    {
        // When
        await _transactionController.CreateTransaction(_createTransactionCommand);
        
        // Then
        _mediator.Verify(x => x.Send(It.IsAny<CreateTransactionCommand>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async void CreateTransaction_GivenCommand_ShouldReturnOk()
    {
        // Given
        _transactionRepository.Setup(x => x.AddTransaction(It.IsAny<Transaction>())).ReturnsAsync(_transaction);
        
        // When
        var result = await _transactionController.CreateTransaction(_createTransactionCommand);
        
        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetAllTransactions_ShouldReturnOk()
    {
        // Given
        _transactionRepository.Setup(x => x.GetAllTransactions()).ReturnsAsync(_transactions);
        
        // When
        var result = await _transactionController.GetAllTransactions();

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void GetAllTransactions_ShouldGetFromRepository()
    {
        // When
        await _transactionController.GetAllTransactions();

        // Then
        _transactionRepository.Verify(x => x.GetAllTransactions(), Times.Once);
    }
    
    [Fact]
    public async void GetTransactionById_GivenValidRequest_ShouldReturnOk()
    {
        // Given
        _transactionRepository.Setup(x => x.GetTransactionById(It.IsAny<Guid>())).ReturnsAsync(_transaction);
        
        // When
        var result = await _transactionController.GetTransactionById(_getByTransactionIdRequest);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void GetTransactionById_GivenValidRequest_ShouldGetFromRepository()
    {
      
        // When
        await _transactionController.GetTransactionById(_getByTransactionIdRequest);

        // Then
        _transactionRepository.Verify(x => x.GetTransactionById(It.IsAny<Guid>()), Times.Once);
    }
    [Fact]
    
    public async void GetTransactionsByAccountId_GivenValidRequest_ShouldReturnOk()
    {
        // Given
        _transactionRepository.Setup(x => x.GetTransactionsByAccountId(It.IsAny<int>())).ReturnsAsync(_transactions);
        
        // When
        var result = await _transactionController.GetTransactionsByAccountId(_getByAccountIdRequest);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void GetTransactionsByAccountId_GivenValidRequest_ShouldGetFromRepository()
    {
        // When
        await _transactionController.GetTransactionsByAccountId(_getByAccountIdRequest);

        // Then
        _transactionRepository.Verify(x => x.GetTransactionsByAccountId(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async void GetTransactionsByCreditAccountId_GivenValidRequest_ShouldReturnOk()
    {
        // Given
        _transactionRepository.Setup(x => x.GetTransactionsByCreditAccountId(It.IsAny<int>())).ReturnsAsync(_transactions);
        
        // When
        var result = await _transactionController.GetTransactionsByCreditAccountId(_getByAccountIdRequest);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void GetTransactionsByCreditAccountId_GivenValidRequest_ShouldGetFromRepository()
    {
        // When
        await _transactionController.GetTransactionsByCreditAccountId(_getByAccountIdRequest);

        // Then
        _transactionRepository.Verify(x => x.GetTransactionsByCreditAccountId(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async void GetTransactionsByDebitAccountId_GivenValidRequest_ShouldReturnOk()
    {
        // Given
        _transactionRepository.Setup(x => x.GetTransactionsByDebitAccountId(It.IsAny<int>())).ReturnsAsync(_transactions);
        
        // When
        var result = await _transactionController.GetTransactionsByDebitAccountId(_getByAccountIdRequest);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async void GetTransactionsByDebitAccountId_GivenValidRequest_ShouldGetFromRepository()
    {
        // When
        await _transactionController.GetTransactionsByDebitAccountId(_getByAccountIdRequest);

        // Then
        _transactionRepository.Verify(x => x.GetTransactionsByDebitAccountId(It.IsAny<int>()), Times.Once);
    }
}

