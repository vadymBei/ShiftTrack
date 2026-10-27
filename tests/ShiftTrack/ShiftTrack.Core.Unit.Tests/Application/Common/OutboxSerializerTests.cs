using ShiftTrack.Application.Common.Outbox.Services;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Events;
using ShiftTrack.Application.Modules.Booking.Vacations.Events;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Core.Unit.Tests.Application.Common;

public class OutboxSerializerTests
{
    [Fact]
    public void TypeName_HasNoVersion_SoMessagesSurviveRedeploy()
    {
        var typeName = OutboxSerializer.GetTypeName(typeof(VacationStatusChangedEvent));

        Assert.Equal(
            "ShiftTrack.Application.Modules.Booking.Vacations.Events.VacationStatusChangedEvent, ShiftTrack.Application",
            typeName);
    }

    [Fact]
    public void Event_SurvivesRoundTrip()
    {
        var domainEvent = new VacationStatusChangedEvent(
            9, VacationStatus.PendingApproval, VacationStatus.ApprovedByUnitDirector);

        var restored = OutboxSerializer.Deserialize(
            OutboxSerializer.GetTypeName(domainEvent.GetType()),
            OutboxSerializer.Serialize(domainEvent));

        Assert.Equal(domainEvent, restored);
    }

    [Fact]
    public void EventWithCollections_SurvivesRoundTrip()
    {
        var domainEvent = new BusinessTripDeletedEvent(
            5, 7, [1, 2], ["kyiv", "lviv"], new DateTime(2026, 11, 1), new DateTime(2026, 11, 3));

        var restored = Assert.IsType<BusinessTripDeletedEvent>(OutboxSerializer.Deserialize(
            OutboxSerializer.GetTypeName(domainEvent.GetType()),
            OutboxSerializer.Serialize(domainEvent)));

        Assert.Equal(domainEvent.BusinessTripId, restored.BusinessTripId);
        Assert.Equal(domainEvent.ParticipantIds, restored.ParticipantIds);
        Assert.Equal(domainEvent.LocationIntegrationIds, restored.LocationIntegrationIds);
        Assert.Equal(domainEvent.EndDate, restored.EndDate);
    }

    [Fact]
    public void UnknownType_Throws()
    {
        Assert.Throws<InvalidOperationException>(() =>
            OutboxSerializer.Deserialize("ShiftTrack.Missing.Event, ShiftTrack.Application", "{}"));
    }

    [Fact]
    public void TypeThatIsNotDomainEvent_Throws()
    {
        Assert.Throws<InvalidOperationException>(() =>
            OutboxSerializer.Deserialize(OutboxSerializer.GetTypeName(typeof(string)), "\"text\""));
    }
}
