﻿#pragma checksum "C:\work iis\DeepZoomMarked\Mark - Copy.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "486017C9508C4B11E1E67EEF1A6D0C4A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
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
    
    
    public partial class Mark : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid markCircle;
        
        internal System.Windows.Shapes.Ellipse markCircleFill;
        
        internal System.Windows.Shapes.Ellipse markCircleBorder;
        
        internal System.Windows.Controls.Grid markRectangle;
        
        internal System.Windows.Shapes.Rectangle markRectangleFill;
        
        internal System.Windows.Shapes.Rectangle markRectangleBorder;
        
        internal System.Windows.Controls.Button EditButton;
        
        internal System.Windows.Controls.HyperlinkButton nameLink;
        
        internal System.Windows.Controls.StackPanel HidedContent;
        
        internal System.Windows.Controls.Grid positionChanger;
        
        internal System.Windows.Controls.StackPanel linkList;
        
        internal System.Windows.Controls.ListBox subLinkList;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/DeepZoomMarked;component/Mark%20-%20Copy.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.markCircle = ((System.Windows.Controls.Grid)(this.FindName("markCircle")));
            this.markCircleFill = ((System.Windows.Shapes.Ellipse)(this.FindName("markCircleFill")));
            this.markCircleBorder = ((System.Windows.Shapes.Ellipse)(this.FindName("markCircleBorder")));
            this.markRectangle = ((System.Windows.Controls.Grid)(this.FindName("markRectangle")));
            this.markRectangleFill = ((System.Windows.Shapes.Rectangle)(this.FindName("markRectangleFill")));
            this.markRectangleBorder = ((System.Windows.Shapes.Rectangle)(this.FindName("markRectangleBorder")));
            this.EditButton = ((System.Windows.Controls.Button)(this.FindName("EditButton")));
            this.nameLink = ((System.Windows.Controls.HyperlinkButton)(this.FindName("nameLink")));
            this.HidedContent = ((System.Windows.Controls.StackPanel)(this.FindName("HidedContent")));
            this.positionChanger = ((System.Windows.Controls.Grid)(this.FindName("positionChanger")));
            this.linkList = ((System.Windows.Controls.StackPanel)(this.FindName("linkList")));
            this.subLinkList = ((System.Windows.Controls.ListBox)(this.FindName("subLinkList")));
        }
    }
}

