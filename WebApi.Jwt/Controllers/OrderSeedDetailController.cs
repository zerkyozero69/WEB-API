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
                    OrderSeedDetail.WeightUnitOid = OrderSeed.WeightUnitOid.UnitName;
                    OrderSeedDetail.AnimalSeedCode = OrderSeed.AnimalSeedCode;
                    OrderSeedDetail.AnimalSeeName = OrderSeed.AnimalSeeName;
                    OrderSeedDetail.AnimalSeedLevel = OrderSeed.AnimalSeedLevel;
                    OrderSeedDetail.BudgetSourceOid = OrderSeed.BudgetSourceOid.BudgetName;
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
        /// หน้ารายละเอียดการส่งเมล็ด
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("SendSeed/Detail_List")]
        public IHttpActionResult OrderseedDetail_List()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeedDetail));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<OrderSeedDetail> list = new List<OrderSeedDetail>();
                IList<SendOrderSeedDetail> collection = ObjectSpace.GetObjects<SendOrderSeedDetail>(CriteriaOperator.Parse("GCRecord is null  "));
                if (collection != null)
                {

                    foreach (SendOrderSeedDetail row in collection)
                    {
                        OrderSeedDetail OrderSeedDetail = new OrderSeedDetail();
                        OrderSeedDetail.LotNumber = row.LotNumber.LotNumber;
                        OrderSeedDetail.WeightUnitOid = row.WeightUnitOid.UnitName;
                        OrderSeedDetail.AnimalSeedCode = row.AnimalSeedCode;
                        OrderSeedDetail.AnimalSeeName = row.AnimalSeeName;
                        OrderSeedDetail.AnimalSeedLevel = row.AnimalSeedLevel;
                        OrderSeedDetail.AnimalSeedOid = row.AnimalSeedOid.SeedName;
                        OrderSeedDetail.AnimalSeedLevelOid = row.AnimalSeedLevelOid.SeedLevelName;
                        OrderSeedDetail.SeedTypeOid = row.SeedTypeOid.SeedTypeName;
                        OrderSeedDetail.BudgetSourceOid = row.BudgetSourceOid.BudgetName;
                        OrderSeedDetail.Weight = row.Weight;
                        OrderSeedDetail.Amount = row.Amount;
                        OrderSeedDetail.Used = row.Used;
                        OrderSeedDetail.SendOrderSeed = row.SendOrderSeed.SendNo;
                        list.Add(OrderSeedDetail);
                    }
                }
                else
                {

                    return BadRequest("NoData");

                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region
        /// <summary>
        /// แสดงรายละเอียดข้อมูลการรับเมล็ด
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("ReceiveOrderSeed")]
        //public IHttpActionResult ReceiveOrderSeed()
        //{

        //    try
        //    {
        //        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //        XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveOrderSeed));
        //        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

        //        List<ReceiveOrderSeed_Model> list = new List<ReceiveOrderSeed_Model>();
        //        IList<ReceiveOrderSeed> collection = ObjectSpace.GetObjects<ReceiveOrderSeed>(CriteriaOperator.Parse("GCRecord is null  "));
        //        if (collection.Count > 0)
        //        {
        //            foreach (ReceiveOrderSeed row in collection)
        //            {
        //                ReceiveOrderSeed_Model Model = new ReceiveOrderSeed_Model();
        //                Model.ReceiveNo = row.ReceiveNo;
        //                Model.SendDate = row.SendDate;
        //                //    FinanceYear = ObjectSpace.GetObject<nutrition.Module.FinanceYear>(CriteriaOperator.Parse(nameof"Oid = @FinanceYearOid ", null));
        //                Model.FinanceYearOid = row.FinanceYearOid.YearName;
        //                Model.SendOrgOid = row.SendOrgOid.OrganizeNameTH;
        //                Model.ReceiveOrgOid = row.ReceiveOrgOid.OrganizeNameTH;
        //                Model.Remark = row.Remark;
        //                Model.SendStatus = (int)row.SendStatus;
        //                list.Add(Model);
        //            }
        //        }

        //        else
        //        {
        //            return BadRequest("NoData");
        //        }
        //        return Ok(list);

        //    }

        //    catch (Exception ex)
        //    { //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        err.message = ex.Message;
        //        //  Return resual
        //        return BadRequest(ex.Message);
        //    }
        //}
        /// <summary>
        /// หน้าส่งเมล็ดพันธุ์ ระหว่างรอดำเนินการ ***ใช้ตัวนี้ในหน้าอนุมัติ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("LoadSendSeed/accept")]
        public IHttpActionResult LoadSendSeed_accept()
        {
            object ReceiveOrgOid;
            try
            {

                ReceiveOrgOid = HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString();


                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
                List<Approve_Model> list = new List<Approve_Model>();
                SendOrderSeed sendOrderSeed;
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 2 and ReceiveOrgOid=?", ReceiveOrgOid));
                if (collection.Count > 0)
                {
                    foreach (SendOrderSeed row in collection)
                    {
                        Approve_Model Approve = new Approve_Model();
                        Approve.Send_No = row.SendNo;
                        Approve.SendDate = row.SendDate.ToString();
                        Approve.FinanceYearOid = row.FinanceYearOid.YearName;
                        Approve.SendOrgOid = row.SendOrgOid.OrganizeNameTH;
                        Approve.ReceiveOrgOid = row.ReceiveOrgOid.OrganizeNameTH;
                        Approve.Remark = row.Remark;
                        Approve.CancelMsg = row.CancelMsg;
                        Approve.SendStatus = row.SendStatus.ToString();
                        //foreach (SendOrderSeedDetail row in sendOrderSeed.SendOrderSeedDetails)
                        //{
                        //    SendOrderSeed_Model send_Detail = new SendOrderSeed_Model();
                        //    send_Detail.LotNumber = row2.LotNumber.LotNumber;
                        //    send_Detail.WeightUnitOid = row2.WeightUnitOid.UnitName;
                        //    send_Detail.AnimalSeedCode = row2.AnimalSeedCode;
                        //    send_Detail.AnimalSeedLevel = row2.AnimalSeedLevel;
                        //    send_Detail.AnimalSeeName = row2.AnimalSeeName;
                        //    send_Detail.BudgetSourceOid = row2.BudgetSourceOid.BudgetName;
                        //    send_Detail.Weight = row2.Weight;
                        //    send_Detail.Used = row2.Used.ToString();
                        //    send_Detail.SendOrderSeed = row2.SendOrderSeed.SendNo;
                        //    send_Detail.AnimalSeedOid = row2.AnimalSeedOid.SeedName;
                        //    send_Detail.AnimalSeedLevelOid = row2.AnimalSeedLevelOid.SeedLevelName;
                        //    send_Detail.SeedTypeOid = row2.SeedTypeOid.SeedTypeName;
                        //    send_Detail.Amount = row2.Amount;
                        //    list_detail.Add(send_Detail);
                        //}
                        Approve.objSeed = list_detail;
                        list.Add(Approve);
                    }

                }

                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest();
                }
                return Ok(list);
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest();
            }
        }

        /// <summary>
        /// หารายละเอียดการส่งด้วย sendOID
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("SendOrder/{SendNo}")] // ใส่ OIDSendOrderSeed ใบนำส่ง
        public IHttpActionResult SendOrderSeedDetail_ByOrderSeedID()
        {
            object SendNo = string.Empty;
            object ReceiveOrgOid = string.Empty;
            Approve_Model sendDetail = new Approve_Model();

            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                if (HttpContext.Current.Request.Form["SendNo"].ToString() != null)
                {
                    SendNo = HttpContext.Current.Request.Form["SendNo"].ToString();
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
                sendOrderSeed = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and SendNo=? and ReceiveOrgOid=? ", SendNo, ReceiveOrgOid));
               //sendOrderSeed = ObjectSpace.GetObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", null));
                if (SendNo != null)
                {   
                    sendDetail.Send_No = sendOrderSeed.SendNo;
                    sendDetail.SendDate = Convert.ToDateTime(sendOrderSeed.SendDate).ToString();
                    sendDetail.SendOrgOid = sendOrderSeed.SendOrgOid.OrganizeNameTH;
                    sendDetail.ReceiveOrgOid = sendOrderSeed.ReceiveOrgOid.OrganizeNameTH;
                    sendDetail.Remark = sendOrderSeed.Remark;
                    sendDetail.SendStatus = sendOrderSeed.SendStatus.ToString();
                    sendDetail.CancelMsg = sendOrderSeed.CancelMsg;
                    foreach (SendOrderSeedDetail row in sendOrderSeed.SendOrderSeedDetails)
                    {
                        SendOrderSeed_Model send_Detail = new SendOrderSeed_Model();
                        send_Detail.LotNumber = row.LotNumber.LotNumber;
                        send_Detail.WeightUnitOid = row.WeightUnitOid.UnitName;
                        send_Detail.AnimalSeedCode = row.AnimalSeedCode;
                        send_Detail.AnimalSeedLevel = row.AnimalSeedLevel;
                        send_Detail.AnimalSeeName = row.AnimalSeeName;
                        send_Detail.BudgetSourceOid = row.BudgetSourceOid.BudgetName;
                        send_Detail.Weight = row.Weight;
                        send_Detail.Used = row.Used.ToString();
                        send_Detail.SendOrderSeed = row.SendOrderSeed.SendNo;
                        send_Detail.AnimalSeedOid = row.AnimalSeedOid.SeedName;
                        send_Detail.AnimalSeedLevelOid = row.AnimalSeedLevelOid.SeedLevelName;
                        send_Detail.SeedTypeOid = row.SeedTypeOid.SeedTypeName;
                        send_Detail.Amount = row.Amount;
                        list_detail.Add(send_Detail);
                    }
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
        /// เรียกหน้าเมล็ด
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("LoadSupplierUseProduct/accept")]
        public IHttpActionResult LoadSupplierUse_accept()
        {
            object OrganizationOid;
            try
            {

                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();


                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
                SupplierUseProduct supplier_UseProduct;
                List<SupplierProductUser> list_detail = new List<SupplierProductUser>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SupplierUseProduct> collection = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Stauts = 2 and OrganizationOid=?", OrganizationOid));
                if (OrganizationOid != null)
                {
                    foreach (SupplierUseProduct row in collection)
                    {
                        SupplierProductUser Supplier = new SupplierProductUser();
                        Supplier.UseDate = row.UseDate;
                        Supplier.UseNo = row.UseNo;
                        Supplier.FinanceYearOid = row.FinanceYearOid.YearName;
                        Supplier.OrganizationOid = row.OrganizationOid.OrganizeNameTH;
                        Supplier.EmployeeOid = row.EmployeeOid.EmployeeFirstName+" "+row.EmployeeOid.EmployeeLastName;
                        Supplier.Remark = row.Remark;
                        Supplier.Stauts = row.Stauts.ToString();
                        Supplier.ApproveDate = row.ApproveDate.ToString();
                        Supplier.ActivityOid = row.ActivityOid.ActivityName;
                        if (row.SubActivityOid.ActivityName == "")
                        {
                            Supplier.SubActivityOid = "ไม่มีข้อมูล";
                        }
                        else
                        {
                            Supplier.SubActivityOid = row.SubActivityOid.ActivityName;
                        }


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


                }

                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest("NoData");
                }
                return Ok(list_detail);
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest("0");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("SupplierUseProduct/{UseNo}")] // ใส่ OIDSendOrderSeed ใบนำส่ง
        public IHttpActionResult SupplierUseProduct_ByOrderSeedID()
        {
           string UseNo = string.Empty;
            string OrganizationOid = string.Empty;

            SupplierProductUser supplierproduct = new SupplierProductUser();

            SupplierUseProductDetail_Model Model = new SupplierUseProductDetail_Model();
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

                
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);              
                List<SupplierUseProductDetail_Model> list_detail = new List<SupplierUseProductDetail_Model>();
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SupplierUseProduct supplierUseProduct;
                supplierUseProduct = ObjectSpace.FindObject<SupplierUseProduct>(CriteriaOperator.Parse("GCRecord is null and Stauts = 2 and UseNo=? and OrganizationOid=? ", UseNo, OrganizationOid));
                //sendOrderSeed = ObjectSpace.GetObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", null));
                if (UseNo != null)
                {
                    supplierproduct.UseDate = supplierUseProduct.UseDate;
                    supplierproduct.UseNo = supplierUseProduct.UseNo;
                    supplierproduct.FinanceYearOid = supplierUseProduct.FinanceYearOid.YearName;
                    supplierproduct.OrganizationOid = supplierUseProduct.OrganizationOid.OrganizeNameTH;
                    supplierproduct.EmployeeOid = supplierUseProduct.EmployeeOid.FullName;
                    supplierproduct.Remark = supplierUseProduct.Remark;
                    supplierproduct.Stauts = supplierUseProduct.Stauts.ToString();
                    supplierproduct.ApproveDate = supplierUseProduct.ApproveDate.ToShortDateString();
                    supplierproduct.ActivityOid = supplierUseProduct.ActivityOid.ActivityName;
                    supplierproduct.SubActivityOid = supplierUseProduct.SubActivityOid.ActivityName;
                   // supplierproduct.ReceiptNo = supplierUseProduct.ReceiptNo;        
                   
                //    supplierproduct.OrgeServiceOid = supplierUseProduct.OrgeServiceOid.OrgeServiceName;

                    foreach (SupplierUseProductDetail row in supplierUseProduct.SupplierUseProductDetails)
                    {
                        SupplierUseProductDetail_Model send_Detail = new SupplierUseProductDetail_Model();
                        send_Detail.AnimalSeedOid = row.AnimalSeedOid.SeedName;
                        send_Detail.AnimalSeedLevelOid = row.AnimalSeedLevelOid.SeedLevelName;
                        send_Detail.StockLimit = row.StockLimit;
                        send_Detail.Weight = row.Weight;
                        send_Detail.WeightUnitOid = row.WeightUnitOid.UnitName;
                        send_Detail.BudgetSourceOid = row.BudgetSourceOid.BudgetName;
                        send_Detail.SupplierUseProduct = row.SupplierUseProduct.Oid.ToString();
                        send_Detail.LotNumber = row.LotNumber.LotNumber;
                        send_Detail.SeedTypeOid = row.SeedTypeOid.SeedTypeName;
                        send_Detail.PerPrice = row.PerPrice;
                        send_Detail.Price = row.Price;              
                        list_detail.Add(send_Detail);
                    }
                    supplierproduct.objProduct = list_detail;
                    return Ok(supplierproduct);
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
        #endregion
    }
}




