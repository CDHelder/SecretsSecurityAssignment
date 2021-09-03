using Microsoft.Extensions.DependencyInjection;
using SecretsSecurityAssignment.Core.Data.Service;
using SecretsSecurityAssignment.Data.Service;
using SecretsSecurityAssignment.Service;
using SecretsSecurityAssignment.Service.Interfaces;

namespace SecretsSecurityAssignment.WebApi.ServiceCollectionExtentions
{
    public static class ConfigureDependecyInjections
    {
        public static IServiceCollection ConfigureAllDependecyInjections(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ISensitiveSecretService, SensitiveSecretService>();
            services.AddScoped<IStateSecretService, StateSecretService>();
            services.AddScoped<ITopSecretService, TopSecretService>();

            return services;
        }
    }
}
