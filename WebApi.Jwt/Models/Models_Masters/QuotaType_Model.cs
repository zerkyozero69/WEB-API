﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class QuotaType_Model
    {
        public string QuotaTypeOid { get; set; }
        public string QuotaName { get; set; }

    }
    public class ManageAnimalSupplier_Model2
    { 
        public string ManageAnimalSupplierOid { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYearName { get; set; }
        public string OrgZoneOid { get; set; }
        public string OrgZoneName { get; set; }
        public string OrganizationOid { get; set; }
        public string OrganizationName { get; set; }
        public string AnimalSupplieOid { get; set; }
        public string AnimalSupplieName { get; set; }
        public string ZoneQTY { get; set; }
      
        public string CenterQTY { get; set; }
     
        public string OfficeQTY { get; set; }

        public string OfficeGAPQTY { get; set; }
 
        public string OfficeBeanQTY { get; set; }
   
        //public string SumProvinceQTY { get; }

        public string Status { get; set; }
 
        public List<ManageSubAnimalSupplier_Model2> Detail { get; set; }
     
        public double SortID { get; set; }
        public object objquota { get; set; }

    }
    public class ManageSubAnimalSupplier_Model2
    {
        public string ManageSubAnimalSupplierOid { get; set; }
        public string ProvinceOid { get; set; }
        public string ProvinceName { get; set; }
        //   [XafDisplayName("ชนิดเสบียงสัตว์")]
        public string AnimalSupplieTypeOid { get; set; }
        public string AnimalSupplieOid { get; set; }
        public string AnimalSupplieName { get; set; }
        public string OrganizationOid { get; set; }
        public string OrganizationName { get; set; }
        public string AnimalSupplieTypeName { get; set; }
        //   [XafDisplayName("โควตาจังหวัด")]
        public double ProvinceQTY { get; set; }

    //    [XafDisplayName("หน่วย")]
        public string UnitOid { get; set; }
        public string UnitName { get; set; }
    }
    public class AnimalProductDetail
    {
        public string QuotaName { get; set; }
     public string AnimalSeedOid { get; set; }
        public string AnimalSupplieTypeOid { get; set; }
        public string AnimalSupplieTypeName { get; set; }
        public Double QuotaQTY { get; set; }
        public Double StockLimit { get; set; }
          public Double StockUsed { get; set; }
        public Double Amount  { get; set; }

    }
}