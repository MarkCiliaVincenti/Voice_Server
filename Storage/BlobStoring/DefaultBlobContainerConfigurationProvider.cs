using Microsoft.Extensions.Options;

namespace Storage.BlobStoring;

public class DefaultBlobContainerConfigurationProvider : IBlobContainerConfigurationProvider
{
    protected BlobStoringOptions Options { get; }

    public DefaultBlobContainerConfigurationProvider(IOptions<BlobStoringOptions> options)
    {
        Options = options.Value;
    }

    public virtual BlobContainerConfiguration Get(string name)
    {
        return Options.Containers.GetConfiguration(name);
    }
}
