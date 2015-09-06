using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

namespace DeepZoomMarked
{
    /// <summary>
    /// Mouse Wheel Helper
    /// </summary>
    public class MouseWheelHelper
    {
        private static Worker worker;
        private bool isMouseOver;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseWheelHelper"/> class.
        /// </summary>
        /// <param name="element">The source UI element.</param>
        public MouseWheelHelper(UIElement element)
        {
            if(worker == null)
                worker = new Worker();
            else
                worker.Moved -= HandleMouseWheel;

            worker.Moved += HandleMouseWheel;

            element.MouseEnter += HandleMouseEnter;
            element.MouseLeave += HandleMouseLeave;
            element.MouseMove += HandleMouseMove;
        }

        /// <summary>
        /// Occurs when [moved].
        /// </summary>
        public event EventHandler<MouseWheelEventArgs> Moved;

        /// <summary>
        /// Handles the mouse wheel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        private void HandleMouseWheel(object sender, MouseWheelEventArgs args)
        {
            if(Moved == null || !isMouseOver) return;
            Moved(this, args);
        }

        /// <summary>
        /// Handles the mouse enter.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void HandleMouseEnter(object sender, EventArgs e)
        {
            isMouseOver = true;
        }

        /// <summary>
        /// Handles the mouse leave.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void HandleMouseLeave(object sender, EventArgs e)
        {
            isMouseOver = false;
        }

        /// <summary>
        /// Handles the mouse move.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void HandleMouseMove(object sender, EventArgs e)
        {
            isMouseOver = true;
        }

        #region Nested type: Worker

        /// <summary>
        /// Worker (handles java-script events for mouse wheel)
        /// </summary>
        private class Worker
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Worker"/> class.
            /// </summary>
            public Worker()
            {
                if(!HtmlPage.IsEnabled) return;
                if(HtmlPage.BrowserInformation.UserAgent.Contains("Chrome"))
                    HtmlPage.Window.AttachEvent("onmousewheel", HandleMouseWheel);
                else if(HtmlPage.BrowserInformation.UserAgent.Contains("Firefox"))
                    HtmlPage.Window.AttachEvent("DOMMouseScroll", HandleMouseWheel);
                else
                    HtmlPage.Document.AttachEvent("onmousewheel", HandleMouseWheel);
            }

            /// <summary>
            /// Occurs when [moved].
            /// </summary>
            public event EventHandler<MouseWheelEventArgs> Moved;

            /// <summary>
            /// Handles the mouse wheel.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="args">The <see cref="System.Windows.Browser.HtmlEventArgs"/> instance containing the event data.</param>
            private void HandleMouseWheel(object sender, HtmlEventArgs args)
            {
                double delta = 0;
                ScriptObject eventObj = args.EventObject;

                if(eventObj.GetProperty("wheelDelta") != null)
                {
                    delta = ((double)eventObj.GetProperty("wheelDelta"));

                    if(HtmlPage.Window.GetProperty("opera") != null)
                        delta = -delta;

                    if(HtmlPage.BrowserInformation.UserAgent.Contains("Chrome"))
                        delta /= 140;
                    else
                        delta /= 120;
                }
                else if(eventObj.GetProperty("detail") != null)
                {
                    delta = -((double)eventObj.GetProperty("detail"));

                    if(!HtmlPage.BrowserInformation.UserAgent.Contains("Macintosh"))
                        delta /= 2;
                }

                if(delta == 0 || Moved == null) return;
                var wheelArgs = new MouseWheelEventArgs(delta);
                if(Moved != null) Moved(this, wheelArgs);

                if(wheelArgs.Handled)
                    args.PreventDefault();
            }
        }

