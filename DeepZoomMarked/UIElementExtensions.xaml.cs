using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DeepZoomMarked
{
    public static class UiExtns
    {
        public static T Binding<T>(this T control, DependencyProperty propDef, Binding b) where T : FrameworkElement
        {
            control.SetBinding(propDef, b);
            return control;
        }

        public static void Add(this UIElementCollection chidren, params UIElement[] content)
        {
            foreach (var item in content)
                chidren.Add(item);
        }

        public static StackPanel Container(this StackPanel control, params UIElement[] content)
        {
            foreach (var item in content)
                control.Children.Add(item);
            return control;
        }

        public static T AttachEvent<T>(this T control, RoutedEvent eventType, Delegate handler) where T : Control
        {
            control.AddHandler(eventType, handler, false);
            return control;
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item)
        {
            return source.Where(i => !i.Equals(item));
        }

        public static void RequestString(this Uri uri, Action<string> onComplited, string postData = null)
        {
           var globalUri = new Uri(App.Current.Host.Source, "../DeepZoomMarked.aspx" + uri);
            //"http://sergey.iis.nsk.su/PA/DZM" + uri);
            var request = new WebClient();
            //  request.Headers[HttpRequestHeader.ContentType] = "text/xml";
            if (postData == null)
            {
                request.DownloadStringCompleted += (sender, e) =>
                                                       {
                                                           if (e.Error != null)
                                                           {
                                                             //  DeepZPoints.TilLoadBlock.Text = e.Error.Message;
                                                                 RequestString(uri, onComplited);
                                                           }
                                                           if (e.Result.StartsWith("Идёт загрузка данных, "))
                                                           {
                                                               DeepZPoints.TilLoadBlock.Text = e.Result.Split('%')[0]+"%";
                                                               RequestString(uri, onComplited);
                                                               return;
                                                           }
                                                           DeepZPoints.TilLoadBlock.Text = "";
                                                           onComplited(e.Result);
                                                       };

                request.DownloadStringAsync(globalUri);
            }
            else
            {
            //postData = "aaaa";
                //var bytes = request.Encoding.GetBytes(postData);
                //request.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                //request.Headers[HttpRequestHeader.ContentLength] = bytes.Length.ToString();
                request.UploadStringCompleted += (sender, e) =>
                {
                    if (e.Error != null )
                    {
                       // DeepZPoints.TilLoadBlock.Text =e.Error.Message;
                            RequestString(uri, onComplited, postData); 
                        return;
                    }
                    if (e.Result.StartsWith("Идёт загрузка данных, "))
                    {
                        DeepZPoints.TilLoadBlock.Text = e.Result.Split('%')[0] + "%";
                        RequestString(uri, onComplited, postData);
                        return;
                    }
                    DeepZPoints.TilLoadBlock.Text = "";
                    onComplited(e.Result);
                };
                request.UploadStringAsync(globalUri, postData); //"POST"
            }
        }


        //public static StackPanel Add(this StackPanel continer, params UIElement[] collection)
        //{
        //    continer.Children.Add(collection);
        //    return continer;
        //}
        public static void Apply<T>(this IEnumerable<T> source, Action<T> apply)
        {
            foreach(var item in source)
                apply(item);
        }
		//public static readonly DependencyProperty UrlProperty = DependencyProperty.Register("URL", typeof(string), typeof(TextBlock), new PropertyMetadata(SetUrl));
		//public static void SetUrl(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		//{
		//	Uri uri;
		//	if (Uri.TryCreate(e.NewValue.ToString(), UriKind.Absolute, out uri))
		//	{
		//		MouseButtonEventHandler h = delegate
		//		 {
		//			 HtmlPage.Window.Navigate(uri);
		//		 };

		//		var owner = sender as TextBlock;
		//		owner.MouseLeftButtonUp -= h;
		//		owner.MouseLeftButtonUp += h;
		//	}
		//}

		//public static void SetURL(DependencyObject obj, string value)
		//{
		//	obj.SetValue(UrlProperty, value);
		//}
		//public static string GetURL(DependencyObject obj)
		//{
		//	return (string)obj.GetValue(UrlProperty);
		//}
        //public static DependencyProperty Attach<TProp, TOwner>(this string name , PropertyChangedCallback whenChanged)
        //{
        //    return DependencyProperty.RegisterAttached(name, typeof(TProp), typeof(TOwner), new PropertyMetadata(whenChanged));
        //}
        //public static DependencyProperty Attach<TOwner>(this string name, TOwner owner, Type typeProp, PropertyChangedCallback whenChanged)
        //{
        //    return DependencyProperty.RegisterAttached(name, typeProp, typeof(TOwner), new PropertyMetadata(whenChanged));
        //}
        //public static Point Add(this Point p, double r)
        //{
        //    return new Point(p.X+r, p.Y+r);
        //}
    }
}