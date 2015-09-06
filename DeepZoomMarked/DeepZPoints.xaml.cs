using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Browser;
using System.Xml.Linq;

namespace DeepZoomMarked
{
    public partial class DeepZPoints
    {
        public static TextBlock TilLoadBlock=new TextBlock(){Text="", Foreground=new SolidColorBrush(Colors.Red), FontSize=20, HorizontalAlignment=HorizontalAlignment.Center, VerticalAlignment=VerticalAlignment.Center};
        string id;
        public double ImageScale { get { return image.ViewportWidth; } }
        List<double> subImagesPosition = new List<double>();
        public MainVM Props { get; set; }
        public string RtfText;
        private Login loginPanel;
        public DeepZPoints()
        {
            Props = new MainVM();
            InitializeComponent();
                                       

            LayoutRoot.Children.Add(TilLoadBlock);
            loginPanel = new Login
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            Props.EditModeChanged += newValue =>
                                         {
                                             newMark.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;//);
                                             Mark.EditMode = newValue;
                                         };
            newMark.Visibility = Visibility.Collapsed;
            loginPanel.SetEditMode = editMode => Props.EditMode = editMode;
            loginPanel.DataContext = loginPanel.Props = Props;
            LayoutRoot.Children.Add(loginPanel);
            bool autoNavigateMarkFinish = false, autoNavigatePageFinish=false;
            image.ImageOpenSucceeded += (sender, e) =>
            {
                if (image.SubImages.Count > 0)
                {
                    double width = 0;
                    //subImagesPosition.Add(width);             .Where((sim,i)=>i!=0)
                    //width = -1; 
                    foreach (var subImage in image.SubImages)
                    {
                        width = width/subImage.AspectRatio;
                        subImage.ViewportWidth = 1/subImage.AspectRatio;
                        subImage.ViewportOrigin = new Point(width, 0);
                        subImagesPosition.Add(width);
                        width -= 1;
                        width = width*subImage.AspectRatio;
                    }

                    imageHeight = Math.Pow(image.SubImages[0].AspectRatio, -1);
                    imageWidth = subImagesPosition.Last() + image.SubImages.Last().ViewportWidth;
                }
                else
                {
                    imageHeight = Math.Pow(image.AspectRatio, -1);
                    imageWidth = image.ViewportWidth;
                }
                zoom = image.ViewportWidth;
                                                ScaleMrksPositions();
                                                Mark markToZoom;
                                                if ((HtmlPage.Document.QueryString.ContainsKey("reflectionid") &&
                                                     (markToZoom =
                                                      Mark.Marks(
                                                          ReflectionId = HtmlPage.Document.QueryString["reflectionid"])) != null) && !autoNavigateMarkFinish)
                                                {
                                                    autoNavigateMarkFinish = true;
                                                    if (markToZoom.FormView == Mark.Form.Circle)
                                                        ZoomToCircle(markToZoom);
                                                    else if (markToZoom.FormView == Mark.Form.Rect)
                                                        ZoomToRect(markToZoom);
                                                }
                                                int pageNumber;
                                                if (HtmlPage.Document.QueryString.ContainsKey("pagenumber") &&
                                                    int.TryParse(HtmlPage.Document.QueryString["pagenumber"], out pageNumber)
                                                    && pageNumber > -1 && pageNumber < image.SubImages.Count
                                                         && !autoNavigatePageFinish)
                                                       {
                                                           autoNavigatePageFinish = true;
                                                           Zoom(1.0 / zoom, new Point());
                                                           var si = image.SubImages[pageNumber];
                                                           var widthScal = 1.0 / (si.ViewportWidth);
                                                           image.ViewportOrigin =
                                                               new Point(-si.ViewportOrigin.X * widthScal, si.ViewportOrigin.Y * widthScal);
                                                       }
                                                // var widthScale = image.ViewportWidth;
                                                //  MessageBox.Show(widthScale.ToString());
                                                //  var canvas = new Point(Canvas.GetLeft(markToZoom), Canvas.GetTop(markToZoom));
                                                //image.ViewportWidth = 2*markToZoom.Props.Radius;//)/image.ActualHeight; //
                                                //image.ViewportOrigin = image.ElementToLogicalPoint(canvas);
                return;
                                                for (int i = 0; i < HtmlPage.Document.QueryString.Keys.Count; i++)
                                                {
                                                    var key = HtmlPage.Document.QueryString.Keys.ElementAt(i);
                                                    Regex r = new Regex("pagenumber([0-9]+)positionX([0-9]+)positionY([0-9]+)");
                                                    var match = r.Match(key);
                                                    if (match.Success)
                                                    {
                                                        var pageNumberFinded = int.Parse(match.Groups[1].Value);


                                                        var si = image.SubImages[pageNumberFinded];
                                                        var widthScal = 1.0 / (si.ViewportWidth);
                                                        image.ViewportOrigin = new Point(-si.ViewportOrigin.X * widthScal, si.ViewportOrigin.Y * widthScal);
                                                        var scale = marksPositions.ActualWidth*widthScal;
                                                        var x = Convert.ToDouble(int.Parse(match.Groups[2].Value)) * marksPositions.ActualWidth;
                                                        var y = Convert.ToDouble(int.Parse(match.Groups[3].Value)) * marksPositions.ActualWidth;

                                                        var ocrsEarchResult = new OCRSEarchResult(x - scale * si.ViewportOrigin.X, y + scale * si.ViewportOrigin.Y, this);
                                                        OCRSEarchResult.searchMarks.Add(ocrsEarchResult);
                                                        
                                                        marksPositions.Children.Add(ocrsEarchResult);
                                                        Canvas.SetZIndex(ocrsEarchResult, 1000000);
                                                     
                                                    }
                                                }
                                            };
            currentMouse = new Point();
            currentViewPort = new Point();
            image.MouseLeftButtonDown += OnLeftButtonDown;
            image.MouseLeftButtonUp += image_MouseLeftButtonUp;
            image.MouseLeave += image_MouseLeftButtonUp;
            image.ViewportChanged += delegate
            {
                ScaleMrksPositions();
            };
            //id = "deepz_markTest_1001";
            //if (HtmlPage.Document.QueryString.ContainsKey("id"))
            //    ImageId = HtmlPage.Document.QueryString["id"];
            //else
            //    MessageBox.Show("can't get id from request string");
            //HtmlPage.Window.Alert(HtmlPage.Document.QueryString["id"]);
            new MouseWheelHelper(this).Moved += (ender, e) =>
                                                    {
                                                        if (e == null) return;
                                                        e.Handled = true;
                                                        Zoom(e.Delta > 0 ? 1.1 : 0.9, currentMouse);
                                                        // ScaleMrksPositions();
                                                    };
            Application.Current.Host.Content.FullScreenChanged += delegate
            {
                fullscreen.Content = (Application.Current.Host.Content.IsFullScreen)
                    ? "выйти из полноэкранного режима"
                    : "полноэкранный режим";
                Dispatcher.BeginInvoke(ScaleMrksPositions);
            };
            SizeChanged += DeepZPoints_SizeChanged;
            InitSourceMarksPages(); 
        }

