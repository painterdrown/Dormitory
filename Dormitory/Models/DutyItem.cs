using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory.Models
{
    class DutyItem : BindableBase
    {
        // name, time, note
        public int cno;  // no是指这个duty是属于宿舍第几号人的
        public string name;
        public DateTime time;
        public string note;

        public DutyItem(int cno, string name, DateTime time, string note)
        {
            this.cno = cno;
            this.name = name;
            this.time = time;
            this.note = note;
        }
    }
}
