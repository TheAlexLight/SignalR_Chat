﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using SharedItems.Enums;

namespace ChatClient.Commands.CustomViewsCommands
{
    public class ChangeUserSettingsCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public ChangeUserSettingsCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter is ChangeSettingsType settingsType)
            {
                _viewModel.UserSettingsType = settingsType;

                if (settingsType == ChangeSettingsType.None)
                {
                    _viewModel.NeedToClearPassword = true;

                    _viewModel.UserCredentials.UsernameSettings = string.Empty;

                    _viewModel.UserCredentials.EmailSettings = string.Empty;

                    _viewModel.UserCredentials.Password = string.Empty;
                    _viewModel.UserCredentials.PasswordConfirm = string.Empty;
                }
            }
        }
    }
}
