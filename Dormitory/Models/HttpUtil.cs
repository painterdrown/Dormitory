using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;

namespace Dormitory.Models
{
    class HttpUtil
    {
        private const string BASE_URL = "http://www.sysu7s.cn:3000/api/dormitory";

        private static async Task<HttpResponseMessage> Post(JObject param, string action)
        {
            var json = JsonConvert.SerializeObject(param);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            using (HttpClient http = new HttpClient())
            {
                return await http.PostAsync(BASE_URL + action, content);
            }
        }

        private static async Task<JObject> PostForJObject(JObject param, string action)
        {
            using (var res = await Post(param, action))
            {
                var jsonString = await res.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            }
        }

        private static async Task<JObject> PostWithImage(JObject param, string action, bool isDefaultImage)
        {
            var data = new MultipartFormDataContent();

            // 文本数据部分
            var jstring = JsonConvert.SerializeObject(param);
            var text = new StringContent(jstring, System.Text.Encoding.UTF8, "application/json");
            data.Add(text);

            // 文件数据部分
            string uri = isDefaultImage ? "ms-appx:///Assets/default.jpg" : "ms-appdata:///temp/temp.png";
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));
            var stream = new StreamContent(await file.OpenStreamForReadAsync());
            data.Add(stream, "image", "temp.png");

