 using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Browser;

namespace DeepZoomMarked
{
    public partial class Login
    {
        private MainVM props;
        private Action<string> ChangeUserId;
        public Action<bool> SetEditMode;
        private IsolatedStorageSettings isolatedStorageSettings;

        public MainVM Props
        {
            get { return props; }
            set
            {
                props = value;
           try{     isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;}
                catch{ return;}
                ChangeUserId = newValue =>
                                   {
                                       props.UserId = ownerView.Text = newValue;
                                       //"error login at "+DateTime.Now.TimeOfDay;
                                       if(string.IsNullOrEmpty(newValue))
                                       {
                                           SetEditMode(false);
                                          // props.UserId = String.Empty;
                                           isolatedStorageSettings.Remove("User.Id");
                                           butLogin.Content = "Login";
                                           LogOutPanel.Visibility = Visibility.Collapsed;
                                           Loginpanel.Visibility = Visibility.Visible;
                                           statusView.Text = "вход";
                                       }
                                       else
                                       {
                                           SetEditMode(true);
                                           butLogin.Content = "Logout";
                                           LogOutPanel.Visibility = Visibility.Visible;
                                           Loginpanel.Visibility = Visibility.Collapsed;
                                           if (!isolatedStorageSettings.Contains("User.Id"))
                                               isolatedStorageSettings.Add("User.Id", newValue);
                                           statusView.Text = newValue;
                                       }
                                   };
                ChangeUserId(isolatedStorageSettings.Contains("User.Id")
                            ? isolatedStorageSettings["User.Id"].ToString()
                            : String.Empty);
             //   HtmlPage.Window.Alert(props.UserId);
            }
        }

        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                new Uri(string.Format("?EncLogin={0}&EncPassword={1}", props.Name, props.Password), UriKind.Relative)
            .RequestString(ressult =>
                               {
                                //   MessageBox.Show(ressult);
                                   var res = ressult.Split('&');
                                   if (res.Length == 2)
                                   {
                                       ChangeUserId(res[0]);
                                       docView.Text = res[1];
                                   }
                                   else
                                       MessageBox.Show("не удалось войти в систему. Возможно не найден документ для редактирования");
                               });
                statusView.Text = "login start at " + DateTime.Now.TimeOfDay;
        }              
        private void butLogout_Click(object sender, RoutedEventArgs e)
        {
            ChangeUserId(String.Empty);
        }
		private void exitClick(object sender, RoutedEventArgs e)
		{
			HtmlPage.Window.Navigate(new Uri(
				App.Current.Host.Source,"../"+ VMMark.PreviousPageName + "?" + HtmlPage.Document.QueryString.Select(pair=>pair.Key+"="+pair.Value).Aggregate((res,i)=>res+"&"+i)));
		}
        #region Cookie don't works :_o(
        //private void SetCookie(string key, string value)
        //{
        //    HtmlPage.Document.SetProperty("cookie",
        //        key + "=" + value + ";expires=" + DateTime.Now.Add(TimeSpan.FromDays(10)).ToString("R"));
        //}
        private void SetCookie(string key, string value)
        {
            // Expire in 7 days
            DateTime expireDate = DateTime.UtcNow + TimeSpan.FromDays(7);

            string newCookie = key + "=" + value + ";expires=" + expireDate.ToString("R");//+";secure";
            HtmlPage.Document.SetProperty("cookie", newCookie);
        }
        private string GetCookie(string key)
        {
            string[] cookies = HtmlPage.Document.Cookies.Split(';');

            return (from cookie in cookies select cookie.Split('=') into keyValue where keyValue.Length == 2 && keyValue[0].Trim() == key select keyValue[1]).FirstOrDefault();
        }
        private static void DeleteCookie(string key)
        {
            string oldCookie = HtmlPage.Document.GetProperty("cookie") as String;
            DateTime expiration = DateTime.UtcNow - TimeSpan.FromDays(1);
            string cookie = String.Format("{0}=;expires={1}", key, expiration.ToString("R"));
            HtmlPage.Document.SetProperty("cookie", cookie);
        }
        private void SaveData(string data, string fileName)
        {
            using(IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using(var isfs = new IsolatedStorageFileStream(fileName, FileMode.Create, isf))
                {
                    using(var sw = new StreamWriter(isfs))
                    {
                        sw.Write(data);
                        sw.Close();
                    }
                }
            }
        }

        private string LoadData(string fileName)
        {
            string data = String.Empty;
            using(IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForSite())
            {
                if(isf.FileExists(fileName))
                using(var isfs = new IsolatedStorageFileStream(fileName, FileMode.Open, isf))
                {
                    using(var sr = new StreamReader(isfs))
                    {
                        string lineOfData;
                        while((lineOfData = sr.ReadLine()) != null)
                            data += lineOfData;
                    }
                }
            }
            return data;
        }  
#endregion

        private void statusView_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            statusView.Opacity = 1;
        }

        private void statusView_MouseLeave_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (view.Visibility == Visibility.Collapsed)
                statusView.Opacity = 0.01;
        }

        private void statusView_MouseLeftButtonUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (view.Visibility == Visibility.Collapsed)
            {
                view.Visibility = Visibility.Visible;
                statusView.Text = "скрыть";
            }
            else
            {
                view.Visibility = Visibility.Collapsed;
                statusView.Text = "служебный вход";
            }
            
        }
    }
}
