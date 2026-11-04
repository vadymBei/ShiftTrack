using System.Net;
using Moq;
using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using ShiftTrack.Application.Modules.Communication.Messenger.Exceptions;
using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Application.Modules.Communication.Messenger.Services;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;
using Microsoft.Extensions.Logging.Abstractions;

namespace ShiftTrack.Core.Unit.Tests.Application.Communication.Messenger;

public class AttachmentTests : MessengerFixture
{
    private static AttachmentUpload File(string name, long size = 1024) => new(new MemoryStream([1, 2, 3]), name, size);

    private void InChat() => Existing(ConversationBuilder.Direct().WithMember(Me).WithMember(2).Build());

    [Fact]
    public async Task Upload_SavesUnderARandomName_WithTheTypeOfTheExtension()
    {
        InChat();
        Storage.Setup(x => x.Save(It.IsAny<Stream>(), ".pdf", It.IsAny<CancellationToken>())).ReturnsAsync("2026/11/abc.pdf");

        var attachment = await AttachmentService.Upload(10, File(@"C:\fakepath\Довідка лікаря.PDF"), default);

        Assert.Equal("Довідка лікаря.PDF", attachment.FileName);
        Assert.Equal("2026/11/abc.pdf", attachment.StoredName);
        Assert.Equal("application/pdf", attachment.ContentType);
        Assert.Null(attachment.MessageId);
        Attachments.Verify(x => x.Add(attachment, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("script.exe")]
    [InlineData("page.html")]
    [InlineData("logo.svg")]
    [InlineData("no-extension")]
    public async Task Upload_DangerousOrUnknownTypes_AreRejected(string name)
    {
        InChat();

        var exception = await Assert.ThrowsAsync<MessengerException>(() => AttachmentService.Upload(10, File(name), default));

        Assert.Equal("CMN_MSG_ATTACHMENT_TYPE_NOT_ALLOWED", exception.ErrorType);
        Storage.Verify(x => x.Save(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Upload_TooLarge_IsRejected()
    {
        InChat();

        var exception = await Assert.ThrowsAsync<MessengerException>(() =>
            AttachmentService.Upload(10, File("big.zip", MessengerConstants.MaxAttachmentSize + 1), default));

        Assert.Equal("CMN_MSG_ATTACHMENT_TOO_LARGE", exception.ErrorType);
        Assert.Equal(HttpStatusCode.RequestEntityTooLarge, exception.Code);
    }

    [Fact]
    public async Task Upload_ToAChatYouAreNotIn_IsForbidden()
    {
        Existing(ConversationBuilder.Direct().WithMember(2).WithMember(3).Build());

        var exception = await Assert.ThrowsAsync<MessengerException>(() => AttachmentService.Upload(10, File("a.pdf"), default));

        Assert.Equal("CMN_MSG_NOT_A_MEMBER", exception.ErrorType);
    }

    [Fact]
    public async Task Upload_FailedToSaveTheRow_RemovesTheFile()
    {
        InChat();
        Storage.Setup(x => x.Save(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("x.pdf");
        Attachments.Setup(x => x.Add(It.IsAny<MessageAttachment>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(() => AttachmentService.Upload(10, File("a.pdf"), default));

        Storage.Verify(x => x.Delete("x.pdf"), Times.Once);
    }

    [Fact]
    public async Task Send_OnlyFiles_IsAMessageWithoutText()
    {
        InChat();
        var upload = new MessageAttachment { Id = 5, ConversationId = 10, AuthorId = Me, FileName = "a.pdf", ContentType = "application/pdf" };
        Attachments.Setup(x => x.GetUnsent(Me, 10, It.Is<IReadOnlyCollection<long>>(i => i.SequenceEqual(new long[] { 5 })),
            It.IsAny<CancellationToken>())).ReturnsAsync([upload]);

        var message = await Command(() => MessageService.Send(10, "  ", null, null, [5, 5], default));

        Assert.Null(message.Text);
        Assert.Same(upload, Assert.Single(message.Attachments));
        Publisher.Verify(x => x.MessageCreated(It.IsAny<IReadOnlyCollection<long>>(),
            It.Is<MessageVm>(m => m.Attachments.Count == 1 && m.Attachments[0].FileName == "a.pdf"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Send_FilesOfSomeoneElseOrAlreadySent_AreRejected()
    {
        InChat();

        var exception = await Assert.ThrowsAsync<MessengerException>(() =>
            MessageService.Send(10, "x", null, null, [5], default));

        Assert.Equal("CMN_MSG_ATTACHMENTS_NOT_FOUND", exception.ErrorType);
        Messages.Verify(x => x.Add(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Send_TooManyFiles_IsRejected()
    {
        InChat();

        var exception = await Assert.ThrowsAsync<MessengerException>(() =>
            MessageService.Send(10, "x", null, null, Enumerable.Range(1, 11).Select(i => (long)i).ToList(), default));

        Assert.Equal("CMN_MSG_TOO_MANY_ATTACHMENTS", exception.ErrorType);
    }

    [Fact]
    public async Task Delete_RemovesTheFiles_AfterTheCommit()
    {
        InChat();
        var message = new Message
        {
            Id = 7, ConversationId = 10, AuthorId = Me, Type = MessageType.Text,
            Attachments = [new MessageAttachment { Id = 1, StoredName = "a.pdf" }, new MessageAttachment { Id = 2, StoredName = "b.png" }]
        };
        Messages.Setup(x => x.GetForUpdate(7, It.IsAny<CancellationToken>())).ReturnsAsync(message);
        var deletedBeforeCommit = true;

        await Command(async () =>
        {
            var deleted = await MessageService.Delete(7, default);
            deletedBeforeCommit = Storage.Invocations.Any(i => i.Method.Name == nameof(Storage.Object.Delete));
            return deleted;
        });

        Assert.False(deletedBeforeCommit);
        Assert.Empty(message.Attachments);
        Attachments.Verify(x => x.Remove(It.Is<IReadOnlyCollection<MessageAttachment>>(a => a.Count == 2), It.IsAny<CancellationToken>()), Times.Once);
        Storage.Verify(x => x.Delete("a.pdf"), Times.Once);
        Storage.Verify(x => x.Delete("b.png"), Times.Once);
    }

    [Fact]
    public async Task Open_ForAMember()
    {
        InChat();
        Attachments.Setup(x => x.GetById(3, default)).ReturnsAsync(
            new MessageAttachment { Id = 3, ConversationId = 10, MessageId = 7, AuthorId = 2, StoredName = "a.pdf" });
        Storage.Setup(x => x.Open("a.pdf")).Returns(new MemoryStream([1]));

        var file = await AttachmentService.Open(3, default);

        Assert.Equal(3, file.Attachment.Id);
    }

    [Fact]
    public async Task Open_SomeoneElsesUnsentUpload_IsNotFound()
    {
        InChat();
        Attachments.Setup(x => x.GetById(3, default)).ReturnsAsync(
            new MessageAttachment { Id = 3, ConversationId = 10, MessageId = null, AuthorId = 2, StoredName = "a.pdf" });

        var exception = await Assert.ThrowsAsync<MessengerException>(() => AttachmentService.Open(3, default));

        Assert.Equal(HttpStatusCode.NotFound, exception.Code);
    }

    [Fact]
    public async Task Open_ByAnOutsider_IsForbidden()
    {
        Existing(ConversationBuilder.Direct().WithMember(2).WithMember(3).Build());
        Attachments.Setup(x => x.GetById(3, default)).ReturnsAsync(
            new MessageAttachment { Id = 3, ConversationId = 10, MessageId = 7, AuthorId = 2, StoredName = "a.pdf" });

        var exception = await Assert.ThrowsAsync<MessengerException>(() => AttachmentService.Open(3, default));

        Assert.Equal("CMN_MSG_NOT_A_MEMBER", exception.ErrorType);
    }

    [Fact]
    public async Task LastMemberLeaves_TheFilesOfTheGroupAreRemoved()
    {
        Existing(ConversationBuilder.Group().WithOwner(Me).Build());
        Attachments.Setup(x => x.GetStoredNames(10, It.IsAny<CancellationToken>())).ReturnsAsync(["a.pdf", "b.png"]);

        await Command(() => ConversationService.Leave(10, default));

        Storage.Verify(x => x.Delete("a.pdf"), Times.Once);
        Storage.Verify(x => x.Delete("b.png"), Times.Once);
    }

    [Fact]
    public async Task Cleanup_RemovesOldUnsentUploads_AndTheirFiles()
    {
        var old = new List<MessageAttachment> { new() { Id = 1, StoredName = "a.pdf" } };
        Attachments.Setup(x => x.GetUnsentBefore(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(old);
        var cleanup = new AttachmentCleanup(Attachments.Object, Storage.Object, NullLogger<AttachmentCleanup>.Instance);

        Assert.Equal(1, await cleanup.RemoveUnsent(default));

        Attachments.Verify(x => x.GetUnsentBefore(It.Is<DateTime>(d => d < DateTime.UtcNow.AddHours(-23)), It.IsAny<int>(),
            It.IsAny<CancellationToken>()), Times.Once);
        Storage.Verify(x => x.Delete("a.pdf"), Times.Once);
    }
}
