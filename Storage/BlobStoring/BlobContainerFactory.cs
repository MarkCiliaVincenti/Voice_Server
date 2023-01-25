﻿namespace Storage.BlobStoring;

public class BlobContainerFactory : IBlobContainerFactory
{
    protected IBlobProviderSelector ProviderSelector { get; }

    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

   

    protected CancellationToken CancellationTokenProvider { get; }

    protected IServiceProvider ServiceProvider { get; }

    protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }

    public BlobContainerFactory(
        IBlobContainerConfigurationProvider configurationProvider,
      
        CancellationToken cancellationTokenProvider,
        IBlobProviderSelector providerSelector,
        IServiceProvider serviceProvider,
        IBlobNormalizeNamingService blobNormalizeNamingService)
    {
        ConfigurationProvider = configurationProvider;
      
        CancellationTokenProvider = cancellationTokenProvider;
        ProviderSelector = providerSelector;
        ServiceProvider = serviceProvider;
        BlobNormalizeNamingService = blobNormalizeNamingService;
    }

    public virtual IBlobContainer Create(string name)
    {
        var configuration = ConfigurationProvider.Get(name);

        return new BlobContainer(
            name,
            configuration,
            ProviderSelector.Get(name),
            CancellationTokenProvider,
            BlobNormalizeNamingService,
            ServiceProvider
        );
    }
}
