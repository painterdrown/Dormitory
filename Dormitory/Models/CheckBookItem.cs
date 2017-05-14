using System;

namespace Dormitory.Models
{
    class CheckbookItem : BindableBase
    {
        private string cno;
        private string cost;
        private DateTime datetime;
        private bool state;
        private string note;
        private string name;

        public string CNO
        {
            get { return cno; }
            set { SetProperty(ref this.cno, value); }
        }


        public string COST
        {
            get { return cost; }
            set { SetProperty(ref this.cost, value); }
        }

        public DateTime DATETIME
        {
            get { return datetime; }
            set { SetProperty(ref this.datetime, value); }
        }

        public bool STATE
        {
            get { return state; }
            set { SetProperty(ref this.state, value); }
        }

        public string NOTE
        {
            get { return note; }
            set { SetProperty(ref this.note, value); }
        }

        public string NAME
        {
            get { return name; }
            set { SetProperty(ref this.name, value); }
        }

        public CheckbookItem(string cno, string cost, string name, DateTime datetime, bool state, string note)
        {
            this.CNO = cno;
            this.COST = cost;
            this.DATETIME = datetime;
            this.STATE = state;
            this.NOTE = note;
            this.NAME = name;
        }
        public CheckbookItem(string cost, string name, DateTime datetime, bool state, string note)
        {
            this.COST = cost;
            this.DATETIME = datetime;
            this.STATE = state;
            this.NOTE = note;
            this.NAME = name;
        }
    }
}