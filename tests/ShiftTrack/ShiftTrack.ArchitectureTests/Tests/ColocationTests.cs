using ShiftTrack.ArchitectureTests.Abstractions;
using ShiftTrack.Kernel.CQRS.Interfaces;
using Shouldly;
using Xunit;

namespace ShiftTrack.ArchitectureTests.Tests;

public class ColocationTests : BaseTest
{
    [Theory]
    [MemberData(nameof(GetHandlerAndCommandPairs))]
    public void Handlers_ShouldResideInSameNamespace_AsTheirCommandOrQuery(
        Type handlerType,
        Type commandOrQueryType)
    {
        handlerType.Namespace.ShouldBe(
            commandOrQueryType.Namespace,
            $"{handlerType.Name} should be in the same namespace as {commandOrQueryType.Name}");
    }

    public static TheoryData<Type, Type> GetHandlerAndCommandPairs()
    {
        Type[] handlerInterfaces =
        [
            typeof(IRequestHandler<>),
            typeof(IRequestHandler<,>),
        ];

        var pairs = new TheoryData<Type, Type>();

        var handlers = ApplicationAssembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false })
            .Where(t => t.DeclaringType is null);

        foreach (var handler in handlers)
        {
            foreach (var iface in handler.GetInterfaces())
            {
                if (!iface.IsGenericType)
                {
                    continue;
                }

                var genericDef = iface.GetGenericTypeDefinition();

                if (!handlerInterfaces.Contains(genericDef))
                {
                    continue;
                }

                var commandOrQueryType = iface.GetGenericArguments()[0];
                pairs.Add(handler, commandOrQueryType);
            }
        }

        return pairs;
    }
}