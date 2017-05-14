using System;
using Windows.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Dormitory.ViewModels
{
    class MemberViewModel
    {
        private ObservableCollection<Models.MemberItem> allItems = new ObservableCollection<Models.MemberItem>();
        public ObservableCollection<Models.MemberItem> AllItems { get { return this.allItems; } }
        private Models.MemberItem selectedItem = default(Models.MemberItem);
        public Models.MemberItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }
        //添加
        public void addMemberItem(Uri pic, string name, DateTime birth, string location, int random_num)
        {
            this.allItems.Add(new Models.MemberItem(pic, name, birth, location, random_num));
        }
        //更新
        public void updateMemberItem(Uri pic, string name, DateTime birth, string location, int random_num)
        {
            var i = selectedItem;
            i.pic = pic;
            i.name = name;
            i.birth = birth;
            i.location = location;
            i.random_num = random_num;
        }
    }
}
