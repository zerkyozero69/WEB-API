using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class activity_Model
    {
        public string Oid { get; set; }

        public string ActivityName { get; set; }

        public string ObjectTypeOid { get; set; }

        public string ObjectTypeName { get; set; }

        public string MasterActivity { get; set; }

       public List<activityDetails_Model> ActivityDetails { get; set; }
    } 
    public class activityDetails_Model
        {
        public string Oid { get; set; }
        public string ActivityName { get; set; }
        }
}