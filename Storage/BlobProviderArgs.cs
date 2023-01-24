using System.Diagnostics.CodeAnalysis;

namespace Storage;

public abstract class BlobProviderArgs
{
    [NotNull] public string ContainerName { get; }

    [NotNull] public string BlobName { get; }

    public CancellationToken CancellationToken { get; }

    protected BlobProviderArgs(
        [NotNull] string containerName,
        [NotNull] string blobName,
        CancellationToken cancellationToken = default)
    {
        ContainerName = containerName;
        BlobName = blobName;
        CancellationToken = cancellationToken;
    }
}