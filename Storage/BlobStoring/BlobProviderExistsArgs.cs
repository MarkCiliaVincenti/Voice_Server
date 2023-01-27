namespace Storage.BlobStoring;

public class BlobProviderExistsArgs : BlobProviderArgs
{
    public BlobProviderExistsArgs(
        string containerName,
        BlobContainerConfiguration configuration,
        string blobName,
        CancellationToken cancellationToken = default)
    : base(
        containerName,
        configuration,
        blobName,
        cancellationToken)
    {
    }
}
