using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class Unit_Modle
    {
        public object Oid { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public bool IsActive { get; set; }
    }
    public class Package_Model
    {
        public string PackageName { get; set; }
     
        public bool IsActive { get; set; }

    }
}