using Abstraction.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Utils.Logger;
using Storage.BlobStoring;
using Storage.FileSystem.FileSystem;

namespace Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddSignalr(this WebApplicationBuilder services)
        => services.Services.AddSignalR(opt => { opt.HandshakeTimeout = TimeSpan.FromMinutes(5); });

    public static void AddCustomSerilog(this WebApplicationBuilder builder)
        => builder.Host.CreateAndUseLogger(builder.Configuration);

    public static IServiceCollection AddSingletons<TService>(
        this IServiceCollection services,
        TService implementationInstance)
        where TService : class
    {
        return services.AddSingleton(typeof(TService), implementationInstance);
    }

    public static void AddBlobStorage(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IBlobContainer<>), typeof(BlobContainer<>));
        builder.Services.AddTransient(typeof(IBlobContainerConfigurationProvider),
            typeof(DefaultBlobContainerConfigurationProvider));
        builder.Services.AddTransient(typeof(IBlobProviderSelector), typeof(DefaultBlobProviderSelector));
        builder.Services.AddTransient(typeof(IBlobNormalizeNamingService), typeof(BlobNormalizeNamingService));
        builder.Services.AddTransient<IBlobContainerFactory, BlobContainerFactory>();
        builder.Services.AddTransient(typeof(IBlobFilePathCalculator), typeof(DefaultBlobFilePathCalculator));
        builder.Services.AddTransient(typeof(IBlobProvider), typeof(FileSystemBlobProvider));
        builder.Services.Configure<BlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = builder.Configuration["BlobStorage:BasePath"];
                    fileSystem.AppendContainerNameToBasePath = true;
                    fileSystem.HostName = builder.Configuration["BlobStorage:RootName"];
                });
            });
        });
    }
}