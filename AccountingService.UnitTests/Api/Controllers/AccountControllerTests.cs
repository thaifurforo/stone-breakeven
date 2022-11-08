// using AccountingService.Api.Contracts.v1.Requests;
// using AccountingService.Api.Contracts.v1.Response;
// using AccountingService.Api.Controllers;
// using AccountingService.Domain.Models;
// using AutoFixture;
// using FluentAssertions;
// using Microsoft.AspNetCore.Mvc;
//
// namespace AccountingService.UnitTests.Api.Controllers;
//
// public class AccountControllerTests
// {
//     private readonly Fixture _fixture = new();
//     private readonly AccountController _controller = new();
//
//     private const string Agency = "001";
//     private const string Document = "12345678909";
//     private const decimal Balance = 1;
//     
//     public AccountControllerTests()
//     {
//         _fixture.Register(() => default(DateOnly));
//     }
//
//     [Fact]
//     public async Task CreateAccount_GivenInvalidRequest_ShouldReturnBadObjectRequest()
//     {
//         var request = _fixture.Create<CreateAccountRequest>();
//         var response = await _controller.CreateAccount(request);
//
//         response.Should().BeOfType<BadRequestObjectResult>();
//     }
//     
//     [Fact]
//     public async Task CreateAccount_GivenValidRequest_ShouldReturnAccepted()
//     {
//         var request = _fixture.Build<CreateAccountRequest>()
//             .With(x => x.Agency, Agency)
//             .With(x => x.Document, Document)
//             .Create();
//         var response = await _controller.CreateAccount(request);
//
//         response.Should().BeOfType<OkObjectResult>();
//     }
//     
//      [Fact]
//      public async Task DeactivateAccount_GivenValidRequest_ShouldReturnAccepted()
//      {
//          var account = _fixture.Build<Account>()
//              .Create();
//         
//          AccountList.Accounts.Add(account);
//          
//          var response = await _controller.DeactivateAccount(account.Id);
//
//          response.Should().BeOfType<BadRequestObjectResult>();
//      }
//      
//      [Fact]
//      public async Task DeactivateAccount_GivenInvalidRequest_ShouldReturnAccepted()
//      {
//          var account = _fixture.Build<Account>()
//              .With(x => x.Balance, Balance)
//              .Create();
//         
//          AccountList.Accounts.Add(account);
//          
//          var response = await _controller.DeactivateAccount(account.Id);
//
//          response.Should().BeOfType<OkObjectResult>();
//      }
// }
//
