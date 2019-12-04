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
    public class OrderSeedDetailController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// หน้ารายละเอียดการส่งแบบระบุเลขที่นำส่ง
        /// </summary>
        /// <param name="SendOrderSeed"></param>
        /// <returns></returns>
        #region เมล็ดพันธุ์
        [AllowAnonymous]
        [HttpGet]
        [Route("SendSeed/Order")]
        public IHttpActionResult OrderseedDetail(string SendOrderSeed)
        {
            OrderSeedDetail OrderSeedDetail = new OrderSeedDetail();
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeedDetail));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SendOrderSeedDetail OrderSeed;

                OrderSeed = ObjectSpace.FindObject<SendOrderSeedDetail>(new BinaryOperator("SendOrderSeed", SendOrderSeed));
                if (SendOrderSeed != null)
                {
                    OrderSeedDetail.LotNumber = OrderSeed.LotNumber.LotNumber;
                    OrderSeedDetail.WeightUnit = OrderSeed.WeightUnitOid.UnitName;
                    OrderSeedDetail.AnimalSeedCode = OrderSeed.AnimalSeedCode;
                    OrderSeedDetail.AnimalSeeName = OrderSeed.AnimalSeeName;
                    OrderSeedDetail.AnimalSeedLevel = OrderSeed.AnimalSeedLevel;
                    OrderSeedDetail.BudgetSource = OrderSeed.BudgetSourceOid.BudgetName;
                    OrderSeedDetail.Weight = OrderSeed.Weight;
                    OrderSeedDetail.Used = OrderSeed.Used;
                    OrderSeedDetail.SendOrderSeed = OrderSeed.SendOrderSeed.SendNo;
                    return Ok(OrderSeedDetail);
                }
                {
                    return BadRequest("NoData");
                }

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// โหลดหน้าสถานะ  2 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("LoadSendSeed")]
        public HttpResponseMessage LoadSendSeed()
        {
            object ReceiveOrgOid;
            try
            {

                ReceiveOrgOid = HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                List<sendSeed_info> list = new List<sendSeed_info>();
                data_info Temp_data = new data_info();
                IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 2 and ReceiveOrgOid=?", ReceiveOrgOid));

                double Amount = 0;
                if (collection.Count > 0)
                {

                    foreach (SendOrderSeed row in collection)
                    {
                        sendSeed_info Approve = new sendSeed_info();
                        Approve.Send_No = row.SendNo;
                        Approve.SendDate = row.SendDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); /* convet เวลา*/
                        Approve.FinanceYear = row.FinanceYearOid.YearName;
                        Approve.SendOrgOid = row.SendOrgOid.Oid;
                        Approve.SendOrgName = row.SendOrgOid.SubOrganizeName;
                        Approve.ReceiveOrgoid = row.ReceiveOrgOid.Oid;
                        Approve.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;

                        foreach (SendOrderSeedDetail row2 in row.SendOrderSeedDetails)
                        {
                            Amount = Amount + row2.Weight;
                        }
                        Approve.Weights = Amount.ToString()+" "+"กิโลกรัม";


                        list.Add(Approve);
                    }
                    Temp_data.sendSS = list;
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }

                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    ////    Return resual
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                ////  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// แสดงรายละเอียดข้อมูลการรับเมล็ด ใช็ศูนย์ส่ง where หา
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ReceiveOrderSeed")]
        public IHttpActionResult ReceiveOrderSeed()
        {
            object SendOrgOid;
                try
               {

                SendOrgOid = HttpContext.Current.Request.Form["SendOrgOid"].ToString();
            XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                List<ReceiveOrderSeed_Model> list = new List<ReceiveOrderSeed_Model>();
                IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and SendOrgOid=? ", SendOrgOid));
                double sum = 0;
                string WeightUnit;
                if (collection.Count > 0)
                {
                    foreach (SendOrderSeed row in collection)
                    {
                        ReceiveOrderSeed_Model Model = new ReceiveOrderSeed_Model();
                        
                        Model.ReceiveNo = row.SendNo;
                        Model.ReceiveDate = row.SendDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); ;
                        //    FinanceYear = ObjectSpace.GetObject<nutrition.Module.FinanceYear>(CriteriaOperator.Parse(nameof"Oid = @FinanceYearOid ", null));
                        Model.FinanceYear = row.FinanceYearOid.YearName;
                        Model.ReceiveOrgOid = row.ReceiveOrgOid.Oid;
                        Model.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                        Model.SendOrgOid = row.SendOrgOid.Oid;
                        Model.SendOrgName = row.SendOrgOid.SubOrganizeName;
                        foreach (SendOrderSeedDetail row2 in row.SendOrderSeedDetails)
                        {
                            sum = sum + row2.Weight;
                            WeightUnit = row2.WeightUnitOid.ToString();
                        }
                        Model.Weight_All = sum.ToString() + " " + "กิโลกรัม";
                        list.Add(Model);
                        
                    }
                    return Ok(list);
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
      
        

        //    catch (Exception ex)
        //    { //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        err.message = ex.Message;
        //        //  Return resual
        //        return BadRequest(ex.Message);
        //    }

        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("LoadSendSeed/accept")]
        //public IHttpActionResult LoadSendSeed_accept()
        //{
        //    object ReceiveOrgOid;
        //    try
        //    {

        //        ReceiveOrgOid = HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString();


        //        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //        XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
        //        XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
        //        List<Approve_Model> list = new List<Approve_Model>();
        //        SendOrderSeed sendOrderSeed;
        //        List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
        //        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //        IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 2 and ReceiveOrgOid=?", ReceiveOrgOid));
        //        if (collection.Count > 0)
        //        {
        //            foreach (SendOrderSeed row in collection)
        //            {
        //                Approve_Model Approve = new Approve_Model();
        //                Approve.Send_No = row.SendNo;
        //                Approve.SendDate = row.SendDate.ToString();
        //                Approve.FinanceYearOid = row.FinanceYearOid.YearName;
        //                Approve.SendOrgOid = row.SendOrgOid.OrganizeNameTH;
        //                Approve.ReceiveOrgOid = row.ReceiveOrgOid.OrganizeNameTH;
        //                Approve.Remark = row.Remark;
        //                Approve.CancelMsg = row.CancelMsg;
        //                Approve.SendStatus = row.SendStatus.ToString();
        //                //foreach (SendOrderSeedDetail row in sendOrderSeed.SendOrderSeedDetails)
        //                //{
        //                //    SendOrderSeed_Model send_Detail = new SendOrderSeed_Model();
        //                //    send_Detail.LotNumber = row2.LotNumber.LotNumber;
        //                //    send_Detail.WeightUnitOid = row2.WeightUnitOid.UnitName;
        //                //    send_Detail.AnimalSeedCode = row2.AnimalSeedCode;
        //                //    send_Detail.AnimalSeedLevel = row2.AnimalSeedLevel;
        //                //    send_Detail.AnimalSeeName = row2.AnimalSeeName;
        //                //    send_Detail.BudgetSourceOid = row2.BudgetSourceOid.BudgetName;
        //                //    send_Detail.Weight = row2.Weight;
        //                //    send_Detail.Used = row2.Used.ToString();
        //                //    send_Detail.SendOrderSeed = row2.SendOrderSeed.SendNo;
        //                //    send_Detail.AnimalSeedOid = row2.AnimalSeedOid.SeedName;
        //                //    send_Detail.AnimalSeedLevelOid = row2.AnimalSeedLevelOid.SeedLevelName;
        //                //    send_Detail.SeedTypeOid = row2.SeedTypeOid.SeedTypeName;
        //                //    send_Detail.Amount = row2.Amount;
        //                //    list_detail.Add(send_Detail);
        //                //}
        //                Approve.objSeed = list_detail;
        //                list.Add(Approve);
        //            }

        //        }

        //        else
        //        {
        //            UserError err = new UserError();
        //            err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //            err.message = "No data";
        //            //  Return resual
        //            return BadRequest();
        //        }
        //        return Ok(list);
        //    }
        //    catch (Exception ex)
        //    { //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        err.message = ex.Message;
        //        //  Return resual
        //        return BadRequest();
        //    }
        //}
        #endregion
        #region เสบียงสัตว์
        /// <summary>
        /// เรียกหน้าใช้เสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("LoadSupplierUseProduct")]
        public IHttpActionResult LoadSupplierUse_accept()
        {
            object OrganizationOid;
            try
            {

                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();


                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));               
                SupplierUseProduct supplier_UseProduct;
                List<SupplierProductUser> list_detail = new List<SupplierProductUser>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SupplierUseProduct> collection = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Stauts = 1 and OrganizationOid=?", OrganizationOid));
                double Weight = 0;
                if (OrganizationOid != null)
                {
                    foreach (SupplierUseProduct row in collection)
                    {
                        
                        SupplierProductUser Supplier = new SupplierProductUser();
                        Supplier.OrgeService = row.OrgeServiceOid.ServicesNumber.ToString();
                        Supplier.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                        Supplier.ReceiptNo = row.ReceiptNo;
                        Supplier.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); ;
                        Supplier.UseNo = row.UseNo;
                        Supplier.FinanceYear = row.FinanceYearOid.YearName;
                        Supplier.OrganizationName = row.OrganizationOid.SubOrganizeName;
                        Supplier.EmployeeName= row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                        Supplier.Remark = row.Remark;
                        //Supplier.Stauts = row.Stauts;
                        Supplier.ApproveDate = row.ApproveDate.ToString();
                        Supplier.ActivityName = row.ActivityOid.ActivityName;

                        if (row.SubActivityOid.ActivityName == null)
                        {
                            Supplier.SubActivityName = "ไม่มีข้อมูล";
                        }
                        else
                        {
                            Supplier.SubActivityName = row.SubActivityOid.ActivityName;
                        }
                        foreach (SupplierUseProductDetail row2 in row.SupplierUseProductDetails)
                        {
                            Weight = row2.Weight;
                        }
                        Supplier.Weight = Weight + " " + "กิโลกรัม";

                        //if (row.RegisCusServiceOid.DisPlayName == "")
                        //{
                        //    Supplier.RegisCusServiceOid = "ไม่มีข้อมูล";
                        //}
                        //else
                        //{
                        //    Supplier.RegisCusServiceOid = row.RegisCusServiceOid.DisPlayName;
                        //}
                        //if (row.OrgeServiceOid.OrgeServiceName == "")
                        //{
                        //    Supplier.OrgeServiceOid = "ไม่มีข้อมูล";
                        //}
                        //else
                        //{
                        //    Supplier.OrgeServiceOid = row.OrgeServiceOid.OrgeServiceName;
                        //}
                        list_detail.Add(Supplier);
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
     

        #endregion
    }
}




