using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeMusic.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;


namespace AwesomeMusic
{
    public partial class OrderStyleSettingsPage : PhoneApplicationPage
    {
        public string pageName;
        public int categoryId;
        public int artistId;

        public OrderStyleSettingsPage()
        {
            InitializeComponent();

            lstOrderStyle.Items.Clear();

            lstOrderStyle.Items.Add(AppResources.Ascending);
            lstOrderStyle.Items.Add(AppResources.Descending);

            lstOrderStyle.SelectedIndex = -1;

            SetBackgroundColor();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                if (pageName.Contains("/GeneralSettingsPage.xaml"))
                {
                    this.NavigationService.Navigate(new Uri("/GeneralSettingsPage.xaml", UriKind.Relative));
                }
                else if (pageName.Contains("/CategorySettingsPage.xaml"))
                {
                    this.NavigationService.Navigate(new Uri("/CategorySettingsPage.xaml#" + categoryId, UriKind.Relative));
                }
                else
                {
                    this.NavigationService.Navigate(new Uri("/ArtistSettingsPage.xaml#" + artistId, UriKind.Relative));
                }
            }
        }

        private void lstOrderStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstOrderStyle.SelectedIndex;
            string orderStyle = "";

            if (pageName.Contains("/GeneralSettingsPage.xaml"))
            {
                if (index == 0)
                {
                    orderStyle = "A";
                }
                else
                {
                    orderStyle = "D";
                }

                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    var appSettings = context.AppSettings;
                    foreach (var appSetting in appSettings)
                    {
                        appSetting.CategoryOrderStyle = orderStyle;
                    }
                    context.SubmitChanges();
                    MessageBox.Show(AppResources.CategoryOrderStyleChangeSuccess);
                }
                NavigationService.Navigate(new Uri("/GeneralSettingsPage.xaml", UriKind.Relative));
            }
            else if (pageName.Contains("/CategorySettingsPage.xaml"))
            {
                if (index == 0)
                {
                    orderStyle = "A";
                }
                else
                {
                    orderStyle = "D";
                }

                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    var categories = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).ToList() as List<Category>;
                    foreach (var category in categories)
                    {
                        category.ArtistOrderStyle = orderStyle;
                    }
                    context.SubmitChanges();
                    MessageBox.Show(AppResources.ArtistOrderStyleChangeSuccess);
                }
                NavigationService.Navigate(new Uri("/CategorySettingsPage.xaml#" + categoryId, UriKind.Relative));
            }
            else
            {
                if (index == 0)
                {
                    orderStyle = "A";
                }
                else
                {
                    orderStyle = "D";
                }

                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    var artists = context.Artists.Where(j => j.ArtistId.Equals(artistId)).ToList() as List<Artist>;
                    foreach (var artist in artists)
                    {
                        artist.AlbumOrderStyle = orderStyle;
                    }
                    context.SubmitChanges();
                    MessageBox.Show(AppResources.AlbumOrderStyleChangeSuccess);
                }
                NavigationService.Navigate(new Uri("/ArtistSettingsPage.xaml#" + artistId, UriKind.Relative));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // hangi sayfadan buraya yönlendirme yapılmışsa onun adını almaya yarıyor bu bölüm
            var lastPage = NavigationService.BackStack.FirstOrDefault();
            pageName = lastPage.Source.ToString();
            if (pageName.Contains("/GeneralSettingsPage.xaml"))
            {
                lblSettings.Text = AppResources.GeneralSettings;
                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    var appSettings =
                        context.AppSettings.First();
                    lblOrderStyle.Text = AppResources.CategoryOrderStyle;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);
            if (pageName.Contains("/CategorySettingsPage.xaml"))
            {
                categoryId = int.Parse(e.Fragment);
                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Single() as Category;
                    lblSettings.Text = AppResources.CategorySettings + " (" + category.CategoryName + ")";
                    lblOrderStyle.Text = AppResources.ArtistOrderStyle;
                }
            }
            else
            {
                artistId = int.Parse(e.Fragment);
                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    var artist = context.Artists.Where(j => j.ArtistId.Equals(artistId)).Single() as Artist;
                    lblSettings.Text = AppResources.ArtistSettings + " (" + artist.ArtistName + ")";
                    lblOrderStyle.Text = AppResources.AlbumOrderStyle;
                }
            }
            SetBackgroundColor();
        }

        private void SetBackgroundColor()
        {
            AppSettings appSettings = new AppSettings();
            using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            {
                appSettings = context.AppSettings.First() as AppSettings;
            }

            if (appSettings.AppBackgroundImage != null)
            {
                MemoryStream stream = new MemoryStream(appSettings.AppBackgroundImage);
                BitmapImage image = new BitmapImage();
                image.SetSource(stream);
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = image;
                this.LayoutRoot.Background = ib;
            }
            else
            {
                switch (appSettings.AppBackgroundColor)
                {
                    case "BLA":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                        break;
                    case "BLU":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Blue);
                        break;
                    case "BRO":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Brown);
                        break;
                    case "RED":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Red);
                        break;
                    case "GRE":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Green);
                        break;
                    case "GRA":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Gray);
                        break;
                    case "YEL":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Yellow);
                        break;
                    case "ORA":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Orange);
                        break;
                    case "PUR":
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Purple);
                        break;
                    default:
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                        break;
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //SetBackgroundColor();
        }
    }
}