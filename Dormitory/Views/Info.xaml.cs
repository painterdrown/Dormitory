using Dormitory.Models;
using Dormitory.ViewModels;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Dormitory.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Info : Page
    {
        public Info()
        {
            this.InitializeComponent();
            ViewModel.init(App.account);
        }
        JournalItem JItem;
        InfoViewModel ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            if (e.Parameter.GetType() == typeof(Models.JournalItem))
            {
                this.JItem = (Models.JournalItem)(e.Parameter);
                if(JItem.message == "create")
                {
                    //database add
                    ViewModel.journalitems.Add(JItem);
                }
                else if(JItem.message == "update")
                {
                    ViewModel.journalitems.Add(JItem);
                }
                else if(JItem.message == "delete")
                {
                    ViewModel.journalitems.Remove(JItem);
                }
                ViewModel.init(App.account);
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
