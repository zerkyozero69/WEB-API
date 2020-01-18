using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class AnimalSupplie_Model
    {
        public string Oid { get; set; }
        public string AnimalSupplieName { get; set; }
      
        public bool IsActive { get; set; }
        
        
    }

    public class AnimalSupplieType_Model
    {
        public string AnimalSupplieTypeOid { get; set; }
        public string SupplietypeName { get; set; }
        public string AnimalSupplie { get; set; }
        public bool IsActive { get; set; }
       // public AnimalSupplie AnimalSupplie { get; set; }

        public double SalePrice { get; set; }
    }
    public class AnimalSeed_Model
    {
        public  string Oid { get; set; }
        public string SeedCode { get; set; }

        public string SeedName { get; set; }
    
        public string SeedNameScience { get; set; }

        public string SeedNameCommon { get; set; }
       
     //   public SeedType SeedTypeOid { get; set; }
        
        public bool IsActive { get; set; }
       
        public double SalePrice { get; set; }


    }
    public class AnimalSeedLevel_Model
    {
        public string Oid { get; set; }
        public string SeedLevelCode { get; set; }
     
        public string SeedLevelName { get; set; }
    
        public int SortID { get; set; }
       
        public bool IsActive { get; set; }

    }
    public class SeedType_Model
    {
        public string Oid { get; set; }
        public string SeedTypeName { get; set; }
       
        public bool IsActive { get; set; }

    }
    public class AnimalType_Model
    {
        public string Oid { get; set; }
        public string AnimalCode { get; set; }
  
        public string AnimalName { get; set; }
        public bool IsActive { get; set; }

    }
    public class ForageType_Model
    {
        public string Oid { get; set; }
        public  string ForageTypeName { get; set; }
       
        public bool IsActive { get; set; }
    }
    public class PlantModel
    {
        public object Oid { get; set; }
        public object ForageTypeOid { get; set; }
        public string HarvestName { get; set; }

        public string IsActive { get; set; }
    }
}