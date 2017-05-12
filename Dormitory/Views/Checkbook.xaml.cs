using Dormitory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Dormitory.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Checkbook : Page
    {
        int clickItem = 0;

        public Checkbook()
        {
            /*var result = await HttpUtil.Login();
            if ((bool)result["ok"])
            {

            } else
            {
                //result["errMsg"]
            }*/
            
            this.InitializeComponent();
            this.ViewModel = new ViewModels.CheckbookViewModel();
        }
        ViewModels.CheckbookViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (typeof(ViewModels.CheckbookViewModel) == e.Parameter.GetType())
            {
                ViewModel = (ViewModels.CheckbookViewModel)(e.Parameter);
            }
        }


        private void HomeAppButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Info), "");
        }

        private void CheckAppButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DutyAppButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Duty), "");
        }



        private async void confirmButton_click(object sender, RoutedEventArgs e)
        {
            //如果没有点击item
            if (clickItem == 0)
            {
                string name = "sucker";
                ViewModel.AddCheckbookItem(number.Text, name, date.Date.DateTime, true, "", tip.Text);
                var i = new MessageDialog(date.Date.DateTime.ToString()).ShowAsync();
            }
            //如果点击了item
            else
            {
                await ViewModel.updateCheckbookItem(number.Text, this.ComboBox.SelectedItem.ToString(), date.Date.DateTime, true, tip.Text);
                clickItem = 0;
            }
            Frame.Navigate(typeof(Checkbook), ViewModel);
        }

        private void item_click(object sender, ItemClickEventArgs e)
        {
            this.clickItem = 1;
            ViewModel.SelectedItem = (Models.CheckbookItem)(e.ClickedItem);
            var i = ViewModel.SelectedItem;
            number.Text = i.cost;
            //this.ComboBox.SelectedItem
            date.Date = i.datetime;
            this.tip.Text = i.note;
        }

        private async void checked_click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as CheckBox).DataContext as CheckbookItem;
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem.state = true;
                await ViewModel.updateCheckbookItem(ViewModel.SelectedItem.id, ViewModel.SelectedItem.name, ViewModel.SelectedItem.datetime, ViewModel.SelectedItem.state, ViewModel.SelectedItem.note);
            }
        }

        private async void unchecked_click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as CheckBox).DataContext as CheckbookItem;
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem.state = false;
                await ViewModel.updateCheckbookItem(ViewModel.SelectedItem.id, ViewModel.SelectedItem.name, ViewModel.SelectedItem.datetime, ViewModel.SelectedItem.state, ViewModel.SelectedItem.note);
            }
        }
    }
}
