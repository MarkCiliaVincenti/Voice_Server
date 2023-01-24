using System.Diagnostics.CodeAnalysis;

namespace Storage;

public class BlobProviderGetArgs : BlobProviderArgs
{
    public BlobProviderGetArgs(
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
