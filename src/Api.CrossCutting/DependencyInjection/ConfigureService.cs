using Api.Data.Implamentation;
using Api.Data.Repository;
using Api.Domain.Interface;
using Api.Domain.Interface.Services.User;
using Api.Domain.Repository;
using Api.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.DependencyInjection
{
    public class ConfigureService
    {
        public static void ConfigureDependenciesService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IUserService),typeof(UserService));
            serviceCollection.AddTransient(typeof(ILoginService),typeof(LoginService));
            serviceCollection.AddTransient(typeof(IUserRepository),typeof(UserRepository));
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        }
    }
}
