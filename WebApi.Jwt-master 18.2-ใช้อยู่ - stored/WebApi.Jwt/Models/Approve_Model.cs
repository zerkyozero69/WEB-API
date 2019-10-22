using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class Approve_Model
    {
        public string Send_No {get;set;}
        public string SendDate { get; set; }
        public string FinanceYearOid { get; set; }
        public string SendOrgOid { get; set; }
        public string ReceiveOrgOid { get; set; }
        public string Remark { get; set; }
        public string SendStatus { get; set; }
      
        public string Send_Messengr { get; set; }
    }
    

}