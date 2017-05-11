using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Dormitory.Models;

namespace Dormitory.ViewModels
{
    class CheckbookViewModel
    {
        private ObservableCollection<Models.CheckbookItem> allItems = new ObservableCollection<Models.CheckbookItem>();
        public ObservableCollection<Models.CheckbookItem> AllItems { get { return this.allItems; } }
        private Models.CheckbookItem selectedItem = default(Models.CheckbookItem);
        public Models.CheckbookItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }
        //添加
        public void RemoveCheckbookItem(string arg)
        {
            allItems.Remove(this.selectedItem);
            selectedItem = null;
        }
        //删除item
        public void AddCheckbookItem(string cost, ComboBoxItem name, DateTime datetime,  string note, List<MemberItem> member_list)
        {
            this.allItems.Add(new Models.CheckbookItem(cost, name, datetime, note, member_list));
        }
        //更新item
        public void updateCheckbookItem(string id, string cost, ComboBoxItem name, DateTime datetime, string note, List<MemberItem> member_list)
        {
            var i = selectedItem;
            i.id = id;
            i.cost = cost;
            i.name = name;
            i.datetime = datetime;
            i.note = note;
            i.member_list = member_list;
            selectedItem = null;
        }
    }
}
