﻿using ChatClient.Services;
using ChatClient.Stores;
using ChatClient.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
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
            HubConnection connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/chat")
                    .Build();

            NavigationStore navigationStore = new();
            SignalRChatService chatService = new(connection);


            LoginConnectionService connectionService = new(navigationStore, chatService);

            navigationStore.CurrentViewModel = connectionService.CreateConnectedViewModel(chatService);

            MainWindow window = new()
            {
                DataContext = new MainViewModel(navigationStore)
            };

            window.Show();
        }
    }
}
