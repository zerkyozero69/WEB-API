using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public  class Approve_Model
    {
        public string Send_No {get;set;}
        public string SendDate { get; set; }
        public string FinanceYearOid { get; set; }
        public string SendOrgOid { get; set; }
        public string ReceiveOrgOid { get; set; }
        public string Remark { get; set; }
        public string SendStatus { get; set; }
        public double Weight { get; set; }
         public string CancelMsg { get; set; }
    
        public  string Send_Messengr { get; set; }
        

        public List<SendOrderSeed_Model> objSeed;
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
       public object SendOrderSeed { get; set; }
        public string AnimalSeedOid { get; set; }
        public string AnimalSeedLevelOid { get; set; }
        public string SeedTypeOid { get; set; }
        public double Amount { get; set; }
   
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
        public  object ReceiveOrderSeed { get; set; }
   
    }
    public class SupplierProductUser
    {
        public List<SupplierUseProductDetail_Model> objProduct;
        public DateTime UseDate { get; set; }
        public string UseNo { get; set; }
        public string FinanceYearOid { get; set; }
        public string OrganizationOid { get; set; }
        public string EmployeeOid { get; set; }
        public string Remark { get; set; }
        public string Stauts { get; set; }
        public string ApproveDate { get; set; }
        public string ActivityOid { get; set; }
        public string SubActivityOid { get; set; }
        public string ReceiptNo { get; set; }
        public string RegisCusService { get; set; }
        public string OrgeService { get; set; }
        public string RegisCusServiceOid { get; set; }
        public string OrgeServiceOid { get; set; }
  
    }
    public class SupplierUseProductDetail_Model
        {
        public string AnimalSeedOid { get; set; }
        public string AnimalSeedLevelOid { get; set; }
        public double StockLimit { get; set; }
        public double Weight { get; set; }
        public string WeightUnitOid { get; set; }
        public string BudgetSourceOid { get; set; }
        public string SupplierUseProduct { get; set; }
        public string LotNumber { get; set; }
        public string SeedTypeOid { get; set; }
        public double PerPrice { get; set; }
        public double Price { get; set; }
      
}
    public class sendSeed_info
        {
        public string Send_No { get; set; }
        public string SendDate { get; set; }
        public string FinanceYearOid { get; set; }
        public string SendOrgOid { get; set; }
        public string ReceiveOrgOid { get; set; }
        public Double Weight { get; set; }
    }
    public class  data_info
    {
      public  List<sendSeed_info> sendSS { get; set;}
    }
    public enum APPROVE
        {
        Draft = 1,
        Approve = 2,
        Not_Approve =3


    }


}