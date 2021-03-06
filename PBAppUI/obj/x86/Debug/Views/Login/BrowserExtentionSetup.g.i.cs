﻿#pragma checksum "..\..\..\..\..\Views\Login\BrowserExtentionSetup.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8CF94D7BDBF8E5A089634D9F9A6133F0A6B8C4A4"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using PasswordBoss.Helpers;
using PasswordBoss.ViewModel.Account;
using PasswordBoss.Views.UserControls;
using QuickZip.UserControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
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


namespace PasswordBoss.Views.Login {
    
    
    /// <summary>
    /// BrowserExtentionSetup
    /// </summary>
    public partial class BrowserExtentionSetup : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\..\..\Views\Login\BrowserExtentionSetup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal QuickZip.UserControls.OnboardingHeader OnboardingHeader;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\..\Views\Login\BrowserExtentionSetup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Body;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\..\Views\Login\BrowserExtentionSetup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel LeftSide;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\..\Views\Login\BrowserExtentionSetup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image BrowserScreenShotImage;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\..\Views\Login\BrowserExtentionSetup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RightSide;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PBAppUI;component/views/login/browserextentionsetup.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\Login\BrowserExtentionSetup.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.OnboardingHeader = ((QuickZip.UserControls.OnboardingHeader)(target));
            return;
            case 2:
            this.Body = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.LeftSide = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.BrowserScreenShotImage = ((System.Windows.Controls.Image)(target));
            return;
            case 5:
            this.RightSide = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

