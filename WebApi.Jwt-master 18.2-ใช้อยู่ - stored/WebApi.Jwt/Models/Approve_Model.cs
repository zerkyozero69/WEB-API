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
    public class SendOrderSeed_Model
    {
        public dynamic LotNumber { get; set; }
        public object WeightUnitOid { get; set; }
        public string AnimalSeedCode { get; set; }
        public string AnimalSeeName { get; set; }
        public string AnimalSeedLevel { get; set; }
        public object BudgetSourceOid { get; set; }
        public Double Weight { get; set; }
        public string Used { get; set; }
       public object   SendOrderSeed { get; set; } 

    }
    public class ReceiveOrderSeed_Model
    {
       public string ReceiveNo { get; set; }
        public DateTime SendDate { get; set; }
        public object FinanceYearOid { get; set; }
        public object SendOrgOid { get; set; }
        public object ReceiveOrgOid { get; set; }
        public string Remark { get; set; }
        public int SendStatus { get; set; }
    
    }
    public class ReceiveOrderSeedDetail_Model
    {
        public object Oid { get; set; }
        public object LotNumber { get; set; }
        public string AnimalSeedCode { get; set; }
        public string AnimalSeeName { get; set; }
        public string AnimalSeedLevel { get; set; }
        public object BudgetSourceOid { get; set; }
        public double Weight { get; set; }
        public string Used { get; set; }
        public string SendNo { get; set; }
        public object ReceiveOrderSeed { get; set; }
   
    }
    public enum APPROVE
        {
        Draft = 1,
        Approve = 2,
        Not_Approve =3


    }


}