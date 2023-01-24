using System.Diagnostics.CodeAnalysis;

namespace Storage;

public class BlobProviderSaveArgs : BlobProviderArgs
{
    [NotNull]
    public Stream BlobStream { get; }

    public bool OverrideExisting { get; }

    public BlobProviderSaveArgs(
        [NotNull] string containerName,
        [NotNull] string blobName,
        [NotNull] Stream blobStream,
        bool overrideExisting = false,
        CancellationToken cancellationToken = default)
        : base(
            containerName,
            blobName,
            cancellationToken)
    {
        BlobStream = blobStream;
        OverrideExisting = overrideExisting;
    }
}
