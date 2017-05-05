using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Dormitory.Models
{
    public class MemberItem
    {
        public string ComboBoxOption { get; set; }
        public string ComboBoxHumanReadableOption { get; set; }

        public static implicit operator ComboBoxItem(MemberItem v)
        {
            throw new NotImplementedException();
        }
    }
}
