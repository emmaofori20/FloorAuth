using FloorAuth.Data;
using FloorAuth.Data.Viewmodel;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FloorAuth.Services
{
    class ApiRequestService
    {
        private static readonly HttpClient client = new HttpClient();
      
        ///Method that get all floors/////
        public static async Task<FloorsViewmodel> GetAllFloors(string username, string AuthKey, string timeStamp)
        {
            var content = "unknown error";

            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/xml")
                    );
                //setting headers///////
                client.DefaultRequestHeaders.Add("ApiKey", username);
                client.DefaultRequestHeaders.Add("Authorization", AuthKey);
                client.DefaultRequestHeaders.Add("ts", timeStamp);

                var stringTask = await client.GetStringAsync("https://assignment.enappgy.io/ems/api/org/floor/list");
                var serializer = new XmlSerializer(typeof(FloorsViewmodel));

                //setting variable
                FloorsViewmodel floorsViewmodel = new FloorsViewmodel();

                using (TextReader reader = new StringReader(stringTask))
                {
                    floorsViewmodel = (FloorsViewmodel)serializer.Deserialize(reader);
                }

                return floorsViewmodel;
            }
            catch (Exception ex)
            {

                content = ex.Message;
                throw;
            }

        }

       ///Method that gets floor sensors and add inserts data to database///
        public static async Task<FixturesViewmodel> GetSensorPerFloorAndAddToDatabase(string username, int apikey, int FloorId)
        {
            try
            {

                //setting dateTime
                DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                //Getting the Timestamp
                var Timestamp = new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();

                string authkey = AuthorizationRequestService.GetAuthorization(username + apikey.ToString() + Timestamp.ToString());

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/xml")
                    );
                // client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
                //client.DefaultRequestHeaders.Add("ApiKey", username);
                //client.DefaultRequestHeaders.Add("Authorization", authkey );
                //client.DefaultRequestHeaders.Add("ts", Timestamp.ToString());

                var stringTask = await client.GetStringAsync($"https://assignment.enappgy.io/ems/api/org/fixture/location/list/floor/{FloorId}/fixtured");
                var serializer = new XmlSerializer(typeof(FixturesViewmodel));

                //setting variable for fixtures
                FixturesViewmodel fixturesViewmodel = new FixturesViewmodel();
                using (TextReader reader = new StringReader(stringTask))
                {
                    fixturesViewmodel = (FixturesViewmodel)serializer.Deserialize(reader);
                }
               
                return fixturesViewmodel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
