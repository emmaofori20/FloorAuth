using FloorAuth.Data;
using FloorAuth.Data.Viewmodel;
using FloorAuth.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FloorAuth
{
    class Program
    {


        static async Task Main(string[] args)
        {
            try
            {
                ////////////////Variable settings/////////////////
                string username = "enappgy-demo";
                int apiKey = 12345;
                //setting dateTime
                DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                //Getting the Timestamp
                var Timestamp = new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
                //Getting function to returning SHA-1 Authorisation
                string AuthorizationKey = AuthorizationRequestService.GetAuthorization(username + apiKey.ToString() + Timestamp.ToString());

                Console.WriteLine("Program Executing!!!!.....");
                //Initialising db context
                floorfixturedbContext floorfixturedb = new floorfixturedbContext();
                //intialising class
                MainFloorAuthProgram mainFloorAuth = new MainFloorAuthProgram(floorfixturedb, username, apiKey, AuthorizationKey, Timestamp.ToString());
                await mainFloorAuth.startProgram();

                Console.WriteLine("Program Complete....");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
    
        }


    }
}
