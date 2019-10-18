using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class Subscriber_User
    {
        public DateTime DateTime { get; set; }
        public object Service_info { get; set; }
        public object ServiceUser { get; set; }
        public object Organization_Service { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
    }
}