using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Contracts.v1.Response;
using AccountingService.Api.Controllers;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Repository;
using AccountingService.Repository.Repositories;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.UnitTests.Api.Controllers;

public class AccountControllerTests
{
    private readonly Fixture _fixture = new();
    private readonly ContextInMemory _context;
    private IAccountRepository accountRepository;
    private AccountController accountController;
    
    public AccountControllerTests()
    {
        _fixture.Register(() => default(DateOnly));
        
        var options = new DbContextOptionsBuilder<ContextInMemory>()
            .UseInMemoryDatabase(databaseName: "FakeDatabase")
            .Options;

        _context = new ContextInMemory(options);
        
        accountRepository = new AccountRepositoryInMemory(_context);
        accountController = new AccountController(accountRepository);
    }
    
    private const string Agency = "001";
    private const string Document = "12345678909";
    private const decimal Balance = 1;
    

    [Fact]
    public void CreateAccount_GivenInvalidRequest_ShouldReturnBadObjectRequest()
    {
        var request = _fixture.Create<CreateAccountRequest>();
        var response = accountController.CreateAccount(request);

        response.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public void CreateAccount_GivenValidRequest_ShouldReturnAccepted()
    {
        var request = _fixture.Build<CreateAccountRequest>()
            .With(x => x.Agency, Agency)
            .With(x => x.Document, Document)
            .Create();
        var response = accountController.CreateAccount(request);

        response.Should().BeOfType<OkObjectResult>();
    }
    
     
     public static readonly object[][] TheoryData =
     {
         new object[] { DateTime.Now, DateTime.Now.AddDays(1), 0, true},
         new object[] { DateTime.Now, DateTime.Now.AddDays(-1), 0, false},
         new object[] { DateTime.Now, DateTime.Now.AddDays(1), 1, false},
         new object[] { DateTime.Now, DateTime.Now.AddDays(1), -1, false},
     };
     
     [Theory, MemberData(nameof(TheoryData))]
     public void DeactivateAccount_GivenRequest_ShouldReturnExpected(DateTime openingDate,
         DateTime closingDate, decimal balance, bool expected)
     {
         
        var account = _fixture.Build<Account>()
            .With(x => x.OpeningDate, openingDate)
            .With(x => x.ClosingDate, closingDate)
            .With(x => x.Balance, balance)
            .Create();

        accountRepository.AddAccount(account);

        accountRepository.Save();

        var result = accountController.DeactivateAccount(account.Id);

        var okResult = result.GetType() == typeof(OkObjectResult);

        okResult.Should().Be(expected);
     }
     
    
}

