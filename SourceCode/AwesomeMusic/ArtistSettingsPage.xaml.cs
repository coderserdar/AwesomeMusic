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
using Microsoft.Phone.Marketplace;

namespace AwesomeMusic
{
    public partial class ArtistSettingsPage : PhoneApplicationPage
    {
        public int artistId;
        public int categoryId;

        public ArtistSettingsPage()
        {
            InitializeComponent();
            using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings.First();
                lblFontFamily.Text = AppResources.FontFamily + " (" + AppResources.Selected + ": " + appSettings.FontFamily + ")";
                lblFontSize.Text = AppResources.FontSize + " (" + AppResources.Selected + ": " + appSettings.FontSize + ")";
            }

            pvArtistSettings.Title = AppResources.ArtistSettings;
            piFont.Header = AppResources.Font;
            piOtherSettings.Header = AppResources.OtherSettings;

            btnFontFamily.Content = AppResources.Select;
            btnFontSize.Content = AppResources.Select;
            btnAlbumOrder.Content = AppResources.Select;
            btnAlbumOrderStyle.Content = AppResources.Select;
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
                var artist = context.Artists.Where(j => j.ArtistId.Equals(e.Fragment)).Single() as Artist;
                artistId = artist.ArtistId;
                var appSettings = context.AppSettings.First();
                categoryId = appSettings.CurrentCategoryNumber;
                string orderStyle = artist.AlbumOrderStyle;

                if (artist.AlbumOrderBy == "NAME")
                {
                    lblAlbumOrder.Text = AppResources.AlbumOrderBy + " (" + AppResources.Selected + ": " + AppResources.Name + ")";
                }
                if (artist.AlbumOrderBy == "CDATE")
                {
                    lblAlbumOrder.Text = AppResources.AlbumOrderBy + " (" + AppResources.Selected + ": " + AppResources.CreationDate + ")";
                }
                if (artist.AlbumOrderBy == "MDATE")
                {
                    lblAlbumOrder.Text = AppResources.AlbumOrderBy + " (" + AppResources.Selected + ": " + AppResources.ModificationDate + ")";
                }
                if (artist.AlbumOrderBy == "RATING")
                {
                    lblAlbumOrder.Text = AppResources.AlbumOrderBy + " (" + AppResources.Selected + ": " + AppResources.AlbumRating + ")";
                }
                //if (artist.AlbumOrderBy == "SDATE")
                //{
                //    lblAlbumOrder.Text = AppResources.AlbumOrderBy + " (" + AppResources.Selected + ": " + AppResources.StartDate + ")";
                //}
                //if (artist.AlbumOrderBy == "FDATE")
                //{
                //    lblAlbumOrder.Text = AppResources.AlbumOrderBy + " (" + AppResources.Selected + ": " + AppResources.FinishDate + ")";
                //}
                if (artist.AlbumOrderStyle == "A")
                {
                    lblAlbumOrderStyle.Text = AppResources.AlbumOrderStyle + " (" + AppResources.Selected + ": " + AppResources.Ascending + ")";
                }
                if (artist.AlbumOrderStyle == "D")
                {
                    lblAlbumOrderStyle.Text = AppResources.AlbumOrderStyle + " (" + AppResources.Selected + ": " + AppResources.Descending + ")";
                }
                //lstNoteList.DisplayMemberPath = "NameCreation";
                SetBackgroundColor();
            }
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

        private void btnAlbumOrder_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/OrderSettingsPage.xaml#" + artistId, UriKind.Relative));
        }

        private void btnAlbumOrderStyle_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/OrderStyleSettingsPage.xaml#" + artistId, UriKind.Relative));
        }

        private void btnFontSize_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FontSizeSettingsPage.xaml#" + artistId, UriKind.Relative));
        }

        private void btnFontFamily_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FontFamilySettingsPage.xaml#" + artistId, UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //pvArtistSettings.Title = AppResources.ArtistSettings;
            //piFont.Header = AppResources.Font;
            //piOtherSettings.Header = AppResources.OtherSettings;

            //btnFontFamily.Content = AppResources.Select;
            //btnFontSize.Content = AppResources.Select;
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/ArtistPage.xaml#" + artistId, UriKind.Relative));
            }
        }
    }
}