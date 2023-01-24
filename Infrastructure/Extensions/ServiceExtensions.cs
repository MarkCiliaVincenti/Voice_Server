using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Utils.Logger;

namespace Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddSignalr(this WebApplicationBuilder services)
        => services.Services.AddSignalR(opt =>
        {
            opt.HandshakeTimeout = TimeSpan.FromMinutes(5);
        });

    public static void AddCustomSerilog(this WebApplicationBuilder builder)
        => builder.Host.CreateAndUseLogger(builder.Configuration);
}