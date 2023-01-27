﻿using Storage.BlobStoring;

namespace Storage.FileSystem.FileSystem;

public class DefaultBlobFilePathCalculator : IBlobFilePathCalculator
{
    public virtual string Calculate(BlobProviderArgs args)
    {
        var fileSystemConfiguration = args.Configuration.GetFileSystemConfiguration();
        var blobPath = fileSystemConfiguration.BasePath;

        blobPath = Path.Combine(blobPath, fileSystemConfiguration.HostName);

        if (fileSystemConfiguration.AppendContainerNameToBasePath)
        {
            blobPath = Path.Combine(blobPath, args.ContainerName);
        }

        blobPath = Path.Combine(blobPath, args.BlobName);

        return blobPath;
    }
}