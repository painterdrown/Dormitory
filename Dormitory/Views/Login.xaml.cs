using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Dormitory.Models;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Windows.Security.Credentials;
using System.Collections.Generic;
using Windows.Storage;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Dormitory.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Login : Page
    {
        private IReadOnlyList<PasswordCredential> credentialList = null;

        public Login()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var vault = new PasswordVault();
            if (vault.RetrieveAll().Count != 0)
            {
                // 如果有登录记录
                credentialList = vault.FindAllByResource("users");
                string lastAccount = (string)ApplicationData.Current.LocalSettings.Values["LastAccount"];
                for (int i = 0; i < credentialList.Count; ++i)
                {
                    if (credentialList[i].UserName == lastAccount)
                    {
                        credentialList[i].RetrievePassword();
                        AccountTB.Text = credentialList[i].UserName;
                        PasswordPB.Password = credentialList[i].Password;
                        break;
                    }
                }
            }
        }

        private async void LoginBT_Click(object sender, RoutedEventArgs e)
        {
            string account = AccountTB.Text;
            string password = PasswordPB.Password;
            if (account.Length == 0)
            {
                var md = new MessageDialog("账号不能为空").ShowAsync();
                return;
            }
            if (password.Length == 0)
            {
                var md = new MessageDialog("密码不能为空").ShowAsync();
                return;
            }

            LoginBT.IsEnabled = false;
            var result = await HttpUtil.Login(account, password);
            if ((bool)result["ok"])
            {
                App.account = account;
                var vault = new PasswordVault();
                var credential = new PasswordCredential("users", account, password);
                vault.Add(credential);
                ApplicationData.Current.LocalSettings.Values["LastAccount"] = account;
                Frame.Navigate(typeof(Info), account);
            }
            else
            {
                LoginBT.IsEnabled = true;
                var md = new MessageDialog((string)result["errMsg"]).ShowAsync();
                return;
            }
        }

        private async void RegisterBT_Click(object sender, RoutedEventArgs e)
        {
            string account = AccountRegTB.Text;
            string password = PasswordRegPB.Password;
            if (account.Length == 0)
            {
                var md = new MessageDialog("账号不能为空").ShowAsync();
                return;
            }
            if (password.Length == 0)
            {
                var md = new MessageDialog("密码不能为空").ShowAsync();
                return;
            }

            RegisterBT.IsEnabled = false;
            var result = await HttpUtil.Register(account, password);
            if ((bool)result["ok"])
            {
                App.account = account;
                var vault = new PasswordVault();
                var credential = new PasswordCredential("users", account, password);
                vault.Add(credential);
                ApplicationData.Current.LocalSettings.Values["LastAccount"] = account;
                Frame.Navigate(typeof(Info));
            }
            else
            {
                RegisterBT.IsEnabled = true;
                var md = new MessageDialog((string)result["errMsg"]).ShowAsync();
                return;
            }
        }

        private void AccountTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (credentialList != null)
            {
                for (int i = 0; i < credentialList.Count; ++i)
                {
                    var credential = credentialList[i];
                    if (credential.UserName == AccountTB.Text)
                    {
                        credential.RetrievePassword();
                        PasswordPB.Password = credential.Password;
                    }
                }
            }
        }
    }
}
