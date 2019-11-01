using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class Supplier
    {
      public  class SupplierSend_Model
        {
            public string SendNo { get; set; }
            public string SendNumber { get; set; }
            public DateTime CreateDate { get; set; }
            public object FinanceYearOid { get; set; }
            public object OrganizationSendOid { get; set; }
            public object OrganizationReceiveOid { get; set; }
            public string Remark { get; set; }
            public object SendStatusOid { get; set; }
            
    
        }
    }
}