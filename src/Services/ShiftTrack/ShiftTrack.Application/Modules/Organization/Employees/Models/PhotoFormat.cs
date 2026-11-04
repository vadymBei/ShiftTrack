namespace ShiftTrack.Application.Modules.Organization.Employees.Models;

/// <summary>An image format recognised by the first bytes of the file, not by its name.</summary>
public record PhotoFormat(string Extension, string ContentType)
{
    public static readonly PhotoFormat Jpeg = new(".jpg", "image/jpeg");
    public static readonly PhotoFormat Png = new(".png", "image/png");
    public static readonly PhotoFormat Webp = new(".webp", "image/webp");

    /// <summary>Null if the content is none of JPEG, PNG and WebP.</summary>
    public static PhotoFormat Detect(ReadOnlySpan<byte> head)
    {
        if (head.Length >= 3 && head[0] == 0xFF && head[1] == 0xD8 && head[2] == 0xFF)
            return Jpeg;

        if (head.Length >= 8 && head[..8].SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }))
            return Png;

        if (head.Length >= 12 && head[..4].SequenceEqual("RIFF"u8) && head[8..12].SequenceEqual("WEBP"u8))
            return Webp;

        return null;
    }

    /// <summary>By the extension of a stored file.</summary>
    public static string ContentTypeOf(string storedName) => Path.GetExtension(storedName).ToLowerInvariant() switch
    {
        ".jpg" or ".jpeg" => Jpeg.ContentType,
        ".png" => Png.ContentType,
        ".webp" => Webp.ContentType,
        _ => "application/octet-stream"
    };

    /// <summary>The stored name changes with every new photo, so it tells the browser cache apart.</summary>
    public static string VersionOf(string storedName) => Path.GetFileNameWithoutExtension(storedName);
}
