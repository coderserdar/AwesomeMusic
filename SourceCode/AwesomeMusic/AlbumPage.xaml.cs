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
    public partial class AlbumPage : PhoneApplicationPage
    {
        public int artistId;
        public string artistName;
        public string categoryName;
        public int categoryId;
        public int albumId;
        public string pageName;
        double InputHeight = 0.0;
        public bool flag;
        public bool isFilled;
        public double ratingValue = 0;

        public AlbumPage()
        {
            InitializeComponent();

            SetBackgroundColor();

            //pvArtist.Title = artistName;
            piAlbumName.Header = AppResources.AlbumName;
            piComment.Header = AppResources.AlbumComment;
            piLabelName.Header = AppResources.LabelName;
            piRating.Header = AppResources.AlbumRating;
            piReleaseYear.Header = AppResources.ReleaseYear;
            piBestSong.Header = AppResources.BestSong;
            piSongCount.Header = AppResources.SongCount;


            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/Save.png", UriKind.Relative);
            button1.Text = AppResources.Save;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(SaveButton_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/SendWithMail.png", UriKind.Relative);
            button2.Text = AppResources.SendWithMail;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(SendMailButton_Click);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton();
            button3.IconUri = new Uri("/Assets/SendWithSMS.png", UriKind.Relative);
            button3.Text = AppResources.SendWithSMS;
            ApplicationBar.Buttons.Add(button3);
            button3.Click += new EventHandler(SendSMSButton_Click);

            ApplicationBarIconButton button4 = new ApplicationBarIconButton();
            button4.IconUri = new Uri("/Assets/Share.png", UriKind.Relative);
            button4.Text = AppResources.ShareAlbum;
            ApplicationBar.Buttons.Add(button4);
            button4.Click += new EventHandler(ShareAlbumButton_Click);

            isFilled = false;

            ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem();
            menuItem1.Text = AppResources.DeleteAlbum;
            ApplicationBar.MenuItems.Add(menuItem1);
            menuItem1.Click += new EventHandler(DeleteAlbumMenuItem_Click);

        }

        private void SendSMSButton_Click(object sender, EventArgs e)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();

            smsComposeTask.To = "";
            smsComposeTask.Body = CreateSendMaterial();

            smsComposeTask.Show();
            //MessageBox.Show(AppResources.SuccessfulSendWithSMS);
        }

        private void ShareAlbumButton_Click(object sender, EventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();

            shareStatusTask.Status = CreateSendMaterial();

            shareStatusTask.Show();
            //MessageBox.Show(AppResources.SuccessfulSendWithSMS);
        }

        private void SendMailButton_Click(object sender, EventArgs e)
        {
            // burada birden fazla e-posta hesabı varsa birini seçmesi söyleniyor
            //EmailAddressChooserTask emailAddressChooserTask;
            //emailAddressChooserTask = new EmailAddressChooserTask();
            //emailAddressChooserTask.Completed += new EventHandler<EmailResult>(emailAddressChooserTask_Completed);
            //emailAddressChooserTask.Show();

            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = txtAlbumName.Text;
            emailComposeTask.Body = CreateSendMaterial();
            emailComposeTask.To = "";
            emailComposeTask.Cc = "";
            emailComposeTask.Bcc = "";

            emailComposeTask.Show();
            //MessageBox.Show(AppResources.SuccessfulSendWithMail);
        }

        private string CreateSendMaterial()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(AppResources.AlbumName + ": " + txtAlbumName.Text);
            sb.AppendLine(AppResources.CategoryName + ": " + categoryName);
            sb.AppendLine(AppResources.ArtistName + ": " + artistName);
            sb.AppendLine(AppResources.ReleaseYear + ": " + txtReleaseYear.Text);
            sb.AppendLine(AppResources.SongCount + ": " + txtSongCount.Text);
            sb.AppendLine(AppResources.LabelName + ": " + txtLabelName.Text);
            sb.AppendLine(AppResources.BestSong + ": " + txtBestSong.Text);
            sb.AppendLine(AppResources.AlbumComment + ": " + txtAlbumComment.Text);
            sb.AppendLine(AppResources.AlbumRating + ": " + rtRating.Value.ToString() + "/10");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(AppResources.SendWithApp);
            return sb.ToString();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //SetBackgroundColor();

            // yazarın adı sayfanın en üstünde görünsün diye yapılıyor bu
            //pvArtist.Title = artistName;

            //pvArtist.Title = artistName;
            //piAlbumName.Header = AppResources.AlbumName;
            //piComment.Header = AppResources.AlbumComment;
            //piLabelName.Header = AppResources.LabelName;
            //piRating.Header = AppResources.AlbumRating;
            //piStartFinishDate.Header = AppResources.Date;
            //lblStartDate.Text = AppResources.StartDate;
            //lblFinishDate.Text = AppResources.FinishDate;
            //piReleaseYear.Header = AppResources.ReleaseYear;
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
            using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings.First();
                categoryId = appSettings.CurrentCategoryNumber;
                artistId = appSettings.CurrentArtistNumber;

                // sayfanın font ayarları için yapılan bir değişiklik
                FontFamily temp = new FontFamily(appSettings.FontFamily);
                double fontsize = double.Parse(appSettings.FontSize);
                txtAlbumComment.FontFamily = temp;
                txtAlbumComment.FontSize = fontsize;
                txtReleaseYear.FontFamily = temp;
                txtReleaseYear.FontSize = fontsize;
                txtAlbumName.FontFamily = temp;
                txtAlbumName.FontSize = fontsize;
                txtAlbumComment.FontFamily = temp;
                txtAlbumComment.FontSize = fontsize;
                txtLabelName.FontFamily = temp;
                txtLabelName.FontSize = fontsize;
                txtBestSong.FontFamily = temp;
                txtBestSong.FontSize = fontsize;
                txtSongCount.FontFamily = temp;
                txtSongCount.FontSize = fontsize;
                // oylamada kolaylık olması için otomatik olarak 5 veriliyor
                // sonradan istenirse 0 da verilebilir.
                rtRating.Value = 5;

                var artist = context.Artists.Where(j => j.ArtistId.Equals(artistId)).Single() as Artist;
                artistName = artist.ArtistName;

                var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Single() as Category;
                categoryName = category.CategoryName;
            }

            var lastPage = NavigationService.BackStack.FirstOrDefault();
            pageName = lastPage.Source.ToString();
            pvArtist.SelectedIndex = 0;
            txtAlbumName.Focus();
            // yazarın adı sayfanın en üstünde görünsün diye yapılıyor bu
            pvArtist.Title = artistName;
            SetBackgroundColor();
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
            albumId = int.Parse(e.Fragment);
            if (pageName.Contains("/ArtistPage.xaml"))
            {
                isFilled = true;
            }
            else
            {
                //using (var context2 = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                //{
                //    var appSettings = context2.AppSettings; ;
                //    var album2 = context2.Albums.Where(j => j.AlbumId.Equals(albumId)) as Album;
                //    var albumArtist = context2.AlbumArtists.Where(j => j.AlbumId.Equals(albumId)).ToList() as List<AlbumArtist>;
                //    var bArtist = albumArtist.First();
                //    var artist = context2.Artists.Where(j => j.ArtistId.Equals(bArtist.ArtistId)) as Artist;
                //    foreach (var item in appSettings)
                //    {
                //        item.CurrentArtistNumber = artist.ArtistId;
                //        item.CurrentCategoryNumber = album2.AlbumCategoryId;
                //    }
                //    context2.SubmitChanges();
                //    pvArtist.Title = artist.ArtistName;
                //}
            }
            using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            {
                var album = context.Albums.Where(j => j.AlbumId.Equals(e.Fragment)).Single() as Album;

                txtAlbumName.Text = album.AlbumName == "" ? "" : album.AlbumName;
                txtSongCount.Text = album.AlbumSongCount.ToString() == "" ? "" : album.AlbumSongCount.ToString();
                txtReleaseYear.Text = album.AlbumReleaseYear.ToString() == "" ? "" : album.AlbumReleaseYear.ToString();
                txtLabelName.Text = album.AlbumLabelName == "" ? "" : album.AlbumLabelName;
                txtBestSong.Text = album.AlbumBestSong == "" ? "" : album.AlbumBestSong;
                //dtStart.Value = album.ReadStartDate == null ? DateTime.Now : album.ReadStartDate;
                //dtFinish.Value = album.ReadFinishDate == null ? DateTime.Now : album.ReadFinishDate;
                rtRating.Value = album.AlbumRating == null ? 0 : album.AlbumRating;
                txtAlbumComment.Text = album.AlbumComment == "" ? "" : album.AlbumComment;
            }

            SetBackgroundColor();
            pvArtist.SelectedIndex = 0;
            //pvArtist.Name = artistName;
            txtAlbumName.Focus();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //SaveButton_Click(this, new EventArgs());
            if (pageName.Contains("/SearchPage.xaml"))
            {
                //this.NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/ArtistPage.xaml#" + artistId, UriKind.Relative));
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            txtAlbumComment_LostFocus(this, new RoutedEventArgs());
            this.pnlKeyboardPlaceHolder.Visibility = Visibility.Collapsed;
            if (txtAlbumName.Text.Trim().Length < 1)
            {
                MessageBox.Show(AppResources.AlbumNameMustBe);
            }
            else
            {
                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    if (isFilled || pageName.Contains("/SearchPage.xaml"))
                    {
                        var album = context.Albums.Where(j => j.AlbumId.Equals(albumId)).Select(j => j);
                        foreach (var item in album)
                        {
                            item.AlbumCategoryId = categoryId;
                            item.AlbumName = txtAlbumName.Text == "" ? "" : txtAlbumName.Text;
                            item.AlbumReleaseYear = txtReleaseYear.Text == "" ? 0 : int.Parse(txtReleaseYear.Text);
                            item.AlbumLabelName = txtLabelName.Text == "" ? "" : txtLabelName.Text;
                            item.AlbumSongCount = txtSongCount.Text == "" ? 0 : int.Parse(txtSongCount.Text);
                            item.AlbumBestSong = txtBestSong.Text == "" ? "" : txtBestSong.Text;
                            //item.ReadStartDate = DateTime.Parse(dtStart.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtStart.Value.ToString());
                            //item.ReadFinishDate = DateTime.Parse(dtFinish.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtFinish.Value.ToString()); ;
                            //item.AlbumRating = rtRating.Value.ToString() == "" ? 0 : int.Parse(rtRating.Value.ToString());
                            item.AlbumRating = int.Parse(ratingValue.ToString()) == 0 ? 0 : int.Parse(ratingValue.ToString());
                            item.AlbumComment = txtAlbumComment.Text == "" ? "" : txtAlbumComment.Text;
                            item.ModificationDate = DateTime.Now;
                            item.AlbumInformation = categoryName + " " + artistName + " " + txtAlbumName.Text + " " + txtLabelName.Text + " " + txtAlbumComment.Text;
                            item.AlbumNameRating = item.AlbumName + " (" + item.AlbumRating + "/10)";
                        }
                        context.SubmitChanges();
                    }
                    else
                    {
                        Album album = new Album();
                        album.AlbumCategoryId = categoryId;
                        album.AlbumGuid = Guid.NewGuid().ToString();
                        album.AlbumName = txtAlbumName.Text == "" ? "" : txtAlbumName.Text;
                        album.AlbumReleaseYear = txtReleaseYear.Text == "" ? 0 : int.Parse(txtReleaseYear.Text);
                        album.AlbumLabelName = txtLabelName.Text == "" ? "" : txtLabelName.Text;
                        album.AlbumSongCount = txtSongCount.Text == "" ? 0 : int.Parse(txtSongCount.Text);
                        album.AlbumBestSong = txtBestSong.Text == "" ? "" : txtBestSong.Text;

                        //album.ReadStartDate = DateTime.Parse(dtStart.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtStart.Value.ToString());
                        //album.ReadFinishDate = DateTime.Parse(dtFinish.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtFinish.Value.ToString()); ;
                        //album.AlbumRating = rtRating.Value.ToString() == "" ? 0 : int.Parse(rtRating.Value.ToString());
                        album.AlbumRating = int.Parse(ratingValue.ToString()) == 0 ? 0 : int.Parse(ratingValue.ToString());
                        album.AlbumComment = txtAlbumComment.Text == "" ? "" : txtAlbumComment.Text;
                        album.ModificationDate = DateTime.Now;
                        album.AlbumInformation = categoryName + " " + artistName + " " + album.AlbumName + " " + album.AlbumReleaseYear.ToString() + " " + album.AlbumLabelName + " " + album.AlbumComment;
                        album.CreationDate = DateTime.Now;
                        album.AlbumNameRating = album.AlbumName + " (" + album.AlbumRating + "/10)";
                        context.Albums.InsertOnSubmit(album);
                        context.SubmitChanges();

                        Album album2 = context.Albums.Where(j => j.AlbumGuid.Equals(album.AlbumGuid)).Single() as Album;

                        AlbumArtist albumArtist = new AlbumArtist();
                        albumArtist.ArtistId = artistId;
                        albumArtist.AlbumId = album2.AlbumId;
                        context.AlbumArtists.InsertOnSubmit(albumArtist);
                        context.SubmitChanges();

                        var category = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Select(j => j);
                        foreach (var item in category)
                        {
                            item.CategoryAlbumCount = item.CategoryAlbumCount + 1;
                            item.CategoryNameCount = item.CategoryName + " (" + item.CategoryAlbumCount + ")";
                            item.ModificationDate = DateTime.Now;
                        }
                        context.SubmitChanges();

                        var artist = context.Artists.Where(j => j.ArtistId.Equals(artistId)).Select(j => j);
                        foreach (var item in artist)
                        {
                            item.ArtistAlbumCount = item.ArtistAlbumCount + 1;
                            item.ArtistNameCount = item.ArtistName + " (" + item.ArtistAlbumCount + ")";
                            item.ModificationDate = DateTime.Now;
                        }
                        context.SubmitChanges();
                    }
                }
                MessageBox.Show(AppResources.AlbumSaveSuccess);
            }
            isFilled = false;
        }

        private void txtAlbumComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                double CurrentInputHeight = txtAlbumComment.ActualHeight;

                if (CurrentInputHeight > InputHeight)
                {
                    svAlbumComment.ScrollToVerticalOffset(svAlbumComment.VerticalOffset + CurrentInputHeight - InputHeight);
                }

                InputHeight = CurrentInputHeight;
            });
        }

        private void txtAlbumComment_GotFocus(object sender, RoutedEventArgs e)
        {
            App.RootFrame.RenderTransform = new CompositeTransform();
            flag = true;
        }

        private void txtAlbumComment_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            txtAlbumComment.Focus();
            //txtAlbumComment.Select(txtAlbumComment.Text.Length, 1);
            svAlbumComment.ScrollToVerticalOffset(e.GetPosition(txtAlbumComment).Y - 80);
        }

        private void txtAlbumComment_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!flag) return;
            txtAlbumComment.Focus();
            flag = false;
            this.pnlKeyboardPlaceHolder.Visibility = Visibility.Collapsed;
        }

        private void txtAlbumComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                svAlbumComment.ScrollToVerticalOffset(txtAlbumComment.ActualHeight);
            }
        }

        private void svAlbumComment_GotFocus(object sender, RoutedEventArgs e)
        {
            this.svAlbumComment.ScrollToVerticalOffset(this.txtAlbumComment.ActualHeight);
            this.svAlbumComment.UpdateLayout();
        }

        private void txtAlbumName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pvArtist.SelectedIndex = 1;
                txtReleaseYear.Focus();
            }
        }

        private void txtReleaseYear_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pvArtist.SelectedIndex = 2;
                txtSongCount.Focus();
            }
        }

        private void txtLabelName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pvArtist.SelectedIndex = 4;
                txtBestSong.Focus();
            }
        }


        private void rtRating_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    pvArtist.SelectedIndex = 5;
            //    txtAlbumComment.Focus();
            //}
        }

        private void rtRating_ValueChanged(object sender, EventArgs e)
        {
            //pvArtist.SelectedIndex = 5;
            ratingValue = rtRating.Value;
            //txtAlbumComment.Focus();
        }

        private void dtFinish_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            //if (isFilled)
            //{
            //    using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            //    {
            //        var album = context.Albums.Where(j => j.AlbumId.Equals(albumId)).Select(j => j);
            //        foreach (var item in album)
            //        {
            //            item.ReadFinishDate = DateTime.Parse(dtFinish.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtFinish.Value.ToString());
            //            item.ModificationDate = DateTime.Now;
            //        }
            //        context.SubmitChanges();
            //    }
            //}
            //pvArtist.SelectedIndex = 4;
            //rtRating.Focus();
        }

        private void dtStart_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            //if (isFilled)
            //{
            //    using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
            //    {
            //        var album = context.Albums.Where(j => j.AlbumId.Equals(albumId)).Select(j => j);
            //        foreach (var item in album)
            //        {
            //            item.ReadStartDate = DateTime.Parse(dtStart.Value.ToString()) == null ? DateTime.Now : DateTime.Parse(dtStart.Value.ToString());
            //            item.ModificationDate = DateTime.Now;
            //        }
            //        context.SubmitChanges();
            //    }
            //}
            //pvArtist.SelectedIndex = 3;
            //dtFinish.Focus();
        }

        private void rtRating_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //pvArtist.SelectedIndex = 5;
            //ratingValue = rtRating.Value;
            //txtAlbumComment.Focus();
        }

        private void DeleteAlbumMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppResources.DeleteAlbumQuestion,
                AppResources.DeleteAlbum, MessageBoxButton.OKCancel)
                != MessageBoxResult.OK)
            {

            }
            else
            {
                using (var context = new AwesomeMusicDataContext(AwesomeMusicDataContext.ConnectionString))
                {
                    var album = context.Albums.Where(j => j.AlbumId.Equals(albumId)).Single() as Album;
                    var albumArtists = context.AlbumArtists.Where(j => j.AlbumId.Equals(albumId)).ToList() as List<AlbumArtist>;
                    context.AlbumArtists.DeleteAllOnSubmit(albumArtists);
                    context.Albums.DeleteOnSubmit(album);

                    var artists = context.Artists.Where(j => j.ArtistId.Equals(artistId)).Select(j => j);
                    foreach (var item in artists)
                    {
                        item.ModificationDate = DateTime.Now;
                        item.ArtistAlbumCount = item.ArtistAlbumCount - 1;
                        item.ArtistNameCount = item.ArtistName + " (" + item.ArtistAlbumCount + ")";
                    }
                    context.SubmitChanges();

                    var categories = context.Categories.Where(j => j.CategoryId.Equals(categoryId)).Select(j => j);
                    foreach (var item in categories)
                    {
                        item.ModificationDate = DateTime.Now;
                        item.CategoryAlbumCount = item.CategoryAlbumCount - 1;
                        item.CategoryNameCount = item.CategoryName + " (" + item.CategoryAlbumCount + ")";
                    }
                    context.SubmitChanges();
                }
                MessageBox.Show(AppResources.AlbumDeleteSuccess);
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            //MessageBox.Show(AppResources.NoteSaved);
        }

        private void btnIncrease_Click(object sender, RoutedEventArgs e)
        {
            if (rtRating.Value != 10.0)
            {
                rtRating.Value = rtRating.Value + 1.0;
            }
        }

        private void btnDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (rtRating.Value != 0.0)
            {
                rtRating.Value = rtRating.Value - 1.0;
            }
        }

        private void txtSongCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pvArtist.SelectedIndex = 3;
                txtLabelName.Focus();
            }
        }

        private void txtBestSong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pvArtist.SelectedIndex = 5;
                rtRating.Focus();
            }
        }
    }
}