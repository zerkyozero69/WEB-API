using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class ManageAnimalSupplier_Model
    {
        public string Oid { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string  OrgZoneOid { get; set; }
        public string OrgZone { get; set; }
        public string  OrganizationOid { get; set; }
        public string Organization { get; set; }
        public string  AnimalSupplieOid { get; set; }
        public string AnimalSupplie { get; set; }
        public double ZoneQTY { get; set; }
        public double CenterQTY { get; set; }     
        public double OfficeQTY { get; set; }
        public double OfficeGAPQTY { get; set; }   
        public double OfficeBeanQTY { get; set; }  
        public double SumProvinceQTY { get; }
   
        public EnumManageBudget Status { get; set; }
       
      //  public XPCollection<ManageSubAnimalSupplier> ManageSubAnimalSuppliers { get; }
      
        public double SortID { get; set; }
    }
}