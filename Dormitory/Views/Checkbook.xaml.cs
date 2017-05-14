using Dormitory.Models;
using Newtonsoft.Json.Linq;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Dormitory.Views
{
    public sealed partial class Checkbook : Page
    {
        private CheckbookItem ck;
        ViewModels.CheckbookViewModel ViewModel { get; set; }
        private DataTransferManager DTM;

        public Checkbook()
        {
            this.InitializeComponent();
            this.ViewModel = new ViewModels.CheckbookViewModel();
        }

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
                        string cno = checkbok[i]["cno"].ToString();
                        DateTime datetim = new DateTime(hi);
                        string io = (string)checkbok[i]["io"];
                        bool state = (bool)checkbok[i]["state"];
                        string note = (string)checkbok[i]["note"];
                        ViewModel.AddCheckbookItem(cno, cost, name, datetim, state, io, note);
                    }
                }
            }
            leftMoney.Text = "￥" + (string)result["checkbook"]["balance"];

            result = await HttpUtil.GetMemberNames(App.account);
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

            // 设置共享源
            DTM = DataTransferManager.GetForCurrentView();
            DTM.DataRequested += DTM_DataRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DTM.DataRequested -= DTM_DataRequested;
        }

        private async void DTM_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataPackage data = args.Request.Data;
            data.Properties.Title = App.account + "'s Checkbook";
            var content = "";
            for (int i = 0; i < ViewModel.AllItems.Count; ++i)
            {
                var item = ViewModel.AllItems[i];
                content += item.DATETIME.ToString().Substring(0, 9) + "\t" + item.COST + "￥\t" + item.NAME + "\t" + item.NOTE + "\n";
            }
            content += "======================\n";
            content += "当前余额：" + leftMoney.Text;
            data.SetText(content);

            DataRequestDeferral GetFiles = args.Request.GetDeferral();
            try
            {
                StorageFile imageFile = await Package.Current.InstalledLocation.GetFileAsync("Assets\\checkbook.jpg");
                data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(imageFile);
                data.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
            }
            finally
            {
                GetFiles.Complete();
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
                TextBlock tb = (TextBlock)ComboBox.SelectedItem;
                string name = tb.Text;
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
                ck = new CheckbookItem(number.Text, name, date.Date.DateTime, false, tip.Text);
                var result = await HttpUtil.AddCheckbookItem(App.account, ck);
                int cno = (int)result["cno"];
                ViewModel.AddCheckbookItem(cno.ToString(), ck.COST, ck.NAME, ck.DATETIME, false, "", ck.NOTE);
                leftMoney.Text = "￥" + left.ToString();
                str = "￥" + left.ToString();
                leftMoney.Text = str;
                number.Text = "";
                tip.Text = "";
            }
            //如果点击了item
            else
            {
                //ViewModel.SelectedItem = (Models.CheckbookItem)(e.ClickedItem);
                TextBlock tb = (TextBlock)ComboBox.SelectedItem;
                string name = tb.Text;
                await HttpUtil.EditCheckbookItem(App.account, int.Parse(ViewModel.SelectedItem.CNO),new CheckbookItem(ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, date.Date.DateTime, ViewModel.SelectedItem.STATE, tip.Text));
                ViewModel.updateCheckbookItem(number.Text, name, date.Date.DateTime, ViewModel.SelectedItem.STATE, tip.Text);
                confirmButton.Content = "确定";
                number.Text = "";
                tip.Text = "";
                ComboBox1.IsEnabled = true;
                number.IsReadOnly = false;
            }

        }

        private void item_click(object sender, ItemClickEventArgs e)
        {
            //ViewModel.SelectedItem = (sender as CheckBox).DataContext as CheckbookItem;
            ViewModel.SelectedItem = (Models.CheckbookItem)(e.ClickedItem);
            ComboBox1.IsEnabled = false;
            number.IsReadOnly = true;
            confirmButton.Content = "更改";
            string name = ViewModel.SelectedItem.NAME;
            int h;
            for (h = 0; h < ComboBox.Items.Count; h++)
            {
                TextBlock tbx = (TextBlock)ComboBox.Items[h];
                if (tbx.Text == name)
                {
                    break;
                }
            }
            ComboBox.SelectedIndex = h;
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
                await HttpUtil.EditCheckbookItem(App.account, int.Parse(ViewModel.SelectedItem.CNO), new CheckbookItem(ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, ViewModel.SelectedItem.DATETIME, ViewModel.SelectedItem.STATE, ViewModel.SelectedItem.NOTE));
                ViewModel.updateCheckbookItem(ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, ViewModel.SelectedItem.DATETIME, ViewModel.SelectedItem.STATE, ViewModel.SelectedItem.NOTE);
            }
        }

        private async void unchecked_click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as CheckBox).DataContext as CheckbookItem;
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem.STATE = false;
                await HttpUtil.EditCheckbookItem(App.account, int.Parse(ViewModel.SelectedItem.CNO), new CheckbookItem(ViewModel.SelectedItem.CNO, ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, ViewModel.SelectedItem.DATETIME, ViewModel.SelectedItem.STATE, ViewModel.SelectedItem.NOTE));
                ViewModel.updateCheckbookItem(ViewModel.SelectedItem.COST, ViewModel.SelectedItem.NAME, ViewModel.SelectedItem.DATETIME, ViewModel.SelectedItem.STATE, ViewModel.SelectedItem.NOTE);
            }
        }

        private void ShareCheckbook(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }
    }

    class combobox_add
    {
        public string text { get; set; }
    }
}
