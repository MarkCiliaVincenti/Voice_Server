using Abstraction.MediaService;
using Microsoft.AspNetCore.Http;

namespace Application.Media;

public class UploadService : IUploadService
{
    /// <inheritdoc />
    public async Task UploadAsync(IFormFile file)
    {
        try
        {
            var folderName = Path.Combine("Resources", "Images", DateTime.Now.Year.ToString(),
                DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
            Directory.CreateDirectory(folderName);
            var fileName = Path.Combine(folderName,
                DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName));
            await using var stream = new FileStream(fileName, FileMode.Create);
            await file.CopyToAsync(stream);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}