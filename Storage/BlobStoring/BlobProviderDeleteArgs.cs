using JetBrains.Annotations;

namespace Storage.BlobStoring;

public class BlobProviderDeleteArgs : BlobProviderArgs
{
    public BlobProviderDeleteArgs(
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
