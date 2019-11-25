using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class PlantModel
    {
        public object Oid { get; set; }
        public object ForageTypeOid { get; set; }
        public string HarvestName { get; set; }
       
        public string IsActive { get; set; }
    }
}