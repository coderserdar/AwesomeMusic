using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Data.Common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Shell;
using AwesomeMusic.Resources;

namespace AwesomeMusic
{
    public partial class ArtistPage : PhoneApplicationPage
    {

        public int artistId;
        public int categoryId;
        public int albumId;
        public Popup popup;
        public string oldArtistName;

        public ArtistPage()
        {
            InitializeComponent();

            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/Add.png", UriKind.Relative);
            button1.Text = AppResources.AddAlbum;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(AddAlbumButton_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/Delete.png", UriKind.Relative);
            button2.Text = AppResources.DeleteArtist;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(DeleteArtistButton_Click);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton();
            button3.IconUri = new Uri("/Assets/Settings.png", UriKind.Relative);
            button3.Text = AppResources.ArtistSettings;
            ApplicationBar.Buttons.Add(button3);
            button3.Click += new EventHandler(ArtistSettingsButton_Click);

            ApplicationBarIconButton button4 = new ApplicationBarIconButton();
            button4.IconUri = new Uri("/Assets/AddCategory.png", UriKind.Relative);
            button4.Text = AppResources.AddCategory;
            ApplicationBar.Buttons.Add(button4);
            button4.Click += new EventHandler(AddCategoryButton_Click);

            popup = new Popup();

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
            List<Album> albums = new List<Album>();
            List<Album> albumsOrdered = new List<Album>();

            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);

            lstAlbums.Items.Clear();

            using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            {

                var appSettings = context.AppSettings.First();
                categoryId = appSettings.CurrentCategoryNumber;

                var artist = context.Artists.Where(j => j.ArtistId.Equals(e.Fragment)).Single() as Artist;
                artistId = artist.ArtistId;

                var appSettings2 = context.AppSettings;
                foreach (var item in appSettings2)
                {
                    item.CurrentArtistNumber = artistId;
                }
                context.SubmitChanges();

                var artistAlbums = context.AlbumArtists.Where(j => j.ArtistId.Equals(e.Fragment)).ToList() as List<AlbumArtist>;
                if (artistAlbums.Count != 0)
                {
                    foreach (var item in artistAlbums)
                    {
                        try
                        {
                            var album = context.Albums.Where(j => j.AlbumCategoryId.Equals(categoryId) && j.AlbumId.Equals(item.AlbumId)).Single() as Album;
                            albums.Add(album);
                        }
                        catch (Exception)
                        {
                        }
                    }

                }


                string orderStyle = artist.AlbumOrderStyle;

                switch (artist.AlbumOrderBy)
                {
                    case "NAME":
                        if (orderStyle == "A")
                        {
                            albumsOrdered = albums.OrderBy(j => j.AlbumName).ToList();
                        }
                        else
                        {
                            albumsOrdered = albums.OrderByDescending(j => j.AlbumName).ToList();
                        }
                        break;
                    case "CDATE":
                        if (orderStyle == "A")
                        {
                            albumsOrdered = albums.OrderBy(j => j.CreationDate).ToList();
                        }
                        else
                        {
                            albumsOrdered = albums.OrderByDescending(j => j.CreationDate).ToList();
                        }
                        break;
                    case "MDATE":
                        if (orderStyle == "A")
                        {
                            albumsOrdered = albums.OrderBy(j => j.ModificationDate).ToList();
                        }
                        else
                        {
                            albumsOrdered = albums.OrderByDescending(j => j.ModificationDate).ToList();
                        }
                        break;
                    case "RATING":
                        if (orderStyle == "A")
                        {
                            albumsOrdered = albums.OrderBy(j => j.AlbumRating).ToList();
                        }
                        else
                        {
                            albumsOrdered = albums.OrderByDescending(j => j.AlbumRating).ToList();
                        }
                        break;
                    //case "SDATE":
                    //    if (orderStyle == "A")
                    //    {
                    //        albumsOrdered = albums.OrderBy(j => j.ReadStartDate).ToList();
                    //    }
                    //    else
                    //    {
                    //        albumsOrdered = albums.OrderByDescending(j => j.ReadStartDate).ToList();
                    //    }
                    //    break;
                    //case "FDATE":
                    //    if (orderStyle == "A")
                    //    {
                    //        albumsOrdered = albums.OrderBy(j => j.ReadFinishDate).ToList();
                    //    }
                    //    else
                    //    {
                    //        albumsOrdered = albums.OrderByDescending(j => j.ReadFinishDate).ToList();
                    //    }
                    //    break;
                    default:
                        if (orderStyle == "A")
                        {
                            albumsOrdered = albums.OrderBy(j => j.AlbumName).ToList();
                        }
                        else
                        {
                            albumsOrdered = albums.OrderBy(j => j.AlbumName).ToList();
                        }
                        break;
                }

                lblArtistName.Text = artist.ArtistName;
                lblAlbumList.Text = AppResources.AlbumList + " (" + artist.ArtistName + ")";
                lstAlbums.ItemsSource = albumsOrdered;
                lstAlbums.DisplayMemberPath = "AlbumNameRating";
                SetBackgroundColor();
                //lstNoteList.DisplayMemberPath = "NameCreation";
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

        private void lstAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var album = (Album)lstAlbums.SelectedItem;
            int albumId = album.AlbumId;
            NavigationService.Navigate(new Uri("/AlbumPage.xaml#" + albumId, UriKind.Relative));
        }

        private void AddAlbumButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AlbumPage.xaml", UriKind.Relative));
        }

