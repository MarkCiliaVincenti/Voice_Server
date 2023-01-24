using Microsoft.Extensions.Configuration;

namespace Storage.FileSystem;

public class DefaultBlobFilePathCalculator : IBlobFilePathCalculator
{
    private readonly IConfiguration _configurationProvider;

    public DefaultBlobFilePathCalculator(IConfiguration configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }

    public virtual string Calculate(BlobProviderArgs args)
    {
        var blobPath = _configurationProvider["BlobStoring:FileSystem:BasePath"];
        blobPath = Path.Combine(blobPath, "host");

        if (args.ContainerName is not null)
        {
            blobPath = Path.Combine(blobPath, args.ContainerName);
        }

        blobPath = Path.Combine(blobPath, args.BlobName);
        return blobPath;
    }
}