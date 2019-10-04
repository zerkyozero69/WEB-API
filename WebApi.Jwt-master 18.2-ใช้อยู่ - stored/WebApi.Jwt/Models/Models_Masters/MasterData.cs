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
        public class Gender
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
}