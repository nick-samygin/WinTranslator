﻿#pragma checksum "..\..\..\..\..\Views\UserControls\ImportFromBrowserControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0EC29A0CCA74D2275BA112F566667983DF1FC8A1"
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


namespace PasswordBoss.Views.UserControls {
    
    
    /// <summary>
    /// ImportFromBrowserControl
    /// </summary>
    public partial class ImportFromBrowserControl : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\..\..\Views\UserControls\ImportFromBrowserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal QuickZip.UserControls.ConfirmationPopupWindow ImportPasswordFromBrowserSceen1;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\..\Views\UserControls\ImportFromBrowserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal QuickZip.UserControls.ConfirmationPopupWindow ImportPasswordFromBrowserSceen2;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\..\Views\UserControls\ImportFromBrowserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar syncProgressBar;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\..\Views\UserControls\ImportFromBrowserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal QuickZip.UserControls.ConfirmationPopupWindow ImportPasswordFromBrowserSceen3;
        
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
            System.Uri resourceLocater = new System.Uri("/PBAppUI;component/views/usercontrols/importfrombrowsercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\UserControls\ImportFromBrowserControl.xaml"
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
            
            #line 9 "..\..\..\..\..\Views\UserControls\ImportFromBrowserControl.xaml"
            ((PasswordBoss.Views.UserControls.ImportFromBrowserControl)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Grid_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ImportPasswordFromBrowserSceen1 = ((QuickZip.UserControls.ConfirmationPopupWindow)(target));
            return;
            case 3:
            this.ImportPasswordFromBrowserSceen2 = ((QuickZip.UserControls.ConfirmationPopupWindow)(target));
            return;
            case 4:
            this.syncProgressBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 5:
            this.ImportPasswordFromBrowserSceen3 = ((QuickZip.UserControls.ConfirmationPopupWindow)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

