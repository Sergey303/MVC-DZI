using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Windows.Data;

namespace DeepZoomMarked
{
    public class SearchControl : StackPanel
    {
        public Reflected Reflected;
        public SearchControl(Reflected refled)
        {
			Reflected = refled;
            nameText = new TextBox();
            nameText.SetBinding(TextBox.TextProperty, new Binding("ItemName") { Mode = BindingMode.TwoWay, Source = Reflected});
            nameText.SelectAll();
            nameText.KeyUp += (sende1, e1) =>
            {
                if (e1.Key != Key.Enter) return;
                if (search && nameText.Text.Length >= 4)
                    SearchService(nameText.Text, Reflected.Type);
                else search = true;
            };
            IsContainsMethod = new CheckBox { Content = "Подстрока" , IsChecked=false };
            ResultsList = new StackPanel();
            Children.Add(nameText, IsContainsMethod, ResultsList);

        }
        bool search = true;

        public Action NewCreate { get; set; }
		public Action NameSet { get; set; }
        public TextBox nameText { get; set; }
        public CheckBox IsContainsMethod;
        public void SearchService(string name, string type)
        {
            new Uri("?string=" + name + "&type=" + type +
                (IsContainsMethod.IsChecked != null && IsContainsMethod.IsChecked.Value ? "&method=contains" : ""),
                UriKind.Relative)
            .RequestString(result =>
                               {
                                   XElement xResults = XElement.Parse(result);
                                   ResultsList.Children.Clear();
                                   SearchResultItem.items.Clear();
                                   if(!Children.Contains(ResultsList))
                                       Children.Add(ResultsList);

                                   foreach(var item in xResults.Elements())
                                   {
                                       var searchItem = new SearchResultItem { Content = item.Element("name").Value, X = item };
                                       searchItem.Click += (sender1, e1) =>
                                               {
                                                   var searchItemSender = sender1 as SearchResultItem;
                                                   search = false;
                                                   nameText.Text = searchItemSender.X.Element("name").Value;
                                                   Reflected.ItemName = nameText.Text;
												   if (NameSet != null) NameSet();
                                                   Reflected.Id = searchItemSender.X.Attribute(SNames.rdfabout).Value;
                                                   if(!(searchItemSender.Selected = !searchItemSender.Selected))
                                                       Children.Remove(ResultsList);
                                               };
                                       if(item.Element("name") != null)
                                           ResultsList.Children.Add(searchItem);
                                   }
                                   var newSearchItem = new SearchResultItem { Content = "создать" };
                                   newSearchItem.Click += (sender1, e1) =>
                                       {
                                           if(NewCreate != null) NewCreate();
                                       };
                                   ResultsList.Children.Add(newSearchItem);

                               });
        }
        public StackPanel ResultsList;

        public class SearchResultItem : HyperlinkButton
        {
            public static List<SearchResultItem> items = new List<SearchResultItem>();

            private static Brush selection = new SolidColorBrush(Colors.Red),
                                 defaultBack = new SolidColorBrush(Colors.White),
                                 foreg = new SolidColorBrush(Colors.Black);
            public SearchResultItem()
            {
                items.Add(this);
                this.Background = defaultBack;
                this.Foreground = foreg;
            }
            public XElement X { get; set; }
            private bool selected;
            public bool Selected
            {
                get { return selected; }
                set
                {
                    if(selected = value)
                    {
                        items.Except(this).Apply(i => i.Selected = false);
                        Background = selection;
                    }
                    else Background = defaultBack;
                }
            }
        }
    }
}
