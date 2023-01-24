using Microsoft.AspNetCore.Http;

namespace Abstraction.MediaService;

public interface IUploadService
{
    Task UploadAsync(IFormFile file);
}