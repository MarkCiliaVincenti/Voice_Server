using JetBrains.Annotations;

namespace Storage.BlobStoring;

public interface IBlobProviderSelector
{
    [NotNull]
    IBlobProvider Get([NotNull] string containerName);
}
