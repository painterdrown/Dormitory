using Dormitory.Models;
using System;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Dormitory.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WriteJournal : Page
    {

        public WriteJournal()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }
        JournalItem jItem;
        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            jItem.message = "back";
            Frame.Navigate(typeof(Info), jItem);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //返回的条件设置
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
            jItem = ((JournalItem)e.Parameter);
            if (jItem == null)
            {
                jItem = new JournalItem();
                jItem.message = "create";
                createButton.Content = "发布";
                createButton.Click += CreateButton_Clicked;
                cancelButton.Content = "清空";
                cancelButton.Click += ClearButton_Clicked;
                var i = new MessageDialog("欢迎发布日志").ShowAsync();
            }
            else
            {
                jItem.message = "update";
                createButton.Content = "更新";
                cancelButton.Content = "删除";
                createButton.Click += UpdateButton_Clicked;
                cancelButton.Click += DeleteButton_Clicked;
                var i = new MessageDialog("欢迎修改你的日志").ShowAsync();

                Details.Text = jItem.content;
                photo.Source = new BitmapImage(jItem.pic);
            }
        }

        private void HomeAppButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Info), jItem);
        }

        private void CheckAppButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DutyAppButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void selectPhoto(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.DecodePixelWidth = 600;
                    await bitmapImage.SetSourceAsync(fileStream);
                    photo.Source = bitmapImage;
                }
            }
            else
            {
                var i = new MessageDialog("select picture operation cancelled!").ShowAsync();
            }
        }
        private void CreateButton_Clicked(object sender, RoutedEventArgs e)
        {
            if(Details.Text == "")
            {
                var WrongMessage = new MessageDialog("内容不能为空!").ShowAsync();
            }else
            {
                var uri = (photo.Source as BitmapImage).UriSource;
                var content = Details.Text;
                jItem.pic = uri;
                jItem.content = content;
                jItem.date = DateTime.Now;

                Frame.Navigate(typeof(Info), jItem);
            }
        }
        private void UpdateButton_Clicked(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            jItem.message = "delete";
            Frame.Navigate(typeof(Info), jItem);
        }

        private void ClearButton_Clicked(object sender, RoutedEventArgs e)
        {
            Details.Text = "";
            Image image = photo as Image;
            BitmapImage bitmapImage = new BitmapImage();
            image.Width = bitmapImage.DecodePixelWidth = 350;
            bitmapImage.UriSource = new Uri(image.BaseUri, "Assets/default.jpg");
            photo.Source = bitmapImage;
        }

    }
}
