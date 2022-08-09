using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FloorAuth.Data.Viewmodel
{
    [XmlRoot("fixtures")]
    public class FixturesViewmodel
    {
        [XmlElement("fixture")]
        public List<Fixturedata> MyFixtures { get; set; }
    }

    public class Fixturedata
    {
        public int id { get; set; }
        public string name { get; set; }
        public int xaxis { get; set; }
        public int yaxis { get; set; }
        public int groupId { get; set; }
        public string macAddress { get; set; }
        public int areaId { get; set; }
        public string classes { get; set; }


    }
}
