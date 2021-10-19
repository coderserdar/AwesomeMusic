using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using AwesomeMusic.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeMusic
{
    public partial class StatisticsPage : PhoneApplicationPage
    {
        public StatisticsPage()
        {
            InitializeComponent();
            lblStatistics.Text = AppResources.Statistics;
            SetBackgroundColor();
            SetStatistic();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        private void SetStatistic()
        {
            StringBuilder sb = new StringBuilder();
            string artistName, categoryName, bestAlbum = "", worstAlbum = "", labelName;
            int albumCount;


            using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            {
                var categories = context.Categories.OrderByDescending(j => j.CategoryAlbumCount).ToList() as List<Category>;
                categoryName = categories.First().CategoryNameCount;

                var artists = context.Artists.OrderByDescending(j => j.ArtistAlbumCount).ToList() as List<Artist>;
                artistName = artists.First().ArtistNameCount;

                albumCount = context.Albums.Count();

                var albums3 = context.Albums.GroupBy(j => j.AlbumLabelName).Select(j => new { Name = j.Key, Total = j.Count() }).OrderByDescending(k => k.Total);
                labelName = albums3.First().Name + " (" + albums3.First().Total + ")";

                var albums = context.Albums.OrderByDescending(j => j.AlbumRating).ToList() as List<Album>;
                int bestAlbumRating = albums.First().AlbumRating;
                int worstAlbumRating = albums.Last().AlbumRating;


                // en iyi ve en kötü puana sahip birden fazla albüm varsa hepsini listelemeye yarayan
                // kod parçası buradadır.
                var bestAlbums = context.Albums.Where(j => j.AlbumRating.Equals(bestAlbumRating)).ToList() as List<Album>;
                if (bestAlbums.Count < 2)
                {
                    bestAlbum = albums.First().AlbumNameRating;
                    
                }
                else
                {
                    for (int i = 0; i < bestAlbums.Count; i++)
                    {
                        bestAlbum = bestAlbum + bestAlbums[i].AlbumNameRating + ", ";
                    }
                    bestAlbum = bestAlbum.Substring(0, bestAlbum.Length - 2);
                }

                var worstAlbums = context.Albums.Where(j => j.AlbumRating.Equals(worstAlbumRating)).ToList() as List<Album>;
                if (worstAlbums.Count < 2)
                {
                    worstAlbum = albums.First().AlbumNameRating;

                }
                else
                {
                    for (int i = 0; i < worstAlbums.Count; i++)
                    {
                        worstAlbum = worstAlbum + worstAlbums[i].AlbumNameRating + ", ";
                    }
                    worstAlbum = worstAlbum.Substring(0, worstAlbum.Length - 2);
                }
            }

            sb.AppendLine(AppResources.TotalAlbumCount + ": " + albumCount);
            sb.AppendLine(AppResources.MostListenCategory + ": " + categoryName);
            sb.AppendLine(AppResources.MostListenArtist + ": " + artistName);
            sb.AppendLine(AppResources.MostListenLabel + ": " + labelName);
            sb.AppendLine(AppResources.BestAlbum + ": " + bestAlbum);
            sb.AppendLine(AppResources.WorstAlbum + ": " + worstAlbum);

            txtStatistics.Text = sb.ToString();
            txtStatistics.IsReadOnly = true;
        }
    }
}