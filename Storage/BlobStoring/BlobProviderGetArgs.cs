using JetBrains.Annotations;

namespace Storage.BlobStoring;

public class BlobProviderGetArgs : BlobProviderArgs
{
    public BlobProviderGetArgs(
        [NotNull] string containerName,
        [NotNull] BlobContainerConfiguration configuration,
        [NotNull] string blobName,
        CancellationToken cancellationToken = default)
        : base(
            containerName,
            configuration,
            blobName,
            cancellationToken)
    {
    }
}
