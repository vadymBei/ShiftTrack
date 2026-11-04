using System.Net;
using Moq;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Exceptions;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Models;
using ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeePhoto;
using ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeePhotos;
using ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.RemovePhoto;
using ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.UploadPhoto;
using ShiftTrack.Core.Unit.Tests.Fakes;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Core.Unit.Tests.Application.Organization.Employees;

public class EmployeePhotoTests
{
    private const long Me = 7;

    private static readonly byte[] Png = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 1, 2, 3, 4];
    private static readonly byte[] Jpeg = [0xFF, 0xD8, 0xFF, 0xE0, 0, 0x10];
    private static readonly byte[] Webp = "RIFF\0\0\0\0WEBPVP8 "u8.ToArray();

    private readonly Dictionary<long, string> _photoNames = new();
    private readonly Mock<IEmployeeRepository> _repository = new();
    private readonly Mock<IEmployeePhotoStorage> _storage = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly ICurrentUserService _currentUser = Mock.Of<ICurrentUserService>(x => x.Employee == new Employee { Id = Me });

    public EmployeePhotoTests()
    {
        _repository
            .Setup(x => x.GetPhotoNames(It.IsAny<IReadOnlyCollection<long>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyCollection<long> ids, CancellationToken _) =>
                _photoNames.Where(x => ids.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value));
        _storage
            .Setup(x => x.Save(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Stream _, string extension, CancellationToken _) => "new" + extension);
    }

    [Theory]
    [InlineData("png", ".png")]
    [InlineData("jpeg", ".jpg")]
    [InlineData("webp", ".webp")]
    public void Format_IsRecognisedByContent(string kind, string extension)
    {
        var bytes = kind switch { "png" => Png, "jpeg" => Jpeg, _ => Webp };

        Assert.Equal(extension, PhotoFormat.Detect(bytes)!.Extension);
    }

    [Fact]
    public void Format_OfOtherContent_IsNull()
    {
        Assert.Null(PhotoFormat.Detect("<svg xmlns="u8));
        Assert.Null(PhotoFormat.Detect([]));
    }

    [Fact]
    public async Task Upload_SavesForTheCurrentUser_AndDeletesThePreviousPhotoAfterTheCommit()
    {
        _photoNames[Me] = "old.png";

        var result = await _unitOfWork.ExecuteInTransaction(async () =>
        {
            var vm = await Upload(Jpeg, "photo.png");
            _storage.Verify(x => x.Delete(It.IsAny<string>()), Times.Never);
            return vm;
        }, CancellationToken.None);

        Assert.Equal(Me, result.EmployeeId);
        Assert.Equal("new", result.Version);
        _storage.Verify(x => x.Save(It.IsAny<Stream>(), ".jpg", It.IsAny<CancellationToken>()), Times.Once);
        _repository.Verify(x => x.UpdatePhoto(new UploadEmployeePhotoDto(Me, "new.jpg"), It.IsAny<CancellationToken>()));
        _storage.Verify(x => x.Delete("old.png"), Times.Once);
    }

    [Fact]
    public async Task Upload_OfNotAnImage_IsRejectedBeforeSaving()
    {
        var error = await Assert.ThrowsAsync<EmployeeException>(() => Upload("%PDF-1.7"u8.ToArray(), "photo.jpg"));

        Assert.Equal("ORG_EMP_PHOTO_TYPE_NOT_ALLOWED", error.ErrorType);
        _storage.Verify(x => x.Save(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Upload_OfATooLargeFile_IsRejected()
    {
        var error = await Assert.ThrowsAsync<EmployeeException>(() =>
            Upload(Png, "photo.png", length: 6 * 1024 * 1024));

        Assert.Equal("ORG_EMP_PHOTO_TOO_LARGE", error.ErrorType);
    }

    [Fact]
    public async Task Upload_WhenSavingTheEmployeeFails_DeletesTheNewFile()
    {
        _repository
            .Setup(x => x.UpdatePhoto(It.IsAny<UploadEmployeePhotoDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(() => Upload(Png, "photo.png"));

        _storage.Verify(x => x.Delete("new.png"), Times.Once);
    }

    [Fact]
    public async Task Remove_ClearsThePhoto_AndDeletesTheFile()
    {
        _photoNames[Me] = "old.png";

        await new RemovePhotoCommandHandler(_currentUser, _repository.Object, _storage.Object, _unitOfWork)
            .Handle(new RemovePhotoCommand(), CancellationToken.None);

        _repository.Verify(x => x.UpdatePhoto(new UploadEmployeePhotoDto(Me, null), It.IsAny<CancellationToken>()));
        _storage.Verify(x => x.Delete("old.png"), Times.Once);
    }

    [Fact]
    public async Task Versions_AreGivenOnlyForEmployeesWithAPhoto()
    {
        _photoNames[1] = "a1b2.jpg";
        _photoNames[3] = "c3d4.png";

        var result = await new GetEmployeePhotosQueryHandler(_repository.Object)
            .Handle(new GetEmployeePhotosQuery([3, 2, 1, 1]), CancellationToken.None);

        Assert.Equal([(1L, "a1b2"), (3L, "c3d4")], result.Select(x => (x.EmployeeId, x.Version)));
    }

    [Fact]
    public async Task Photo_OfAnEmployeeWithout_IsNotFound()
    {
        var error = await Assert.ThrowsAsync<EmployeeException>(() =>
            new GetEmployeePhotoQueryHandler(_repository.Object, _storage.Object)
                .Handle(new GetEmployeePhotoQuery(2), CancellationToken.None));

        Assert.Equal(HttpStatusCode.NotFound, error.Code);
    }

    [Fact]
    public async Task Photo_IsGivenWithItsType()
    {
        _photoNames[1] = "a1b2.webp";
        _storage.Setup(x => x.Open("a1b2.webp")).Returns(new MemoryStream(Webp));

        var photo = await new GetEmployeePhotoQueryHandler(_repository.Object, _storage.Object)
            .Handle(new GetEmployeePhotoQuery(1), CancellationToken.None);

        Assert.Equal("image/webp", photo.ContentType);
    }

    private Task<ShiftTrack.Application.Modules.Organization.Employees.ViewModels.EmployeePhotoVm> Upload(
        byte[] content, string fileName, long? length = null) =>
        new UploadPhotoCommandHandler(_currentUser, _repository.Object, _storage.Object, _unitOfWork)
            .Handle(new UploadPhotoCommand(new PhotoUpload(new MemoryStream(content), fileName, length ?? content.Length)),
                CancellationToken.None);
}
