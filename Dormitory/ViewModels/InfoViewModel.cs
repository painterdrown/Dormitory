using Dormitory.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Windows.UI.Popups;

namespace Dormitory.ViewModels
{
    class InfoViewModel
    {
        //public Dictionary<int, MemberItem> Member_list = new Dictionary<int, MemberItem>();
        public NewObservableCollection<Models.JournalItem> journalitems = new NewObservableCollection<JournalItem>();
        public NewObservableCollection<Models.JournalItem> Journalitems { get { return this.journalitems; } }

        public NewObservableCollection<Models.MemberItem> memberitems = new NewObservableCollection<MemberItem>();
        public NewObservableCollection<Models.MemberItem> Memberitems { get { return this.memberitems; } }

        public InfoViewModel()
        {
            JournalItem j1 = new JournalItem();
            JournalItem j2 = new JournalItem();
            j2.content = "init2";
            journalitems.Add(j1);
            journalitems.Add(j2);
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

            }
            else
            {
                var md = new MessageDialog((string)result["info view models init fail!!"]).ShowAsync();
                return;
            }
        }
    }
}
