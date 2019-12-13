using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class OrgeService_info
    { 
        public string OrganizationOid { get; set; }
        public string OrgeServiceID { get; set; }
        public string OrgeServiceName { get; set; }
        public string Tel { get; set; }
        public object Email { get; set; }
        public string Address { get; set; }
        public string Moo { get; set; }
        public object Soi { get; set; }
        public string Road { get; set; }
        public object ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string SubDistrictName { get; set; }
        public string IsActive { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress { get; set; }
        public List<OrgeServiceDetail_Model> OrgeServiceDetails { get; set; }
    }
    public class OrgeServiceDetail_Model
    {
        public string ServiceTypeOid { get; set; }
        public string SubServiceTypeOid { get; set; }
        public string OrgeServiceOid { get; set; }

    }
    public class OrgeService_Data
        {
        public List<OrgeService_info> Data { get; set; }
    }
}