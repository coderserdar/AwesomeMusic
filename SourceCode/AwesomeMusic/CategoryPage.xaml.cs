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
    public partial class CategoryPage : PhoneApplicationPage
    {
        public Popup popup;
        public int categoryId;
        public string oldCategoryName;
        public CategoryPage()
        {
            InitializeComponent();

            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/Add.png", UriKind.Relative);
            button1.Text = AppResources.AddArtist;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(AddArtistButton_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/Delete.png", UriKind.Relative);
            button2.Text = AppResources.DeleteCategory;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(DeleteCategoryButton_Click);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton();
            button3.IconUri = new Uri("/Assets/Settings.png", UriKind.Relative);
            button3.Text = AppResources.CategorySettings;
            ApplicationBar.Buttons.Add(button3);
            button3.Click += new EventHandler(CategorySettingsButton_Click);

            SetBackgroundColor();
            popup = new Popup();
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
            List<Artist> artists = new List<Artist>();
            List<Artist> artistsOrdered = new List<Artist>();

            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);
            using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            {
                var category = context.Categories.Where(j => j.CategoryId.Equals(e.Fragment)).Single() as Category;
                string orderStyle = category.ArtistOrderStyle;
                var categoryArtist = context.CategoryArtists.Where(j => j.CategoryId.Equals(e.Fragment)).ToList() as List<CategoryArtist>;

                foreach (var item in categoryArtist)
                {
                    try
                    {
                        artists.Add(context.Artists.Where(j => j.ArtistId.Equals(item.ArtistId)).Single());
                    }
                    catch (Exception)
                    {
                    }

                }

                switch (category.ArtistOrderBy)
                {
                    case "NAME":
                        if (orderStyle == "A")
                        {
                            artistsOrdered = artists.OrderBy(j => j.ArtistName).ToList();
                        }
                        else
                        {
                            artistsOrdered = artists.OrderByDescending(j => j.ArtistName).ToList();
                        }
                        break;
                    case "ALBUMCOUNT":
                        if (orderStyle == "A")
                        {
                            artistsOrdered = artists.OrderBy(j => j.ArtistAlbumCount).ToList();
                        }
                        else
                        {
                            artistsOrdered = artists.OrderByDescending(j => j.ArtistAlbumCount).ToList();
                        }
                        break;
                    case "CDATE":
                        if (orderStyle == "A")
                        {
                            artistsOrdered = artists.OrderBy(j => j.CreationDate).ToList();
                        }
                        else
                        {
                            artistsOrdered = artists.OrderByDescending(j => j.CreationDate).ToList();
                        }
                        break;
                    case "MDATE":
                        if (orderStyle == "A")
                        {
                            artistsOrdered = artists.OrderBy(j => j.ModificationDate).ToList();
                        }
                        else
                        {
                            artistsOrdered = artists.OrderByDescending(j => j.ModificationDate).ToList();
                        }
                        break;
                    default:
                        if (orderStyle == "A")
                        {
                            artistsOrdered = artists.OrderBy(j => j.ArtistName).ToList();
                        }
                        else
                        {
                            artistsOrdered = artists.OrderByDescending(j => j.ArtistName).ToList();
                        }
                        break;
                }

                lstArtists.Items.Clear();
                categoryId = category.CategoryId;
                lblCategoryName.Text = category.CategoryName;
                lblArtistList.Text = AppResources.ArtistList + " (" + category.CategoryName + ")";
                lstArtists.ItemsSource = artistsOrdered;
                lstArtists.DisplayMemberPath = "ArtistNameCount";
                SetBackgroundColor();
                //lstNoteList.DisplayMemberPath = "NameCreation";
            }
        }

        private void AddArtistButton_Click(object sender, EventArgs e)
        {
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
            control.txtName.Focus();

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

                // aynı isimde bir klasörün daha önceden oluşturulup oluşturulmadığını
                // kontrol eden bir kod bölümü
                using (var contextArtist = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    isCreated =
                        contextArtist.Artists.Any(j => j.ArtistName.Equals(artistName));
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
                        Artist artist = new Artist();
                        artist.ArtistName = artistName;
                        artist.CreationDate = DateTime.Now;
                        artist.ModificationDate = DateTime.Now;
                        artist.ArtistAlbumCount = 0;
                        // burada yazarın kitaplarını
                        // bitirme tarihine göre azalan bir şekilde ayarlamak için gerekli düzenleme yapılıyor
                        artist.AlbumOrderBy = "MDATE";
                        artist.AlbumOrderStyle = "D";
                        artist.ArtistNameCount = artist.ArtistName + " (" + artist.ArtistAlbumCount + ")";
                        //note.NameDescriptionWithoutNewline = note.NameDescription.Replace(Environment.NewLine," ");
                        //note.IsPasswordProtected = false;

                        context.Artists.InsertOnSubmit(artist);
                        context.SubmitChanges();

                        Artist artist3 = context.Artists.Where(j => j.ArtistName.Equals(artistName)).Single() as Artist;

                        CategoryArtist categoryArtist = new CategoryArtist();
                        categoryArtist.CategoryId = categoryId;
                        categoryArtist.ArtistId = artist3.ArtistId;
                        context.CategoryArtists.InsertOnSubmit(categoryArtist);
                        context.SubmitChanges();

                        var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Select(j => j);
                        foreach (var item in category)
                        {
                            item.ModificationDate = DateTime.Now;
                            //item.CategoryNameCount = item.CategoryName + " (" + item.auth + ")";
                        }
                        context.SubmitChanges();

                        var appSettings = context.AppSettings;
                        foreach (var appSetting in appSettings)
                        {
                            appSetting.CurrentCategoryNumber = categoryId;
                        }
                        context.SubmitChanges();

                        List<Artist> artists = new List<Artist>();
                        var categoryArtists = context.CategoryArtists.Where(j => j.CategoryId.Equals(categoryId)).ToList() as List<CategoryArtist>;
                        foreach (var item in categoryArtists)
                        {
                            artists.Add(context.Artists.Where(j => j.ArtistId.Equals(item.ArtistId)).Single());
                        }
                        lstArtists.ItemsSource = artists;
                        MessageBox.Show(AppResources.ArtistAddSuccess);
                        //Artist artist2 = context.Artists.Where(j => j.ArtistName.Equals(artistName)).Single() as Artist;

                        var appSettings2 = context.AppSettings;
                        foreach (var item in appSettings2)
                        {
                            item.CurrentArtistNumber = artist3.ArtistId;
                        }
                        context.SubmitChanges();
                        NavigationService.Navigate(new Uri("/ArtistPage.xaml#" + artist3.ArtistId, UriKind.Relative));
                    }
                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };

            //PhoneApplicationPage_Loaded(this, new RoutedEventArgs());
        }

        private void CategorySettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/CategorySettingsPage.xaml#" + categoryId, UriKind.Relative));
        }

        private void lblCategoryName_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            oldCategoryName = lblCategoryName.Text;
            popup = new Popup();
            popup.Height = 300;
            popup.Width = 400;
            popup.VerticalOffset = 20;
            PopupAddChange control = new PopupAddChange();
            control.txtLabel.Text = AppResources.EnterCategoryName;
            control.btnCancel.Content = AppResources.Cancel;
            control.btnOK.Content = AppResources.OK;
            popup.Child = control;
            popup.IsOpen = true;
            control.txtName.Text = lblCategoryName.Text;
            control.txtName.Focus();
            control.txtName.Select(0, control.txtName.Text.Length);

            control.btnOK.Click += (s, args) =>
            {
                bool isCreated;
                string categoryName;
                popup.IsOpen = false;

                int length = control.txtName.Text.Length;
                string space = control.txtName.Text.Substring(length - Math.Min(1, length));
                if (space == " ")
                {
                    categoryName = control.txtName.Text.Remove(length - 1, 1);
                }
                else
                {
                    categoryName = control.txtName.Text;
                }

                if (categoryName != lblCategoryName.Text)
                {
                    // aynı isimde bir klasörün daha önceden oluşturulup oluşturulmadığını
                    // kontrol eden bir kod bölümü
                    using (var contextFolder = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                    {
                        isCreated =
                            contextFolder.Categories.Any(j => j.CategoryName.Equals(categoryName));
                    }
                    if (isCreated == true)
                    {
                        MessageBox.Show(AppResources.CategoryExists);
                    }
                    // eğer bu isimde bir klasör oluşturulmamışsa
                    // oluşturulması için gerekli kodlar aşağıdadır
                    else
                    {
                        using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                        {

                            // buraya kitapla ilgili bilginin güncelleneceği kod da eklenecek

                            var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Select(j => j);
                            foreach (var item in category)
                            {
                                item.CategoryName = categoryName;
                                item.ModificationDate = DateTime.Now;
                                item.CategoryNameCount = categoryName + " (" + item.CategoryAlbumCount.ToString() + ")";
                            }
                            context.SubmitChanges();

                            var album = context.Albums.Where(j => j.AlbumCategoryId.Equals(categoryId)).Select(j => j);
                            foreach (var item in album)
                            {
                                item.AlbumInformation = item.AlbumInformation.Replace(oldCategoryName, categoryName);
                                item.ModificationDate = DateTime.Now;
                            }
                            context.SubmitChanges();
                            //lstFolders.ItemsSource = context.NoteFolders;
                            //lstArtists.ItemsSource = context.Categories;
                            MessageBox.Show(AppResources.CategoryNameChangeSuccess);
                            popup.IsOpen = false;
                            Category category2 = context.Categories.Where(j => j.CategoryName.Equals(categoryName)).Single() as Category;
                            NavigationService.Navigate(new Uri("/CategoryPage.xaml#" + category2.CategoryId, UriKind.Relative));
                        }
                    }
                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }

        private void DeleteCategoryButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppResources.DeleteCategoryQuestion,
                AppResources.DeleteCategory, MessageBoxButton.OKCancel)
                != MessageBoxResult.OK)
            {

            }
            else
            {
                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    var albums = context.Albums.Where(j => j.AlbumCategoryId.Equals(categoryId)).ToList() as List<Album>;
                    foreach (var item in albums)
                    {
                        var albumArtists = context.AlbumArtists.Where(j => j.AlbumId.Equals(item.AlbumId)).ToList() as List<AlbumArtist>;
                        context.AlbumArtists.DeleteAllOnSubmit(albumArtists);
                    }
                    context.Albums.DeleteAllOnSubmit(albums);

                    var artistCategories = context.CategoryArtists.Where(j => j.CategoryId.Equals(categoryId)).ToList() as List<CategoryArtist>;
                    foreach (var item in artistCategories)
                    {
                        var artist = context.Artists.Where(j => j.ArtistId.Equals(item.ArtistId)).ToList() as List<Artist>;
                        context.Artists.DeleteAllOnSubmit(artist);
                    }
                    context.CategoryArtists.DeleteAllOnSubmit(artistCategories);

                    var categories = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Single() as Category;
                    context.Categories.DeleteOnSubmit(categories);

                    context.SubmitChanges();
                }
                MessageBox.Show(AppResources.CategoryDeleteSuccess);
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            //MessageBox.Show(AppResources.NoteSaved);
        }

        private void lstArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var artist = (Artist)lstArtists.SelectedItem;
            int artistId = artist.ArtistId;
            using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings;
                foreach (var item in appSettings)
                {
                    item.CurrentArtistNumber = artistId;
                }
                context.SubmitChanges();
            }
            NavigationService.Navigate(new Uri("/ArtistPage.xaml#" + artistId, UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (popup.IsOpen)
            {
                popup.IsOpen = false;
            }
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //SetBackgroundColor();
        }
    }
}