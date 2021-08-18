﻿#pragma checksum "..\..\..\..\Views\RegistrationView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "FA7D67C76B1649109FB25853F78628787E5F7C4A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ChatClient.Validators;
using ChatClient.ViewModels;
using ChatClient.Views;
using ChatClient.Views.AdditionalViews;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ChatClient.Views {
    
    
    /// <summary>
    /// RegistrationView
    /// </summary>
    public partial class RegistrationView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\Views\RegistrationView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border fullRegistrationBorder;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\Views\RegistrationView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtUsername;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\..\Views\RegistrationView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtEmail;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\..\Views\RegistrationView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ChatClient.Views.AdditionalViews.BindablePasswordBox pwBoxPassword;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\Views\RegistrationView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ChatClient.Views.AdditionalViews.BindablePasswordBox pwBoxPasswordConfirm;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\..\Views\RegistrationView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRegistration;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.9.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ChatClient;component/views/registrationview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\RegistrationView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.9.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.9.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 12 "..\..\..\..\Views\RegistrationView.xaml"
            ((ChatClient.Views.RegistrationView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.registrationField_TextOrPasswordChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.fullRegistrationBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.txtUsername = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\..\..\Views\RegistrationView.xaml"
            this.txtUsername.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.registrationField_TextOrPasswordChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtEmail = ((System.Windows.Controls.TextBox)(target));
            
            #line 59 "..\..\..\..\Views\RegistrationView.xaml"
            this.txtEmail.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.registrationField_TextOrPasswordChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.pwBoxPassword = ((ChatClient.Views.AdditionalViews.BindablePasswordBox)(target));
            
            #line 80 "..\..\..\..\Views\RegistrationView.xaml"
            this.pwBoxPassword.AddHandler(System.Windows.Controls.PasswordBox.PasswordChangedEvent, new System.Windows.RoutedEventHandler(this.registrationField_TextOrPasswordChanged));
            
            #line default
            #line hidden
            return;
            case 6:
            this.pwBoxPasswordConfirm = ((ChatClient.Views.AdditionalViews.BindablePasswordBox)(target));
            
            #line 102 "..\..\..\..\Views\RegistrationView.xaml"
            this.pwBoxPasswordConfirm.AddHandler(System.Windows.Controls.PasswordBox.PasswordChangedEvent, new System.Windows.RoutedEventHandler(this.registrationField_TextOrPasswordChanged));
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnRegistration = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

