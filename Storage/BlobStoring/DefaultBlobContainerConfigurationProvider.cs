using Microsoft.Extensions.Options;

namespace Storage.BlobStoring;

public class DefaultBlobContainerConfigurationProvider : IBlobContainerConfigurationProvider
{
    protected AbpBlobStoringOptions Options { get; }

    public DefaultBlobContainerConfigurationProvider(IOptions<AbpBlobStoringOptions> options)
    {
        Options = options.Value;
    }

    public virtual BlobContainerConfiguration Get(string name)
    {
        return Options.Containers.GetConfiguration(name);
    }
}
