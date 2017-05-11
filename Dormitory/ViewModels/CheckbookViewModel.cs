using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public void AddCheckbookItem(string cost, string name, DateTime datetime,  string note)
        {
            this.allItems.Add(new Models.CheckbookItem(cost, name, datetime, note));
        }
        //更新item
        public void updateCheckbookItem(string cost, string name, DateTime datetime, string note)
        {
            var i = selectedItem;
            i.cost = cost;
            i.datetime = datetime;
            i.note = note;
            i.name = name;
            selectedItem = null;
        }
    }
}
