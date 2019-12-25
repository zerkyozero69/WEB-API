using System;
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
using DevExpress.Xpo;
using System.Globalization;
using static WebApi.Jwt.Models.Supplier;

namespace WebApi.Jwt.Controllers
{
    public class Approval_SendSeed : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
       
        #region แบบใช้ฟังค์ชั่นของ xaf 

        /// <summary>
        /// หารายละเอียดการส่งเมล็ดด้วย sendOID
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrder/{Send_No}")] // ใส่ OIDSendOrderSeed ใบนำส่ง /SendOrder/226-0011
        //[JwtAuthentication]
        public IHttpActionResult SendOrderSeedDetail_ByOrderSeedID()

        {

            object Send_No = string.Empty;
            object ReceiveOrgOid = string.Empty;
            Approve_Model sendDetail = new Approve_Model();

            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                if (HttpContext.Current.Request.Form["Send_No"].ToString() != null)
                {
                    Send_No = HttpContext.Current.Request.Form["Send_No"].ToString();
                }
                if (HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString() != null)
                {
                    ReceiveOrgOid = HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString();
                }
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                List<Approve_Model> list = new List<Approve_Model>();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SendOrderSeed sendOrderSeed;
                sendOrderSeed = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and SendNo=? and ReceiveOrgOid=? ", Send_No, ReceiveOrgOid));
                //sendOrderSeed = ObjectSpace.GetObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", null));
                if (Send_No != null)
                {
                    double sum = 0;
                    sendDetail.Send_No = sendOrderSeed.SendNo;
                    sendDetail.SendDate = Convert.ToDateTime(sendOrderSeed.SendDate).ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                    sendDetail.SendOrgName = sendOrderSeed.SendOrgOid.SubOrganizeName;
                    sendDetail.ReceiveOrgName = sendOrderSeed.ReceiveOrgOid.SubOrganizeName;
                    sendDetail.Remark = sendOrderSeed.Remark;
                    sendDetail.SendStatus = sendOrderSeed.SendStatus.ToString();
                    sendDetail.FinanceYear = sendOrderSeed.FinanceYearOid.YearName;

                    //if (sendOrderSeed.CancelMsg == null)
                    //{
                    //    sendDetail.CancelMsg = "ไม่";
                    //}
                    //else
                    //{
                    //    sendDetail.CancelMsg = sendOrderSeed.CancelMsg;
                    //}

                    foreach (SendOrderSeedDetail row in sendOrderSeed.SendOrderSeedDetails)
                    {

                        SendOrderSeed_Model send_Detail = new SendOrderSeed_Model();
                        send_Detail.LotNumber = row.LotNumber.LotNumber;
                        send_Detail.WeightUnit = row.WeightUnitOid.UnitName;
                        send_Detail.AnimalSeedCode = row.AnimalSeedCode;
                        send_Detail.AnimalSeedLevel = row.AnimalSeedLevel;
                        send_Detail.AnimalSeedName = row.AnimalSeeName;
                        send_Detail.BudgetSource = row.BudgetSourceOid.BudgetName;

                        send_Detail.Weight = row.Weight.ToString();
                        send_Detail.Used = row.Used.ToString();
                        send_Detail.SendOrderSeed = row.SendOrderSeed.SendNo;
                        send_Detail.AnimalSeedOid = row.AnimalSeedOid.SeedName;
                        send_Detail.AnimalSeedLevelOid = row.AnimalSeedLevelOid.SeedLevelName;
                        send_Detail.SeedTypeOid = row.SeedTypeOid.SeedTypeName;
                        send_Detail.Amount = row.Amount;
                        sum = sum + row.Weight;


                        list_detail.Add(send_Detail);
                    }
                    sendDetail.Weight_All = sum.ToString() + " " + "กิโลกรัม";
                    sendDetail.objSeed = list_detail;
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

        /// <summary>
        /// เรียกหน้าใช้เมล็ดพันธุ์ ช่วยภัยพิบัติ
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("LoadSupplierUseProduct_Detail")]
        //public IHttpActionResult LoadSupplierUse_accept_Detail()
        //{
        //    object OrganizationOid;
        //    string UseNo;
        //    SupplierProductUser Detail = new SupplierProductUser();
        //    try
        //    {

        //        OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
        //        UseNo = HttpContext.Current.Request.Form["UseNo"].ToString();
        //        string ActivityOid = "B100C7C1 - 4755 - 4AF0 - 812E-3DD6BA372D45";
        //        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //        XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));
        //        List<SupplierUseProductDetail_Model> list_detail = new List<SupplierUseProductDetail_Model>();
        //        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //        var ActivityOid = "B100C7C1-4755-4AF0-812E-3DD6BA372D45";
        //        SupplierUseProduct supplier_Use;
        //        supplier_Use = ObjectSpace.FindObject<SupplierUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Stauts = 1 and OrganizationOid=? and ActivityOid = ? and UseNo = ? ", OrganizationOid, ActivityOid, UseNo));
        //            < SupplierUseProduct > collection = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Stauts = 1 and OrganizationOid=? and ActivityOid = ?", OrganizationOid, ActivityOid));


        //        DataSet ds = new DataSet();
        //        ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select  UseNo from SupplierUseProduct where UseNo = '" + UseNo + "' ");
        //        if (ds.Tables[0].Rows.Count != 0)
        //        {
        //            double sum = 0;
        //            Detail.Oid = supplier_Use.Oid.ToString();
        //            Detail.UseNo = supplier_Use.UseNo;
        //            Detail.FinanceYearOid = supplier_Use.FinanceYearOid.ToString();
        //            Detail.FinanceYear = supplier_Use.FinanceYearOid.YearName;
        //            Detail.OrganizationName = supplier_Use.OrganizationOid.SubOrganizeName;
        //            Detail.OrgeService = supplier_Use.OrgeServiceOid.ToString();
        //            Detail.OrgeServiceName = supplier_Use.OrgeServiceOid.OrgeServiceName;
        //            Detail.RegisCusService = supplier_Use.RegisCusServiceOid.ToString();
        //            Detail.RegisCusServiceName = supplier_Use.RegisCusServiceOid.FirstNameTH + supplier_Use.RegisCusServiceOid.LastNameTH;
        //            Detail.ServiceCount = supplier_Use.ServiceCount;
        //            Detail.Addrres = supplier_Use.RegisCusServiceOid.FullAddress;
        //            Detail.ActivityNameOid = supplier_Use.SubActivityOid.ActivityName;
        //            if (supplier_Use.Remark == null)
        //            {
        //                Detail.Remark = "ไม่พบข้อมูล";
        //            }
        //            {
        //                Detail.Remark = supplier_Use.Remark;
        //            }
        //            foreach (SupplierUseProductDetail row in supplier_Use.SupplierUseProductDetails)
        //            {
        //                SupplierUseProductDetail_Model model = new SupplierUseProductDetail_Model();
        //                model.Oid = row.Oid.ToString();
        //                model.AnimalSeedName = row.AnimalSeedOid.SeedName;
        //                model.AnimalSeed = row.AnimalSeedLevelOid.SeedLevelName;

        //                sum = sum + row.Weight;
        //                list_detail.Add(model);
        //            }
        //            Detail.Weight_All = sum.ToString() + " " + "กิโลกรัม";
        //            Detail.objProduct = list_detail;
        //            return Ok(Detail);
        //        }
        //        else
        //        {
        //            return BadRequest("Nodata");
        //        }
        //    }
        //    catch (Exception ex)
        //    { //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

        //        err.message = ex.Message;
        //        Return resual
        //        return BadRequest(err.message);

        //    }

        //}

        /// <summary>
        /// หน้าเรียกการขอใช้เมล็ด
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("SupplierUseProduct/{UseNo}")] // ใส่ OIDSendOrderSeed ใบนำส่ง
        //public IHttpActionResult SupplierUseProduct_ByOrderSeedID()
        //{
        //    string UseNo = string.Empty;
        //    string OrganizationOid = string.Empty;

