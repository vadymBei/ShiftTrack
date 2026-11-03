using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Application.Modules.Communication.Messenger.Services;
using ShiftTrack.Domain.Modules.System.User.Employees.Enums;

namespace ShiftTrack.Core.Unit.Tests.Application.Communication.Messenger;

public class SystemMessageTextsTests
{
    [Theory]
    [InlineData(EmployeeGender.Female, "створила")]
    [InlineData(EmployeeGender.Male, "створив")]
    [InlineData(EmployeeGender.None, "створив(-ла)")]
    public void Verb_FollowsTheGenderOfTheActor(EmployeeGender gender, string verb)
    {
        var actor = ConversationBuilder.Employee(1, "Коваль", "Олена", gender);

        Assert.Equal($"Коваль Олена  {verb} групу «Склад»", SystemMessageTexts.Created(actor, "Склад"));
    }

    [Fact]
    public void Added_ListsEveryNewMember()
    {
        var actor = ConversationBuilder.Employee(1, "Коваль", "Олена");
        var added = new[]
        {
            ConversationBuilder.Employee(2, "Шевчук", "Ірина"),
            ConversationBuilder.Employee(3, "Бондар", "Тарас", EmployeeGender.Male)
        };

        Assert.Equal("Коваль Олена  додала до групи: Шевчук Ірина , Бондар Тарас ",
            SystemMessageTexts.Added(actor, added));
    }

    [Fact]
    public void NewOwner_UsesTheFeminineNoun()
    {
        var owner = ConversationBuilder.Employee(2, "Шевчук", "Ірина");

        Assert.Equal("Шевчук Ірина  тепер власниця групи", SystemMessageTexts.NewOwner(owner));
    }
}
