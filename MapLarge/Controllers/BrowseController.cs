using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BrowseController : ControllerBase
{
    private readonly string rootDir = "/home/bradstricherz/Downloads"; // Adjust as needed

    [HttpGet("{*path}")]
    public IActionResult Get(string path = "")
    {
        var fullPath = Path.Combine(rootDir, path ?? "");

        if (!Directory.Exists(fullPath))
            return NotFound();

        var dirs = Directory.GetDirectories(fullPath)
            .Select(d => new FileItem { Name = Path.GetFileName(d), Type = "folder" });

        var files = Directory.GetFiles(fullPath)
            .Select(f => new FileItem
            {
                Name = Path.GetFileName(f),
                Type = "file",
                Size = new FileInfo(f).Length
            });

        var items = dirs.Concat(files);

        return Ok(items);
    }

    [HttpPost("upload/{path?}")]
    public async Task<IActionResult> UploadFile([FromRoute] string? path, IFormFile file)
    {
        Console.WriteLine($"Upload request to path: {path}");
        Console.WriteLine($"File: {file?.FileName}, Size: {file?.Length}");

        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var targetDir = Path.Combine(rootDir, path ?? "");
        Console.WriteLine($"Resolved targetDir: {targetDir}");

        if (!Directory.Exists(targetDir))
            return NotFound("Target directory does not exist.");

        var targetPath = Path.Combine(targetDir, file.FileName);
        Console.WriteLine($"Saving to: {targetPath}");

        using var stream = new FileStream(targetPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Ok(new { success = true, file = file.FileName });
    }
}