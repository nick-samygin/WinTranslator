﻿#pragma checksum "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E6893001ECADBD4B1D0573483C470A8E44C400E7"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

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
    /// AboutControlDialog
    /// </summary>
    public partial class AboutControlDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid mainGrid;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Run runVersion;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Run runLicense;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock supportCenter;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock privacyPolicy;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock termsAndConditions;
        
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
            System.Uri resourceLocater = new System.Uri("/PBAppUI;component/views/usercontrols/aboutcontroldialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
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
            this.mainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            
            #line 36 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
            ((System.Windows.Controls.Image)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Image_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.runVersion = ((System.Windows.Documents.Run)(target));
            return;
            case 4:
            this.runLicense = ((System.Windows.Documents.Run)(target));
            return;
            case 5:
            this.supportCenter = ((System.Windows.Controls.TextBlock)(target));
            
            #line 51 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
            this.supportCenter.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.supportCenter_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.privacyPolicy = ((System.Windows.Controls.TextBlock)(target));
            
            #line 52 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
            this.privacyPolicy.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.privacyPolicy_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.termsAndConditions = ((System.Windows.Controls.TextBlock)(target));
            
            #line 53 "..\..\..\..\..\Views\UserControls\AboutControlDialog.xaml"
            this.termsAndConditions.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.termsAndConditions_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

