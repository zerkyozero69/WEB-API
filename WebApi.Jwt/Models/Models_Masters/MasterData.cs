using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WebApi.Jwt.Models
{
    public class MasterData
    { public class TitleName
        {
            public string SubTitle_Name { get; set; }
            public string Titlen_Name { get; set; }
            public Boolean IsActive { get; set; }
        }
        public class  TitleNames
        {
            public bool status { get; set; }
        public int total_count { get; set; }
        public List<TitleName> results { get; set; }
    }
        public class Gender_Model
        {
            public object Oid { get; set; }
            public string GenderName { get; set; }
            public Boolean IsActive { get; set; }
        }
    }
    public class _Districts
    {
        public string Oid { get; set; }
        public string DistrictName_TH { get; set; }
       
    }
    public class _CustomerType
    {
          public object Oid { get; set; }
          public string TypeName { get; set; }
           public bool IsActive { get; set; }
           public string Remark { get; set; }
           public string MasterCustomerType { get; set; }
           public int Status  { get;set; }
           public string Message { get; set; }

    }
    public class EmployeeType_Model
    {
        public  string EmployeeTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}