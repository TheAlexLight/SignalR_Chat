﻿using ChatClient.Services;
using ChatClient.ViewModels;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.Commands
{
    public class SendChatCommand : ICommand
    {
        private readonly ChatViewModel _viewModel;
        private readonly SignalRChatService _chatService;

        public SendChatCommand(ChatViewModel viewModel, SignalRChatService chatService)
        {
            _viewModel = viewModel;
            _chatService = chatService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !UserStatusService.IsBanned;
        }

        public async void Execute(object parameter)
        {
            try
            {
                MessageModel model = new MessageModel()
                {
                    Message = _viewModel.Message,
                    Time = DateTime.Now,
                    UserInfo = _viewModel.CurrentUser
                };

                await _chatService.SendMessage(model);

                _viewModel.ErrorMessage = string.Empty;
                _viewModel.Message = "";
            }
            catch (Exception)
            {
                _viewModel.ErrorMessage = "Unable to send message.";
                _viewModel.Message = "";
            }
        }
    }
}
