﻿using ChatClient.Factories.ViewModelFactories;
using ChatClient.Interfaces;
using ChatClient.Services;
using ChatClient.Stores;
using ChatClient.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using SharedItems.Models;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();

            Window window = serviceProvider.GetRequiredService<WindowConfigurationService>().SetWindowStartupData(
                     top: 80,
                     left: 425,
                     width: 385,
                     height: 385);

            window.Show();
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<ISignalRChatService, SignalRChatService>();
            services.AddSingleton<HubConnectionBuilder>();
            services.AddSingleton<WindowConfigurationService>();

            services.AddSingleton<IViewModelAbstractFactory, ViewModelFactory>();
            services.AddSingleton<IViewModelConcreteFactory<LoginViewModel>, LoginViewModelFactory>();
            services.AddSingleton<IViewModelConcreteFactory<RegistrationViewModel>, RegistrationViewModelFactory>();
            services.AddSingleton<IViewModelConcreteFactory<ChatViewModel>, ChatViewModelFactory>();

            services.AddScoped<INavigator, NavigationStore>();
            services.AddScoped<MainViewModel>();
            services.AddScoped<ChatViewModelBase, LoginViewModel>();

            services.AddScoped<Window>(s=> new MainWindow(s.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }
    }
}
