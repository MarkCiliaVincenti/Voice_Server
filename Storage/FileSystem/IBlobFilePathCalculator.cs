namespace Storage.FileSystem;

public interface IBlobFilePathCalculator
{
    string Calculate(BlobProviderArgs args);
}