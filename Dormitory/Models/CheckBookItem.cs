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
        public ComboBoxItem name { get; set; }
        public DateTime datetime { get; set; }
        public string note { get; set; }
        public List<MemberItem> member_list;

        public CheckbookItem(string cost, ComboBoxItem name, DateTime datetime, string note, List<MemberItem> member_list)
        {
            this.id = Guid.NewGuid().ToString();
            this.cost = cost;
            this.name = name;
            this.datetime = datetime;
            this.note = note;
            this.member_list = member_list;
        }
    }
}
