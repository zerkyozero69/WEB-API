﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{

    public class SendOrderSupplierModel
    {
        public string org_oid { get; set; }
        /// <summary>
        /// รายการข้อมูลที่ส่ง
        /// </summary>
        public List<SendOrderSupplierType> Sender { get; set; }
        /// <summary>
        /// รายการข้อมูลที่รับ
        /// </summary>
        public List<SendOrderSupplierType> Receive { get; set; }
    }

    public class SendOrderSupplierType
    {
        public string SendNo { get; set; }
        public string  SendDate { get; set; }
        public string FinanceYear { get; set; }
        public string SendOrgOid { get; set; }
        public string SendOrgName { get; set; }
        //[XafDisplayName("หน่วยจัดส่ง")]
        public string SendOrgFullName { get; set; }
        public string ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }
        public string ReceiveOrgFullName { get; set; }
        public string SendStatus { get; set; }
        public string Remark { get; set; }
        public string CancelMsg { get; set; }

        public string RefNo { get; set; }
        //[XafDisplayName("หน่วยจัดส่ง")]
        //[XafDisplayName("หน่วยจัดส่ง")]
        // [XafDisplayName("แหล่งงบประมาณ")]

        //[XafDisplayName("ปีงบประมาณ")]



        // [XafDisplayName("วันที่ส่ง")]

        // [XafDisplayName("เลขที่ใบส่ง")]


        //[XafDisplayName("พันธุ์พืชอาหารสัตว์")]
        public string AnimalSeedOid { get; set; }
  
   
        


        //     [XafDisplayName("หน่วยนับ")]
        public string UnitName { get; set; }

        //      [XafDisplayName("สาเหตุยกเลิก")]

        
       // [XafDisplayName("สถานะ")]
      
        // [XafDisplayName("หมายเหตุ")]
 

        // [XafDisplayName("ภาชนะบรรจุ")]
        public string PackageName { get; set; }

        //  [XafDisplayName("ปริมาณ")]
        public double QTY { get; set; }

        //  [XafDisplayName("วัตถุประสงค์")]
        public string ObjectTypeName { get; set; }
        //      [XafDisplayName("โควตา")]
        public string QuotaTypeName { get; set; }

        //    [XafDisplayName("ผลผลิตคงเหลือ")]
        public double StockLimit { get; set; }


        //  [XafDisplayName("ประเภทชนิดเสบียงสัตว์")]
        public string AnimalSupplieName { get; set; }

        //    [XafDisplayName("หน่วยที่ได้รับ")]
      

        // [XafDisplayName("ปศุสัตว์จังหวัด")]
    //    public ManageSubAnimalSupplier ManageSubAnimalSupplierOid { get; set; }
        public string TotalWeight { get; set; }
  
       // public List<SendOrderSupplierDetailType> Details { get; set; }
    }

    //public class SendOrderSupplierDetailType
    //{
    
    //    //[XafDisplayName("หน่วย")]
    //    public string WeightUnitOid { get; set; }
    //    public string WeightUnitName { get; set; }
    //    //[XafDisplayName("รหัสพันธุ์")]
    //    public string AnimalSeedCode { get; set; }
    //    //[XafDisplayName("ชื่อเมล็ด")]
    //    public string AnimalSeeName { get; set; }
    //    //[XafDisplayName("ชั้นพันธุ์")]
    //    public string AnimalSeedLevel { get; set; }
    //    public string AnimalSeedLevelName { get; set; }
    //    //[XafDisplayName("พันธุ์พืชอาหารสัตว์")]
    //    public string AnimalSeedOid { get; set; }
    //    public string AnimalSeedName { get; set; }
    //    //[XafDisplayName("ชั้นพันธุ์")]
    //    public string AnimalSeedLevelOid { get; set; }
    //    //[XafDisplayName("ประเภทเมล็ดพันธุ์")]
    //    public string SeedTypeOid { get; set; }
    //    public string SeedTypeName { get; set; }
    //    //[XafDisplayName("แหล่งงบประมาณ")]
    //    public string BudgetSourceOid { get; set; }
    //    public string BudgetSourceName { get; set; }
    //    //[XafDisplayName("นน.เมล็ด(กก.)")]
    //    public double Weight { get; set; }
    //    //[XafDisplayName("คงเหลือ(กก.)")]
    //    public double Amount { get; set; }
    //    public bool Used { get; set; }
    //    public string SendOrderSeed { get; set; }
    //}


}