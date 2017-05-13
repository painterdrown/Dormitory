using Dormitory.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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

                //JArray M = (JArray)result[""]

            }
            else
            {
                var md = new MessageDialog("info view models init fail!!").ShowAsync();
                return;
            }
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
    }
}
