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
        public string FinanceYear { get; set; }
        public string SendOrgName { get; set; }
        public string ReceiveOrgName { get; set; }
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
        public string ReceiveDate { get; set; }
        public string FinanceYear { get; set; }
        public object ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }
        public object SendOrgOid { get; set; }
        public string SendOrgName { get; set; }
       
        public string Weight { get; set; }
    }
    public class ReceiveOrderSeedDetail_Model
    {
        public object Oid { get; set; }
        public object LotNumber { get; set; }
        public string AnimalSeedCode { get; set; }
        public string AnimalSeeName { get; set; }
        public string AnimalSeedLevel { get; set; }
        public object BudgetSource { get; set; }
        public double Weight { get; set; }
        public string Used { get; set; }
        public string SendNo { get; set; }
        public  object ReceiveOrderSeed { get; set; }
   
    }
    public class SupplierProductUser
    {
    
        public string UseDate { get; set; }
        public string UseNo { get; set; }
        public string FinanceYear { get; set; }
        public string OrganizationName { get; set; }
        public string Addrres { get; set; }
        public string EmployeeName { get; set; }
        public string Remark { get; set; }
        public string Stauts { get; set; }
        public string ApproveDate { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityName { get; set; }
        public string ReceiptNo { get; set; }
        public string RegisCusService { get; set; }
        public string OrgeService { get; set; }
        public string RegisCusServiceName { get; set; }
        public string OrgeServiceName { get; set; }
        public string Weight { get; set; }
 
        public List<SupplierUseProductDetail_Model> objProduct;

    }
    public class SendOrderSupplierAnimal_info
    {
        public string SendNo { get; set; }
        public string SendDate { get; set; }
        public string FinanceYear { get; set; }
        public object SendOrgOid { get; set; }
        public string SendOrgName { get; set; }
        public object ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }
        
        public double Weight { get; set; }
        public string Remark { get; set; }

        public string Send_Messengr { get; set; }
    }
    public class ReceiveOrderAnimal_info
    {
        public string SendNo { get; set; }
        public string SendDate { get; set; }
        public string FinanceYear { get; set; }
        public object ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }
        public object SendOrgOid { get; set; }
        public string SendOrgName { get; set; }
        public double Weight { get; set; }
        public string CancelMsg { get; set; }

        public string Send_Messengr { get; set; }
    }

    public class SupplierUseProductDetail_Model
        {
        public string AnimalSeedName { get; set; }
        public string AnimalSeedLevelName { get; set; }
        public double StockLimit { get; set; }
        public double Weight { get; set; }
        public string WeightUnit { get; set; }
        public string BudgetSourceName { get; set; }
        public string SupplierUseProduct { get; set; }
        public string LotNumber { get; set; }
        public string SeedType { get; set; }
        public double PerPrice { get; set; }
        public double Price { get; set; }
      
}
    public class sendSeed_info
        {
        public string Send_No { get; set; }
        public string SendDate { get; set; }
        public string FinanceYear { get; set; }
        public object SendOrgOid { get; set; }

        public string SendOrgName { get; set; }
        public  object ReceiveOrgoid { get; set;}
        public string ReceiveOrgName { get; set; }
        public string Weight { get; set; }
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