using Dormitory.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;

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
        public void AddCheckbookItem(string cost, string name, DateTime datetime, bool state, string inOrOut, string note)
        {
            this.allItems.Add(new Models.CheckbookItem(cost, name, datetime, state, note));
        }
        //更新item
        public void updateCheckbookItem(string cost, string name, DateTime datetime, bool state, string note)
        {
            var i = selectedItem;
            i.COST = cost;
            i.STATE = state;
            i.DATETIME = datetime;
            i.NOTE = note;
            i.NAME = name;
            selectedItem = null;
        }
    }
}