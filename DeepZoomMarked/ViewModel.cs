using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Xml.Linq;

namespace DeepZoomMarked
{
    public class Reflected : INotifyPropertyChanged
    {
        public string id, itemName;
        public Action<string> IdChanged;

        public string ItemName
        {
            get { return itemName; }
            set
            {
                itemName = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ItemName"));
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                id = value;

                Url = new Uri(App.Current.Host.Source, "../" + (PreviousPageName ?? "") + "?id=" + id);
                
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                if (IdChanged != null) IdChanged(id);
            }
        }

        public static string PreviousPageName
        {
            get { return 
                    VMMark.Previous.Split(new[] {@"/", @"?"}, StringSplitOptions.RemoveEmptyEntries).  //  @"\"
                        FirstOrDefault(uriPath => uriPath.Contains(".aspx"));}}

        private Uri url;

        public Uri Url
        {
            get { return url; }
            set
            {
                url = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Url"));
            }
        }

        public string Type { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private string reflectionId;

        public string ReflectionId
        {
            get { return reflectionId; }
            set
            {
                reflectionId = value;
                PropertyChange("ReflectionId");
            }
        }

        public void PropertyChange(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public XElement ToRDF(string inDocId, params object[] content)
        {
            return new XElement("reflection", reflectionId == null
                                                  ? null
                                                  : new XAttribute(SNames.rdfabout, reflectionId),
                                new XElement("reflected",
                                             new XAttribute(SNames.rdfresource, id)),
                                new XElement("in-doc",
                                             new XAttribute(SNames.rdfresource, inDocId)),
                                content);
        }
    }

    public class VMMark : Reflected
	{ 
		private Point origin;                   
		public event Action<Point> OriginChanged;
		public Point Origin
		{
			get { return origin; }
			set
			{
				origin = value;
				if (OriginChanged != null)
					OriginChanged(origin);
                PropertyChange("Origin");
			}
		}                  
		public bool PositionSetted { get; set; }
		public double Radius { get; set; }      
		public bool IsCreatedEditMenu { get; set; }

        public Reflected Autor { get; set; }

        private ObservableCollection<Reflected> reflecteds=new ObservableCollection<Reflected>();

        public ObservableCollection<Reflected> Reflecteds
        {
            get { return reflecteds; }
            set
            {
                reflecteds = value; PropertyChange("Reflecteds");
            }
        }
		public bool IsPositionEdit { get; set; }
		public string ImageId { get; set; }
		public Size RectangleSize { get; set; }

        private static string previous;
        public static string Previous
        {
            get { return previous; }
            set { previous = value; }
        }

        public IEnumerable<XElement> ToRDFAll(string imageId, Mark.Form form)
        {
            if(Id==null) yield break;
            yield return ToRDF(ImageId,
                               new XElement("position",
                                            new XAttribute("x", origin.X),
                                            new XAttribute("y", origin.Y),
                                          form==Mark.Form.Circle
                                                ? new XAttribute("r", Radius)
                                                : new XAttribute("rect",
                                                                 RectangleSize.Width + "|" + RectangleSize.Height)));
            //if(Autor.Id!=null && )
            //yield return   return new XElement("reflection", Autor.reflectionId == null
            //                                      ? null
            //                                      : new XAttribute(SNames.rdfabout, reflectionId),
            //                    new XElement("reflected",
            //                                 new XAttribute(SNames.rdfresource, id)),
            //                    new XElement("in-doc",
            //                                 new XAttribute(SNames.rdfresource, inDocId)),
            //                    content);
            foreach (var reflected in Reflecteds.Where(reflected=>reflected.Id!=null))
                yield return reflected.ToRDF(id);
        }
	}

	public class MainVM : INotifyPropertyChanged
	{
		private string userId;
		private bool editMode;
		private string password;
		private string name;

		public string UserId
		{
			get
			{
				return userId;
			}
			set
			{
				userId = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("UserId"));
			}
		}

        public event Action<bool> EditModeChanged;
		public bool EditMode
		{
			get
			{
				return editMode;
			}
			set
			{
				editMode = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("EditMode"));
				if (EditModeChanged != null)
					EditModeChanged(value);
			}
		}

		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				password = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Password"));
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Name"));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
