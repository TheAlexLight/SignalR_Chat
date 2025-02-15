﻿using ChatClient.Commands.NavigatonCommands;
using ChatClient.MVVM.ViewModels.BaseViewModels;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using ChatClient.Services.ConcreteConfiguration;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using SharedItems.Models.StatusModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.Commands.AuthenticationCommands
{
    public class ReconnectionCommand : CommandBase
    {
        private readonly ChatViewModelBase _viewModel;
        private readonly UserModel _currentUser;
        private readonly ICommand _navigationCommand;

        public ReconnectionCommand(ChatViewModelBase viewModel, UserModel currentUser)
        {
            _viewModel = viewModel;
            _currentUser = currentUser;

            _navigationCommand = new NavigateCommand<ChatViewModel>(_viewModel.BaseConfiguration, _viewModel.BaseConfiguration);
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
                string username = _currentUser == null && _viewModel is ChatViewModel chatViewModel
                ? chatViewModel.CurrentUser.UserProfile.Username
                : _currentUser.UserProfile.Username;

                await _viewModel.BaseConfiguration.ChatService.AuthorizationModel.Reconnect(username);

                if (parameter is BanStatusModel banStatus)
                {
                    await _viewModel.BaseConfiguration.ChatService.AdminActionModel.Ban(username, banStatus);
                }

                _navigationCommand.Execute(null);
            }

            _viewModel.IsLoading = false;
            RaiseCanExecuteChanged();
        }
    }
}
