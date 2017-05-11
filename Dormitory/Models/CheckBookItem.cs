using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
namespace Dormitory.Models
{
    class CheckbookItem
    {
        public string id;
        public string cost { get; set; }
        public DateTime datetime { get; set; }
        public string note { get; set; }
        public string name;

        public CheckbookItem(string cost, string name, DateTime datetime, string note)
        {
            this.id = Guid.NewGuid().ToString();
            this.cost = cost;
            this.datetime = datetime;
            this.note = note;
            this.name = name;
        }
    }
}
