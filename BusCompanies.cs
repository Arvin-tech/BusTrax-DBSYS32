using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusTrax
{
    public class BusCompanies
    {
        //This class is used to map the MongoDB documents to objects.
        public ObjectId Id { get; set; }
        public string BusComp_ID { get; set; }
        public string BusComp_Name { get; set; }
        public string BusComp_ContactNo { get; set; }
        public string Email { get; set; }

     
       
        //constructor
        public BusCompanies(string compid, string name, string contact, string email)
        {
            BusComp_ID = compid;
            BusComp_Name = name;
            BusComp_ContactNo = contact;
            Email = email;
    
        }
    }
}