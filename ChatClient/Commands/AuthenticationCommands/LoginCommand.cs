﻿using ChatClient.Interfaces.SignalRTransmitting;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using ChatClient.Services;
using ChatClient.Services.ConcreteConfiguration;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using SharedItems.Models.AuthenticationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Commands.AuthenticationCommands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _viewModel;

        public LoginCommand( LoginViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return !_viewModel.IsLoading;
        }

        public override async void Execute(object parameter)
        {
            _viewModel.IsLoading = true;
            RaiseCanExecuteChanged();

            if (await _viewModel.ConnectToServer(_viewModel) != HubConnectionState.Disconnected)
            {
                UserLoginModel loginData = new()
                {
                    Username = _viewModel.UserCredentials.Username,
                    Password = _viewModel.UserCredentials.Password
                };

                await _viewModel.BaseConfiguration.ChatService.AuthorizationModel.Login(loginData);
            }
        }
    }
}
