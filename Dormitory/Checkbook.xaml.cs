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

namespace Dormitory
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Checkbook : Page
    {

        private ObservableCollection<MemberItem> ComboBoxOptions;

        public Checkbook()
        {
            this.InitializeComponent();
            /*
            // 复选框
            ComboBoxOptions = new ObservableCollection<MemberItem>();
            ComboBoxOptionsManager.GetComboBoxList(ComboBoxOptions);
            // 要用Converter实现数据类型转换，现在乱写的
            SelectedComboBoxOption = ComboBoxOptions[0];
            */
        }
        ViewModels.CheckbookViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (typeof(ViewModels.CheckbookViewModel) == e.Parameter.GetType())
            {
                ViewModel = (ViewModels.CheckbookViewModel)(e.Parameter);
            }
        }

        /*
        public class ComboBoxOptionsManager
        {
            public static void GetComboBoxList(ObservableCollection<MemberItem> ComboBoxItems)
            {
                var allItems = getComboBoxItems();
                ComboBoxItems.Clear();
                allItems.ForEach(p => ComboBoxItems.Add(p));
            }

            private static List<MemberItem> getComboBoxItems()
            {
                var items = new List<MemberItem>();

                items.Add(new MemberItem() { ComboBoxOption = "Option1", ComboBoxHumanReadableOption = "Option 1" });
                items.Add(new MemberItem() { ComboBoxOption = "Option2", ComboBoxHumanReadableOption = "Option 2" });
                items.Add(new MemberItem() { ComboBoxOption = "Option3", ComboBoxHumanReadableOption = "Option 3" });

                return items;
            }
        }
        

        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;


        */


        private void HomeAppButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DormitoryInfo), "");
        }

        private void CheckAppButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DutyAppButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Duty), "");
        }

        private void confirmButton_click(object sender, RoutedEventArgs e)
        {

        }

        private void item_click(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.CheckbookItem)(e.ClickedItem);
            var i = ViewModel.SelectedItem;
            number.Text = i.cost;
            foreach (var value in i.member_list)
            {
                this.ComboBox.Items.Add(value.name);
            }
            date.Date = i.datetime;
            this.tip.Text = i.note;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }
    }
}
