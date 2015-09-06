using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Browser;
using System.Windows.Data;

namespace DeepZoomMarked
{
	public partial class OCRSEarchResult
	{

		public VMMark Props { get; set; }

        public static List<OCRSEarchResult> searchMarks = new List<OCRSEarchResult>();
		public DeepZPoints Owner;

		public enum Form
		{
			Circle, Rect
		}
		Form form;

	
		public OCRSEarchResult(DeepZPoints owner)
		{
			Owner = owner;
			DataContext = Props = new VMMark{Type="document"};
			InitializeComponent();
			Props.OriginChanged += delegate { Props.PositionSetted = true; };
			Props.RectangleSize = new Size(0.1, 0.1);
			//   props.ReflectionId(typeof(string), new PropertyChangedCallback(delegate { }));

			//Props.Registr<HyperlinkButton, string>("ItemName", , nameLink);
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Top;
			Props.IsCreatedEditMenu = false;
			//props.Registr("Url", typeof(Uri), new Binding("[Id]") { Converter = new IdUrlConverter(), Source = props });
            //foreach (var reflected in Props.Reflecteds)
            //{


            //    reflected.IdChanged = newValue =>
            //                       {
            //                           nameLink.NavigateUri = reflected.Url = new Uri(LinkCombo.SelectedItem + newValue);
            //                       };
         
			ViewedRadius = 15;

		}
	
	
		public OCRSEarchResult(string id, DeepZPoints owner, Form f)
			: this(owner)
		{
			Props.IsPositionEdit = true;
			Props.ImageId = id;
			searchMarks.Add(this);
			
			//	linkList.ItemsSource = Props.Reflecteds;
			//  SetScaledPosition();    // CreateEditMenu();
		}                 
	



		public void SetPosition(double x, double y)
		{
			//var r = ViewedRadius;
			//Props.Origin = new Point( x/ Owner.image.ActualWidth, y / Owner.image.ActualHeight );
			ViewedPosition = new Point(x, y);
			SetScaledPosition();
		}
		public void SetScaledPosition()
		{
		    var vp = ViewedPosition;
		    var vS = ViewedRectangleSize;
		    Canvas.SetLeft(this, vp.X);
		    Canvas.SetTop(this, vp.Y);
		    markRectangle.Height = vS.Height;
		    markRectangle.Width = vS.Width;
		    //var marginX = vS.Width * 0.5 - positionChanger.Width * 0.5;
		    //var marginY = vS.Height * 0.5 - positionChanger.Height * 0.5;
		    //positionChanger.Margin = new Thickness(marginX, marginY, 0, 0);
		    Props.PositionSetted = true;
		}


	    public OCRSEarchResult(double x, double y, DeepZPoints owner):this(owner)
	    {
            InitializeComponent();
            SetPosition(x, y);
           
	    }


	    public double ViewedRadius
		{
			get
			{
				return Props.Radius * Owner.image.ActualWidth / Owner.ImageScale;
			}
			set
			{
				Props.Radius = value * Owner.ImageScale / Owner.image.ActualWidth;
			}
		}
		public Point ViewedPosition
		{
			get
			{
				var origin = Props.Origin;
				var scale = Owner.ImageScale;
				return new Point(origin.X * Owner.image.ActualWidth / scale, origin.Y * Owner.image.ActualWidth / scale);
			}
			set
			{
				Props.Origin = new Point(value.X * Owner.ImageScale / Owner.image.ActualWidth, value.Y * Owner.ImageScale / Owner.image.ActualWidth);
			}
		}  
		public Size ViewedRectangleSize
		{
			get
			{
				var scale = Owner.image.ActualWidth / Owner.ImageScale;
				return new Size(Props.RectangleSize.Width * scale, Props.RectangleSize.Height * scale);
			}
		}
	}
}
