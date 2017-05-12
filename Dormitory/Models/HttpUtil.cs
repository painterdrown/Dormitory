using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;

namespace Dormitory.Models
{
    class HttpUtil
    {
        private const string BASE_URL = "http://192.168.199.115:3000/api/dormitory";

        private static async Task<JObject> PostForJObject(JObject param, string action)
        {
            var json = JsonConvert.SerializeObject(param);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            using (HttpClient http = new HttpClient())
            {
                using (var res = await http.PostAsync(BASE_URL + action, content))
                {
                    var retString = await res.Content.ReadAsStringAsync();
                    var ret = JObject.Parse(retString);
                    return ret;
                }
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

        public static async Task<JObject> AddJournal(string content, bool isDefaultImage)
        {
            var data = new MultipartFormDataContent();

            // 文本数据部分
            var json = new JObject();
            json["content"] = content;
            var jstring = JsonConvert.SerializeObject(json);
            var text = new StringContent(jstring, System.Text.Encoding.UTF8, "application/json");
            data.Add(text);

            // 文件数据部分
            string uri = isDefaultImage ? "ms-appx:///Assets/default.jpg" : "ms-appdata:///temp/temp.png";
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));
            var stream = new StreamContent(await file.OpenStreamForReadAsync());
            data.Add(stream, "image", "temp.png");

            // POST并接收
            var http = new HttpClient();
            var res = await http.PostAsync(BASE_URL + "/add-journal", data);
            var retString = await res.Content.ReadAsStringAsync();
            var ret = JObject.Parse(retString);
            return ret;
        }
    }
}
