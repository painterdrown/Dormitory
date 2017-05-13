using System;
using Windows.UI.Xaml.Media;

namespace Dormitory.Models
{
    class MemberItem :BindableBase
    {
        public string name { get; set; }
        public DateTime birth { get; set; }
        public string location { get; set; }
        public int id { get; set; }
        public int random_num { get; set; }
        string message;

        public MemberItem(string name, DateTime birth, string location, int random_num)
        {
            this.name = name;
            this.birth = birth;
            this.location = location;
            this.random_num = random_num;
        }

        public MemberItem()
        {
            birth = DateTime.Now;
            name = "";
            location = "";
            random_num = 0;
        }
    }
}