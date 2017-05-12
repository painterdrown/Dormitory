using System;
using Windows.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory.Models
{
    public class MemberItem
    {
        public ImageSource pic { get; set; }
        public string name { get; set; }
        public DateTime birth { get; set; }
        public string location { get; set; }
        public int id { get; set; }
        public int random_num { get; set; }

        public MemberItem(ImageSource pic, string name, DateTime birth, string location, int random_num)
        {
            this.pic = pic;
            this.name = name;
            this.birth = birth;
            this.location = location;
            this.random_num = random_num;
        }
    }
}
