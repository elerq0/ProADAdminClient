using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Timers;
using System.Threading.Tasks;

namespace ADAdmin.Models
{
    public class Whitelist
    {
        public List<Objects.Address> ListDomain;
        public List<Objects.Address> ListAddress;
        private System.Timers.Timer Timer;
        private Objects.LogFile logFile;

        public Whitelist()
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
            await RefreshDomain();
            await RefreshAddress();
            Timer.Start();
        }

        public async Task RefreshDomain()
        {
            logFile.Write("System");
            string url;
            HttpClient http = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            List<Objects.Address> list;

            url = Properties.Resources.ServerUrl + "Whitelist/DomainRefresh";
            logFile.Write("        REST POST Whitelist/DomainRefresh");
            await http.PostAsync(url, null);

            url = Properties.Resources.ServerUrl + "Whitelist/DomainList";
            logFile.Write("        REST GET Whitelist/DomainList");
            HttpResponseMessage message = await http.GetAsync(url);
                logFile.Write("        DeserializeObejct - DomainList");
            string msg = await message.Content.ReadAsStringAsync();
            list = JsonConvert.DeserializeObject<List<Objects.Address>>(msg);

            ListDomain = list;
            logFile.Write("        Completed Successfully");
        }

        public async Task RefreshAddress()
        {
            logFile.Write("System");
            string url;
            HttpClient http = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            List<Objects.Address> list;

            url = Properties.Resources.ServerUrl + "Whitelist/AddressRefresh";
            logFile.Write("        REST POST Whitelist/AddressRefresh");
            await http.PostAsync(url, null);

            url = Properties.Resources.ServerUrl + "Whitelist/AddressList";
            logFile.Write("        REST GET Whitelist/AddressList");
            HttpResponseMessage message = await http.GetAsync(url);
            logFile.Write("        DeserializeObejct - AddressList");
            list = JsonConvert.DeserializeObject<List<Objects.Address>>(await message.Content.ReadAsStringAsync());

            ListAddress = list;
            logFile.Write("        Completed Successfully");
        }

        public string GetDomainList(string identity)
        {
            logFile.Write(identity);
            logFile.Write("        GetDomainList");

            int timeout = 0;
            while (ListDomain == null && timeout < 10)
            {
                Task.Delay(1000).Wait();
                timeout += 1;
            }

            logFile.Write("        Completed");
            if (ListDomain != null && ListDomain.Count != 0)
            {
                return JsonConvert.SerializeObject(ListDomain.OrderBy(x => x.Path));
            }
            else
                return String.Empty;
        }

        public string GetAddressList(string identity)
        {
            logFile.Write(identity);
            logFile.Write("        GetAddressList");

            int timeout = 0;
            while (ListAddress == null && timeout < 10)
            {
                Task.Delay(1000).Wait();
                timeout += 1;
            }
            logFile.Write("        Completed");
            if (ListAddress != null && ListAddress.Count != 0)
            {
                return JsonConvert.SerializeObject(ListAddress.OrderBy(x => x.Path));
            }
            else
                return String.Empty;
        }

        public async Task<Boolean> DomainAdd(string identity, string path)
        {
            logFile.Write(identity);
            logFile.Write("        DomainAdd");
            logFile.Write("        Path=[" + path + "]");

            string url = Properties.Resources.ServerUrl + "Whitelist/DomainAdd";
            HttpClient http = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(path, "path")
            });
            logFile.Write("        REST POST Whitelist/DomainAdd");
            HttpResponseMessage response = await http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                ListDomain.Add(new Objects.Address
                {
                    Path = path
                });
               
                logFile.Write("        Completed Successfully");
                return true;
            }
            else
            {
                logFile.Write("        Failed");
                return false;
            }
        }

        public async Task<Boolean> AddressAdd(string identity, string path)
        {
            logFile.Write(identity);
            logFile.Write("        AddressAdd");
            logFile.Write("        Path=[" + path + "]");

            string url = Properties.Resources.ServerUrl + "Whitelist/AddressAdd";
            HttpClient http = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(path, "path")
            });
            logFile.Write("        REST POST Whitelist/AddressAdd");
            HttpResponseMessage response = await http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                ListAddress.Add(new Objects.Address
                {
                    Path = path
                });
                logFile.Write("        Completed Successfully");
                return true;
            }
            else
            {
                logFile.Write("        Failed");
                return false;
            }
        }

        public async Task<Boolean> DomainRemove(string identity, string path)
        {
            logFile.Write(identity);
            logFile.Write("        DomainRemove");
            logFile.Write("        Path=[" + path + "]");

            string url = Properties.Resources.ServerUrl + "Whitelist/DomainRemove";
            HttpClient http = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(path, "path")
            });
            logFile.Write("        REST POST Whitelist/DomainRemove");
            HttpResponseMessage response = await http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                ListDomain.RemoveAll(x => (x.Path == path));
                logFile.Write("        Completed Successfully");
                return true;
            }
            else
            {
                logFile.Write("        Failed");
                return false;
            }
        }

        public async Task<Boolean> AddressRemove(string identity, string path)
        {
            logFile.Write(identity);
            logFile.Write("        AddressRemove");
            logFile.Write("        Path=[" + path + "]");

            string url = Properties.Resources.ServerUrl + "Whitelist/AddressRemove";
            HttpClient http = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(path, "path")
            });
            logFile.Write("        REST POST Whitelist/AddressRemove");
            HttpResponseMessage response = await http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                ListAddress.RemoveAll(x => (x.Path == path));
                logFile.Write("        Completed Successfully");
                return true;
            }
            else
            {
                logFile.Write("        Failed");
                return false;
            }
        }
    }
}
