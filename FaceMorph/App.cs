using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FaceMorph
{
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<IFaceMorphApi>(x => new FaceMorphApi("https://api.facemorph.me"));
            services.AddTransient<IFaceMorpher, FaceMorpher>();            
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow!.Show();
        }

    }
}
