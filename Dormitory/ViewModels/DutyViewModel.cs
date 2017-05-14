using Dormitory.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Dormitory.ViewModels
{
    class DutyViewModel
    {
        public NewObservableCollection<Models.DutyItem> dutyitems = new NewObservableCollection<Models.DutyItem>();
        public NewObservableCollection<Models.CountItem> countitems = new NewObservableCollection<Models.CountItem>();
        public NewObservableCollection<Models.MemberItem> memberitems = new NewObservableCollection<Models.MemberItem>();
        public async void init(string did)
        {
            var result = await HttpUtil.GetDuties(did);
            if ((bool)result["ok"])
            {
                JArray duties = (JArray)result["duties"];
                JArray counts = (JArray)result["counts"];

                for (var i = 0; i < duties.Count; i++)
                {
                    DutyItem D = new DutyItem();
                    D.cno = (int)duties[i]["_v"];
                    D.name = (string)duties[i]["name"];
                    D.time = (DateTime)duties[i]["time"];
                    D.note = (string)duties[i]["note"];
                    dutyitems.Add(D);
                }
                for (var i = 0; i < counts.Count; i++)
                {
                    CountItem C = new CountItem();
                    C.count = (int)counts[i]["count"];
                    C.name = (string)counts[i]["name"];
                    C.no = (int)counts[i]["mno"];
                    countitems.Add(C);
                }

            }
            else
            {
                var md = new MessageDialog("duty models init fail!!").ShowAsync();
                return;
            }
            result = await HttpUtil.GetMembers(App.account);
            if ((bool)result["ok"])
            {
                JArray member = (JArray)result["members"];
                for (var i = 0; i < member.Count; i++)
                {
                    string mno = (string)member[i]["mno"];
                    MemberItem m = new MemberItem();
                    m.name = (string)member[i]["name"];
                    long second = (long)member[i]["birth"];
                    m.birth = new System.DateTime(second);
                    m.pic = new System.Uri("http://www.sysu7s.cn:3000/api/dormitory//get-member-image/" + App.account + "/" + mno);
                    m.location = (string)member[i]["location"];
                    memberitems.Add(m);
                }
            }
        }
    }
}
