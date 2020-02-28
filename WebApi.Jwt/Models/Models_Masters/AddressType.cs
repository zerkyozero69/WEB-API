using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class AddressType_Model
    {
      public string  AddressTypeName { get; set; }
       public bool IsActive { get; set; }
    }
    public class xxx
        {
        }
    public class DLDArea_Model
    {
        public string DLDAreaOid { get; set; }
        public string DLDAreaName { get; set; }
       
        public bool IsActive { get; set; }
    }
    public class GetProvinceDLD
        {
        public string OId { get; set; }
        public string ProvinceOid   { get; set; }

        public string ProvinceNameTH { get; set; }

        public string DLDZoneOid { get; set; }
        public string DLDZone { get; set; }
  
        public string OrganizationOid { get; set; }
        public string OrganizationName { get; set; }
        public bool IsActive { get; set; }
    
        public double Latitude { get; set; }
      
        public double Longitude { get; set; }
      
       

    }

}