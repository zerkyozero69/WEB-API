﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.ApplicationBlocks.Data;
using DevExpress.Persistent.BaseImpl;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using System.Web.Http;
using System.Web;
using static WebApi.Jwt.helpclass.helpController;
using static WebApi.Jwt.Models.user;
using System.Data;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.Security;
using WebApi.Jwt.Models;
using WebApi.Jwt.Filters;
using WebApi.Jwt.helpclass;
using NTi.CommonUtility;
using System.IO;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using nutrition.Module;
using System.Globalization;
using static WebApi.Jwt.Models.Supplier;
using DevExpress.Xpo;

namespace WebApi.Jwt.Controllers.MasterData
{
    /// <summary>
    /// ข้อมูลส่ง-รับเสบียงสัตว์ให้หน่วยงาน
    /// ==================
    /// SendStatus:
    ///     รับเสบียงสัตว์
    ///         NotSend = 0 (อยู่ระหว่างดำเนินการ)
    ///         Send = 1(จัดส่งเมล็ดพันธุ์แล้ว)
    ///         Accept = 2 (ตรวจสอบแล้ว)
    ///         Approve = 3(อนุมัติรับเมล็ดพันธุ์)
    ///         Cancel = 4 (ยกเลิก)
    ///         Eject = 8(ไม่อนุมัติ)
    ///     ==================
    ///     ส่งเสบียงสัตว์
    ///         SendAccept = 5(ตรวจสอบแล้ว)
    ///         SendApprove = 6 (อนุมัติส่งเมล็ดพันธุ์)
    ///         SendCancel = 7(ยกเลิกส่งเมล็ดพันธุ์)
    ///         SendEject = 9 (ไม่อนุมัติส่งเมล็ดพันธุ์)
    /// </summary>
    public class SendorderSuppilerAnimal_Controller : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();


