using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class OrgeService_info
    {
        public object OrgeOid { get; set; }
        public string OrganizationOid { get; set; }
        public string OrgeServiceID { get; set; }
        public string OrgeServiceName { get; set; }
        public string Tel { get; set; }
        public object Email { get; set; }
        public string Address { get; set; }
        public string Moo { get; set; }
        public object Soi { get; set; }
        public string Road { get; set; }
        public object ProvinceOid { get; set; }
        public string DistrictOid { get; set; }
        public string SubDistrictOid { get; set; }
        public string IsActive { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress { get; set; }
        
    }
    public class OrgeService_Data
        {
        public List<OrgeService_info> Data { get; set; }
    }
}