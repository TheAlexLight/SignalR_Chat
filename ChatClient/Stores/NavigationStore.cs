﻿using ChatClient.Interfaces;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Stores
{
    public class NavigationStore : INavigator
    {
        public event Action CurrentViewModelChanged;

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel { 
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
