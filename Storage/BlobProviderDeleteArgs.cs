using System.Diagnostics.CodeAnalysis;

namespace Storage;

public class BlobProviderDeleteArgs : BlobProviderArgs
{
    public BlobProviderDeleteArgs(
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
