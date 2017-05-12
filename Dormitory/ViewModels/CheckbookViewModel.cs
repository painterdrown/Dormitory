﻿using System;
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
        public void AddCheckbookItem(string cost, string name, DateTime datetime, bool state, string inOrOut, string note)
        {
            this.allItems.Add(new Models.CheckbookItem(cost, name, datetime, state, inOrOut ,note));
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
