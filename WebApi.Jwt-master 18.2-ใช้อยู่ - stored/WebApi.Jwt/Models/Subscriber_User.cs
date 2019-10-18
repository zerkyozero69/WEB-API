﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class Subscriber_User
    {
        public DateTime DateTime { get; set; }
        public object Service_info { get; set; }
        public object ServiceUser_Name { get; set; }
        public object Organization_Service { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
    }
    public class RegisterSubscriber_User
    {
        public string OrgeServiceID { get; set; }
        public string OrgeServiceName { get; set; }
        public string E_Mail { get; set; }
        public string Tel { get; set; }
        public string Address_No { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string ProvinceNameTH { get; set; }
        public string DistrictNameTH { get; set; }
        public string SubDistrictNameTH { get; set; }
        public string ZipCode { get; set; }
        public int Status { get; set; }
    }
}