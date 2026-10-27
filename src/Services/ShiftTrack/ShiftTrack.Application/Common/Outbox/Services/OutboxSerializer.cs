using System.Text.Json;
using System.Text.Json.Serialization;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Common.Outbox.Services;

public static class OutboxSerializer
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>Type name without version, so messages survive a redeploy.</summary>
    public static string GetTypeName(Type type) => $"{type.FullName}, {type.Assembly.GetName().Name}";

    public static string Serialize(IDomainEvent domainEvent) =>
        JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), Options);

    public static IDomainEvent Deserialize(string typeName, string content)
    {
        var type = Type.GetType(typeName, throwOnError: false)
                   ?? throw new InvalidOperationException($"Outbox event type '{typeName}' is not found.");

        if (!typeof(IDomainEvent).IsAssignableFrom(type))
            throw new InvalidOperationException($"Outbox event type '{typeName}' is not a domain event.");

        return (IDomainEvent)(JsonSerializer.Deserialize(content, type, Options)
                              ?? throw new InvalidOperationException($"Outbox event '{typeName}' is empty."));
    }
}