            // POST并接收
            using (var http = new HttpClient())
            {
                var res = await http.PostAsync(BASE_URL + action, data);
                var jsonString = await res.Content.ReadAsStringAsync();
                var ret = JObject.Parse(jsonString);
                return ret;
            }
        }

        public static async Task<JObject> Login(string account, string password)
        {
            var param = new JObject();
            param["account"] = account;
            param["password"] = password;
            return await PostForJObject(param, "/login");
        }

        public static async Task<JObject> Register(string account, string password)
        {
            var param = new JObject();
            param["account"] = account;
            param["password"] = password;
            return await PostForJObject(param, "/register");
        }

        /** 添加一条Journal
         * isDefaultImage是表示图片是否为Asset目录下的default.jpg
         * 如果是false，需要提前将用户自己选择的图片保存到临时文件夹的temp.png
         */
        public static async Task<JObject> AddJournal(string did, string content, bool isDefaultImage)
        {
            var param = new JObject();
            param["did"] = did;
            param["content"] = content;
            return await PostWithImage(param, "/add-journal", isDefaultImage);
        }

        public static async Task<JObject> DeleteJournal(string did, long jid)
        {
            var param = new JObject();
            param["did"] = did;
            param["jid"] = jid;
            return await PostForJObject(param, "/delete-journal");
        }

        public static async Task<JObject> EditJournal(long jid, string content, bool isDefaultImage)
        {
            var param = new JObject();
            param["jid"] = jid;
            param["content"] = content;
            return await PostWithImage(param, "/edit-journal", isDefaultImage);
        }

        /** 获取某个宿舍所有journals
         * 返回的JObject的属性：ok, journals（JArray类型）
         * 每个journal的属性：jid, content
         */
        public static async Task<JObject> GetJournals(string did)
        {
            var param = new JObject();
            param["did"] = did;
            var ret = await PostForJObject(param, "/get-journals");
            return ret;
        }

        public static async Task<JObject> AddMember(string did, MemberItem item, bool isDefaultImage = true)
        {
            var param = new JObject();
            param["did"] = did;
            param["name"] = item.name;
            param["birth"] = item.birth.Ticks;
            param["location"] = item.location;
            return await PostWithImage(param, "/add-member", isDefaultImage);
        }

        /** 获取某个宿舍所有member的名字
         * 返回的JObject的属性：ok, names（JArray类型）
         * 直接用：(string)names[i]就能得到name
         */
        public static async Task<JObject> GetMemberNames(string did)
        {
            var param = new JObject();
            param["did"] = did;
            var ret = await PostForJObject(param, "/get-member-names");
            return ret;
        }

        public static async Task<JObject> AddCheckbookItem(string did, CheckbookItem item)
        {
            var param = new JObject();
            param["did"] = did;
            param["name"] = item.NAME;
            param["time"] = item.DATETIME.Ticks;
            param["cost"] = float.Parse(item.COST);
            param["state"] = item.STATE;
            param["note"] = item.NOTE;
            return await PostForJObject(param, "/add-checkbook-item");
        }

        public static async Task<JObject> EditCheckbookItem(string did, CheckbookItem item)
        {
            var param = new JObject();
            param["did"] = did;
            param["cno"] = item.CNO;
            param["name"] = item.NAME;
            param["time"] = item.DATETIME.Ticks;
            param["cost"] = float.Parse(item.COST);
            param["state"] = item.STATE;
            param["note"] = item.NOTE;
            return await PostForJObject(param, "/edit-checkbook-item");
        }

        /** 获取某个宿舍的账本
         * 返回的JObject的属性：ok, checkbook（JObject类型）
         * checkbook的属性有：balance和item（item是JArray类型）
         * item的属性有：did, no（下标，表示第几条）, name, time, cost, io, state, note
         */
        public static async Task<JObject> GetCheckbook(string did)
        {
            var param = new JObject();
            param["did"] = did;
            return await PostForJObject(param, "/get-checkbook");
        }

        public static async Task<JObject> AddDuty(string did, DutyItem item)
        {
            var param = new JObject();
            param["did"] = did;
            param["cno"] = item.cno;
            param["name"] = item.name;
            param["time"] = item.time.Ticks;
            param["note"] = item.note;
            return await PostForJObject(param, "/add-duty");
        }

        /** 返回这个宿舍的duties信息
         * 返回的JObect有三个属性：ok, duties, counts
         * duties是JArray类型，每一个元素又有did, no, name, time, note的属性
         * counts是JArray类型，每一个元素又有no, name, count的属性，其中count是no对应的duty次数
         */
        public static async Task<JObject> GetDuties(string did)
        {
            var param = new JObject();
            param["did"] = did;
            return await PostForJObject(param, "/get-duties");
        }

        private static async void test()
        {
            //await HttpUtil.Register("test", "test");
            //await HttpUtil.Register("test", "test");  // 重复注册
            //await HttpUtil.Login("test", "test1");  // 密码错误
            //await HttpUtil.Login("test", "test");

            //await HttpUtil.AddJournal("test", "111111", true);
            //await HttpUtil.AddJournal("test", "222222", false);
            //await HttpUtil.AddJournal("test", "333333", false);
            //var res = await HttpUtil.GetJournals("test");
            //var journals = res["journals"];
            //await HttpUtil.EditJournal((long)journals[0]["jid"], "zz脱单了！", true);
            //await HttpUtil.EditJournal((long)journals[1]["jid"], "zz又成单身狗啦！", true);
            //await HttpUtil.DeleteJournal("test", (long)journals[2]["jid"]);

            //await HttpUtil.AddMember("test", new MemberItem(null, "zz", new System.DateTime(1997, 5, 4), "cz", 1));
            //await HttpUtil.AddMember("test", new MemberItem(null, "bc", new System.DateTime(1997, 5, 4), "cz", 1), false);
            //await HttpUtil.AddMember("test", new MemberItem(null, "zy", new System.DateTime(1997, 5, 4), "cz", 1));
            //await HttpUtil.AddMember("test", new MemberItem(null, "xt", new System.DateTime(1997, 5, 4), "cz", 1));

            //await HttpUtil.GetMemberNames("test");

            //await HttpUtil.AddCheckbookItem("test", new CheckbookItem("-11.5", "zz", new System.DateTime(2017, 5, 4), false, "买TT"));
            //await HttpUtil.AddCheckbookItem("test", new CheckbookItem("400", "zz", new System.DateTime(2017, 5, 4), false, "收宿舍费"));
            //await HttpUtil.EditCheckbookItem("test", new CheckbookItem("-11.5", "zz", new System.DateTime(2017, 5, 4), true, "买TT"));
            //await HttpUtil.GetCheckbook("test");

            //await HttpUtil.AddDuty("test", new DutyItem(0, "zz", new System.DateTime(2017, 1, 1), "拿外卖"));
            //await HttpUtil.AddDuty("test", new DutyItem(2, "zy", new System.DateTime(2017, 1, 2), "拿外卖"));
            //await HttpUtil.GetDuties("test");
        }
    }
}
