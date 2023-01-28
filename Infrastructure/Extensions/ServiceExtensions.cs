using Abstraction.Storage;
using Core.ExceptionHandling;
using EventBus;
using EventBus.Distributed;
using EventBus.RabbitMq.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RabbitMq;
using Shared.Utils.Logger;
using Storage.BlobStoring;
using Storage.FileSystem.FileSystem;
using Threading;
using Uow;

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

    public static void AddUnitOfWork(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(typeof(IUnitOfWorkManager),
            typeof(UnitOfWorkManager));
        builder.Services.AddSingleton(typeof(IAmbientUnitOfWork),
            typeof(AmbientUnitOfWork));
    }

    public static void AddExceptionHandler(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IExceptionNotifier), typeof(ExceptionNotifier));
    }


    public static void AddDistributedEventBus(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(typeof(IDistributedEventBus),
            typeof(RabbitMqDistributedEventBus));
        builder.Services.AddSingleton(typeof(IConnectionPool),
            typeof(ConnectionPool));
        builder.Services.AddSingleton(typeof(RabbitMqDistributedEventBus));
        builder.Services.AddTransient(typeof(IRabbitMqSerializer),
            typeof(Utf8JsonRabbitMqSerializer));
        builder.Services.AddSingleton(typeof(IRabbitMqMessageConsumerFactory),
            typeof(RabbitMqMessageConsumerFactory));
        builder.Services.AddSingleton(typeof(IEventHandlerInvoker),
            typeof(EventHandlerInvoker));


        builder.Services.AddTransient(typeof(IRabbitMqMessageConsumer),
            typeof(RabbitMqMessageConsumer));

        builder.Services.AddTransient(typeof(RabbitMqMessageConsumer));
        builder.Services.AddTransient(typeof(EqnAsyncTimer));


        builder.Services.Configure<EqnRabbitMqOptions>(options =>
        {
            options.Connections.Default.UserName = "guest";
            options.Connections.Default.Password = "guest";
            options.Connections.Default.HostName = "localhost";
            options.Connections.Default.Port = 5672;
        });
        builder.Services.Configure<EqnRabbitMqEventBusOptions>(builder.Configuration.GetSection("RabbitMQ:EventBus"));
        var rabbitMqDistributedEventBus = builder.Services.BuildServiceProvider()
            .GetRequiredService<RabbitMqDistributedEventBus>();

        rabbitMqDistributedEventBus.Initialize();
    }
}