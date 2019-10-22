using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class OrderSeedDetail
    {
        public object Oid { get; set; }
        public object LotNumber { get; set; }
        public object WeightUnitOid { get; set; }
        public string AnimalSeedCode { get; set; }
        public string AnimalSeeName { get; set; }
        public string AnimalSeedLevel { get; set; }
        public object BudgetSourceOid { get; set; }
        public double Weight { get; set; }
        public bool Used { get; set; }
        public object SendOrderSeed { get; set; }
      
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