        private void OnLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentMouse = e.GetPosition(image);
            currentViewPort = image.ViewportOrigin;
            isMouseDown = true;
        }

        void DeepZPoints_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //	MessageBox.Show(
          //  debugInfo.Text = image.ActualWidth.ToString();//);
            ScaleMrksPositions();
        }
        double zoom, imageHeight, imageWidth;
        private void ScaleMrksPositions()
        {
            //BackBounds();
            
            var widthScal = 1.0 / ImageScale;     
            var origin = image.ViewportOrigin;
            // ImageScale = image.ViewportWidth;
            // MessageBox.Show(ImageScale.ToString());
            var scale = -image.ActualWidth * widthScal;
            transform.TranslateX = origin.X * scale;
            transform.TranslateY = origin.Y * scale;
            //transform.ScaleX = widthScal;
            //transform.ScaleY = widthScal;
            marksPositions.Width *= widthScal;
            marksPositions.Height *= widthScal;
            foreach (var mark in Mark.marks)
                mark.SetScaledPosition();
            foreach (var mark in OCRSEarchResult.searchMarks)
                mark.SetScaledPosition();
        }
        private void BackBounds()
        {
            var newPos = image.ViewportOrigin;
            bool change;
            if ((change = (newPos.Y < 0)))
                newPos.Y = 0;
            if (newPos.X < 0)
            {
                change = true;
                newPos.X = 0;
            }
            if (newPos.Y + image.ViewportWidth / image.AspectRatio > 1.0)
            {
                change = true;
                newPos.Y = Math.Max(0.0, 1.0 - image.ViewportWidth / image.AspectRatio);
            }
            //if (newPos.X + image.ViewportWidth > imageWidth)
            //{
            //    change = true;
            //    newPos.Y = Math.Max(0.0, imageWidth - image.ViewportWidth);
            //}
            if (change)
                image.ViewportOrigin = newPos;
            if (zoom < imageHeight)
                Zoom(1.1, currentMouse);
        }
       // Dictionary<string, string> images = new Dictionary<string, string>();
        public string  ImageId
        {
            get { try { return id ?? (id = HtmlPage.Document.QueryString["id"]); } catch { return null; } }
            set
            {
                id = value;
            }
        }
        public string UserId
        {
            get { return Props.UserId; }
            set { Props.UserId = value; }
        }
        //void LoadAll()
        //{
        //    new Uri("ImagesByDate.aspx", UriKind.Relative).RequestString(all =>
        //    {
        //        foreach (var pair in all.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
        //            .Select(pair => pair.Split('='))
        //            .Where(pair => !images.ContainsKey(pair[0])))
        //            images.Add(pair[0], pair[1]);
        //        if (images.Count == 0)
        //            MessageBox.Show("There is no one image");
        //        //id  =   images.First().Value;
        //        //DataServise(id);
        //    });
        //}
        //// Executes when the user navigates to this page.
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //}

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
                Move(e.GetPosition(image));
            else
                currentMouse = e.GetPosition(image);
        }

        private void Move(Point centr)
        {
            var widthScal = image.ViewportWidth / image.ActualWidth;
            Point newPos=new Point(currentViewPort.X + widthScal * (currentMouse.X - centr.X), currentViewPort.Y + widthScal * (currentMouse.Y - centr.Y));
            //if ((currentMouse.Y - centr.Y) < 0 && newPos.Y < 0)
            //    newPos.Y = 0;
            //else if ((currentMouse.Y - centr.Y) > 0 && currentViewPort.Y + widthScal * (currentMouse.Y - centr.Y) + image.ViewportWidth / image.AspectRatio > 1.0)
            //    newPos.Y =max 1.0 - image.ViewportWidth / image.AspectRatio;
            //if ((currentMouse.X - centr.X) < 0 && newPos.X > 0)
            //    newPos.X = 0;
            //else if ((currentMouse.X - centr.X) > 0 && currentViewPort.X + widthScal * (currentMouse.X - centr.X) + image.ViewportWidth > imageWidth)
            //    newPos.X = - imageWidth + image.ViewportWidth;
            
            image.ViewportOrigin = newPos;
            //ScaleMrksPositions();
        }
        
        private void image_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {   
            Zoom(e.Delta > 0 ? 1.1 : 0.9, currentMouse);
        }
        private void Zoom(double newzoom, Point toPoint)
        {
            var zoomNewValue = newzoom * zoom;
          //if (zoomNewValue < imageHeight) zoomNewValue=
          zoom = zoomNewValue;
            var point = image.ElementToLogicalPoint(toPoint);
            image.ZoomAboutLogicalPoint(newzoom, point.X, point.Y);
        }
        private bool isMouseDown { get; set; }
        private Point currentMouse { get; set; }
        private Point currentViewPort { get; set; }
        public string ReflectionId { get; set; }
        void image_MouseLeftButtonUp(object sender, EventArgs e)
        {
            isMouseDown = false; image.UseSprings = true;
        }
        //private void DataServise(string imageId)
        //{
        //    ani.Begin();
        //    //   timerOnLoad.Visibility = Visibility.Visible;
        //    seconds.Visibility = Visibility.Visible;

        //    //new Uri("Init.aspx?imageId=" + imageId, UriKind.Relative)
        //    //    .RequestString(OnComplited);
        //}
        private void InitSourceMarksPages() //(string result)
        {
            // ImageId = Application.Current.Host.InitParams["imageId"];
           // if (String.IsNullOrWhiteSpace(ImageId)) MessageBox.Show("ImageId in request url losted");
            string path, sxPages, sxMarks, previous;
            if (!Application.Current.Host.InitParams.TryGetValue("path", out path)
                || String.IsNullOrWhiteSpace(path)) MessageBox.Show("path in initParams losted");
            //Application.Current.Host.InitParams.TryGetValue("previous", out previous);
            //if (!Application.Current.Host.InitParams.TryGetValue("xPages", out sxPages)
            //    || String.IsNullOrWhiteSpace(sxPages)) MessageBox.Show("xPages in initParams losted");
            //if (!Application.Current.Host.InitParams.TryGetValue("xMarks", out sxMarks)
            //    || String.IsNullOrWhiteSpace(sxMarks)) MessageBox.Show("xMarks in initParams losted");
            //if (Application.Current.Host.InitParams.TryGetValue("rtfText", out RtfText))
            //{
            //}
           XElement xMarks = new XElement("h"), xPages = new XElement("h");
            //VMMark.Previous = previous;
            //try
            //{
            //    xPages = XElement.Parse(sxPages);
            //}
            //catch (Exception exception)
            //{
            //    MessageBox.Show(exception.Message + " " + sxPages);
            //}
            //try
            //{
            //    xMarks = XElement.Parse(sxMarks.Replace('.', ',').Replace("rdf:about", "id"));
            //}
            //catch (Exception exception)
            //{
            //    MessageBox.Show(exception.Message + " " + sxMarks);
            //}

            Uri sourceUri = new Uri(path, UriKind.RelativeOrAbsolute);
            
            
            image.Source = new DeepZoomImageTileSource(sourceUri);
            //if (result == null)	new Uri("Init.aspx?imageId=" + ImageId, UriKind.Relative)
            //	.RequestString(OnComplited);  //	return;//ani.Stop();
            //	seconds.Visibility = Visibility.Collapsed;
            marksPanel.Items.Clear();
            pagesPanel.Children.Clear();
            foreach (var linkTo in xPages.Elements()
                .OrderBy(page =>
                {
                    int number = -1;
                    if (Int32.TryParse(page.Value, out number)) return number;
                    var strNum = page.Value.Split('-');
                    if (strNum.Any() && Int32.TryParse(strNum.First(), out number)) return number;
                    return number;
                })
                .Select(
                    (page, i) =>
                        new HyperlinkButton
                        {
                            Content = page.Value,
                            Tag = i,
                            Style = App.Current.Resources["HLinkStyle"] as Style
                        }))
            {
                linkTo.Click += (sender, e) =>
                {
                    var j = (int) ((HyperlinkButton) sender).Tag;
                    Zoom(1.0/zoom, new Point());
                    var si = image.SubImages[j];
                    var widthScal = 1.0/(si.ViewportWidth);
                    image.ViewportOrigin =
                        new Point(-si.ViewportOrigin.X*widthScal, si.ViewportOrigin.Y*widthScal);
                };
                pagesPanel.Children.Add(linkTo);
            }
            foreach (
                var mark in
                    xMarks.Elements()
                        .Where(x => x.Elements("reflected").Any())
                        .Select(reflection => new Mark(reflection, id, this))
                        .OrderBy(mark => mark.Props.Reflecteds.Count))
            {
                mark.SetScaledPosition();
                mark.markCircleBorder.MouseWheel += image_MouseWheel;
                mark.markCircleFill.MouseWheel += image_MouseWheel;
                mark.markCircleFill.MouseMove += image_MouseMove;
                mark.markCircleFill.MouseLeftButtonUp += image_MouseLeftButtonUp;
                mark.markCircleFill.MouseLeftButtonDown += OnLeftButtonDown;
                mark.markRectangleFill.MouseLeftButtonDown += OnLeftButtonDown;
                mark.markRectangleFill.MouseMove += image_MouseMove;
                mark.markRectangleFill.MouseLeftButtonUp += image_MouseLeftButtonUp;
                mark.markRectangleFill.MouseWheel += image_MouseWheel;
                mark.markRectangleBorder.MouseWheel += image_MouseWheel;
                if (!Mark.marks.Contains(mark))
                    continue;
                ViewNavigateMark(mark, null);
            }

            //HtmlPage.Document.QueryString["pagenumber"]


            //HtmlPage.Document.QueryString["id"];
            //new Uri(App.Current.Host.Source, "../" + path.Value));
            //new DeepZoomTileSource((int)Math.Pow(2, 30),(int)Math.Pow(2, 30), 128, 128, imageId);
        }

        private void ViewNavigateMark(Mark mark, MouseEventArgs e)
        { 
            mark.ViewReflectedsList();
            marksPositions.Children.Add(
                mark.EditPosition(e, image, marksPositions));
            var linkToMark = new HyperlinkButton { Content = new TextBlock { Text= mark.Props.ItemName,
                                                                             Style=App.Current.Resources["TextBlockStyle"] as Style
            },
                                                   Style=App.Current.Resources["HLinkStyle"] as Style
            };
            linkToMark.Click += delegate
                                    {
                                            ZoomToCircle(mark);
                                            ZoomToRect(mark);
                                    };
            var treeNavigation=new TreeViewItem { Header=linkToMark, IsExpanded=true };
            foreach(var linkTo in mark.Props.Reflecteds
                .Select(reflected => new HyperlinkButton { Content = new TextBlock { Text= reflected.ItemName,
                                                                                     Style=App.Current.Resources["TextBlockStyle"] as Style
                },
                                                           Style=App.Current.Resources["HLinkStyle"] as Style }))
            {
                linkTo.Click += delegate
                                    {
                                        ZoomToCircle(mark);
                                        ZoomToRect(mark);
                                    };
                treeNavigation.Items.Add(linkTo);
            }
            marksPanel.Items.Add(treeNavigation);
        }


        //public static string GetCurrentUri()
        // {
        //     var origin = HtmlPage.Document.DocumentUri.AbsoluteUri;
        //     var index = origin.IndexOf('?');
        //     return index==-1 ? origin : origin.Substring(0, index);
        // }
        //public static DeepZPoints Owner;

        private void newMark_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var mark = new Mark(id, this, newMarkAreaType.SelectedIndex == 0 ? Mark.Form.Rect : Mark.Form.Circle) { IsEditMenuVisible = true };
            mark.markCircleFill.MouseWheel += image_MouseWheel;
            mark.markCircleBorder.MouseWheel += image_MouseWheel;
            AddNewMark(e, mark);
        }

        private void ZoomToCircle(Mark mark)
        {
            if(mark.FormView != Mark.Form.Circle) return;
            Zoom(0.5 * ActualHeight / (mark.Props.Radius * zoom * ActualWidth), new Point());
            image.ViewportOrigin = new Point(mark.Props.Origin.X - mark.Props.Radius, mark.Props.Origin.Y - mark.Props.Radius);
        }
   
        //private void newMark_Rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    var mark = new Mark(id, this, Mark.Form.Rect) { IsEditMenuVisible = true };
        //    mark.markRectangle.MouseWheel += image_MouseWheel;
        //    AddNewMark(e, mark);
        //}

        private void ZoomToRect(Mark mark)
        {
            if(mark.FormView != Mark.Form.Rect) return;
            Zoom(ActualHeight / (mark.Props.RectangleSize.Height * zoom * ActualWidth), new Point());
            image.ViewportOrigin = mark.Props.Origin;
        }

        private void AddNewMark(MouseButtonEventArgs e, Mark mark)
        {
            mark.SetPosition(e.GetPosition(marksPositions));
          ViewNavigateMark(mark, e);
        }
        // private double oldSizeX, oldSizeY;
        private void fullscreen_MouseLeftButtonUp(object sender, RoutedEventArgs routedEventArgs)
        { Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;}
        private void pagesPanelMouseLeave(object sender, MouseEventArgs e)
        { hider.Begin(); }
        private void pagesPanelMouseEnter(object sender, MouseEventArgs e)
        { hider.Stop(); pagesPanelView.Opacity = 1; }

        private void pagesPanelHider_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            pagesPanel.Visibility = marksPanel.Visibility = pagesPanel.Visibility == Visibility.Visible
                                        ? Visibility.Collapsed
                                        : Visibility.Visible;
        }
    }
    
   

}
