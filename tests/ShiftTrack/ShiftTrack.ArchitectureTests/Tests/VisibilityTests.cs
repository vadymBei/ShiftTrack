using ArchUnitNET.xUnit;
using ShiftTrack.ArchitectureTests.Abstractions;
using ShiftTrack.Kernel.CQRS.Interfaces;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ShiftTrack.ArchitectureTests.Tests;

public class VisibilityTests : BaseTest
{
    [Fact]
    public void CommandAndQueryHandlers_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Or()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should().BeInternal()
            .Check(Architecture);
    }
}