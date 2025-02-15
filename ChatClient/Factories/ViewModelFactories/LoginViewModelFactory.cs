﻿using ChatClient.Interfaces.Factories;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using ChatClient.Services.BaseConfiguration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Factories.ViewModelFactories
{
    public class LoginViewModelFactory : IViewModelConcreteFactory<LoginViewModel>
    {
        private readonly ChatBaseConfiguration _baseConfiguration;

        public LoginViewModelFactory(ChatBaseConfiguration baseConfiguration)
        {
            _baseConfiguration = baseConfiguration;
        }

        public LoginViewModel CreateViewModel()
        {
            return new LoginViewModel(_baseConfiguration);
        }
    }
}