        //    SupplierProductUser supplierproduct = new SupplierProductUser();

        //    SupplierUseProductDetail_Model Model = new SupplierUseProductDetail_Model();
        //    try
        //    {
        //        if (HttpContext.Current.Request.Form["UseNo"].ToString() != null)
        //        {
        //            UseNo = HttpContext.Current.Request.Form["UseNo"].ToString();
        //        }
        //        if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != null)
        //        {
        //            OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
        //        }


        //        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //        XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));
        //        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
        //        List<SupplierUseProductDetail_Model> list_detail = new List<SupplierUseProductDetail_Model>();
        //        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //        SupplierUseProduct supplierUseProduct;
        //        supplierUseProduct = ObjectSpace.FindObject<SupplierUseProduct>(CriteriaOperator.Parse("GCRecord is null and Stauts = 1 and UseNo=? and OrganizationOid=? ", UseNo, OrganizationOid));
        //        sendOrderSeed = ObjectSpace.GetObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", null));
        //        /*ใส่พารามิเตอร์ 2 ตัว */
        //        if (UseNo != null)
        //        {
        //            double sum = 0;
        //            supplierproduct.UseDate = supplierUseProduct.
      //  .ToString("dd-MM-yyyy", new CultureInfo("us-US")); ;
        //            supplierproduct.UseNo = supplierUseProduct.UseNo;
        //            supplierproduct.FinanceYear = supplierUseProduct.FinanceYearOid.YearName;
        //            supplierproduct.OrganizationName = supplierUseProduct.OrganizationOid.SubOrganizeName;
        //            supplierproduct.Addrres = supplierUseProduct.OrganizationOid.FullAddress;
        //            supplierproduct.EmployeeName = supplierUseProduct.EmployeeOid.FullName;
        //            supplierproduct.Remark = supplierUseProduct.Remark;
        //            supplierproduct.Stauts = supplierUseProduct.Stauts.ToString();
        //            supplierproduct.ApproveDate = supplierUseProduct.ApproveDate.ToShortDateString();
        //            supplierproduct.ActivityName = supplierUseProduct.ActivityOid.ActivityName;
        //            supplierproduct.SubActivityName = supplierUseProduct.SubActivityOid.ActivityName;
        //            supplierproduct.Weight_All = sum.ToString() + " " + "กิโลกรัม";

