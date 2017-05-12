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
            string str;
            //如果没有点击item
            if (confirmButton.Content.ToString() == "确定")
            {
                string name = "sucker";
                //var i = new MessageDialog(this.ComboBox1.SelectedIndex.ToString()).ShowAsync();
                if (this.ComboBox1.SelectedIndex == 0)
                {
                    number.Text = "+" + number.Text;
                }
                else
                {
                    number.Text = "-" + number.Text;
                }
                int money = int.Parse(number.Text);
                int left = int.Parse(leftMoney.Text.ToString().Substring(1));
                left += money;
                ViewModel.AddCheckbookItem(number.Text, name, date.Date.DateTime, false, "", tip.Text);
                leftMoney.Text = "￥" + left.ToString();
                str = "￥" + left.ToString();
                leftMoney.Text = str;
                number.Text = "";
                tip.Text = "";
            }
            //如果点击了item
            else
            {
                //await ViewModel.updateCheckbookItem(number.Text, this.ComboBox.SelectedItem.ToString(), date.Date.DateTime, ViewModel.SelectedItem.state, tip.Text);
                ViewModel.updateCheckbookItem(number.Text, "sucker", date.Date.DateTime, ViewModel.SelectedItem.STATE, tip.Text);
                confirmButton.Content = "确定";
                number.Text = "";
                tip.Text = "";
                ComboBox1.IsEnabled = true;
                number.IsReadOnly = false;
            }
            
        }

        private void item_click(object sender, ItemClickEventArgs e)
        {
            ComboBox1.IsEnabled = false;
            number.IsReadOnly = true;
            confirmButton.Content = "更改";
            ViewModel.SelectedItem = (Models.CheckbookItem)(e.ClickedItem);
            var i = ViewModel.SelectedItem;
            number.Text = i.COST;
            date.Date = i.DATETIME;
            this.tip.Text = i.NOTE;
        }

        private void deleteInfo(object sender, ItemClickEventArgs e)
        {
            number.Text = "";
            tip.Text = "";
        }

        private async void checked_click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as CheckBox).DataContext as CheckbookItem;
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem.STATE = true;
                ViewModel.updateCheckbookItem(ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, ViewModel.SelectedItem.DATETIME, ViewModel.SelectedItem.STATE, ViewModel.SelectedItem.NOTE);
            }
        }

        private async void unchecked_click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as CheckBox).DataContext as CheckbookItem;
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem.STATE = false;
                ViewModel.updateCheckbookItem(ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, ViewModel.SelectedItem.DATETIME, ViewModel.SelectedItem.STATE, ViewModel.SelectedItem.NOTE);
            }
        }
    }
}
