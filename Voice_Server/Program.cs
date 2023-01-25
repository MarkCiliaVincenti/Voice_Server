using Abstraction.Notification;
using Abstraction.Storage;
using Application.Hubs;
using Application.Notifications;
using Infrastructure.Extensions;
using Serilog;
using Storage.BlobStoring;
using static System.DateTime;

namespace Voice_Server;

public partial class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.AddSignalr();
        builder.Services.AddSwaggerGen();
        builder.AddCustomSerilog();
        //add cors

        //add service IBlobContainer<> and BlobContainer<> for dependency injection
        builder.Services.AddTransient(typeof(IBlobContainer<>),typeof(BlobContainer<>));
        builder.Services.AddTransient(typeof(IBlobContainerConfigurationProvider),typeof(DefaultBlobContainerConfigurationProvider));
        builder.Services.AddTransient(typeof(IBlobProviderSelector),typeof(DefaultBlobProviderSelector));
        builder.Services.AddTransient(typeof(IBlobNormalizeNamingService),typeof(BlobNormalizeNamingService));
        builder.Services.AddTransient<IBlobContainerFactory,BlobContainerFactory>();

        builder.Services.AddTransient<IRealTimeNotifier, SignalRRealTimeNotifier>();

        builder.Services.AddCors(options =>
        {
            //add policy
            options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.WithOrigins("http://localhost:5244").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
        });
        //builder.Services.AddHostedService<Socket_Test_Background>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Voice_Server v1"));
        }

        app.UseCors("CorsPolicy");
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<OnlineClientHubBase>("/voiceHub");
            endpoints.MapControllers();
        });
        try
        {
            app.Logger.LogInformation("Starting app {}", Now);
            await app.RunAsync();
        }
        catch (Exception e)
        {
            app.Logger.LogCritical("App crashed", e);
        }
        finally
        {
            app.Logger.LogError("Stopping app");
            await Log.CloseAndFlushAsync();
        }
    }
}