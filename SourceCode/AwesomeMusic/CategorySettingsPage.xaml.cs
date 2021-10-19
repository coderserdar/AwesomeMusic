using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeMusic.Resources;
using Microsoft.Live;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;


namespace AwesomeMusic
{
    public partial class CategorySettingsPage : PhoneApplicationPage
    {
        public int categoryId;
        public CategorySettingsPage()
        {
            InitializeComponent();

            pvCategorySettings.Title = AppResources.CategorySettings;

            piOtherSettings.Header = AppResources.OtherSettings;
            btnArtistOrder.Content = AppResources.Select;
            btnArtistOrderStyle.Content = AppResources.Select;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);
            using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            {
                var category = context.Categories.Where(j => j.CategoryId.Equals(e.Fragment)).Single() as Category;
                string orderStyle = category.ArtistOrderStyle;
                categoryId = category.CategoryId;

                if (category.ArtistOrderBy == "NAME")
                {
                    lblArtistOrder.Text = AppResources.ArtistOrderBy + " (" + AppResources.Selected + ": " + AppResources.Name + ")";
                }
                if (category.ArtistOrderBy == "ALBUMCOUNT")
                {
                    lblArtistOrder.Text = AppResources.ArtistOrderBy + " (" + AppResources.Selected + ": " + AppResources.AlbumCount + ")";
                }
                if (category.ArtistOrderBy == "CDATE")
                {
                    lblArtistOrder.Text = AppResources.ArtistOrderBy + " (" + AppResources.Selected + ": " + AppResources.CreationDate + ")";
                }
                if (category.ArtistOrderBy == "MDATE")
                {
                    lblArtistOrder.Text = AppResources.ArtistOrderBy + " (" + AppResources.Selected + ": " + AppResources.ModificationDate + ")";
                }
                if (category.ArtistOrderStyle == "A")
                {
                    lblArtistOrderStyle.Text = AppResources.ArtistOrderStyle + " (" + AppResources.Selected + ": " + AppResources.Ascending + ")";
                }
                if (category.ArtistOrderStyle == "D")
                {
                    lblArtistOrderStyle.Text = AppResources.ArtistOrderStyle + " (" + AppResources.Selected + ": " + AppResources.Descending + ")";
                }
                //lstNoteList.DisplayMemberPath = "NameCreation";
                SetBackgroundColor();
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //pvCategorySettings.Title = AppResources.CategorySettings;

            //piOtherSettings.Header = AppResources.OtherSettings;
            //btnArtistOrder.Content = AppResources.Select;
            //btnArtistOrderStyle.Content = AppResources.Select;
            //SetBackgroundColor();
        }

        private void btnArtistOrder_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/OrderSettingsPage.xaml#" + categoryId, UriKind.Relative));
        }

        private void btnArtistOrderStyle_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/OrderStyleSettingsPage.xaml#" + categoryId, UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/CategoryPage.xaml#" + categoryId, UriKind.Relative));
            }
        }
    }
}