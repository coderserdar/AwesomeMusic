﻿#pragma checksum "D:\KodServer\VisualStudioProjeleri\AwesomeMusic\AwesomeMusic\AddCategoryPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AB14935C561FE9A55BC1EC93122F78E1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace AwesomeMusic {
    
    
    public partial class AddCategoryPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock lblArtistName;
        
        internal System.Windows.Controls.TextBlock lblCategories;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ListBox lstCategories;
        
        internal System.Windows.Controls.ScrollViewer svCategories;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Awesome%20Music;component/AddCategoryPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.lblArtistName = ((System.Windows.Controls.TextBlock)(this.FindName("lblArtistName")));
            this.lblCategories = ((System.Windows.Controls.TextBlock)(this.FindName("lblCategories")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.lstCategories = ((System.Windows.Controls.ListBox)(this.FindName("lstCategories")));
            this.svCategories = ((System.Windows.Controls.ScrollViewer)(this.FindName("svCategories")));
        }
    }
}

