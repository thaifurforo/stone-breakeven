using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Controllers;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Repository;
using AccountingService.Repository.Repositories;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TransactioningService.Api.Controllers;

namespace AccountingService.UnitTests.Api.Controllers;

public class EventControllerTests : IAsyncLifetime
{
    // Given
    private readonly EventController _eventController;
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly Mock<IMediator> _mediator = new();
    private readonly EventStore _event;


    public EventControllerTests()
    {
        var options = new DbContextOptionsBuilder<EventStoreSqlContext>()
            .UseInMemoryDatabase(databaseName: "FakeDatabase")
            .Options;
        var eventStoreSqlContext = new EventStoreSqlContext(options);
        _eventStoreRepository = new EventStoreSqlRepository(eventStoreSqlContext);
        _eventController = new EventController(_eventStoreRepository, _mediator.Object);

        var fixture = new Fixture();
        _event = fixture.Build<EventStore>()
            .With(x => x.Metadata, "{\"Id\":1}")
            .Create();
    }

    public async Task InitializeAsync()
    {
        await _eventStoreRepository.AddEvent(_event);
        await _eventStoreRepository.Save();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async void GetAllEvents()
    {
        // When
        var result = await _eventController.GetAllEvents();

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetEventById()
    {
        // When
        var request = new GetByEventId() { Id = _event.EventStoreId.ToString() };
        var result = await _eventController.GetEventById(request);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetEventByAccountId()
    {
  
        // When
        var request = new GetByAccountId() { Id = 1 };
        var result = await _eventController.GetEventByAccountId(request);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

}

