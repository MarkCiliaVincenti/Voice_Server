using System.Diagnostics.CodeAnalysis;

namespace Storage;

public class BlobProviderExistsArgs : BlobProviderArgs
{
    public BlobProviderExistsArgs(
        [NotNull] string containerName,
        [NotNull] string blobName,
        CancellationToken cancellationToken = default)
        : base(
            containerName,
            blobName,
            cancellationToken)
    {
    }
}