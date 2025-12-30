using GameLister.Api;
using Microsoft.AspNetCore.Mvc;

namespace GameLister.Api.Controllers;



[ApiController]
[Route("api/uploads")]
public class UploadsController : ControllerBase
{
    private readonly StoragePaths _paths;

    public UploadsController(StoragePaths paths)
    {
        _paths = paths;
    }

    // POST /api/uploads/images
    // multipart/form-data: file=<file>
    [HttpPost("images")]
    [RequestSizeLimit(50_000_000)] // 50MB na start
    public async Task<ActionResult<StoredFileDto>> UploadImage(
        IFormFile file,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "File is required." });

        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(ext))
            ext = ".jpg";

        // Bezpieczna nazwa
        var fileName = $"{Guid.NewGuid():N}{ext}".ToLowerInvariant();
        var fullPath = Path.Combine(_paths.ImagesRoot, fileName);

        await using (var stream = System.IO.File.Create(fullPath))
        {
            await file.CopyToAsync(stream, ct);
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var url = $"{baseUrl}/storage/images/{fileName}";

        return Created(url, new StoredFileDto(
            Url: url,
            FileName: fileName,
            SizeBytes: file.Length,
            ContentType: file.ContentType
        ));
    }
}

public record StoredFileDto(
    string Url,
    string FileName,
    long SizeBytes,
    string? ContentType
);
