using Dormitory.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace Dormitory.ViewModels
{
    class InfoViewModel : BindableBase
    {
        //public Dictionary<int, MemberItem> Member_list = new Dictionary<int, MemberItem>();
        public NewObservableCollection<Models.JournalItem> journalitems = new NewObservableCollection<JournalItem>();
        public NewObservableCollection<Models.JournalItem> Journalitems { get { return this.journalitems; } }

        public NewObservableCollection<Models.MemberItem> memberitems = new NewObservableCollection<MemberItem>();
        public NewObservableCollection<Models.MemberItem> Memberitems { get { return this.memberitems; } }

        public InfoViewModel()
        {
        }

        public async void init(string did)
        {
            var result = await HttpUtil.GetJournals(did);
            if ((bool)result["ok"])
            {
                JArray journals = (JArray)result["journals"];
                for(var i = 0; i < journals.Count; i++)
                {
                    JournalItem J = new JournalItem();
                    J.id = (long)journals[i]["jid"];
                    J.content = (string)journals[i]["content"];
                    J.pic = new System.Uri("http://www.sysu7s.cn:3000/api/dormitory/get-journal-image/" + J.id);
                    journalitems.Add(J);
                }

                UpdateTileForAllItems();
            }
            else
            {
                var md = new MessageDialog("info view models init fail!!").ShowAsync();
                return;
            }
            result = await HttpUtil.GetMembers(did);
            if ((bool)result["ok"])
            {
                JArray member = (JArray)result["members"];
                for (var i = 0; i < member.Count; i++)
                {
                    string mno = (string)member[i]["mno"];
                    MemberItem m = new MemberItem();
                    m.name = (string)member[i]["name"];
                    long second = (long)member[i]["birth"];
                    m.birth = new System.DateTime(second);
                    m.pic = new System.Uri("http://www.sysu7s.cn:3000/api/dormitory//get-member-image/" + App.account + "/" + mno);
                    m.location = (string)member[i]["location"];
                    memberitems.Add(m);
                }
            }
        }

        public void add(JournalItem j)
        {
            journalitems.Add(j);
            UpdateTileForOneItem(j);
        }
        public void update(JournalItem j)
        {
            for(var i = 0; i < journalitems.Count; i++)
            {
                if(journalitems[i].id == j.id)
                {
                    journalitems.SetItem(i, j);
                }
            }
        }
        public void delete(JournalItem j)
        {
            for (var i = 0; i < journalitems.Count; i++)
            {
                if (journalitems[i].id == j.id)
                {
                    var item = journalitems[i];
                    journalitems.Remove(item);
                }
            }
        }

        private void UpdateTileForAllItems()
        {
            var updator = TileUpdateManager.CreateTileUpdaterForApplication();
            updator.Clear();
            // 启动更新队列轮询
            updator.EnableNotificationQueue(true);

            // 将每一个TodoItem都添加到更新队列
            foreach (var item in Journalitems)
            {
                XmlDocument tile = new XmlDocument();
                tile.LoadXml(File.ReadAllText("Tile.xml"));
                XmlNodeList textNodes = tile.GetElementsByTagName("text");

                int i = 0;
                // TileSmall
                (textNodes[i++] as XmlElement).InnerText = item.content;
                // TileMedium
                (textNodes[i++] as XmlElement).InnerText = item.content;
                // TileWide
                (textNodes[i++] as XmlElement).InnerText = item.content;
                (textNodes[i++] as XmlElement).InnerText = item.date.ToString();

                TileNotification notification = new TileNotification(tile);
                updator.Update(notification);
            }
        }

        private void UpdateTileForOneItem(JournalItem item)
        {
            var updator = TileUpdateManager.CreateTileUpdaterForApplication();

            XmlDocument tile = new XmlDocument();
            tile.LoadXml(File.ReadAllText("Tile.xml"));
            XmlNodeList textNodes = tile.GetElementsByTagName("text");

            int i = 0;
            // TileSmall
            (textNodes[i++] as XmlElement).InnerText = item.content;
            // TileMedium
            (textNodes[i++] as XmlElement).InnerText = item.content;
            // TileWide
            (textNodes[i++] as XmlElement).InnerText = item.content;
            (textNodes[i++] as XmlElement).InnerText = item.date.ToString();

            TileNotification notification = new TileNotification(tile);
            updator.Update(notification);
            updator.EnableNotificationQueue(true);
        }
    }
}
