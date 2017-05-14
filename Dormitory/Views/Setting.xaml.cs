using Dormitory.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Dormitory.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Setting : Page
    {
        public Setting()
        {
            this.InitializeComponent();
        }
        MemberItem MItem;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MItem = ((MemberItem)e.Parameter);
            if (MItem == null)
            {
                MItem = new MemberItem();
            }
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
                    bitmapImage.DecodePixelWidth = 353;
                    await bitmapImage.SetSourceAsync(fileStream);
                    userFace.Source = bitmapImage;

                    var fileToSave = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("temp.png", CreationCollisionOption.ReplaceExisting);
                    var stream = await file.OpenReadAsync();
                    var bytes = await Temp.GetBytesFromStream(stream);
                    await FileIO.WriteBytesAsync(fileToSave, bytes);
                }
            }
        }

        private void HomeAppButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckAppButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DutyAppButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void saveContext(object sender, RoutedEventArgs e)
        {
            if (username.Text == "" || province.Text == "" || area.Text == "")
            {
                var i = new MessageDialog("内容不能为空!").ShowAsync();
            }
            var u = (userFace.Source as BitmapImage).UriSource;
            if (u == null)
            {
                u = new Uri("ms-appdata:///temp/temp.png");
            }
            MItem.pic = u;
            MItem.name = username.Text;
            MItem.birth = BirthDay.Date.DateTime;
            MItem.location = province.Text + "_" + area.Text;
            MItem.random_num = 0;
            //ViewModel.addMemberItem(userFace.Source, username.Text, BirthDay.Date.DateTime, province.Text + "_" + area.Text, 0);
            await HttpUtil.AddMember(App.account, new MemberItem(u, username.Text, BirthDay.Date.DateTime, province.Text + "_" + area.Text, 0), false);
            Frame.Navigate(typeof(Info), MItem);
        }

        private void clearContext(object sender, RoutedEventArgs e)
        {
            username.Text = "";
            BirthDay.Date = DateTime.Now;
            province.Text = area.Text = "";
        }
    }
}
