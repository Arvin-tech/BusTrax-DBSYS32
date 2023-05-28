using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusTrax
{
    public class Subscriptions
    {
        //This class is used to map the MongoDB documents to objects.
        public ObjectId Id { get; set; }
        public string BusComp_ID { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int Payment { get; set; }
    }
}