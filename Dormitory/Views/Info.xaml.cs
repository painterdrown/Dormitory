using Dormitory.Models;
using Dormitory.ViewModels;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Dormitory.Views
{
    public sealed partial class Info : Page
    {
        JournalItem JItem;
        MemberItem MItem;
        InfoViewModel ViewModel;
        MemberViewModel ViewModel_1 { get; set; }

        public Info()
        {
            this.InitializeComponent();
            ViewModel = new InfoViewModel();
            ViewModel.init(App.account);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;

            var left = await HttpUtil.GetCheckbook(App.account);
            leftMoney.Text = "宿舍余额：￥" + (string)left["checkbook"]["balance"];

            if (e.Parameter != null)
            {
                if (e.Parameter.GetType() == typeof(Models.JournalItem))
                {
                    this.JItem = (Models.JournalItem)(e.Parameter);
                    if (JItem.message == "create")
                    {
                        //database add
                        await HttpUtil.AddJournal(App.account, JItem.content, JItem.ImageChange);

                        ViewModel.add(JItem);
                        return;
                    }
                    else if (JItem.message == "update")
                    {
                        await HttpUtil.EditJournal(JItem.id, JItem.content, JItem.ImageChange);
                        ViewModel.update(JItem);
                        return;
                    }
                    else if (JItem.message == "delete")
                    {
                        await HttpUtil.DeleteJournal(App.account ,JItem.id);
                        ViewModel.delete(JItem);
                        return;
                    }
                }
                if (typeof(Models.MemberItem) == e.Parameter.GetType())
                {
                    MItem = (Models.MemberItem)(e.Parameter);
                    //ViewModel.memberitems.Add(MItem);
                }
            }
            else return;
        }

        private void selectPhoto(object sender, RoutedEventArgs e)
        {

        }

        private void HomeAppButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToSetting(object sender, RoutedEventArgs e)
        {
            MemberItem m = null;
            Frame.Navigate(typeof(Setting), m);
        }

        private void CheckAppButton_Click(object sender, RoutedEventArgs e)
        {

            Frame.Navigate(typeof(Checkbook), null);
        }

        private void AddJournalButton_Click(object sender, RoutedEventArgs e)
        {
            JournalItem j = null;
            Frame.Navigate(typeof(WriteJournal), j);
        }

        private void JournalItemClick(object sender, ItemClickEventArgs e)
        {
            JournalItem j = (JournalItem)(e.ClickedItem);
            Frame.Navigate(typeof(WriteJournal), j);
        }

        private void DutyAppButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveContext(object sender, RoutedEventArgs e)
        {

        }

        private void clearContext(object sender, RoutedEventArgs e)
        {

        }
    }
}
