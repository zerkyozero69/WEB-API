using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class Disaster_assistance
    {
        public string Number_Give{ get; set; }
        public string Give_Station { get; set; }
        public string  Subscriber_Info { get; set; }
        public int Weight_all { get; set; }
    }
    public class Detail_Disaster_assistance
    {
        public string Number_Give { get; set; }
        public string budget_year { get; set; }
        public string Give_Station { get; set; }
        public string Quota { get; set; }
        public string Subscriber_Info { get; set; }
        public string Citizen_ID { get; set; }
        public string Name_Subscriber { get; set; }
        public string Subscriber_Count { get; set; }
        public string Address_Subscriber { get; set; }
        public string activities_info { get; set; }
        public string Remarks { get; set; }

    }
}