        //            supplierproduct.ReceiptNo = supplierUseProduct.ReceiptNo;

        //            supplierproduct.OrgeServiceOid = supplierUseProduct.OrgeServiceOid.OrgeServiceName;

        //            foreach (SupplierUseProductDetail row in supplierUseProduct.SupplierUseProductDetails)
        //            {
        //                SupplierUseProductDetail_Model send_Detail = new SupplierUseProductDetail_Model();
        //                send_Detail.AnimalSeedName = row.AnimalSeedOid.SeedName;
        //                send_Detail.AnimalSeedLevelName = row.AnimalSeedLevelOid.SeedLevelName;
        //                send_Detail.StockLimit = row.StockLimit;
        //                send_Detail.Weights = row.Weight;
        //                send_Detail.WeightUnit = row.WeightUnitOid.UnitName;
        //                send_Detail.BudgetSourceName = row.BudgetSourceOid.BudgetName;
        //                send_Detail.SupplierUseProduct = row.SupplierUseProduct.Oid.ToString();
        //                send_Detail.LotNumber = row.LotNumber.LotNumber;
        //                send_Detail.SeedType = row.SeedTypeOid.SeedTypeName;
        //                send_Detail.PerPrice = row.PerPrice;
        //                send_Detail.Price = row.Price;
        //                sum = sum + row.Weight;
        //                list_detail.Add(send_Detail);
        //            }
        //            supplierproduct.objProduct = list_detail;

        //            return Ok(supplierproduct);
        //        }
        //        else
        //        {
        //            return BadRequest("NoData");
        //        }
        //    }

        //    catch (Exception ex)
        //    { //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        err.message = ex.Message;
        //        Return resual
        //        return BadRequest(ex.Message);
        //    }

        //}

        #endregion การอนุมัติเบิกใช้เมล็ดพันธุ์



        #region SendOrderSeedApprove ยืนยันเมล็ดพันธุ์
        [AllowAnonymous]
        [HttpPost]
        [Route("SendSeed/ApprovalSend2")]

        public IHttpActionResult ApprovalSend_SupplierUseProduct(string Send_No)
        {




            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                sendSeed_info sendDetail = new sendSeed_info();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                SendOrderSeed ObjMaster;
                ObjMaster = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("SendNo=?", Send_No));
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();