        private void DeleteArtistButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppResources.DeleteArtistQuestion,
                AppResources.DeleteArtist, MessageBoxButton.OKCancel)
                != MessageBoxResult.OK)
            {

            }
            else
            {
                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    var albumArtists = context.AlbumArtists.Where(j => j.ArtistId.Equals(artistId)).ToList() as List<AlbumArtist>;
                    foreach (var item in albumArtists)
                    {
                        var album = context.Albums.Where(j => j.AlbumId.Equals(item.AlbumId)).Single() as Album;
                        var albumArtists2 = context.AlbumArtists.Where(j => j.AlbumId.Equals(albumId)).ToList() as List<AlbumArtist>;
                        context.AlbumArtists.DeleteAllOnSubmit(albumArtists2);
                        context.Albums.DeleteOnSubmit(album);
                    }

                    var categoryArtists = context.CategoryArtists.Where(j => j.ArtistId.Equals(artistId)).ToList() as List<CategoryArtist>;
                    context.CategoryArtists.DeleteAllOnSubmit(categoryArtists);

                    var artists = context.Artists.Where(j => j.ArtistId.Equals(artistId)).Single() as Artist;
                    context.Artists.DeleteOnSubmit(artists);

                    context.SubmitChanges();

                    var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Select(j => j);
                    foreach (var item in category)
                    {
                        item.CategoryAlbumCount = context.Albums.Where(j => j.AlbumCategoryId.Equals(item.CategoryId)).ToList().Count;
                        item.CategoryNameCount = item.CategoryName + " (" + item.CategoryAlbumCount + ")";
                        item.ModificationDate = DateTime.Now;
                        context.SubmitChanges();
                    }
                }
                MessageBox.Show(AppResources.ArtistDeleteSuccess);
                NavigationService.Navigate(new Uri("/CategoryPage.xaml#" + categoryId, UriKind.Relative));
            }
            //MessageBox.Show(AppResources.NoteSaved);
        }

        private void lblArtistName_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            oldArtistName = lblArtistName.Text;
            popup = new Popup();
            popup.Height = 300;
            popup.Width = 400;
            popup.VerticalOffset = 20;
            PopupAddChange control = new PopupAddChange();
            control.txtLabel.Text = AppResources.EnterArtistName;
            control.btnCancel.Content = AppResources.Cancel;
            control.btnOK.Content = AppResources.OK;
            popup.Child = control;
            popup.IsOpen = true;
            control.txtName.Text = lblArtistName.Text;
            control.txtName.Focus();
            control.txtName.Select(0, control.txtName.Text.Length);

            control.btnOK.Click += (s, args) =>
            {
                bool isCreated;
                string artistName;
                popup.IsOpen = false;
                int length = control.txtName.Text.Length;
                string space = control.txtName.Text.Substring(length - Math.Min(1, length));
                if (space == " ")
                {
                    artistName = control.txtName.Text.Remove(length - 1, 1);
                }
                else
                {
                    artistName = control.txtName.Text;
                }

                if (artistName != lblArtistName.Text)
                {
                    // aynı isimde bir klasörün daha önceden oluşturulup oluşturulmadığını
                    // kontrol eden bir kod bölümü
                    using (var contextFolder = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                    {
                        isCreated =
                            contextFolder.Artists.Any(j => j.ArtistName.Equals(artistName));
                    }
                    if (isCreated == true)
                    {
                        MessageBox.Show(AppResources.ArtistExists);
                    }
                    // eğer bu isimde bir klasör oluşturulmamışsa
                    // oluşturulması için gerekli kodlar aşağıdadır
                    else
                    {
                        using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                        {

                            // buraya kitapla ilgili bilginin güncelleneceği kod da eklenecek

                            var artist = context.Artists.Where(j => j.ArtistId.Equals(artistId)).Select(j => j);
                            foreach (var item in artist)
                            {
                                item.ArtistName = artistName;
                                item.ModificationDate = DateTime.Now;
                                item.ArtistNameCount = artistName + " (" + item.ArtistAlbumCount.ToString() + ")";
                            }
                            context.SubmitChanges();

                            var albumArtists = context.AlbumArtists.Where(j => j.ArtistId.Equals(artistId)).Select(j => j);
                            foreach (var item in albumArtists)
                            {
                                var album = context.Albums.Where(j => j.AlbumId.Equals(item.AlbumId)).Select(j => j);
                                foreach (var item2 in album)
                                {
                                    item2.AlbumInformation = item2.AlbumInformation.Replace(oldArtistName, artistName);
                                    item2.ModificationDate = DateTime.Now;
                                    context.SubmitChanges();
                                }
                            }

                            //lstFolders.ItemsSource = context.NoteFolders;
                            //lstArtists.ItemsSource = context.Categories;
                            MessageBox.Show(AppResources.ArtistNameChangeSuccess);
                            popup.IsOpen = false;
                            CategoryArtist categoryArtist = context.CategoryArtists.Where(j => j.ArtistId.Equals(artistId) && j.CategoryId.Equals(categoryId)).Single() as CategoryArtist;
                            Artist artist2 = context.Artists.Where(j => j.ArtistName.Equals(artistName) && j.ArtistId.Equals(categoryArtist.ArtistId)).Single() as Artist;
                            NavigationService.Navigate(new Uri("/ArtistPage.xaml#" + categoryArtist.ArtistId, UriKind.Relative));
                        }
                    }
                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (popup.IsOpen)
            {
                popup.IsOpen = false;
            }
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/CategoryPage.xaml#" + categoryId, UriKind.Relative));
            }
        }

        private void ArtistSettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ArtistSettingsPage.xaml#" + artistId, UriKind.Relative));
        }

        private void AddCategoryButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddCategoryPage.xaml#" + artistId, UriKind.Relative));
        }

    }
}