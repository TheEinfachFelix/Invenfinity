using Backend.Application.UseCases;
using InvenfinityApp.ViewModel;
using InvenfinityApp.ViewModel.Grid;
using InvenfinityApp.ViewModel.Part;
using InvenfinityApp.ViewModel.Tree;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace InvenfinityApp
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Backend
            services.AddSingleton<UcRoot>();

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<GridViewModel>();
            services.AddTransient<LocationTreeViewModel>();
            services.AddTransient<LocationEditViewModel>();
            services.AddTransient<CreateLocationViewModel>();
            services.AddTransient<CreateGridViewModel>();
            services.AddTransient<CreateBinTypeViewModel>();
            services.AddTransient<CreateBinViewModel>();
            services.AddTransient<CreateLocationViewModel>();
            services.AddTransient<CreatePartViewModel>();
            services.AddTransient<EditBinViewModel>();


            // Views
            services.AddSingleton<MainWindow>();
        }
    }
}
