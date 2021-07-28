using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UserList
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string Token = ConfigurationManager.AppSettings["Token"];
            string Url = ConfigurationManager.AppSettings["Url"];
            int Days = int.Parse(ConfigurationManager.AppSettings["Days"]);

            using (var client = GetHttpClient(Token))
            {
                HttpResponseMessage response = client.GetAsync(Url).Result;
                if (response.IsSuccessStatusCode)
                {
                    ResponseModel content = JsonConvert.DeserializeObject<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                    //var activeUsers = content.items.Where(x => x.lastLoginOn?.AddDays(Days) > DateTime.Now).Select(y => y.email);
                    var activeUsers = content.items.Where(x => x.lastLoginOn?.AddDays(Days) > DateTime.Now && x.email.Contains("@mayo.edu")).Select(y => y.email);
                    string allUsersEmails = "";
                    Console.WriteLine("all users count: " + content.items.Count());
                    Console.WriteLine("all activeUsers count: " + activeUsers.Count());                   
                        foreach (var user in activeUsers)
                        {                        
                            allUsersEmails = allUsersEmails + user + ";";
                        }            
                    
                    Console.WriteLine(allUsersEmails);

                }
            }

            Console.WriteLine("press any button to exit");
            Console.ReadLine();
        }
        public static HttpClient GetHttpClient(string Token)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", Token);
            return client;
        }
    }
    public class ResponseModel
    {
        public List<user> items;
    }
    public class user
    {
        public string email;
        public DateTime? lastLoginOn;
    }
}
