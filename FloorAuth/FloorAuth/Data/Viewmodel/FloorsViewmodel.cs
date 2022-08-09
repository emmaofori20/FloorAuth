using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FloorAuth.Data.Viewmodel
{
    [XmlRoot("floors")]
    public class FloorsViewmodel
    {
        [XmlElement("floor")]
        public List<Floordata> MyFloor { get; set; }
    }

    public class Floordata
    {
        public int id { get; set; }
        public int name { get; set; }
        public int building { get; set; }
        public int campus { get; set; }
        public int company { get; set; }
        public string description { get; set; }
        public string floorPlanUrl { get; set; }
        public string parentFloorId { get; set; }


    }
}
