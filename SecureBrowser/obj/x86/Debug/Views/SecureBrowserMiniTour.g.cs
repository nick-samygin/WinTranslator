﻿#pragma checksum "..\..\..\..\Views\SecureBrowserMiniTour.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "FF22B6FDAE830FB336D5A5E5BF814930F0708646"
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
using System.Windows.Forms.Integration;
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


namespace PasswordBoss.Views {
    
    
    /// <summary>
    /// SecureBrowserMiniTour
    /// </summary>
    public partial class SecureBrowserMiniTour : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\..\Views\SecureBrowserMiniTour.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal PasswordBoss.Views.SecureBrowserMiniTour SecureBrowserTour;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Views\SecureBrowserMiniTour.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid SearchBarFavoritesGrid;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Views\SecureBrowserMiniTour.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel SafelyStorInfoPopup;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\Views\SecureBrowserMiniTour.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSafelyStorInfoPopupNext;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\Views\SecureBrowserMiniTour.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid FavoriteSitesGrid;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\..\..\Views\SecureBrowserMiniTour.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel FavoriteSitesPopup;
        
        #line default
        #line hidden
        
        
        #line 175 "..\..\..\..\Views\SecureBrowserMiniTour.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFavoriteSitesNext;
        
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
            System.Uri resourceLocater = new System.Uri("/SecureBrowser;component/views/securebrowserminitour.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\SecureBrowserMiniTour.xaml"
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
            this.SecureBrowserTour = ((PasswordBoss.Views.SecureBrowserMiniTour)(target));
            return;
            case 2:
            this.SearchBarFavoritesGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.SafelyStorInfoPopup = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.btnSafelyStorInfoPopupNext = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.FavoriteSitesGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.FavoriteSitesPopup = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.btnFavoriteSitesNext = ((System.Windows.Controls.Button)(target));
            
            #line 176 "..\..\..\..\Views\SecureBrowserMiniTour.xaml"
            this.btnFavoriteSitesNext.Click += new System.Windows.RoutedEventHandler(this.btnSafelyStorInfoPopupNext_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

