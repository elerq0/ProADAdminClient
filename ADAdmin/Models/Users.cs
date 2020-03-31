using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Timers;
using Newtonsoft.Json;

namespace ADAdmin.Models
{
    public class Users
    {

        public List<Objects.User> List;
        private System.Timers.Timer Timer;
        private Objects.LogFile logFile;

        public Users()
        {
            logFile = new Objects.LogFile();
            Timer = new System.Timers.Timer();
            Timer.Interval = 2000;
            Timer.Elapsed += new ElapsedEventHandler(OnElapsed);
            Timer.Start();
        }

        private async void OnElapsed(object sender, ElapsedEventArgs e)
        {
            Timer.Stop();
            Timer.Interval = 600000;
            await Refresh();
            Timer.Start();
        }

        public async Task Refresh()
        {
            logFile.Write("System");
            string url;
            HttpClient http = new HttpClient(new HttpClientHandler() {  UseDefaultCredentials = true });
            List<Objects.User> list;

            url = Properties.Resources.ServerUrl + "Users/Refresh";
            logFile.Write("        REST POST Users/Refresh");
            await http.PostAsync(url, null);

            logFile.Write("        REST GET Users/List");
            url = Properties.Resources.ServerUrl + "Users/List";
            HttpResponseMessage message = await http.GetAsync(url);
            logFile.Write("        DeserializeObejct - Users");
            string msg = await message.Content.ReadAsStringAsync();
            list = JsonConvert.DeserializeObject<List<Objects.User>>(msg);

            List = list;
            logFile.Write("        Completed Successfully");
        }

        public string GetUsersList(string identity)
        {
            logFile.Write(identity);
            logFile.Write("        GetUsersList");

            int timeout = 0;
            while(List == null && timeout < 10)
            {
                Task.Delay(1000).Wait();
                timeout += 1;
            }

            logFile.Write("        Completed");
            if (List != null && List.Count != 0)
                return JsonConvert.SerializeObject(List);
            else
                return String.Empty;

        }

        public async Task<Boolean> UnlockAccount(string identity, string username)
        {
            logFile.Write(identity);
            logFile.Write("        UnlockAccount");
            logFile.Write("        UserName=[" + username + "]");

            string url = Properties.Resources.ServerUrl + "Users/Unlock";
            HttpClient http = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(username, "username")
            });
            logFile.Write("        REST POST Users/Unlock");
            HttpResponseMessage response = await http.PostAsync(url,content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                logFile.Write("        Completed Successfully");
                return true;
            }
            else
            {
                logFile.Write("        Failed");
                return false;
            }
        }

        public async Task<Boolean> ResetPassword(string identity, string username, string password)
        {
            logFile.Write(identity);
            logFile.Write("        ResetPassword");
            logFile.Write("        UserName=[" + username + "]");
            logFile.Write("        Password=[*****]");

            string url = Properties.Resources.ServerUrl + "Users/ResetPassword";
            HttpClient http = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(username, "username"),
                new KeyValuePair<string, string>(password, "password")
            });
            logFile.Write("        REST POST Users/ResetPassword");
            HttpResponseMessage response = await http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                logFile.Write("        Completed Successfully");
                return true;
            }
            else
            {
                logFile.Write("        Failed");
                return false;
            }
        }

        public async Task<Boolean> ResetSession(string identity, string username)
        {
            logFile.Write(identity);
            logFile.Write("        ResetSession");
            logFile.Write("        UserName=[" + username + "]");

            string url = Properties.Resources.ServerUrl + "Users/ResetSession";
            HttpClient http = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(username, "username")
            });
            logFile.Write("        REST POST Users/ResetSession");
            HttpResponseMessage response = await http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                logFile.Write("        Completed Successfully");
                return true;
            }
            else
            {
                logFile.Write("        Failed");
                return false;
            }
        }

        public async Task<string> GetStatus(string identity, string username)
        {
            logFile.Write(identity);
            logFile.Write("        GetStatus");
            logFile.Write("        UserName=[" + username + "]");

            string url = Properties.Resources.ServerUrl + "Users/GetStatus?username=" + username;
            HttpClient http = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });

            logFile.Write("        REST GET Users/GetStatus");
            HttpResponseMessage message = await http.GetAsync(url);
            string msg = await message.Content.ReadAsStringAsync();
            return msg;
        }
    }
}