        #endregion
    }

    /// <summary>
    /// Mouse Wheel Event Arguments
    /// </summary>
    public class MouseWheelEventArgs : EventArgs
    {
        private readonly double delta;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseWheelEventArgs"/> class.
        /// </summary>
        /// <param name="delta">The delta.</param>
        public MouseWheelEventArgs(double delta)
        {
            this.delta = delta;
        }

        /// <summary>
        /// Gets the delta.
        /// </summary>
        /// <value>The delta.</value>
        public double Delta
        {
            get { return delta; }
        }

        // Use handled to prevent the default browser behavior!
        public bool Handled { get; set; }
    }

    /// <summary>
    /// Extends UIElement class (and its' inheritors) for mouse wheel support
    /// </summary>
    public static class UIElementExtender
    {
        /// <summary>
        /// event hanlers' container
        /// </summary>
        private static readonly Dictionary<EventHandler<MouseWheelEventArgs>, MouseWheelHelper> handlers;

        /// <summary>
        /// Initializes the <see cref="UIElementExtender"/> class.
        /// </summary>
        static UIElementExtender()
        {
            handlers = new Dictionary<EventHandler<MouseWheelEventArgs>, MouseWheelHelper>();
            Application.Current.Exit += Current_Exit;
        }

        /// <summary>
        /// Handles the Exit event of the Current control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void Current_Exit(object sender, EventArgs e)
        {
            if(handlers == null || handlers.Count < 1) return;
            // detach all handlers
            foreach(var pair in handlers)
            {
                pair.Value.Moved -= pair.Key;
            }
            handlers.Clear();
        }

        /// <summary>
        /// Adds the mouse wheel handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        private static void AddMouseWheelHandler(UIElement sender, EventHandler<MouseWheelEventArgs> handler)
        {
            if(handlers == null || handlers.ContainsKey(handler)) return;
            var helper = new MouseWheelHelper(sender);
            helper.Moved += handler;
            handlers.Add(handler, helper);
        }

        /// <summary>
        /// Removes the mouse wheel handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        private static void RemoveMouseWheelHandler(EventHandler<MouseWheelEventArgs> handler)
        {
            if(handlers == null || !handlers.ContainsKey(handler)) return;
            handlers[handler].Moved -= handler;
            handlers.Remove(handler);
        }

        /// <summary>
        /// Unregisters the mouse wheel handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        public static void UnregisterMouseWheelHandler(this UIElement sender, EventHandler<MouseWheelEventArgs> handler)
        {
            RemoveMouseWheelHandler(handler);
        }

        /// <summary>
        /// Unregisters the mouse wheel handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        public static void UnregisterMouseWheelHandler(this FrameworkElement sender, EventHandler<MouseWheelEventArgs> handler)
        {
            RemoveMouseWheelHandler(handler);
        }

        /// <summary>
        /// Unregisters the mouse wheel handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        public static void UnregisterMouseWheelHandler(this Control sender, EventHandler<MouseWheelEventArgs> handler)
        {
            RemoveMouseWheelHandler(handler);
        }

        /// <summary>
        /// Unregisters the mouse wheel handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        public static void UnregisterMouseWheelHandler(this UserControl sender, EventHandler<MouseWheelEventArgs> handler)
        {
            RemoveMouseWheelHandler(handler);
        }

        /// <summary>
        /// Registers the mouse wheel handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        public static void RegisterMouseWheelHandler(this UIElement sender, EventHandler<MouseWheelEventArgs> handler)
        {
            AddMouseWheelHandler(sender, handler);
        }

        /// <summary>
        /// Registers the mouse wheel handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        public static void RegisterMouseWheelHandler(this FrameworkElement sender, EventHandler<MouseWheelEventArgs> handler)
        {
            AddMouseWheelHandler(sender, handler);
        }

        /// <summary>
        /// Registers the mouse wheel handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        public static void RegisterMouseWheelHandler(this Control sender, EventHandler<MouseWheelEventArgs> handler)
        {
            AddMouseWheelHandler(sender, handler);
        }

        /// <summary>
        /// Registers the mouse wheel handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        public static void RegisterMouseWheelHandler(this UserControl sender, EventHandler<MouseWheelEventArgs> handler)
        {
            AddMouseWheelHandler(sender, handler);
        }
    }
}