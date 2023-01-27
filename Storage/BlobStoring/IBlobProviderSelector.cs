namespace Storage.BlobStoring;

public interface IBlobProviderSelector
{
    IBlobProvider Get(string containerName);
}
