using Dormitory.Models;
using Newtonsoft.Json.Linq;
using System;
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
            //init(App.account);
            this.ViewModel = new ViewModels.CheckbookViewModel();
        }
        ViewModels.CheckbookViewModel ViewModel { get; set; }

        public async void init(string did)
        {
            var result = await HttpUtil.GetCheckbook(did);
            if ((bool)result["ok"])
            {
                JArray checkbok = (JArray)result["checkbook"]["items"];
                if (checkbok != null)
                {
                    for (var i = 0; i < checkbok.Count; i++)
                    {
                        string cost = (string)checkbok[i]["cost"];
                        string name = (string)checkbok[i]["name"];
                        string d = checkbok[i]["time"].ToString();
                        long hi = long.Parse(d);
                        DateTime datetim = new DateTime(hi);
                        string io = (string)checkbok[i]["io"];
                        bool state = (bool)checkbok[i]["state"];
                        string note = (string)checkbok[i]["note"];
                        ViewModel.AddCheckbookItem(cost, name, datetim, state, io, note);
                    }
                }
            }
            leftMoney.Text = "￥" + (string)result["checkbook"]["balance"];

            result = await HttpUtil.GetCheckbook(did);
            if ((bool)result["ok"])
            {
                JArray members = (JArray)result["names"];
                if (members != null)
                {
                    for (var i = 0; i < members.Count; i++)
                    {
                        string n = (string)members[i];
                        TextBlock tb = new TextBlock();
                        tb.Text = n;
                        ComboBox.Items.Add(tb);
                    }
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //if (typeof(ViewModels.CheckbookViewModel) == e.Parameter.GetType())
            //{
            //    ViewModel = (ViewModels.CheckbookViewModel)(e.Parameter);
            //}
            init(App.account);
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
                float money = float.Parse(number.Text);
                string heheda = leftMoney.Text;
                float left = float.Parse(leftMoney.Text.Substring(1));
                left += money;
                await HttpUtil.AddCheckbookItem(App.account, new CheckbookItem(number.Text, name, date.Date.DateTime, false, tip.Text));
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
                //ViewModel.updateCheckbookItem(number.Text, ComboBox.SelectedItem.ToString(), date.Date.DateTime, ViewModel.SelectedItem.STATE, tip.Text);
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
                await HttpUtil.EditCheckbookItem(App.account, new CheckbookItem(ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, ViewModel.SelectedItem.DATETIME, ViewModel.SelectedItem.STATE, ViewModel.SelectedItem.NOTE));
                ViewModel.updateCheckbookItem(ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, ViewModel.SelectedItem.DATETIME, ViewModel.SelectedItem.STATE, ViewModel.SelectedItem.NOTE);
            }
        }

        private async void unchecked_click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as CheckBox).DataContext as CheckbookItem;
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem.STATE = false;
                await HttpUtil.EditCheckbookItem(App.account, new CheckbookItem(ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, ViewModel.SelectedItem.DATETIME, ViewModel.SelectedItem.STATE, ViewModel.SelectedItem.NOTE));
                ViewModel.updateCheckbookItem(ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, ViewModel.SelectedItem.DATETIME, ViewModel.SelectedItem.STATE, ViewModel.SelectedItem.NOTE);
            }
        }
    }

    class combobox_add
    {
        public string text { get; set; }
    }
}
