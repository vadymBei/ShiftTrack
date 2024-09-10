using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnitById;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Units.Queries
{
    public class GetUnitByIdQueryTests : BaseIntegrationTest
    {
        public GetUnitByIdQueryTests(
            IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetById_ShouldReturnEntityNotFoundException_WhenUnitNotFound()
        {
            // Arrange
            var getUnitByIdQuery = new GetUnitByIdQuery(100);

            // Act
            Func<Task> act = async () => await Sender.Send(getUnitByIdQuery);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetById_ShouldReturnUnit_WhenUnitExists()
        {
            // Arrange
            var createUnitCommand = new CreateUnitCommand(
                "Хмельницький",
                "Хмельницький регіон",
                "Хм");

            var newUnit = await Sender.Send(createUnitCommand);

            var getUnitByIdQuery = new GetUnitByIdQuery(newUnit.Id);

            // Act
            var unit = await Sender.Send(getUnitByIdQuery);

            // Assert
            unit.Should().NotBeNull();

            unit.Should().BeEquivalentTo(
                new UnitVM
                {
                    Id = newUnit.Id,
                    Code = newUnit.Code,
                    Name = newUnit.Name,
                    Description = newUnit.Description,
                    FullName = newUnit.Code + " " + newUnit.Name
                });
        }
    }
}
