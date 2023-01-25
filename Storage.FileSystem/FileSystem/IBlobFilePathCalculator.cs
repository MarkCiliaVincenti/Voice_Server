using Storage.BlobStoring;

namespace Storage.FileSystem.FileSystem;

public interface IBlobFilePathCalculator
{
    string Calculate(BlobProviderArgs args);
}
