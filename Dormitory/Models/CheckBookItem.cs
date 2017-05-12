using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
namespace Dormitory.Models
{
    class CheckbookItem : BindableBase
    {
        private string id;
        private string cost;
        private DateTime datetime;
        private bool state;
        private string inOrOut;
        private string note;
        private string name;

        public string ID
        {
            get { return id; }
            set { SetProperty(ref this.id, value); }
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

        public string INOROUT
        {
            get { return inOrOut; }
            set { SetProperty(ref this.inOrOut, value); }
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

        public CheckbookItem(string cost, string name, DateTime datetime, bool state, string inOrOut, string note)
        {
            this.ID = Guid.NewGuid().ToString();
            this.COST = cost;
            this.DATETIME = datetime;
            this.INOROUT = inOrOut;
            this.STATE = state;
            this.NOTE = note;
            this.NAME = name;
        }
    }
}
