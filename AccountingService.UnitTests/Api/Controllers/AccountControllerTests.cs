using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Controllers;
using AccountingService.Domain.Models;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace AccountingService.UnitTests.Api.Controllers;

public class AccountControllerTests
{
    private readonly Fixture _fixture = new();
    private readonly AccountController _controller = new();

    private const string agency = "001";
    private const string document = "12345678909";
    
    public AccountControllerTests()
    {
        _fixture.Register(() => default(DateOnly));
    }

    [Fact]
    public async Task CreateAccount_GivenInvalidRequest_ShouldReturnBadObjectRequest()
    {
        var request = _fixture.Create<CreateAccountRequest>();
        var response = await _controller.CreateAccount(request);

        response.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public async Task CreateAccount_GivenValidRequest_ShouldReturnAccepted()
    {
        var request = _fixture.Build<CreateAccountRequest>()
            .With(x => x.Agency, agency)
            .With(x => x.Document, document)
            .Create();
        var response = await _controller.CreateAccount(request);

        response.Should().BeOfType<OkObjectResult>();
    }
    
}

