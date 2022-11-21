using AccountingService.Api.Contracts.v1.Requests;
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
using TransactioningService.Api.Controllers;

namespace AccountingService.UnitTests.Api.Controllers;

public class EventControllerTests
{
    // Given
    private readonly EventController _eventController;
    private readonly Mock<IEventStoreRepository> _eventStoreRepository = new();
    private readonly Mock<IMediator> _mediator = new();
    private readonly EventStore _event;
    private readonly List<EventStore> _events = new();
    private readonly GetByAccountId _getByAccountIdRequest;
    private readonly GetByEventId _getByEventIdRequest;
    private readonly GetByEventName _getByEventNameRequest;


    public EventControllerTests()
    {

        _eventController = new EventController(_eventStoreRepository.Object, _mediator.Object);

        var fixture = new Fixture();
        _event = fixture.Build<EventStore>().Create();
        _events.Add(_event);
        _getByAccountIdRequest = fixture.Build<GetByAccountId>().Create();
        _getByEventIdRequest = fixture.Build<GetByEventId>().With(x => x.Id, _event.EventStoreId.ToString).Create();
        _getByEventNameRequest = fixture.Build<GetByEventName>().Create();
    }

    [Fact]
    public async void GetAllEvents_ShouldReturnOk()
    {
        // Given
        _eventStoreRepository.Setup(x => x.GetAllEvents()).ReturnsAsync(_events);

        // When
        var result = await _eventController.GetAllEvents();

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetAllEvents_ShouldGetFromRepository()
    {
        // When
        await _eventController.GetAllEvents();

        // Then
        _eventStoreRepository.Verify(x => x.GetAllEvents(), Times.Once);
    }

    [Fact]
    public async void GetEventById_GivenValidRequest_ShouldReturnOk()
    {
        // Given
        _eventStoreRepository.Setup(x => x.GetEventById(It.IsAny<Guid>())).ReturnsAsync(_event);

        // When
        var result = await _eventController.GetEventById(_getByEventIdRequest);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetEventById_GivenValidRequest_ShouldGetFromRepository()
    {
        // When
        await _eventController.GetEventById(_getByEventIdRequest);

        // Then
        _eventStoreRepository.Verify(x => x.GetEventById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async void GetEventsByAccountId_GivenValidRequest_ShouldReturnOk()
    {

        // Given
        _eventStoreRepository.Setup(x => x.GetEventsByAccountId(It.IsAny<int>())).ReturnsAsync(_events);

        // When
        var result = await _eventController.GetEventByAccountId(_getByAccountIdRequest);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetEventsByAccountId_GivenValidRequest_ShouldGetFromRepository()
    {
        // When
        await _eventController.GetEventByAccountId(_getByAccountIdRequest);

        // Then
        _eventStoreRepository.Verify(x => x.GetEventsByAccountId(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async void GetEventsByName_GivenValidRequest_ShouldReturnOk()
    {

        // Given
        _eventStoreRepository.Setup(x => x.GetEventsByName(It.IsAny<string>())).ReturnsAsync(_events);

        // When
        var result = await _eventController.GetEventByName(_getByEventNameRequest);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetEventsByName_GivenValidRequest_ShouldGetFromRepository()
    {
        // When
        await _eventController.GetEventByName(_getByEventNameRequest);

        // Then
        _eventStoreRepository.Verify(x => x.GetEventsByName(It.IsAny<string>()), Times.Once);
    }
}