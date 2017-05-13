using Dormitory.Models;
using System.Collections.Generic;

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
    }
}
