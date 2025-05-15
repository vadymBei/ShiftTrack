// using Moq;
// using ShiftTrack.API.Controllers.Organization.Timesheet;
// using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
// using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
// using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShifts;
// using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;
// using ShiftTrack.Kernel.CQRS.Queries;
//
// namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Queries;
//
// public class GetShiftsQueryTests(
//     IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
// {
//     [Fact]
//     public async Task GetShifts_ReturnsExpectedShifts()
//     {
//         // Arrange
//         var mockHandler = new Mock<IQueryHandler<GetShiftsQuery, IEnumerable<ShiftVM>>>();
//     
//         var expectedShifts = new List<ShiftVM>
//         {
//             new()
//             {
//                 Id = 1, 
//                 Code = "ВХ", 
//                 Description = "Вихідний",
//                 Color = "#FFFFF",
//                 Type = ShiftType.DayOff,
//                 EndTime = null,
//                 StartTime = null,
//                 WorkHours = null
//             },
//         };
//     
//         mockHandler.Setup(h => h.Handle(It.IsAny<GetShiftsQuery>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(expectedShifts);
//     
//         var controller = new ORG_TSH_ShiftsController(mockHandler.Object);
//     
//         // Act
//         var result = await controller.GetShifts();
//     
//         // Assert
//         Assert.Equal(expectedShifts, result);
//     
//         mockHandler.Verify(h => h.Handle(It.IsAny<GetShiftsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
//     }
// }