        /// <summary>
        /// แสดงข้อมูลส่ง-รับเสบียงสัตว์ให้หน่วยงาน
        /// </summary>
        /// <param name="Org_Oid">OID ของหน่วยงาน</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrderAnimalstock/List")]
        public HttpResponseMessage GetSendOrderAnimalstock()
        {
            try
            {
                string org_oid = HttpContext.Current.Request.Form["Org_Oid"].ToString();
                string type = HttpContext.Current.Request.Form["type"].ToString(); //รับ=1/ส่ง=2

                if (org_oid != "" && type != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSupplierAnimal));

                    List<SendOrderSupplierType> SendItems = new List<SendOrderSupplierType>();
                    List<ReceiveOrderSupplierType> ReceiveItems = new List<ReceiveOrderSupplierType>();
                    SendOrderSupplierModel lists = new SendOrderSupplierModel();
                    lists.org_oid = org_oid;

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    if (type == "2")
                    {  //ส่ง

                        IList<SendOrderSupplierAnimal> collection = ObjectSpace.GetObjects<SendOrderSupplierAnimal>(CriteriaOperator.Parse("GCRecord is null and SendStatus=1 and SendOrgOid.Oid='" + org_oid + "'", null));
                        if (collection.Count > 0)
                        {
                            foreach (SendOrderSupplierAnimal row in collection)
                            {
                                SendOrderSupplierType item = new SendOrderSupplierType();
                                item.SendNo = row.SendNo;
                                item.SendDate = row.SendDate.ToString("dd/MM/yyyy");
                                item.SendOrgOid = row.SendOrgOid.Oid.ToString();
                                item.SendOrgName = row.SendOrgOid.SubOrganizeName;
                                item.SendOrgFullName = row.SendOrgOid.OrganizeNameTH;
                                item.Remark = row.Remark;
                                item.SendStatus = row.SendStatus.ToString();
                                item.FinanceYear = row.FinanceYearOid.YearName;
                                item.CancelMsg = row.CancelMsg;
                                item.ReceiveOrgOid = row.ReceiveOrgOid.Oid.ToString();
                                item.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                                item.ReceiveOrgFullName = row.ReceiveOrgOid.OrganizeNameTH;
                                item.PerUnit = row.PerUnit.ToString();
                                item.RefNo = row.SendNo + "|" + row.SendOrgOid.Oid.ToString() + "|2";

                                item.TotalWeight = row.QTY + " กิโลกรัม";
                                SendItems.Add(item);
                            }
                        }
                        //lists.Sender = null; //SendItems;
                        return Request.CreateResponse(HttpStatusCode.OK, SendItems);
                    }
                    else if (type == "1")
                    {  //รับ

                         IList<SendOrderSupplierAnimal> collection2 = ObjectSpace.GetObjects<SendOrderSupplierAnimal>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and  ReceiveStatus=1 and ReceiveOrgOid.Oid='" + org_oid + "'", null));
                        if (collection2.Count > 0)
                        {
                            foreach (SendOrderSupplierAnimal row in collection2)
                            {
                                ReceiveOrderSupplierType item = new ReceiveOrderSupplierType();
                                item.SendNo = row.SendNo;
                                item.SendDate = row.SendDate.ToString("dd/MM/yyyy");
                                item.SendOrgOid = row.SendOrgOid.Oid.ToString();
                                item.SendOrgName = row.SendOrgOid.SubOrganizeName;
                                item.SendOrgFullName = row.SendOrgOid.OrganizeNameTH;
                                item.Remark = row.Remark;
                                item.ReceiveStatus = row.SendStatus.ToString();
                                item.FinanceYear = row.FinanceYearOid.YearName;
                                item.CancelMsg = row.CancelMsg;
                                item.ReceiveOrgOid = row.ReceiveOrgOid.Oid.ToString();
                                item.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                                item.ReceiveOrgFullName = row.ReceiveOrgOid.OrganizeNameTH;
                                item.PerUnit = row.PerUnit.ToString();
                                item.RefNo = row.SendNo + "|" + row.ReceiveOrgOid.Oid.ToString() + "|1";
                                item.TotalWeight = row.QTY + " กิโลกรัม";
                                ReceiveItems.Add(item);
                            }
                            //lists.Receive = ReceiveItems;   
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, ReceiveItems);
                    }
                    else
                    {
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "0";
                        err.message = "กรุณาใส่ type ให้ถูกต้อง";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                   
                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล Org_Oid และ type (1=รับ/2=ส่ง) ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.status = "false";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = "ไม่พบข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }


        /// <summary>
        /// แสดงรายละเอียดข้อมูลส่ง-รับเสบียงสัตว์
        /// </summary>
        /// <param name="RefNo">เลขที่อ้างอิง|Oid หน่วยงาน|ประเภท</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrderAnimalstock/Detail")]
        public HttpResponseMessage GetSendOrderAnimalstockDetail()
        {
            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString();
              

                if (RefNo != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSupplierAnimal));
                    SendOrderSupplierType item = null;
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    IList<SendOrderSupplierAnimal> collection = ObjectSpace.GetObjects<SendOrderSupplierAnimal>(CriteriaOperator.Parse("GCRecord is null and SendNo='" + _refno + "'", null));

                    foreach (SendOrderSupplierAnimal row in collection)
                    {
                        item = new SendOrderSupplierType();

                        item.SendNo = row.SendNo;
                        if (row.SendDate.ToString("dd/MM/yyyy") !="")
                        {
                            item.SendDate = row.SendDate.ToString("dd/MM/yyyy");
                        }
                 
                        item.SendOrgOid = row.SendOrgOid.Oid.ToString();
                        item.SendOrgName = row.SendOrgOid.SubOrganizeName;
                        item.SendOrgFullName = row.SendOrgOid.OrganizeNameTH;
                        item.BudgetSourceName = row.BudgetSourceOid.BudgetName.ToString();
                        item.Remark = row.Remark;
                        item.SendStatus = row.SendStatus.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        if (row.CancelMsg != null)
                        {
                            item.CancelMsg = row.CancelMsg.ToString();
                        }
                        if (row.ReceiveOrgOid != null)
                        {
                            item.ReceiveOrgOid = row.ReceiveOrgOid.Oid.ToString();
                            item.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName.ToString();
                            item.ReceiveOrgFullName = row.ReceiveOrgOid.OrganizeNameTH.ToString();
                        }
                        
                        if (row.ObjectTypeOid==null)
                        {
                            item.ObjectTypeName = "ไม่พบข้อมูลวัตถุประสงค์";
                        }
                        else
                        {
                            item.ObjectTypeName = row.ObjectTypeOid.ObjectTypeName.ToString();
                        }
                       
                        if (row. QuotaTypeOid == null)
                        {
                            item.QuotaTypeName = "ไม่พบข้อมูลโควตาศูนย์";
                        }
                        else
                        {
                            item.QuotaTypeName = row.QuotaTypeOid.QuotaName;
                        }
                        if (row.StockLimit.ToString() != "")
                        {
                   
                            item.StockLimit = row.StockLimit;
                        }
                        
                        if (row.AnimalSupplieOid == null )
                        {
                            item.AnimalSupplieName = "ไม่พบข้อมูลชนิดเสบียงสัตว์";
                        }
                        else
                        {
                            item.AnimalSupplieName = row.AnimalSupplieOid.AnimalSupplieName;
                        }
                        if  (row.UnitOid == null)
                        {
                            item.UnitName = "ไม่พบข้อมูลหน่วยนับ";
                        }
                        else
                        {
                            item.UnitName = row.UnitOid.UnitName;
                        }

                        item.AnimalSeedName = "";
                   
                        item.AnimalSupplieTypeName = row.AnimalSupplieTypeOid.SupplietypeName.ToString();
                        item.PackageName = row.PackageOid.PackageName;
                        item.PerUnit = row.PerUnit.ToString();
                   
                        item.RefNo = RefNo;
                      
                        item.QTY = row.QTY;
                        item.TotalWeight = row.QTY.ToString();
                        //List<SendOrderSeedDetailType> details = new List<SendOrderSeedDetailType>();
                        //SendOrderSeedDetailType _dt = null;

                 
                        List<SendOrderSupplierType_Model> listitem = new List<SendOrderSupplierType_Model>();

                        foreach (SendOrderSupplierAnimal row2 in collection)
                        {
                            SendOrderSupplierType_Model itemD = new SendOrderSupplierType_Model();
                            itemD.AnimalSupplieTypeName = row2.AnimalSupplieTypeOid.SupplietypeName.ToString();
                            if (row2.AnimalSupplieOid == null)
                            {
                                itemD.AnimalSupplieName = "ไม่พบข้อมูลชนิดเสบียงสัตว์";
                            }
                            else
                            {
                                itemD.AnimalSupplieName = row2.AnimalSupplieOid.AnimalSupplieName;
                            }
                            itemD.BudgetSourceName = row2.BudgetSourceOid.BudgetName.ToString();
                            itemD.AnimalSeedName = "";

                            itemD.PackageName = row2.PackageOid.PackageName;
                            itemD.TotalWeight = row2.QTY.ToString();
                            itemD.QTY = row2.QTY.ToString();
                            listitem.Add(itemD);

                        }
                        item.Details = listitem;



                    }
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล RefNo ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }


        /// <summary>
        /// ปรับปรุงข้อมูลส่ง-รับเมล็ดพันธุ์
        /// </summary>
        /// <param name="RefNo">เลขที่อ้างอิง|Oid หน่วยงาน|ประเภท</param>
        /// <param name="Status">สถานะการตรวจสอบ (1=อนุมัติ/2=ไม่อนุมัติ)</param>
        /// <param name="CancelMsg">หมายเหตุกรณีไม่อนุมัติ</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrderAnimalstock/Update")]
        public HttpResponseMessage UpdateSendOrderAnimalstock()
        {
            object objStockAnimalInfo = null;
            HistoryWork ObjHistory = null;
            string Username = " ";

            IList<StockAnimalInfo_Report> objStockAnimalInfo_Detail = null;

            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                string CancelMsg = HttpContext.Current.Request.Form["CancelMsg"].ToString(); //หมายเหตุ
                Username = HttpContext.Current.Request.Form["Username"].ToString();
                if (RefNo != "" && Status != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SendOrderSupplierAnimal));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockAnimalInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockAnimalInfo_Report));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SeedType));
                    List<SendOrderSeed> list = new List<SendOrderSeed>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SendOrderSupplierAnimal objSupplierProduct = ObjectSpace.FindObject<SendOrderSupplierAnimal>(CriteriaOperator.Parse("SendNo=?", _refno));


                    if (objSupplierProduct.SeedTypeOid != null)
                    {
                        objStockAnimalInfo = ObjectSpace.GetObjects<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and SeedTypeOid=? and AnimalSupplieOid=? and AnimalProductNumber=? ", objSupplierProduct.SendOrgOid.Oid, objSupplierProduct.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.SeedTypeOid.Oid, objSupplierProduct.AnimalSupplieOid, objSupplierProduct.SendNo));
                        // XPCollection<StockAnimalInfo_Report> objStockAnimalInfo_Detail;
                        objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and SeedTypeOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? ", objSupplierProduct.SendOrgOid.Oid, objSupplierProduct.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.SeedTypeOid.Oid, objSupplierProduct.AnimalSupplieOid, objSupplierProduct.AnimalSupplieTypeOid));
                    }
                    else
                    {
                        objStockAnimalInfo = ObjectSpace.GetObjects<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and SeedTypeOid=? and AnimalSupplieOid=? and AnimalProductNumber=? ", objSupplierProduct.SendOrgOid.Oid, objSupplierProduct.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.SeedTypeOid.Oid, objSupplierProduct.AnimalSupplieOid, objSupplierProduct.SendNo));
                        // XPCollection<StockAnimalInfo_Report> objStockAnimalInfo_Detail;
                        objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and SeedTypeOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? ", objSupplierProduct.SendOrgOid.Oid, objSupplierProduct.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.SeedTypeOid.Oid, objSupplierProduct.AnimalSupplieOid, objSupplierProduct.AnimalSupplieTypeOid));

                    }
                    if (objStockAnimalInfo != null)
                    {
                        var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockAnimalInfo>();
                        var withBlock = ObjStockSeedInfoInfo;
                        withBlock.TransactionDate = DateTime.Now;
                        withBlock.AnimalProductNumber = objSupplierProduct.SendNo;
                        withBlock.FinanceYearOid = objSupplierProduct.FinanceYearOid;
                        withBlock.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                        withBlock.OrganizationOid = objSupplierProduct.SendOrgOid;
                        withBlock.AnimalSupplieOid = objSupplierProduct.AnimalSupplieOid;
                        //  '.AnimalSeedOid = ObjMaster.AnimalSeedOid;
                        withBlock.AnimalSupplieTypeOid = objSupplierProduct.AnimalSupplieTypeOid;
                        withBlock.Remark = "อนุมัติส่งเสบียงสัตว์ เลขที่: " + "" + objSupplierProduct.SendNo;
                        withBlock.Weight = 0 - objSupplierProduct.QTY;
                        withBlock.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                        withBlock.Description = "ส่งเสบียงสัตว์ให้ : " + "" + objSupplierProduct.ReceiveOrgOid.SubOrganizeName;
                    }
                
                var objStockAnimalInfo_DetailNew = ObjectSpace.CreateObject<StockAnimalInfo_Report>();
                    if (objStockAnimalInfo_Detail.Count == 0)
                    {
                        var withBlock = objStockAnimalInfo_DetailNew;
                        withBlock.AnimalProductNumber = objSupplierProduct.SendNo;
                        withBlock.FinanceYearOid = objSupplierProduct.FinanceYearOid;
                        withBlock.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                        withBlock.OrganizationOid = objSupplierProduct.SendOrgOid;
                        withBlock.AnimalSupplieOid = objSupplierProduct.AnimalSupplieOid;
                        //  ' .AnimalSeedOid = ObjMaster.AnimalSeedOid
                        withBlock.AnimalSupplieTypeOid = objSupplierProduct.AnimalSupplieTypeOid;
                        //  '.Weight = ObjMaster.Weight
                        withBlock.TotalForward = 0;
                        withBlock.TotalChange = 0 - objSupplierProduct.QTY;
                        withBlock.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                        withBlock.Description = "ส่งเสบียงสัตว์ให้ : " + "" + objSupplierProduct.ReceiveOrgOid.SubOrganizeName;
                    }
                    else
                    {
                        var ObjStockAnimalInfo_DetailSource = (from Item in objStockAnimalInfo_Detail orderby Item.TransactionDate descending select Item).First().TotalWeight ;
                        var withBlock = objStockAnimalInfo_DetailNew;
                        withBlock.AnimalProductNumber = objSupplierProduct.SendNo;
                        withBlock.FinanceYearOid = objSupplierProduct.FinanceYearOid;
                        withBlock.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                        withBlock.OrganizationOid = objSupplierProduct.SendOrgOid;
                        withBlock.AnimalSupplieOid = objSupplierProduct.AnimalSupplieOid;
                        //  ' .AnimalSeedOid = ObjMaster.AnimalSeedOid
                        withBlock.AnimalSupplieTypeOid = objSupplierProduct.AnimalSupplieTypeOid;
                        //  '.Weight = ObjMaster.Weight
                        withBlock.TotalForward = ObjStockAnimalInfo_DetailSource;
                        withBlock.TotalChange = 0 - objSupplierProduct.QTY;
                        withBlock.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                        withBlock.Description = "ส่งเสบียงสัตว์ให้ : " + "" + objSupplierProduct.ReceiveOrgOid.SubOrganizeName;
                    }
                    ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                    ObjHistory.RefOid = objSupplierProduct.Oid.ToString();
                    ObjHistory.FormName = "เสบียงสัตว์";
                    ObjHistory.Message = "อนุมัติ (ส่งเสบียงสัตว์ให้หน่วยงานในสังกัด (Mobile Application)) เลขที่ส่ง : " + " "+ objSupplierProduct.SendNo;
                    ObjHistory.CreateBy = Username;
                    ObjHistory.CreateDate = DateTime.Now;
                    ObjectSpace.CommitChanges();

                    
                    if (objSupplierProduct != null)
                    {
                        if (_type == "1") //รับ
                        {
                            if (Status == "1")
                            { //Approve
                                objSupplierProduct.ReceiveStatus = EnumReceiveOrderAnimalStatus.Approve; //2
                                objSupplierProduct.Remark = CancelMsg;
                                ObjectSpace.CommitChanges();
                            }
                            else if (Status == "2")
                            { //Reject
                                objSupplierProduct.SendStatus = EnumSendOrderAnimalStatus.Eject;
                                objSupplierProduct.ReceiveStatus = EnumReceiveOrderAnimalStatus.Eject;//4
                                objSupplierProduct.CancelMsg = CancelMsg;
                                ObjectSpace.CommitChanges();
                            }
                        }
                        else if (_type == "2") //หน่วยส่ง
                        {
                            if (Status == "1")
                            { //Approve
                                objSupplierProduct.SendStatus = EnumSendOrderAnimalStatus.Approve; //2
                                objSupplierProduct.ReceiveStatus = EnumReceiveOrderAnimalStatus.InProgess;
                                objSupplierProduct.Remark = CancelMsg;
                                ObjectSpace.CommitChanges();
                            }
                            else if (Status == "2")
                            { //Reject
                                objSupplierProduct.SendStatus = EnumSendOrderAnimalStatus.Eject; //4
                                objSupplierProduct.CancelMsg = CancelMsg;
                                ObjectSpace.CommitChanges();
                            }
                        }

                        UpdateResult ret = new UpdateResult();
                        ret.status = "true";
                        ret.message = "บันทึกข้อมูลเสร็จเรียบร้อยแล้ว";
                        return Request.CreateResponse(HttpStatusCode.OK, ret);

                    }
                    else
                    {
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "-1";
                        err.message = "ไม่พบข้อมูล";
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }
                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล RefNo ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// อนุมัติ-ไม่อนุมัติการใช้เมล็ดพันธุ์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimalProduct/Update")]
        public HttpResponseMessage UpdateSupplierUseProduct()  ///SupplierUseAnimalProduct/Update
        {
            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                string Remark = HttpContext.Current.Request.Form["Remark"].ToString();


                if (RefNo != "" && Status != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseAnimalProduct));
                   
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SupplierUseAnimalProduct objSupplierUseProduct = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse("UseNo=?", _refno));
                    if (objSupplierUseProduct != null)
                    {

                        if (Status == "1")
                        { //อนุมัติ
                            objSupplierUseProduct.Status = EnumRodBreedProductSeedStatus.Approve; //2
                            objSupplierUseProduct.Remark = Remark;
                            ObjectSpace.CommitChanges();
                        }
                        else if (Status == "2")
                        { //ไม่อนุมัติ
                            objSupplierUseProduct.Status = EnumRodBreedProductSeedStatus.Eject; //4
                            objSupplierUseProduct.Remark = Remark;
                            ObjectSpace.CommitChanges();
                        }

                        UpdateResult ret = new UpdateResult();
                        ret.status = "true";
                        ret.message = "บันทึกข้อมูลเสร็จเรียบร้อยแล้ว";
                        return Request.CreateResponse(HttpStatusCode.OK, ret);

                    }
                    else
                    {
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "-1";
                        err.message = "ไม่พบข้อมูล";
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }
                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล RefNo ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
       







        /// <summary>
        /// หน้าส่งเสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost ]
        [Route("SendOrderAnimal/accept")]
        public IHttpActionResult LoadSendAnimal_accept()
        {
            object SendOrgOid;
            try
            {

                SendOrgOid = HttpContext.Current.Request.Form["SendOrgOid"].ToString();


                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSupplierAnimal));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
                List<SendOrderSupplierAnimal_info> list = new List<SendOrderSupplierAnimal_info>();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SendOrderSupplierAnimal> collection = ObjectSpace.GetObjects<SendOrderSupplierAnimal>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 5 and SendOrgOid=?", SendOrgOid));
                if (collection.Count > 0)
                {
                                foreach (SendOrderSupplierAnimal row in collection)
                                {
                                    SendOrderSupplierAnimal_info SupplierAnimal = new SendOrderSupplierAnimal_info();
                                    SupplierAnimal.SendNo = row.SendNo;
                                    SupplierAnimal.SendDate = row.SendDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); 
                                    SupplierAnimal.FinanceYear = row.FinanceYearOid.YearName;
                                    SupplierAnimal.SendOrgOid = row.SendOrgOid.Oid;
                                    SupplierAnimal.SendOrgName = row.SendOrgOid.SubOrganizeName;
                                    SupplierAnimal.ReceiveOrgOid = row.ReceiveOrgOid.Oid;
                                    SupplierAnimal.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;                      
                                    SupplierAnimal.Remark = row.Remark;
                                    SupplierAnimal.Send_Messengr = row.SendStatus.ToString();
                                    SupplierAnimal.Weight = row.QTY;
                                    list.Add(SupplierAnimal);
                                }
                    return Ok(list);
                }

                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest("NoData");
                }
               
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// หน้ารับเสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ReceiveOrderAnimal/accept")]
        public IHttpActionResult LoadReceiveAnimal_accept()
        {
            object ReceiveOrgOid;
            try
            {
                ReceiveOrgOid = HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSupplierAnimal));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
                List<ReceiveOrderAnimal_info> list = new List<ReceiveOrderAnimal_info>();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SendOrderSupplierAnimal> collection = ObjectSpace.GetObjects<SendOrderSupplierAnimal>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 2 and ReceiveOrgOid=?", ReceiveOrgOid));
                if (collection.Count > 0)
                {
                    foreach (SendOrderSupplierAnimal row in collection)
                    {
                        ReceiveOrderAnimal_info SupplierAnimal = new ReceiveOrderAnimal_info();
                        SupplierAnimal.SendNo = row.SendNo;
                        SupplierAnimal.SendDate = row.SendDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); 
                        SupplierAnimal.FinanceYear = row.FinanceYearOid.YearName;                     
                        SupplierAnimal.ReceiveOrgOid = row.ReceiveOrgOid.Oid;
                        SupplierAnimal.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                        SupplierAnimal.SendOrgOid = row.SendOrgOid.Oid;
                        SupplierAnimal.SendOrgName = row.SendOrgOid.SubOrganizeName;
                        SupplierAnimal.CancelMsg = row.CancelMsg;
                        SupplierAnimal.Send_Messengr = row.SendStatus.ToString();
                        SupplierAnimal.Package =row.PackageOid.PackageName;
                        SupplierAnimal.Weight = row.QTY;
                        list.Add(SupplierAnimal);
                    }
                    return Ok(list);
                }

                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest(err.message);
                }

            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost ]
        [Route("SupplierUseAnimalProduct/accept")] ///การใช้เสบียงสัตว์
        public IHttpActionResult Get_SupplierUseAnimalProduct()
        {
            object OrganizationOid;
            string ActivityOid;
            try
            {

                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                ActivityOid = HttpContext.Current.Request.Form["ActivityOid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
                List<SupplierAnimalUseProduct_Model> list_detail = new List<SupplierAnimalUseProduct_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SupplierUseAnimalProduct> collection = ObjectSpace.GetObjects<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null and Status = 1  and OrganizationOid=? and ActivityOid = ? ", OrganizationOid,ActivityOid));
               
                if (OrganizationOid != null)
                {
                    foreach (SupplierUseAnimalProduct row in collection)
                    {

                        SupplierAnimalUseProduct_Model SupplierAnimal = new SupplierAnimalUseProduct_Model();
                        SupplierAnimal.Oid = row.Oid.ToString();
                        SupplierAnimal.UseDate= row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        SupplierAnimal.UseNo = row.UseNo;
                        SupplierAnimal.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        SupplierAnimal.FinanceYear = row.FinanceYearOid.YearName;
                        SupplierAnimal.OrganizationOid = row.OrganizationOid.Oid.ToString();
                        SupplierAnimal.Organization = row.OrganizationOid.SubOrganizeName;
                        if (row.EmployeeOid == null)
                        {
                            SupplierAnimal.EmployeeOid = "ไม่มีข้อมูลรายชื่อ";
                        }
                        else
                        {
                            SupplierAnimal.EmployeeOid = row.EmployeeOid.ToString();
                        }
                        
                        if (row.EmployeeOid  == null)
                        {
                            SupplierAnimal.Employee = "ไม่มีข้อมูลรายชื่อ";
                        }
                        else
                        {
                            SupplierAnimal.Employee = row.EmployeeOid.EmployeeFirstName + row.EmployeeOid.EmployeeLastName;
                        }
                            
                        SupplierAnimal.ActivityOid = row.ActivityOid.Oid.ToString();
                        SupplierAnimal.ActivityName = row.ActivityOid.ActivityName;
                       // SupplierAnimal.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        SupplierAnimal.SubActivityOid = row.SubActivityOid.Oid.ToString();
                        SupplierAnimal.SubActivityName = row.SubActivityOid.ActivityName;
                        SupplierAnimal.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                        SupplierAnimal.RegisCusService = row.RegisCusServiceOid.FirstNameTH + row.RegisCusServiceOid.LastNameTH;
                        if (row.
                            
                            
                            
                            
                            
                            
                            Oid == null)
                        {
                            SupplierAnimal.OrgeServiceOid = "ไม่พบข้อมูลองค์กร";
                            SupplierAnimal.OrgeService = "ไม่พบข้อมูลองค์กร";
                        }
                        else
                        {
                            SupplierAnimal.OrgeServiceOid = row.OrgeServiceOid.Oid.ToString();

                        }
                        

                        

                        //foreach (SupplierUseProductDetail row2 in row.SupplierUseProductDetails)
                        //{
                        //    Weight = row2.Weight;
                        //}
                        //Supplier.Weight = Weight + " " + "กิโลกรัม";

                        list_detail.Add(SupplierAnimal);
                    }

                    return Ok(list_detail);
                }

                else if (list_detail == null)
                {
                    UserError err = new UserError();
                    err.code = "3"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest("รายการอนุมัติไม่สมบูรณ์");
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest(err.message);
                }
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("LoadSupplierUseProduct/{UseNo}")] // ใส่ OIDSendOrderSeed ใบนำส่ง
        public IHttpActionResult SendSupplierSeedDetail_ByOrderSeedID()
        {
            object UseNo = string.Empty;
            object OrganizationOid = string.Empty;
            string ActivityOid = string.Empty;
            SupplierAnimalUseProduct_Model sendDetail = new SupplierAnimalUseProduct_Model();

            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                if (HttpContext.Current.Request.Form["UseNo"].ToString() != null)
                {
                    UseNo = HttpContext.Current.Request.Form["UseNo"].ToString();
                }
                if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != null)
                {
                    OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                }
                if (HttpContext.Current.Request.Form["ActivityOid"].ToString()!=null)
                {
                    ActivityOid = HttpContext.Current.Request.Form["ActivityOid"].ToString();
                }
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                List<SupplierAnimalUseProduct_Model> list = new List<SupplierAnimalUseProduct_Model>();
                List<SupplierAnimalUseProductDetail_Model> list_detail = new List<SupplierAnimalUseProductDetail_Model>();
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SupplierUseAnimalProduct SupplierUseAnimalProduct_;
                SupplierUseAnimalProduct_ = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse("GCRecord is null and Status = 1  and OrganizationOid=? and ActivityOid = ? ", UseNo, OrganizationOid, ActivityOid));
                //sendOrderSeed = ObjectSpace.GetObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", null));
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select UseNo from SupplierUseAnimalProduct where UseNo = '" + UseNo + "'");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    sendDetail.Oid = SupplierUseAnimalProduct_.Oid.ToString();
                    sendDetail.UseNo = SupplierUseAnimalProduct_.UseNo;
                    sendDetail.UseDate = SupplierUseAnimalProduct_.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                    sendDetail.FinanceYearOid = SupplierUseAnimalProduct_.FinanceYearOid.ToString();
                    sendDetail.FinanceYear = SupplierUseAnimalProduct_.FinanceYearOid.YearName;
                    sendDetail.OrganizationOid = SupplierUseAnimalProduct_.OrganizationOid.ToString();
                    sendDetail.Organization = SupplierUseAnimalProduct_.OrganizationOid.SubOrganizeName;
                    if (SupplierUseAnimalProduct_.EmployeeOid == null)
                    {
                        sendDetail.EmployeeOid = "ไม่มีข้อมูลรายชื่อ";
                        sendDetail.Employee = "ไม่พบข้อมูล";
                    }
                    else
                    {
                        sendDetail.EmployeeOid = SupplierUseAnimalProduct_.EmployeeOid.ToString();
                        sendDetail.Employee = SupplierUseAnimalProduct_.EmployeeOid.EmployeeFirstName + SupplierUseAnimalProduct_.EmployeeOid.EmployeeLastName;
                    }
                  
                    sendDetail.ActivityOid = SupplierUseAnimalProduct_.ActivityOid.ToString();
                    sendDetail.ActivityName = SupplierUseAnimalProduct_.ActivityOid.ActivityName;
                    sendDetail.SubActivityOid = SupplierUseAnimalProduct_.SubActivityOid.ToString();
                    sendDetail.SubActivityName = SupplierUseAnimalProduct_.SubActivityOid.ActivityName;
                    sendDetail.Remark = SupplierUseAnimalProduct_.Remark;
                    sendDetail.ApproveDate = SupplierUseAnimalProduct_.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                    //SupplierAnimalUseProductDetail_Model list_detail = new SupplierAnimalUseProductDetail_Model();
                    //foreach (SupplierUseProductDetail row in SupplierUseProduct_.SupplierUseAnimalProductDetails)
                    {

                    }


                    return Ok(sendDetail);
                }
                else
                {
                    return BadRequest("NoData");
                }
            }

            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }

        }

    }
}
