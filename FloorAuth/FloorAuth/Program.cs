using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FloorAuth
{
    class Program
    {
        public static string GetSha1(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash.ToLower();
        }

        private static readonly HttpClient client = new HttpClient();


        static async Task Main(string[] args)
        {
            string username = "enappgy-demo";
            int apiKey = 12345;
            //setting dateTime
            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            //Getting the Timestamp
            var Timestamp = new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
            //Getting function to returning SHA-1 Authorisation
            string AuthorizationKey = GetSha1(username + apiKey.ToString() + Timestamp.ToString());
            
            Console.Write(AuthorizationKey);


            /////////Interaction with API///////////
            await GetAllFloors(username,AuthorizationKey,Timestamp.ToString());
        }

        private static async Task GetAllFloors(string username, string AuthKey, string timeStamp)
        {
            var content = "unknown error";

            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/xml")
                    );
               // client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
                client.DefaultRequestHeaders.Add("ApiKey", username);
                client.DefaultRequestHeaders.Add("Authorization", AuthKey);
                client.DefaultRequestHeaders.Add("ts", timeStamp);

                var stringTask = await client.GetStringAsync("https://assignment.enappgy.io/ems/api/org/floor/list");

                Console.Write(stringTask);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                content = ex.Message;
            }

        }

        ////Getting Floor sensor id
    }
}
