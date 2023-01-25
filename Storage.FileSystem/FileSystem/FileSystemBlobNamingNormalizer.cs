using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Storage.BlobStoring;

namespace Storage.FileSystem.FileSystem;

public class FileSystemBlobNamingNormalizer : IBlobNamingNormalizer
{
    public FileSystemBlobNamingNormalizer()
    {
    }

    public virtual string NormalizeContainerName(string containerName)
    {
        return Normalize(containerName);
    }

    public virtual string NormalizeBlobName(string blobName)
    {
        return Normalize(blobName);
    }

    protected virtual string Normalize(string fileName)
    {
        fileName = Regex.Replace(fileName, "[:\\*\\?\"<>\\|]", string.Empty);
        return fileName;
    }
}