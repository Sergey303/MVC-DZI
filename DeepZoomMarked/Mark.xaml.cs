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
	public partial class Mark
	{

		public VMMark Props { get; set; }

		public static List<Mark> marks = new List<Mark>();
		public DeepZPoints Owner;

		public enum Form
		{
			Circle, Rect
		}
		Form form;

		public Form FormView
		{
			set
			{
				form = value;
				if (form == Form.Circle)
				{
					markCircle.Visibility = Visibility.Visible;
					markRectangle.Visibility = Visibility.Collapsed;
				}
				else if (form == Form.Rect)
				{
					markCircle.Visibility = Visibility.Collapsed;
					markRectangle.Visibility = Visibility.Visible;
				}
			}
			get { return form; }
		}
		public Mark(DeepZPoints owner)
		{
			Owner = owner;
			DataContext = Props = new VMMark{Type="document"};
			InitializeComponent();
			Props.OriginChanged += delegate { Props.PositionSetted = true; };
			Props.Radius = markCircleBorder.Width * 0.5;//, markPoint.Width * 0.5);
			Props.RectangleSize = new Size(0.1, 0.1);
			//   props.ReflectionId(typeof(string), new PropertyChangedCallback(delegate { }));

			nameLink.SetBinding(ContentControl.ContentProperty, new Binding("ItemName"));
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
            //}
			IsEditMenuVisible = editMode;
			EditModeChange(editMode);
			ViewedRadius = 15;

		}

		private static bool editMode;
		public static bool EditMode
		{
			get { return editMode; }
			set
			{
				editMode = value;
				marks.Apply(m => m.EditModeChange(value));
			}
		}

		public Mark(string id, DeepZPoints owner, Form f)
			: this(owner)
		{
			Props.IsPositionEdit = true;
			Props.ImageId = id;
			marks.Add(this);
			FormView = f;
			//	linkList.ItemsSource = Props.Reflecteds;
			//  SetScaledPosition();    // CreateEditMenu();
		    ViewReflectedsList();
		}

        public void ViewReflectedsList()
        {
            //subLinkList.ItemsSource=Props.re
        //    linkList.Items.Clear();
        //    var textMain = new TextBlock { Text = Props.ItemName };
        //    textMain.MouseLeftButtonUp += delegate
        //    {
        //        HtmlPage.Window.Navigate(Props.Url);
        //    };
        //    linkList.Items.Add(textMain);
        //    foreach (var item in Props.Reflecteds)
        //    {
        //        var text = new TextBlock { Text = item.ItemName };
        //        text.MouseLeftButtonUp += delegate
        //        {
        //            HtmlPage.Window.Navigate(item.Url);
        //        };
        //        linkList.Items.Add(text);
        //    }
        //    linkList.UpdateLayout();
        //  //  linkList.Margin=new Thickness(0,-linkList.Height, 0,0);
        }
		public void EditModeChange(bool edit)
		{
			//HtmlPage.Window.Alert(edit.ToString());
			positionChanger.Visibility = EditButton.Visibility = edit ? Visibility.Visible : Visibility.Collapsed;
            markRectangleBorder.Stroke = markCircleBorder.Stroke = edit
								   ? new SolidColorBrush(Colors.Red)
								   : new SolidColorBrush(Color.FromArgb(70, 255, 0, 0));
			if (edit)
			{
				//MouseLeftButtonUp -= markPoint_MouseLeftButtonUp;
			}
			else
			{
				//MouseLeftButtonUp += markPoint_MouseLeftButtonUp;
				IsEditMenuVisible = false;
			}
			//if(edit)
			//    markPoint.MouseLeftButtonUp
			//   IsEditMenuVisible = edit;
		}
		public Mark(XElement xRef, string id, DeepZPoints owner)
			: this(owner)
		{
			Owner = owner;
			var about = xRef.Attribute("id");
            //MessageBox.Show(xRef.ToString());
			if (about != null)
			{
              //  MessageBox.Show(xRef.ToString());
				Props.ReflectionId = about.Value;
				if (marks.All(m => m.Props.ReflectionId != Props.ReflectionId))
					marks.Add(this);
			}

		    var xMainReflected = xRef.Element("reflected").Elements().FirstOrDefault();
            if (xMainReflected == null) return;
            // HtmlPage.Window.Alert(props.ReflectionId);
            var name = xMainReflected.Element("name");
            if (name != null)
            {
                Props.ItemName = name.Value;
               // MessageBox.Show(Props.ItemName);
            }
            var personId = xMainReflected.Attribute("id");
            if (personId != null)
                Props.Id = personId.Value;
           ;
           if ((Props.Type = xMainReflected.Name.ToString())=="document")
               foreach (var person in xMainReflected.Elements("reflection").Select(reflected => reflected.Elements().FirstOrDefault().Elements().FirstOrDefault()))
               {
                   if (person == null) continue;
                   var newReflected = new Reflected{Type = person.Name.ToString() };
                   name = person.Element("name");
                   // MessageBox.Show(person.ToString());
                   if (name != null)
                   {
                       newReflected.ItemName = name.Value;
                       //   MessageBox.Show(newReflected.ItemName);
                   }
                 var  SubPersonId = person.Attribute("id");
                   if (SubPersonId != null)
                       newReflected.Id = SubPersonId.Value;
                   Props.Reflecteds.Add(newReflected);
               }

		    var position = xRef.Element("position");
			if (position == null)
				xRef.Add(position = new XElement("position", new XAttribute("x", 0), new XAttribute("y", 0)));
			var xAttribute = position.Attribute("x");
			var yattribute = position.Attribute("y");
			if (xAttribute != null && yattribute != null)
			{
				var x = Convert.ToDouble(xAttribute.Value);
				var y = Convert.ToDouble(yattribute.Value);
				//Margin = new Thickness(x, y, 0, 0);
				//  Props.Origin = new Point(x / Owner.ImageScale, y / Owner.ImageScale);
				//                SetPosition(x, y);
				//props.Url = new Uri("http://sergey.iis.nsk.su/pa/?id=" + props.Id);
				Props.Origin = new Point(x, y);
			}
			var xRadius = position.Attribute("r");
			if (xRadius != null)
			{
				double r = !double.TryParse(xRadius.Value, out r) ? 0.01 : r;
			    Props.Radius = r;
				FormView = Form.Circle;
			}
			else
			{
				var xSize = position.Attribute("rect");
				if (xSize != null)
				{
					var rect = xSize.Value.Split('|');
					if (rect.Length != 2) MessageBox.Show("size of rectangle has strange value " + Props.ReflectionId);
					else
					{
						double width = double.TryParse(rect[0], out width) ? width : 0.01;
						double height = double.TryParse(rect[1], out height) ? height : 0.01;
						Props.RectangleSize = new Size(width, height);
						FormView = Form.Rect;
					}
				}
			}
			Props.ImageId = id;
			Props.IsPositionEdit = false;
			ViewReflectedsList();
		}

		public static Mark Marks(string reflectionId)
		{
			return marks.FirstOrDefault(m => m.Props.ReflectionId == reflectionId);
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
			positionChanger.Height = positionChanger.Width = 20.0;// *Owner.ImageScale;
			if (form == Form.Circle)
			{
				var vr = ViewedRadius;
				Canvas.SetLeft(this, vp.X - vr);
				Canvas.SetTop(this, vp.Y - vr);
				markCircle.Height = markCircle.Width = 2 * vr;

				var margin = vr - positionChanger.Height * 0.5;
				positionChanger.Margin = new Thickness(margin, margin, 0, 0);
			}
			else if (form == Form.Rect)
			{
				var vS = ViewedRectangleSize;
				Canvas.SetLeft(this, vp.X);
				Canvas.SetTop(this, vp.Y);
				markRectangle.Height = vS.Height;
				markRectangle.Width = vS.Width;
				//var marginX = vS.Width * 0.5 - positionChanger.Width * 0.5;
				//var marginY = vS.Height * 0.5 - positionChanger.Height * 0.5;
				//positionChanger.Margin = new Thickness(marginX, marginY, 0, 0);
			}
			Props.PositionSetted = true;
		}

		public void SetPosition(Point position)
		{
			SetPosition(position.X, position.Y);
		}

		//private void SetState(string parameter, StateSetted changedState)
		//{
		//    state = (String.IsNullOrEmpty(parameter))
		//                ? state ^ changedState
		//                : state | changedState;
		//}

        //private ComboBox linkCombo;
        //public ComboBox LinkCombo
        //{
        //    get
        //    {
        //        if (linkCombo == null)
        //        {
        //            linkCombo = new ComboBox();
        //            linkCombo.Items.Add("Http://sergey.iis.nsk.su/PA/?id=");
        //            linkCombo.SelectedIndex = 0;
        //        }
        //        return linkCombo;
        //    }
        //    set { linkCombo = value; }
        //}

		private void CreateEditMenu()
		{
			if (Props.IsCreatedEditMenu) return;
			Props.IsCreatedEditMenu = true;
			//var blue = new SolidColorBrush(Colors.Blue);			
			HidedContent.Background = new SolidColorBrush(Colors.White);
			var list = new ListBox{Margin = new Thickness(10,0,0,0)};  
			foreach (var reflected in Props.Reflecteds)
            {
            StackPanel reflecteddXaml= null;
                list.Items.Add(reflecteddXaml =
                               ReflectedToXaml(reflected,
                                               delegate
                                                   {
                                                       list.Items.Remove(reflecteddXaml);
                                                       Props.Reflecteds.Remove(reflected);
                                                       ViewReflectedsList();
                                                   }));
            }
			Button addNewReflected = new Button { Content = "Добавить отражаемое" };
			addNewReflected.Click += delegate
			{
				var newReflected = new Reflected{Type="person"};
				Props.Reflecteds.Add(newReflected);
			    StackPanel newReflecteddXaml=null;
			    list.Items.Add(newReflecteddXaml = ReflectedToXaml(newReflected, delegate
			                                                                         {
			                                                                             list.Items.Remove(newReflecteddXaml);
			                                                                             Props.Reflecteds.Remove(newReflected);
			                                                                             ViewReflectedsList();
			                                                                         }));
			};
		    HidedContent.Children.Add(
                ReflectedToXaml(Props, SendDelete),
			//	new TextBlock { Text = "Идентификатор Отношения:", Foreground = foregr },
                //new TextBlock { Foreground = foregr }
                // .Binding(TextBlock.TextProperty, new Binding("ReflectionId") { Mode = BindingMode.TwoWay }),
         
				   list, addNewReflected);
		}
		static SolidColorBrush foregr = new SolidColorBrush(Colors.Black);
        private StackPanel ReflectedToXaml(Reflected reflected, RoutedEventHandler onDelete)
		{
			var refledContainer = new StackPanel();
			var typeCombo = new ComboBox { Foreground = foregr };
			foreach (var type in Types.Keys)
				typeCombo.Items.Add(type);
            typeCombo.SelectedItem = Types.Values.Contains(reflected.Type) ? (Types.FirstOrDefault(keyValue => keyValue.Value == reflected.Type)).Key : Types.Keys.First();
            typeCombo.SelectionChanged += delegate { reflected.Type = Types[typeCombo.SelectedItem.ToString()]; };
			//refledContainer.Children.Add(new TextBlock { Text = "Тип: ", Foreground = foregr });
			refledContainer.Children.Add(typeCombo);
            //refledContainer.Children.Add(new TextBlock { Text = "Идентификатор Отражаемого: ", Foreground = foregr });
            //refledContainer.Children.Add(new TextBox { Foreground = foregr }
		   // .Binding(TextBox.TextProperty, new Binding("Id") { Mode = BindingMode.TwoWay, Source = reflected }));
			//refledContainer.Children.Add(delete);

			refledContainer.Children.Add(new TextBlock { Text = "Наименование(поиск по Enter) : ", Foreground = foregr });
			refledContainer.Children.Add(new SearchControl(reflected)
			{
				NameSet=ViewReflectedsList,
				NewCreate = () =>
				{
					reflected.Id = Guid.NewGuid().ToString();
					SendCreateNew(reflected);
					IsEditMenuVisible = false;
				}

			});
			var delref = new Button { Content = "открепить" };
            delref.Click += onDelete;// delegate { if (list != null)list.Items.Remove(refledContainer); Props.Reflecteds.Remove(reflected); ViewReflectedsList(); };
			refledContainer.Children.Add(delref);
			return refledContainer;
		}
		MouseEventHandler mouseMovePosChange = null;
		MouseEventHandler mouseDownPosChange = null;
		MouseEventHandler mouseUp = null;
		MouseEventHandler onMouseMoveRadiusChange = null;
		MouseEventHandler mouseDownRadiusChange = null;
		public Mark EditPosition(MouseEventArgs e, UIElement parent, UIElement positionParent)
		{

			DateTime lastClick = new DateTime();
			mouseMovePosChange = (sender2, e2) =>
				{
					//if (!Props.IsPositionEdit) return;
					ViewedPosition = e2.GetPosition(positionParent);
					SetScaledPosition();
				};
			onMouseMoveRadiusChange = (sender4, e4) =>
							  {
								  Props.IsPositionEdit = false;
								  var pos = e4.GetPosition(positionParent);
								  var viewedPosition = ViewedPosition;
								  var dx = Math.Abs(pos.X - viewedPosition.X);
								  var dy = Math.Abs(pos.Y - viewedPosition.Y);
								  if (form == Form.Circle)
								  {
									  //  var d = Math.Max(dx, dy);
									  ViewedRadius = Math.Sqrt(dx * dx + dy * dy);
								  }
								  else if (form == Form.Rect)
								  {
									  ViewedRectangleSize = new Size(dx, dy);
								  }
								  SetScaledPosition();
								  //markCircle.MouseLeftButtonUp -= markPoint_MouseLeftButtonUp;
								  //markRectangle.MouseLeftButtonUp -= markPoint_MouseLeftButtonUp;
							  };

			mouseUp = (sender3, e3) =>
			{					
				parent.MouseLeftButtonUp -= new MouseButtonEventHandler(mouseUp);
				MouseLeftButtonUp -= new MouseButtonEventHandler(mouseUp);
				Props.IsPositionEdit = false;
				MouseMove -= mouseMovePosChange;
				parent.MouseMove -= mouseMovePosChange;
				parent.MouseMove -= onMouseMoveRadiusChange;
				MouseMove -= onMouseMoveRadiusChange;   //parents.ForEach(p=>p.MouseLeave -= new MouseEventHandler(mouseUp));
                markRectangleBorder.MouseLeftButtonUp -= markPoint_MouseLeftButtonUp;
                markRectangleBorder.MouseLeftButtonUp += markPoint_MouseLeftButtonUp;
				markCircleBorder.MouseLeftButtonUp -= markPoint_MouseLeftButtonUp;
                markCircleBorder.MouseLeftButtonUp += markPoint_MouseLeftButtonUp;
				IsEditMenuVisible = true;
				lastClick = DateTime.Now;
				//markCircle.MouseLeftButtonUp += markPoint_MouseLeftButtonUp;
				//markRectangle.MouseLeftButtonUp += markPoint_MouseLeftButtonUp;
			};
			mouseDownPosChange = (sender2, e2) =>
			{
				if (DateTime.Now - lastClick < TimeSpan.FromMilliseconds(200)) //double click
				{
					if (form == Form.Circle)
					{
						//  var d = Math.Max(dx, dy);
						ViewedRadius += 10;
					}
					else if (form == Form.Rect)
					{
						var old = ViewedRectangleSize;
						ViewedRectangleSize = new Size(old.Width + 10, old.Height + 10);
					}
					SetScaledPosition();
					return;
				}

				markCircleBorder.MouseLeftButtonUp -= markPoint_MouseLeftButtonUp;
				markRectangleBorder.MouseLeftButtonUp -= markPoint_MouseLeftButtonUp;
				if (Props.IsPositionEdit)
					mouseUp(sender2, e2);
				else
				{
					Props.IsPositionEdit = true;
					parent.MouseMove += mouseMovePosChange;
					MouseMove += mouseMovePosChange;
					parent.MouseLeftButtonUp += new MouseButtonEventHandler(mouseUp);
					MouseLeftButtonUp += new MouseButtonEventHandler(mouseUp);
					//props.TempisEditMenuVisible = IsEditMenuVisible;    // parents.ForEach(p=>p.MouseLeave += mouseUp);
					IsEditMenuVisible = false;
				}
			};
			positionChanger.MouseLeftButtonDown += new MouseButtonEventHandler(mouseDownPosChange);


			mouseDownRadiusChange = (sender, e1) =>
										{
											if (!Owner.Props.EditMode)
												return;
											MouseMove += onMouseMoveRadiusChange;
											parent.MouseMove += onMouseMoveRadiusChange;
											// Props.IsPositionEdit = true;
											parent.MouseLeftButtonUp += new MouseButtonEventHandler(mouseUp);
											MouseLeftButtonUp += new MouseButtonEventHandler(mouseUp);
											//props.TempisEditMenuVisible = IsEditMenuVisible;    // parents.ForEach(p=>p.MouseLeave += mouseUp);
											IsEditMenuVisible = false;
										};
			markCircleBorder.MouseLeftButtonDown += new MouseButtonEventHandler(mouseDownRadiusChange);
            markRectangleBorder.MouseLeftButtonDown += new MouseButtonEventHandler(mouseDownRadiusChange);
			if (Props.IsPositionEdit)
			{
				Props.IsPositionEdit = false;
				mouseDownPosChange(this, e);
			}
			return this;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!(IsEditMenuVisible = !IsEditMenuVisible))
				Send();
		}
		private void SendCreateNew(Reflected reflected)
		{
			if (reflected.ItemName != null)
				new Uri("?ask=newItem&user=" + Owner.Props.UserId + "&name=" + reflected.ItemName + "&type=" + reflected.Type, UriKind.Relative)
					.RequestString(newId =>
					{
						reflected.Id = newId;
						Send();
					});
		}
		private void Send()
		{
			if (Owner.Props.UserId == null)
			{
				MessageBox.Show("Пользователь не определён.");
				return;
			}
			if (!Props.PositionSetted || Owner.ImageId == null) return;
            //if (Props.ReflectionId == null)
            //{
            //    new Uri("?ask=newReflection&user=" + Owner.Props.UserId + "&reflectedId=" + Props.Reflecteds.Aggregate("", (res, refl) => res + "|" + refl.Id)
            //        + "&imageid=" + Owner.ImageId
            //            + ((form == Form.Circle) ? ("&r=" + Props.Radius)
            //            : (form == Form.Rect) ? ("&rect=" + SizeToStr(Props.RectangleSize))
            //            : "")
            //            + "&x=" + Props.Origin.X + "&y=" + Props.Origin.Y, UriKind.Relative)
            //        .RequestString(newReflId => Props.ReflectionId = newReflId);
            //    Props.ReflectionId = "coming soon";
            //}
            //else
		   // MessageBox.Show("?ask=editReflection&user=" + Owner.Props.UserId);
		    new Uri("?ask=editReflection&user=" + Owner.Props.UserId, UriKind.Relative)
						.RequestString(id =>
						                   {
						                       int j = 0;
                                               try
                                               {
                                                   var ids = id.Split('|');
                                                   if (Props.ReflectionId == null)
                                                       Props.ReflectionId = ids[j++];
                                                   if(Props.Autor !=null && !String.IsNullOrWhiteSpace(Props.Autor.Id) 
                                                       && String.IsNullOrWhiteSpace(Props.Autor.ReflectionId))
                                                       Props.ReflectionId = ids[j++];
                                                   foreach (Reflected t in Props.Reflecteds.Where(t =>
                                                       !String.IsNullOrWhiteSpace(t.Id) 
                                                       && String.IsNullOrWhiteSpace(t.ReflectionId)))
                                                       t.ReflectionId = ids[j++];
                                               }catch(Exception ex)
                                               {
                                                   MessageBox.Show("mark.cs521 "+ex.Message);
                                               }
						                   }, new XElement("all",
                                               new XAttribute(XNamespace.Xmlns+"rdf", SNames.rdfns),
                                               Props.ToRDFAll(Owner.ImageId, form)).ToString());
		}
        //string SizeToStr(Size s)
        //{
        //    return s.Width + "|" + s.Height;
        //}
		private void SendDelete(object sender, RoutedEventArgs e)
		{
			if (Props.ReflectionId != null)
			{
				new Uri("?ask=delete&user=" + Owner.Props.UserId +
						"&reflectionId=" + Props.ReflectionId,
						UriKind.Relative)
					.RequestString(ok =>
									   {
										   marks.Remove(this);
										   Owner.marksPositions.Children.Remove(this);
									   });

			}
			else
			{
				marks.Remove(this);
				Owner.marksPositions.Children.Remove(this);
			}
			IsEditMenuVisible = false;
		}

		private static readonly Dictionary<string,string> Types = new Dictionary<string, string> {   {"организация", "org-sys"},  {"персона", "person"}, {"статья", "document"}};

		public bool IsEditMenuVisible
		{
			get { return HidedContent.Parent != null; }
			set
			{
				if (value && !IsEditMenuVisible)
				{
					if (!Props.IsCreatedEditMenu) CreateEditMenu();
					EditButton.Content = "OK";
					LayoutRoot.Children.Add(HidedContent);
				}
				else
				{
					EditButton.Content = "edit";
					LayoutRoot.Children.Remove(HidedContent);
				}
			}
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

		private void markPoint_MouseEnter(object sender, MouseEventArgs e)
		{
			linkList.Visibility =
			nameLink.Visibility = Visibility.Visible;
		}

		private void markPoint_MouseLeave(object sender, MouseEventArgs e)
		{
			linkList.Visibility =
			nameLink.Visibility = Visibility.Collapsed;
		}

		private void markPoint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Uri reflectedUrl = ((FrameworkElement)sender).Tag as Uri;
			HtmlPage.Window.Navigate(reflectedUrl);
		}

		private void markPoint_MouseMove(object sender, MouseEventArgs e)
		{
			var pos = e.GetPosition(this);
			Canvas.SetLeft(nameLink, pos.X + 5);
			Canvas.SetTop(nameLink, pos.Y + 5);
			Canvas.SetLeft(linkList, pos.X + 5);
			Canvas.SetTop(linkList, pos.Y + 5);
		}



		public Size ViewedRectangleSize
		{
			get
			{
				var scale = Owner.image.ActualWidth / Owner.ImageScale;
				return new Size(Props.RectangleSize.Width * scale, Props.RectangleSize.Height * scale);
			}
			set
			{
				var scale = Owner.image.ActualWidth / Owner.ImageScale;
				Props.RectangleSize = new Size(value.Width / scale, value.Height / scale);
			}
		}
	}
}
