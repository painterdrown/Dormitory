using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory.Models
{
    class JournalItem
    {
        public long id { get; set; }
        public DateTimeOffset date { get; set; }
        public Uri pic { get; set; }
        public string content { get; set; }
        public string message;
        public bool ImageChange;
        public JournalItem()
        {
            date = DateTime.Now;
            content = "init";
            pic = new Uri("ms-appx:///Assets/default.jpg");
        }
    }
}
