﻿#pragma checksum "C:\Users\luxveritatis\Documents\dev\DeepZoomMarked\NavigationPanel.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F5E092C252AD6BEAC4AEFCF5805D0074"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace DeepZoomMarked {
    
    
    public partial class NavigationPanel : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid pagesPanelView;
        
        internal System.Windows.Controls.TextBlock pagesPanelHider;
        
        internal System.Windows.Controls.StackPanel pagesPanel;
        
        internal System.Windows.Controls.StackPanel marksPanel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/DeepZoomMarked;component/NavigationPanel.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.pagesPanelView = ((System.Windows.Controls.Grid)(this.FindName("pagesPanelView")));
            this.pagesPanelHider = ((System.Windows.Controls.TextBlock)(this.FindName("pagesPanelHider")));
            this.pagesPanel = ((System.Windows.Controls.StackPanel)(this.FindName("pagesPanel")));
            this.marksPanel = ((System.Windows.Controls.StackPanel)(this.FindName("marksPanel")));
        }
    }
}
