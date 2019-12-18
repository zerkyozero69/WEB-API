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
using DevExpress.Xpo;
using System.Globalization;
using static WebApi.Jwt.Models.Supplier;
using DevExpress.ExpressApp.Security.ClientServer;

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
        [HttpPost]
        [Route("SendSeed/Order")]
        public IHttpActionResult OrderseedDetail(string SendOrderSeed)
        {
            OrderSeedDetail OrderSeedDetail = new OrderSeedDetail();
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeedDetail));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
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
        //[JwtAuthentication]
        [HttpPost]
        [Route("SendOrderSeed/accept")] /*ส่งเมล็ดให้หน่วยงานอื่น*/
        public HttpResponseMessage LoadSendSeed()
        {
            object SendOrgOid;

            try
            {

                SendOrgOid = HttpContext.Current.Request.Form["SendOrgOid"].ToString();


                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));

                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();

                List<sendSeed_info> list = new List<sendSeed_info>();
                data_info Temp_data = new data_info();
                IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 5 and SendOrgOid=? ", SendOrgOid));


                if (collection.Count > 0)
                {

                    foreach (SendOrderSeed row in collection)
                    {
                        double sum = 0;
                        string WeightUnit;
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
                            sum = sum + row2.Weight;
                            WeightUnit = row2.WeightUnitOid.ToString();
                        }
                        Approve.Weight_All = sum.ToString() + " " + "กิโลกรัม";


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
        /// แสดงรายละเอียดข้อมูลการรับเมล็ด ใช้ศูนย์ส่ง where หา
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ReceiveOrderSeed/accept")]
        public IHttpActionResult ReceiveOrderSeed()
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
                List<ReceiveOrderSeed_Model> list = new List<ReceiveOrderSeed_Model>();
                IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", ReceiveOrgOid));
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



        #endregion
        #region ใช้เมล็ดพันธุ์
        /// <summary>
        /// เรียกหน้าใช้เมล็ดพันธุ์  ช่วยภัยพิบัติ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("LoadSupplierUseProduct_calamity")]  ///LoadSupplierUseProduct_calamity
        public IHttpActionResult LoadSupplierUse_accept()
        {
            object OrganizationOid;
           
            try
            {

                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
               
               // string ActivityOid = "B100C7C1 - 4755 - 4AF0 - 812E-3DD6BA372D45";
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));
                
                List<SupplierProductUser> list_detail = new List<SupplierProductUser>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                var ActivityOid = "B100C7C1-4755-4AF0-812E-3DD6BA372D45";
                IList<SupplierUseProduct> collection = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Stauts = 1 and OrganizationOid=? and ActivityOid = ?", OrganizationOid, ActivityOid)); 
                double Weight = 0;
                if (collection.Count > 0)
                {
                    foreach (SupplierUseProduct row in collection)
                    {
                        
                        SupplierProductUser Supplier_ = new SupplierProductUser();
                        Supplier_.Oid = row.Oid.ToString();
                        Supplier_.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        Supplier_.UseNo = row.UseNo;
                        if (row.RegisCusServiceOid == null)
                        {
                            Supplier_.RegisCusService = "ไม่ใช่รายเดี่ยว";
                            Supplier_.RegisCusServiceName = "ไม่ใช่รายเดี่ยว";
                        }
                        else
                        {
                            Supplier_.RegisCusService = row.RegisCusServiceOid.Oid.ToString();
                            Supplier_.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + row.RegisCusServiceOid.LastNameTH;
                        }
                        if (row.OrgeServiceOid == null)
                        {
                            Supplier_.OrgeServiceName = "ไม่ใช่รายหน่วยงาน";
                        }
                        else
                        {
                            Supplier_.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                        }
                       
                        Supplier_.ActivityName = row.ActivityOid.ActivityName;
                        Supplier_.FinanceYear = row.FinanceYearOid.YearName;
                        Supplier_.OrganizationName = row.OrganizationOid.SubOrganizeName;
                        if (row.EmployeeOid == null)
                        {
                            Supplier_.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                        }
                        else
                        {
                            Supplier_.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                        }

                        Supplier_.Remark = row.Remark;
                        //Supplier.Stauts = row.Stauts;
                        Supplier_.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        Supplier_.ActivityNameOid = row.ActivityOid.Oid.ToString();
                        Supplier_.ActivityName = row.ActivityOid.ActivityName;

                        if (row.SubActivityOid == null)
                        {
                            Supplier_.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                        }
                        else
                        {
                            Supplier_.SubActivityName = row.SubActivityOid.ActivityName;
                        }
                        foreach (SupplierUseProductDetail row2 in row.SupplierUseProductDetails)
                        {
                            Weight = row2.Weight;
                        }
                        Supplier_.Weight = Weight + " " + "กิโลกรัม";
                        Supplier_.ServiceCount = row.ServiceCount;

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
                        list_detail.Add(Supplier_);
                    }

                    return Ok(list_detail);
                }

                else if (list_detail.Count == 0)
                {
                    UserError err = new UserError();
                    err.code = "3"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest(err.message);
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
        /// เรียกหน้าใช้เมล็ดพันธุ์   การจำหน่าย
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("LoadSupplierUseProduct_Sale")]
        public IHttpActionResult LoadSupplierUse_accept_Sale()
        {
            object OrganizationOid;

            try
            {

                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();

                // string ActivityOid = "B100C7C1 - 4755 - 4AF0 - 812E-3DD6BA372D45";
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));
  
                List<SupplierProductUser> list_detail = new List<SupplierProductUser>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                var ActivityOid = "1B648296-1105-4216-B4C2-ECEEF6859E96";
                IList<SupplierUseProduct> collection = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Stauts = 1 and OrganizationOid=? and ActivityOid = ?", OrganizationOid, ActivityOid));
                double Weight = 0;
                if (collection.Count >0)
                {
                    foreach (SupplierUseProduct row in collection)
                    {

                        SupplierProductUser Supplier_ = new SupplierProductUser();
                        Supplier_.Oid = row.Oid.ToString();
                        Supplier_.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        Supplier_.UseNo = row.UseNo;
                        if (row.RegisCusServiceOid == null)
                        {
                            Supplier_.RegisCusService = "ไม่พบข้อมูล";
                            Supplier_.RegisCusServiceName = "ไม่พบข้อมูล";
                        }
                        else
                        {
                            Supplier_.RegisCusService = row.RegisCusServiceOid.Oid.ToString();
                            Supplier_.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + row.RegisCusServiceOid.LastNameTH;
                        }

                        if (row.OrgeServiceOid == null)
                        {
                            Supplier_.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
                        }
                        else
                        {
                            Supplier_.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                        }
                        Supplier_.ActivityName = row.ActivityOid.ActivityName;
                        Supplier_.FinanceYear = row.FinanceYearOid.YearName;
                        Supplier_.OrganizationName = row.OrganizationOid.SubOrganizeName;
                        if (row.EmployeeOid == null)
                        {
                            Supplier_.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                        }
                        else
                        {
                            Supplier_.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                        }

                        Supplier_.Remark = row.Remark;
                        //Supplier.Stauts = row.Stauts;
                        Supplier_.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        Supplier_.ActivityNameOid = row.ActivityOid.Oid.ToString();
                        Supplier_.ActivityName = row.ActivityOid.ActivityName;

                        if (row.SubActivityOid == null)
                        {
                            Supplier_.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                        }
                        else
                        {
                            Supplier_.SubActivityName = row.SubActivityOid.ActivityName;
                        }
                        foreach (SupplierUseProductDetail row2 in row.SupplierUseProductDetails)
                        {
                            Weight = row2.Weight;
                        }
                        Supplier_.Weight = Weight + " " + "กิโลกรัม";
                        Supplier_.ServiceCount = row.ServiceCount;

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
                        list_detail.Add(Supplier_);
                    }

                    return Ok(list_detail);
                }

                else if (list_detail.Count == 0)
                {
                    UserError err = new UserError();
                    err.code = "3"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest(err.message);
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
        /// เรียกหน้าใช้เมล็ดพันธุ์   การแจก
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("LoadSupplierUseProduct_Serve")]
        public IHttpActionResult LoadSupplierUse_accept_serve()
        {
            string OrganizationOid;

            try
            {

                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
              string  YearName = HttpContext.Current.Request.Form["YearName"].ToString();
                // string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString();
                if ( OrganizationOid != "")
                {
                    //string[] arr = RefNo.Split('|');
                    //string _refno = arr[0]; //เลขที่อ้างอิง
                    //string _org_oid = arr[1]; //oid หน่วยงาน
                    //string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    // string ActivityOid = "B100C7C1 - 4755 - 4AF0 - 812E-3DD6BA372D45";
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));

                    List<SupplierProductUser> list_detail = new List<SupplierProductUser>();
                 
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    var ActivityOid = HttpContext.Current.Request.Form["ActivityOid"].ToString(); //รหัสกิจกรรม
                    IList< SupplierUseAnimalProduct> collection = ObjectSpace.GetObjects< SupplierUseAnimalProduct> (CriteriaOperator.Parse(" GCRecord is null and Stauts = 1 and OrganizationOid=? and ActivityOid = ? and FinanceYearOid.YearName = ?", OrganizationOid, ActivityOid,YearName));
                    double Weight = 0;
                    if (collection.Count > 0)
                    {
                        foreach (SupplierUseAnimalProduct row in collection)
                        {

                            SupplierProductUser Supplier_ = new SupplierProductUser();
                            Supplier_.Oid = row.Oid.ToString();
                            Supplier_.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                            Supplier_.UseNo = row.UseNo.ToString();
                            if (row.RegisCusServiceOid == null)
                            {
                                Supplier_.RegisCusService = "ไม่พบข้อมูล";
                                Supplier_.RegisCusServiceName = "ไม่พบข้อมูล";
                            }
                            else
                            {
                                Supplier_.RegisCusService = row.RegisCusServiceOid.Oid.ToString();
                                Supplier_.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + row.RegisCusServiceOid.LastNameTH;
                            }

                            if (row.OrgeServiceOid == null)
                            {
                                Supplier_.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
                            }
                            else
                            {
                                Supplier_.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                            }
                            Supplier_.ActivityName = row.ActivityOid.ActivityName.ToString();
                            if (row.SubActivityOid != null)
                            {
                                Supplier_.SubActivityName = row.SubActivityOid.ActivityName;
                            }
                            if (row.SubActivityLevelOid != null)
                            {
                                Supplier_.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                            }
                           
                            Supplier_.FinanceYear = row.FinanceYearOid.YearName.ToString();
                            Supplier_.OrganizationName = row.OrganizationOid.SubOrganizeName.ToString();
                            if (row.EmployeeOid == null)
                            {
                                Supplier_.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                            }
                            else
                            {
                                Supplier_.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                            }

                            Supplier_.Remark = row.Remark;
                            Supplier_.Stauts = row.Stauts.ToString();
                            Supplier_.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                            Supplier_.ActivityNameOid = row.ActivityOid.Oid.ToString();
                            Supplier_.ActivityName = row.ActivityOid.ActivityName.ToString();

                            if (row.SubActivityOid == null)
                            {
                                Supplier_.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                            }
                            else
                            {
                                Supplier_.SubActivityName = row.SubActivityOid.ActivityName.ToString();
                            }
                            List<SupplierUseProductDetail_Model> listD = new List<SupplierUseProductDetail_Model>();
                            foreach (SupplierUseAnimalProductDetail row2 in row.SupplierUseAnimalProductDetails)
                            {
                                SupplierUseProductDetail_Model item2 = new SupplierUseProductDetail_Model();
                                item2.SupplierUseAnimalProductOid = row2.SupplierUseAnimalProductOid.Oid.ToString();
                                item2.Weight = row2.Weight;
                                if (row2.Weight == 0)
                                {
                                    Weight = 0;
                                }
                                else
                                {
                                    Weight =  Weight+row2.Weight;
                                }
                                if (row2.AnimalSupplieTypeOid != null)
                                {
                                    item2.AnimalSupplieTypeName = row2.AnimalSupplieTypeOid.SupplietypeName;
                                }
                                if (row2.AnimalSeedOid != null)
                                {
                                    item2.AnimalSeedName = row2.AnimalSeedOid.SeedName;
                                }

                                if (row2.BudgetSourceOid != null)
                                {
                                    item2.BudgetSourceName = row2.BudgetSourceOid.BudgetName;
                                }
                                item2.PerPrice = row2.PerPrice;
                                item2.Price = row2.Price;
                                if (row2.QuotaTypeOid != null)
                                {
                                    item2.QuotaTypeName = row2.QuotaTypeOid.QuotaName;
                                }
                                item2.QuotaQTY = row2.QuotaQTY;
                                item2.StockLimit = row2.StockLimit;
                                item2.StockUsed = row2.StockUsed;
                                item2.Amount = row2.Amount;
                                listD.Add(item2);


                            }
                            
                            Supplier_.Weight = Weight + " " + "กิโลกรัม";
                            Supplier_.ReceiptNo = row.ReceiptNo;
                            Supplier_.ServiceCount = row.ServiceCount;
                            Supplier_.objProduct = listD;
                            list_detail.Add(Supplier_);



                        }
                        
                        return Ok(list_detail);
                    }

                    else if (list_detail == null)
                    {
                        UserError err = new UserError();
                        err.code = "3"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                        err.message = "No data";
                        //  Return resual
                        return BadRequest(err.message);
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
                else
                {
                    UserError err = new UserError();
                    err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "กรุณากรอก RefNo และ OrganizationOid ";
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

        #endregion ใช้เมล็ดพันธุ์


            #region SendOrderSeedApprove ยืนยันเมล็ดพันธุ์ ทดสอบ
        [AllowAnonymous]
        [HttpPost]
        [Route("ApprovalSend/")]

        public IHttpActionResult ApprovalSend_SupplierUseProduct() 
        {
            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                string Send_No = HttpContext.Current.Request.Form["Send_No"].ToString();

                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
              sendSeed_info sendDetail = new sendSeed_info(); 
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                SendOrderSeed ObjMaster ;
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
       var objSupplierProduct = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("Oid =?",objsend_Detail.LotNumber));

                    var objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? and StockType=0 ", ObjMaster.SendOrgOid.Oid, ObjMaster.FinanceYearOid ,objsend_Detail.BudgetSourceOid, objsend_Detail.AnimalSeedOid
                    , objsend_Detail.AnimalSeedLevelOid, objsend_Detail.LotNumber));
                    if (objStockSeedInfo.Count !=0)
                    {

                        //var stockSeedInfos = from Item in objStockSeedInfo
                        //                     orderby Item.StockDate descending
                        //                     select Item;
                        //XafTypesInfo.Instance.RegisterEntity(typeof(StockSeedInfo));
    //  string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["Scc"].ConnectionString;
                    //    string userName = "chai-nat";
                    //    string password = "123456";
                    //    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    //    XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                    //    XafTypesInfo.Instance.RegisterEntity(typeof(RoleInfo));

                    //    AuthenticationMixed authentication = new AuthenticationMixed();
                    //    authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
                    //    authentication.AddAuthenticationStandardProvider(typeof( UserInfo));
                    //    authentication.AddIdentityAuthenticationProvider(typeof(RoleInfo));
                    //    //CustomLogin authentication = new  CustomLogin();
                    //    //authentication.SetupAuthenticationProvider(authenticationName, parameter);
                    //    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(UserInfo), typeof(RoleInfo), authentication);
                    ////    security.RegisterXPOAdapterProviders();   
                    //    //SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(UserInfo), typeof(RoleInfo), authentication);
                    //    SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, ConnStr, null);

                    //    PasswordCryptographer.EnableRfc2898 = true;
                    //    PasswordCryptographer.SupportLegacySha512 = false;

                    //    authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
                    //    IObjectSpace loginObjectSpace = objectSpaceProvider.CreateObjectSpace();
                    //    security.Logon(loginObjectSpace);

                    //    IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
 

                        StockSeedInfo objSotockSeedInfoNew;
                        XafTypesInfo.Instance.RegisterEntity(typeof(StockSeedInfo));
                        objSotockSeedInfoNew = ObjectSpace.CreateObject<StockSeedInfo>();
                        objSotockSeedInfoNew.StockDate = DateTime.Now;
                        objSotockSeedInfoNew.OrganizationOid = ObjMaster.SendOrgOid;
                        objSotockSeedInfoNew.FinanceYearOid = ObjMaster.FinanceYearOid;
                        objSotockSeedInfoNew.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                        objSotockSeedInfoNew.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                        objSotockSeedInfoNew.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                        objSotockSeedInfoNew.StockDetail = "ส่งเมล็ดพันธุ์ Lot Number: " + objSupplierProduct.LotNumberFactory;
                        objSotockSeedInfoNew.TotalForward = objSupplierProduct.Weight;
                        objSotockSeedInfoNew.TotalChange = 0 - Convert.ToDouble(objsend_Detail.Weight);
                        objSotockSeedInfoNew.StockType = 0;
                        objSotockSeedInfoNew.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                        objSotockSeedInfoNew.ReferanceCode = objSupplierProduct.LotNumberFactory;
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




