using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class Customer_Service
    {
        public object OrgeOid { get; set; }
        public string OrgeID { get; set; }
        public string Name_Group { get; set; }
        public string Address { get; set; }
        public object ProvinceOid { get; set; }
        public string DLD { get; set; }
        public string ProvinceNameTH { get; set; }
        public object DistrictOid{ get; set; }
        public string DistrictNameTH { get; set; }
        public object SubDistrictOid{ get; set; }
        public string SubDistrictNameTH { get; set; }
        public string ZipCode { get; set; }
        public string Tel { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}