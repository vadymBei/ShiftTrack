using ArchUnitNET.xUnit;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.ArchitectureTests.Abstractions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace User.ArchitectureTests.Tests;

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