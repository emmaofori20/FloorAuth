using FloorAuth.Data.Viewmodel;
using FloorAuth.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorAuth.Data
{
   public class MainFloorAuthProgram
    {
        private readonly floorfixturedbContext dbcontext;
        private readonly string username;
        private readonly int apiKey;
        private readonly string authorizationKey;
        private readonly string timestamp;

        public MainFloorAuthProgram(floorfixturedbContext dbcontext, string username, int apiKey, string AuthorizationKey, string Timestamp)
        {
            this.dbcontext = dbcontext;
            this.username = username;
            this.apiKey = apiKey;
            this.authorizationKey = AuthorizationKey;
            this.timestamp = Timestamp;
        }

        public async Task startProgram()
        {
            try
            {
                /////////Interaction with API to get all floors///////////
                var results = await ApiRequestService.GetAllFloors(username, authorizationKey, timestamp);
                Console.WriteLine("All floors data recieved");
                ////After getting all floors...
                await AddFloorToDatabase(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Ocuured at start program function... {ex.Message}");
            }
         
        }

        public async Task AddFloorToDatabase(FloorsViewmodel floorViewmodel)
        {

            try
            {
               
                foreach (var item in floorViewmodel.MyFloor)
                {
 
                    //Adding each floor
                    var res = AddNewFloor(item);

                    if ((bool)res)
                    {
                        //if true: make reponse to fixture api and add to table in database
                        var fixtureResponse = await ApiRequestService.GetSensorPerFloorAndAddToDatabase(username, apiKey, item.id);

                        foreach (var fix in fixtureResponse.MyFixtures)
                        {
                            AddNewFixture(fix, item.id);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ocuured at adding floorToDatabase function{ex.Message}");
            }
        }


        //Adding NewFloor to the database
        public bool AddNewFloor( Floordata _floor)
        {
           var res= dbcontext.Floors.Where(x => x.FloorId == _floor.id).FirstOrDefault();
            if (res!=null)
            {
                Console.WriteLine("Floor is already added!!!");
                return false;
            }
            else
            {
                Floor floor = new Floor
                {
                    FloorId = _floor.id,
                    Name = _floor.name,
                    Building = _floor.building,
                    Campus = _floor.campus,
                    Description = _floor.description,
                    FloorPlanUrl = _floor.floorPlanUrl,
                    ParentFloorId = _floor.parentFloorId
                };
                dbcontext.Floors.Add(floor);
                dbcontext.SaveChanges();
                Console.WriteLine($"Floor with Id:{_floor.id} Added ");
                return true;

            }


        }
        
        //Adding NewFixtures to the database
        public Fixture AddNewFixture(Fixturedata fixture, int FloorId)
        {
            var res = dbcontext.Fixtures.Where(x => x.FixtureId == fixture.id).FirstOrDefault();
            if (res != null)
            {
                Console.WriteLine("Fixture is already added!!!");
                return res;
            }
            else
            {
                Fixture _fixture = new Fixture
                {
                    FixtureId = fixture.id,
                    FloorId = FloorId,
                    Name = fixture.name,
                    Xaxis = fixture.xaxis,
                    Yaxis = fixture.yaxis,
                    GroupId = fixture.groupId,
                    MacAddress = fixture.macAddress,
                    ClassName = fixture.classes
                };
                dbcontext.Fixtures.Add(_fixture);
                dbcontext.SaveChanges();
                return _fixture;

            }
        }
    }
}
