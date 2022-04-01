using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RedisDemo.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallerServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installer = typeof(Startup).Assembly.ExportedTypes.Where(x => typeof(IInstaller).IsAssignableFrom(x)
                                                                              && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installer.ForEach(i => i.InstallServices(services, configuration));
        }
    }
}