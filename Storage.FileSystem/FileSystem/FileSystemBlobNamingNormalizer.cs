using System.Text.RegularExpressions;
using Storage.BlobStoring;


namespace Storage.FileSystem.FileSystem
{
    public class FileSystemBlobNamingNormalizer : IBlobNamingNormalizer
    {
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
            // A filename cannot contain any of the following characters: \ / : * ? " < > |
            // In order to support the directory included in the blob name, remove / and \
            fileName = Regex.Replace(fileName, "[:\\*\\?\"<>\\|]", string.Empty);

            return fileName;
        }
    }
}