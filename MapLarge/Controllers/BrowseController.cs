// This controller handles directory browsing, file uploads, and deletions.
// It returns JSON to a single-page frontend served from wwwroot/index.html.

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BrowseController : ControllerBase
{
    private readonly string rootDir;

    public BrowseController(IConfiguration config)
    {
        rootDir = config["HOME_DIR"] ?? "/home/bradstricherz/Downloads";
    }


    // GET /api/browse/{*path} - Returns directory listing (folders and files)
    // Returns a list of files and folders in the specified path
    // If no path is provided, lists contents of the rootDir
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

    // POST /api/browse/upload/{*path} - Uploads a file to the given directory
    // Accepts a multipart file upload to the specified folder
    // Saves the file to disk under rootDir/path/
    [HttpPost("upload/{*path}")]
    public async Task<IActionResult> UploadFile([FromRoute] string? path, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var targetDir = Path.Combine(rootDir, path ?? "");

        // ✅ Ensure the directory exists
        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }

        var targetPath = Path.Combine(targetDir, file.FileName);

        using var stream = new FileStream(targetPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Ok(new { success = true, file = file.FileName });
    }


    // DELETE /api/browse/delete/{*path} - Deletes a file or folder (recursive if folder)
    // Deletes a file or directory at the given path (relative to rootDir)
    // Folders are deleted recursively (use with caution)
    [HttpDelete("delete/{*path}")]
    public IActionResult DeleteItem([FromRoute] string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return BadRequest("Path is required.");

        var fullPath = Path.Combine(rootDir, path);

        if (!System.IO.File.Exists(fullPath) && !Directory.Exists(fullPath))
            return NotFound("File or folder not found.");

        try
        {
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
            else if (Directory.Exists(fullPath))
                Directory.Delete(fullPath, recursive: true); // use caution

            return Ok(new { success = true, path });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("download/{*path}")]
    public IActionResult DownloadFile([FromRoute] string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return BadRequest("Path is required.");

        var fullPath = Path.Combine(rootDir, path);

        if (!System.IO.File.Exists(fullPath))
            return NotFound("File not found.");

        var contentType = "application/octet-stream";
        var fileName = Path.GetFileName(fullPath);
        return PhysicalFile(fullPath, contentType, fileName);
    }
}