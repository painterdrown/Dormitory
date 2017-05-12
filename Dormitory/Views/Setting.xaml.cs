using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
            this.ViewModel = new ViewModels.MemberViewModel();
        }
        ViewModels.MemberViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (typeof(ViewModels.MemberViewModel) == e.Parameter.GetType())
            {
                ViewModel = (ViewModels.MemberViewModel)(e.Parameter);
            }
        }

        private void selectPhoto(object sender, RoutedEventArgs e)
        {

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

        private void saveContext(object sender, RoutedEventArgs e)
        {
            ViewModel.addMemberItem(userFace.Source, username.Text, BirthDay.Date.DateTime, province.Text + "_" + area.Text, 0);
            
        }

        private void clearContext(object sender, RoutedEventArgs e)
        {
            username.Text = "";
            BirthDay.Date = DateTime.Now;
            province.Text = area.Text = "";
        }
    }
}
