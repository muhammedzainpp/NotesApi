using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Notes.Application.DependencyInjection;

public static class DependencyInjections
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}
