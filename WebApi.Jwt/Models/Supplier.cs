using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class Supplier
    {
        public class SupplierSend_Model
        {
            public string SendNo { get; set; }
            public string SendNumber { get; set; }
            public DateTime CreateDate { get; set; }
            public object FinanceYearOid { get; set; }
            public object OrganizationSendOid { get; set; }
            public object OrganizationReceiveOid { get; set; }
            public string Remark { get; set; }
            public object SendStatusOid { get; set; }


        }
        public class SupplierProduct_Model
        {
            public string LotNumber { get; set; }
            public string FinanceYearOid { get; set; }
            public string BudgetSourceOid { get; set; }
            public string OrganizationOid { get; set; }
            public string AnimalSeedOid { get; set; }
            public string AnimalSeedLevelOid { get; set; }
            public string PlotHeaderOid { get; set; }
            public string Stauts { get; set; }
            public double Weight { get; set; }
            public string UnitOid { get; set; }
            public DateTime LastCleansingDate { get; set; }
            public bool Used { get; set; }
            public string ReferanceUsed { get; set; }
            public string PlotInfoOidOid { get; set; }
            public string FormType { get; set; }
            public string SeedTypeOid { get; set; }
        

        }
        public class SupplierAnimalProduct_info
        {
            public string SupplierAnimalNumber { get; set; }
            public string FinanceYear { get; set; }
            public string BudgetSource { get; set; }
            public string OrganizationName { get; set; }
            public string AnimalSupplie { get; set; }
            public string AnimalSeed { get; set; }
            public string PlotInfoOid{ get; set; }
            public string Weight { get; set; }
            public string Unit { get; set; }
            public string AnimalSupplieType { get; set; }
            public string Area { get; set; }
            public string ManufactureDate { get; set; }
    
        }
    }
}