                ;
                //SendOrderSeed sendOrderSeed;
                //sendOrderSeed = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("SendNo=?", Send_No));

                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select SendNo from SendOrderSeed where SendNo = '" + Send_No + "'");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    double sum = 0;
                    //sendDetail.Oid = sendOrderSeed.Oid;
                    //sendDetail.Send_No = sendOrderSeed.SendNo;
                    //sendDetail.SendDate = Convert.ToDateTime(sendOrderSeed.SendDate).ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                    //sendDetail.SendOrgOid = sendOrderSeed.SendOrgOid.Oid;
                    //sendDetail.SendOrgName = sendOrderSeed.SendOrgOid.SubOrganizeName;
                    //sendDetail.ReceiveOrgoid = sendOrderSeed.ReceiveOrgOid.Oid;
                    //sendDetail.ReceiveOrgName = sendOrderSeed.ReceiveOrgOid.SubOrganizeName;
                    //sendDetail.FinanceYearOid = sendOrderSeed.FinanceYearOid;
                    //sendDetail.FinanceYear = sendOrderSeed.FinanceYearOid.YearName;

                    SendOrderSeed_Model objsend_Detail = new SendOrderSeed_Model();
                    foreach (SendOrderSeedDetail row in ObjMaster.SendOrderSeedDetails)
                    {

                        objsend_Detail.LotNumber = row.LotNumber.Oid;

                        objsend_Detail.WeightUnit = row.WeightUnitOid.UnitName;
                        objsend_Detail.AnimalSeedLevelOid = row.AnimalSeedLevelOid;
                        objsend_Detail.BudgetSourceOid = row.BudgetSourceOid;
                        objsend_Detail.BudgetSource = row.BudgetSourceOid.BudgetName;
                        objsend_Detail.Weight = row.Weight.ToString();
                        objsend_Detail.Used = row.Used.ToString();
                        objsend_Detail.AnimalSeedOid = row.AnimalSeedOid;
                        objsend_Detail.AnimalSeedLevelOid = row.AnimalSeedLevelOid;
                        objsend_Detail.SeedTypeOid = row.SeedTypeOid.SeedTypeName;
                        objsend_Detail.Amount = row.Amount;
                        sum = sum + row.Weight;
                        list_detail.Add(objsend_Detail);

                    }
                    nutrition.Module.StockSeedInfo ObjStockSeedInfoInfo;
                    var objSupplierProduct = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("Oid =?", objsend_Detail.LotNumber));

                    var objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? and StockType=0 ", ObjMaster.SendOrgOid.Oid, ObjMaster.FinanceYearOid, objsend_Detail.BudgetSourceOid, objsend_Detail.AnimalSeedOid
                    , objsend_Detail.AnimalSeedLevelOid, objsend_Detail.LotNumber));
                    if (objStockSeedInfo == null)
                    {

                        //var stockSeedInfos = from Item in objStockSeedInfo
                        //                     orderby Item.StockDate descending
                        //                     select Item;
                        XafTypesInfo.Instance.RegisterEntity(typeof(StockSeedInfo));
                        ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();

                        ObjStockSeedInfoInfo.StockDate = DateTime.Now;
                        ObjStockSeedInfoInfo.OrganizationOid = ObjMaster.SendOrgOid;
                        ObjStockSeedInfoInfo.FinanceYearOid = ObjMaster.FinanceYearOid;
                        ObjStockSeedInfoInfo.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                        ObjStockSeedInfoInfo.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                        ObjStockSeedInfoInfo.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                        ObjStockSeedInfoInfo.StockDetail = "ส่งเมล็ดพันธุ์ Lot Number: " + objSupplierProduct.LotNumber;
                        ObjStockSeedInfoInfo.TotalForward = objSupplierProduct.Weight;
                        ObjStockSeedInfoInfo.TotalChange = 0 - Convert.ToDouble(objsend_Detail.Weight);
                        ObjStockSeedInfoInfo.StockType = 0;
                        ObjStockSeedInfoInfo.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                        ObjStockSeedInfoInfo.ReferanceCode = objSupplierProduct.LotNumber;
                        ObjectSpace.CommitChanges();
                    }
                    ObjMaster.SendStatus = EnumSendOrderSeedStatus.SendApprove;
                    ObjectSpace.CommitChanges();
                    return Ok(true);

                }
                else
                {
                    return BadRequest();
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

        #endregion
    }
}



