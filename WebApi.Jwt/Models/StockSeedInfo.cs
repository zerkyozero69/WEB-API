using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class StockSeedInfo_Model
    {
        public string Oid { get; set; }
        public string StockDate { get; set; }
        public string OrganizationOid { get; set; }
        public string Organization { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string BudgetSourceOid { get; set; }
        public string BudgetSource { get; set; }
        public string AnimalSeedOid { get; set; }
        public string AnimalSeed { get; set; }
        public string AnimalSeedLevelOid { get; set; }
        public string AnimalSeedLevel{ get; set; }
        public string StockDetail { get; set; }
        public double TotalForward { get; set; }
        public double TotalChange { get; set; }    
        public double TotalWeight { get; set; }    
        public string Remark { get; set; }   
        public string StockType { get; set; }
        public string ReferanceCode { get; set; }
        public string SeedTypeOid { get; set; }
        public string SeedType { get; set; }
    }
}