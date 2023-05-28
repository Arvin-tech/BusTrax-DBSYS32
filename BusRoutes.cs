using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusTrax
{
    public class BusRoutes
    {
        //This class is used to map the MongoDB documents to objects.
        public ObjectId Id { get; set; }
        public string Route_ID { get; set; }
        public string Route_Name { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Fare_Amount { get; set; }
        public string ETA { get; set; }

        public BusRoutes(string route_ID, string route_Name, string origin, string destination, int fare_Amount, string eTA)
        {     
            Route_ID = route_ID;
            Route_Name = route_Name;
            Origin = origin;
            Destination = destination;
            Fare_Amount = fare_Amount;
            ETA = eTA;
        }
    }
}