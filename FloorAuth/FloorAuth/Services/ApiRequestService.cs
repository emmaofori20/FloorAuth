using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FloorAuth.Services
{
    class ApiRequestService
    {
        private static readonly HttpClient client = new HttpClient();

      
        ///Method that get all floors/////
        public static async Task GetAllFloors(string username, string AuthKey, string timeStamp)
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

                content = ex.Message;
            }

        }

    }
}
