using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusTrax
{
    public class BusInformation
    {
        //This class is used to map the MongoDB documents to objects.
        public ObjectId Id { get; set; }
        public string Bus_ID { get; set; }
        public string BusComp_ID { get; set; }
        public string Bus_Driver { get; set; }
        public string Bus_Conductor { get; set; }
        public string Bus_Route { get; set; }
        public string PlateNumber { get; set; }

        //constructor
        public BusInformation(string bus_id, string bus_comp_id, string bus_driver, string bus_conductor, string bus_route, string platenumber)
        {
            Bus_ID = bus_id;
            BusComp_ID = bus_comp_id;
            Bus_Driver = bus_driver;
            Bus_Conductor = bus_conductor;
            Bus_Route = bus_route;
            PlateNumber = platenumber;  

        }


    }
}