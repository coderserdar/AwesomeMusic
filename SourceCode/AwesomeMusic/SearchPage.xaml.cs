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
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
            SetBackgroundColor();

            txtSearchResult.Text = AppResources.SearchResults;
            lblSearch.Text = AppResources.Search;
            //btnSearch.Content = AppResources.Search;
            //lstSearch.SelectedIndex = -1;
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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text.TrimStart().Length < 1)
            {
                MessageBox.Show(AppResources.SearchTrimFault);
            }
            else
            {
                lstSearch.Items.Clear();
                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    var albumList =
                        context.Albums.Where(j => j.AlbumInformation.ToLower().Contains(txtSearch.Text.ToLower())).ToList() as List<Album>;
                    //var noteList = context.Notes.ToList() as List<Note>;

                    if (albumList != null)
                    {
                        txtSearchResult.Text = AppResources.SearchResults + " (" + albumList.Count() + ")";
                    }

                    //lstSearch.ItemsSource = noteList;
                    for (int i = 0; i < albumList.Count; i++)
                    {
                        //if (noteList[i].NameDescriptionWithoutNewline.ToLower(Thread.CurrentThread.CurrentCulture).IndexOf(txtSearch.Text.ToLower(Thread.CurrentThread.CurrentCulture)) != -1)
                        //{
                        lstSearch.Items.Add(albumList[i] as Album);
                        //}
                    }
                    //lstSearch.ItemTemplate.
                    //lstSearch.DisplayMemberPath = "NoteName" + " (" + "CreationDate" + ")";
                    lstSearch.DisplayMemberPath = "AlbumNameRating";
                    MessageBox.Show(AppResources.SearchCompleted);
                }
            }
        }

        private void lstSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstSearch.SelectedIndex != -1)
                {

                    Album selectedAlbum = lstSearch.SelectedItem as Album;
                    using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                    {
                        var appSettings = context.AppSettings;
                        var category =
                            context.Categories.Where(j => j.CategoryId.Equals(selectedAlbum.AlbumCategoryId)).Single() as
                                Category;

                        var artistAlbum = context.AlbumArtists.Where(j => j.AlbumId.Equals(selectedAlbum.AlbumId)).Single() as AlbumArtist;

                        foreach (var item in appSettings)
                        {
                            item.CurrentCategoryNumber = category.CategoryId;
                            item.CurrentArtistNumber = artistAlbum.ArtistId;
                        }
                        context.SubmitChanges();

                        NavigationService.Navigate(new Uri("/AlbumPage.xaml#" + selectedAlbum.AlbumId, UriKind.Relative));
                    }
                    lstSearch.SelectedIndex = -1;
                }

            }
            catch (Exception)
            {
                MessageBox.Show(AppResources.SystemFault);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void txtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtSearch.Text.TrimStart().Length < 1)
                {
                    MessageBox.Show(AppResources.SearchTrimFault);
                }
                else
                {
                    lstSearch.Items.Clear();
                    using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                    {
                        var albumList =
                            context.Albums.Where(j => j.AlbumInformation.ToLower().Contains(txtSearch.Text.ToLower())).ToList() as List<Album>;
                        //var noteList = context.Notes.ToList() as List<Note>;

                        if (albumList != null)
                        {
                            txtSearchResult.Text = AppResources.SearchResults + " (" + albumList.Count() + ")";
                        }

                        //lstSearch.ItemsSource = noteList;
                        for (int i = 0; i < albumList.Count; i++)
                        {
                            //if (noteList[i].NameDescriptionWithoutNewline.ToLower(Thread.CurrentThread.CurrentCulture).IndexOf(txtSearch.Text.ToLower(Thread.CurrentThread.CurrentCulture)) != -1)
                            //{
                            lstSearch.Items.Add(albumList[i] as Album);
                            //}
                        }
                        //lstSearch.ItemTemplate.
                        //lstSearch.DisplayMemberPath = "NoteName" + " (" + "CreationDate" + ")";
                        lstSearch.DisplayMemberPath = "AlbumNameRating";
                        MessageBox.Show(AppResources.SearchCompleted);
                    }
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            txtSearch.Focus();
        }
    }
}