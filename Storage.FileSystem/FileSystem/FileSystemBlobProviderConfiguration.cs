using Core;
using Storage.BlobStoring;

namespace Storage.FileSystem.FileSystem;

public class FileSystemBlobProviderConfiguration
{
    public string BasePath
    {
        get => _containerConfiguration.GetConfiguration<string>(FileSystemBlobProviderConfigurationNames.BasePath);
        set => _containerConfiguration.SetConfiguration(FileSystemBlobProviderConfigurationNames.BasePath,
            Check.NotNullOrWhiteSpace(value, nameof(value)));
    }

    /// <summary>
    /// Default value: true.
    /// </summary>
    public bool AppendContainerNameToBasePath
    {
        get => _containerConfiguration.GetConfigurationOrDefault(
            FileSystemBlobProviderConfigurationNames.AppendContainerNameToBasePath, true);
        set => _containerConfiguration.SetConfiguration(
            FileSystemBlobProviderConfigurationNames.AppendContainerNameToBasePath, value);
    }

    public string HostName
    {
        get => _containerConfiguration.GetConfiguration<string>(FileSystemBlobProviderConfigurationNames.HostName);
        set => _containerConfiguration.SetConfiguration(FileSystemBlobProviderConfigurationNames.HostName,
            Check.NotNullOrWhiteSpace(value, nameof(value)));
    }

    private readonly BlobContainerConfiguration _containerConfiguration;

    public FileSystemBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
    {
        _containerConfiguration = containerConfiguration;
    }
}