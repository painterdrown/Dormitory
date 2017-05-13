using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.System;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using Windows.Storage;
using System;
using System.Threading.Tasks;

namespace Dormitory.Models
{
    class HttpUtil
    {
        private const string BASE_URL = "http://192.168.199.115:3000/api/dormitory";

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

        private static async Task<JObject> PostWithImage(JObject param, string action)
        {
            var data = new MultipartFormDataContent();

            // 文本数据部分
            var jstring = JsonConvert.SerializeObject(param);
            var text = new StringContent(jstring, System.Text.Encoding.UTF8, "application/json");
            data.Add(text);

            // 文件数据部分
            string uri = (bool)param["isDefaultImage"] ? "ms-appx:///Assets/default.jpg" : "ms-appdata:///temp/temp.png";
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
            param["isDefaultImage"] = isDefaultImage;
            return await PostWithImage(param, "/add-journal");
        }

        public static async Task<JObject> EditJournal(long jid, string content, bool isDefaultImage)
        {
            var param = new JObject();
            param["jid"] = jid;
            param["content"] = content;
            param["isDefaultImage"] = isDefaultImage;
            return await PostWithImage(param, "/edit-journal");
        }

        public static async Task<JObject> AddMember(string did, string name, string birth, string location, bool isDefaultImage)
        {
            var param = new JObject();
            param["did"] = did;
            param["name"] = name;
            param["birth"] = birth;
            param["location"] = location;
            param["isDefaultImage"] = isDefaultImage;
            return await PostWithImage(param, "/add-member");
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
    }
}
