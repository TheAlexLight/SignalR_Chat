﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient.MVVM.Views
{
    /// <summary>
    /// Interaction logic for RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : UserControl
    {
        public RegistrationView()
        {
            InitializeComponent();
        }

        private void RegistrationField_TextOrPasswordChanged(object sender, RoutedEventArgs e)
        {
            btnRegistration.IsEnabled = !Validation.GetHasError(pwBoxPassword)
                    && !Validation.GetHasError(pwBoxPasswordConfirm)
                    && !Validation.GetHasError(txtUsername)
                    && !Validation.GetHasError(txtEmail)
                    && pwBoxPassword.Password != null
                    && pwBoxPasswordConfirm.Password != null
                    && pwBoxPassword.Password.Length > 0
                    && pwBoxPasswordConfirm.Password.Length > 0
                    && txtUsername.Text.Length > 0
                    && txtEmail.Text.Length > 0;
        }
    }
}
