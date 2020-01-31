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
using DevExpress.ExpressApp.Security.ClientServer;
using WebApi.Jwt.Models.Models_Masters;
using WebApi.Jwt.Models.ภัยภิบัติ;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
                if (OrderSeed != null)
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
                else
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
        /// เรียกหน้าใช้เสบียงสัตว์   การแจก
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimal/Detail")]  ///SupplierUseAnimal/Detail
        public IHttpActionResult GetSupplierSupplierUseAnimal()  ///ยังไม่มีตัวแทน ผู้รับบริการ***
        {
            string OrganizationOid;

            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString();
                //  string  YearName = HttpContext.Current.Request.Form["YearName"].ToString();

                if (RefNo != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    // string ActivityOid = "B100C7C1 - 4755 - 4AF0 - 812E-3DD6BA372D45";
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));

                    List<SupplierAnimalUse_Model> list_detail = new List<SupplierAnimalUse_Model>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    IList<SupplierUseAnimalProduct> collection = ObjectSpace.GetObjects<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null and Status = 1 and UseNo = '" + _refno + "'", null));
                    if (collection.Count > 0)
                    {
                        foreach (SupplierUseAnimalProduct row in collection)
                        {
                            double Weight = 0;

                            SupplierAnimalUse_Model Supplier_ = new SupplierAnimalUse_Model();
                            Supplier_.TypeMoblie = _type;
                            Supplier_.Oid = row.Oid.ToString();
                            Supplier_.UseNo = row.UseNo;
                            Supplier_.UseDate = row.UseDate.ToString("dd/MM/yyyy");
                            string TempSubDistrict, TempDistrict;
                            if (row.RegisCusServiceOid != null)
                            {
                               
                                if (row.RegisCusServiceOid.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                                {
                                    TempSubDistrict = "แขวง";
                                }
                                else
                                {
                                    TempSubDistrict = "ตำบล";
                                };

                                if (row.RegisCusServiceOid.DistrictOid.DistrictNameTH.Contains("กรุงเทพ"))
                                {
                                    TempDistrict = "เขต";
                                }
                                else
                                {
                                    TempDistrict = "อำเภอ";
                                };
                                Supplier_.TypeMoblie = _type;
                                Supplier_.Oid = row.Oid.ToString();
                                Supplier_.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_.UseNo = row.UseNo.ToString();
                            }
                            else
                            {
                          
                                if (row.OrgeServiceOid.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                                {
                                    TempSubDistrict = "แขวง";
                                }
                                else
                                {
                                    TempSubDistrict = "ตำบล";
                                };

                                if (row.OrgeServiceOid.DistrictOid.DistrictNameTH.Contains("กรุงเทพ"))
                                {
                                    TempDistrict = "เขต";
                                }
                                else { TempDistrict = "อำเภอ"; };
                            }
                            if (row.RegisCusServiceOid == null)
                            {

                                Supplier_.RegisCusServiceOid = "ไม่พบข้อมูล";
                                Supplier_.RegisCusServiceName = "ไม่พบข้อมูลรายชื่อบุคคลรายเดี่ยว";
                            }
                            else
                            {
                                //string TempSubDistrict, TempDistrict;
                                //if (row.RegisCusServiceOid.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                                //{
                                //    TempSubDistrict = "แขวง";
                                //}
                                //else
                                //{
                                //    TempSubDistrict = "ตำบล";
                                //};

                                //if (row.RegisCusServiceOid.DistrictOid.DistrictNameTH.Contains("กรุงเทพ"))
                                //{
                                //    TempDistrict = "เขต";
                                //}
                                //else { TempDistrict = "อำเภอ"; };
                                Supplier_.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                                Supplier_.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + " " + row.RegisCusServiceOid.LastNameTH;
                                //Supplier_.RegisCusServiceAddress = "เลขที่" + row.RegisCusServiceOid.Address + " หมู่ที่" + checknull(row.RegisCusServiceOid.Moo) + " ถนน" + checknull(row.RegisCusServiceOid.Road) +
                                //TempSubDistrict + " " + row.RegisCusServiceOid.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + row.RegisCusServiceOid.DistrictOid.DistrictNameTH + " " +
                                //"จังหวัด" + row.RegisCusServiceOid.ProvinceOid.ProvinceNameTH + " รหัสไปรษณีย์ " + row.RegisCusServiceOid.DistrictOid.PostCode;


                            }

                            if (row.OrgeServiceOid == null)
                            {
                                Supplier_.OrgeServiceOid = "ไม่พบข้อมูล";
                                Supplier_.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
               
                            }
                            else
                            {
                                //string TempSubDistrict = null, TempDistrict = null;
                                //if (row.OrgeServiceOid.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                                //{
                                //    TempSubDistrict = "แขวง";
                                //}
                                //else
                                //{
                                //    TempSubDistrict = "ตำบล";
                                //};

                                //if (row.OrgeServiceOid.DistrictOid.DistrictNameTH.Contains("กรุงเทพ"))
                                //{
                                //    TempDistrict = "เขต";
                                //}
                                //else { TempDistrict = "อำเภอ"; };
                                Supplier_.OrgeServiceOid = row.OrgeServiceOid.Oid.ToString();
                                Supplier_.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;

                                //Supplier_.OrgeServicAddress = "เลขที่" + row.OrgeServiceOid.Address + " หมู่ที่" + checknull(row.OrgeServiceOid.Moo) + " ถนน" + checknull(row.OrgeServiceOid.Road) +
                                //TempSubDistrict + " " + row.OrgeServiceOid.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + row.OrgeServiceOid.DistrictOid.DistrictNameTH + " " +
                                //"จังหวัด" + row.OrgeServiceOid.ProvinceOid.ProvinceNameTH + " รหัสไปรษณีย์ " + row.OrgeServiceOid.DistrictOid.PostCode;
                            }

                            if (row.OrgeServiceOid != null)
                            {

                                Supplier_.FullAddress = "เลขที่" + row.OrgeServiceOid.Address + " หมู่ที่" + checknull(row.OrgeServiceOid.Moo) + " ถนน" + checknull(row.OrgeServiceOid.Road) +
                                TempSubDistrict + " " + row.OrgeServiceOid.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + row.OrgeServiceOid.DistrictOid.DistrictNameTH + " " +
                                "จังหวัด" + row.OrgeServiceOid.ProvinceOid.ProvinceNameTH + " รหัสไปรษณีย์ " + row.OrgeServiceOid.DistrictOid.PostCode; ;
                            }
                            else
                            {
                                Supplier_.FullAddress = "เลขที่" + row.RegisCusServiceOid.Address + " หมู่ที่" + checknull(row.RegisCusServiceOid.Moo) + " ถนน" + checknull(row.RegisCusServiceOid.Road) +
                                TempSubDistrict + " " + row.RegisCusServiceOid.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + row.RegisCusServiceOid.DistrictOid.DistrictNameTH + " " +
                                "จังหวัด" + row.RegisCusServiceOid.ProvinceOid.ProvinceNameTH + " รหัสไปรษณีย์ " + row.RegisCusServiceOid.DistrictOid.PostCode;
                                
                            }
                            Supplier_.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                            Supplier_.OrganizationOid = row.OrganizationOid.Oid.ToString();
                            Supplier_.ActivityNameOid = row.ActivityOid.Oid.ToString();
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
                            Supplier_.Status = row.Status.ToString();
                            Supplier_.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                            Supplier_.ActivityNameOid = row.ActivityOid.Oid.ToString();
                            Supplier_.ActivityName = row.ActivityOid.ActivityName.ToString();
                            //Supplier_.ServiceCount = 1;


                            if (row.SubActivityOid == null)
                            {
                                Supplier_.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                            }
                            else
                            {
                                Supplier_.SubActivityName = row.SubActivityOid.ActivityName.ToString();
                            }
                            string BudgetSourceName = null;
                            double Amount = 0;
                            double Price = 0;
                            //ค้นหา Role Detail
                            List<SupplierUseAnimalDetail_Model> listD = new List<SupplierUseAnimalDetail_Model>();
                            foreach (SupplierUseAnimalProductDetail row2 in row.SupplierUseAnimalProductDetails)
                            {
                                SupplierUseAnimalDetail_Model item2 = new SupplierUseAnimalDetail_Model();
                                if (Supplier_.Oid != null)
                                {
                                    item2.SupplierUseAnimalProductOid = Supplier_.Oid.ToString();
                                }
                    
                                item2.Oid = row2.Oid.ToString();
                                item2.SupplierUseAnimalProductOid = row2.SupplierUseAnimalProductOid.Oid.ToString();

                                item2.Weight = row2.Weight;
                                if (row2.Weight == 0)
                                {
                                    Weight = 0;
                                }
                                else
                                {
                                    Weight = Weight + row2.Weight;
                                }
                                if (row2.AnimalSupplieTypeOid != null)
                                {
                                    item2.AnimalSupplieTypeName = row2.AnimalSupplieTypeOid.SupplietypeName;
                                }

                                item2.AnimalSeedName = "";

                                if (row2.BudgetSourceOid != null)
                                {
                                    item2.BudgetSourceName = row2.BudgetSourceOid.BudgetName;
                                }
                                if (row2.QuotaTypeOid != null)
                                {
                                    item2.QuotaTypeOid = row2.QuotaTypeOid.Oid.ToString();
                                    item2.QuotaTypeName = row2.QuotaTypeOid.QuotaName;
                                }
                                item2.Price = row2.Price;
                                item2.PerPrice = row2.PerPrice;
                                item2.StockUsed = row2.StockUsed;
                                item2.StockLimit = row2.StockLimit;
                                item2.AnimalSupplieName = row2.AnimalSupplieOid.AnimalSupplieName;
                                item2.QuotaQTY = row2.QuotaQTY;

                                item2.Amount = row2.Amount;
                                Price = Price + item2.Price;
                                Amount = Amount + item2.Amount;
                                BudgetSourceName = item2.BudgetSourceName;
                                listD.Add(item2);


                            }
                            Supplier_.BudgetSourceName = BudgetSourceName;
                            Supplier_.Weight = Weight + " " + "กิโลกรัม";
                            Supplier_.ReceiptNo = row.ReceiptNo;

                            Supplier_.ServiceCount = row.ServiceCount;
                            Supplier_.TotalPrice = Price.ToString() + " " + "บาท";
                            Supplier_.TotalAmout = Amount.ToString();

                            Supplier_.Refno = RefNo;
                            Supplier_.details = listD;
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
                    err.code = "-1"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
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

        /// <summary>
        /// ค้นหาเสบียงสัวต์ตามเลข Oid
        /// </summary> 
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimal/ByOid")]  ///SupplierUseAnimal/ByUseNo
        public IHttpActionResult GetSupplierSupplierUseAnimalDetail_ByOid()  ///ยังไม่มีตัวแทน ผู้รับบริการ***
        {
            string OrganizationOid;

            try
            {
                string Oid = HttpContext.Current.Request.Form["supplierUseAnimalProductOid"].ToString();
                //  string  YearName = HttpContext.Current.Request.Form["YearName"].ToString();

                if (Oid != "")
                {
                    // string ActivityOid = "B100C7C1 - 4755 - 4AF0 - 812E-3DD6BA372D45";
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));
                    List<SupplierAnimalUsecalarity_Model> list_detail = new List<SupplierAnimalUsecalarity_Model>();

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SupplierUseAnimalProduct collection = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null  and Oid = '" + Oid + "'", null));

                    if (collection != null)
                    {


                        double Weight = 0;

                        SupplierAnimalUsecalarity_Model Supplier_ = new SupplierAnimalUsecalarity_Model();

                        Supplier_.TypeMoblie = "3";
                        Supplier_.SupplierUseAnimalProductOid = collection.Oid.ToString();
                        Supplier_.UseDate = collection.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        Supplier_.UseNo = collection.UseNo.ToString();
                        if (collection.RegisCusServiceOid != null)
                        {
                            Supplier_.RegisCusServiceOid = collection.RegisCusServiceOid.Oid.ToString();
                            Supplier_.RegisCusServiceName = collection.RegisCusServiceOid.DisPlayName;
                        }

                        if (collection.OrgeServiceOid != null)
                        {
                            Supplier_.OrgeServiceOid = collection.OrgeServiceOid.Oid.ToString();
                            Supplier_.OrgeServiceName = collection.OrgeServiceOid.OrgeServiceName;

                        }


                        Supplier_.FinanceYearOid = collection.FinanceYearOid.Oid.ToString();
                        Supplier_.OrganizationOid = collection.OrganizationOid.Oid.ToString();
                        Supplier_.ActivityNameOid = collection.ActivityOid.Oid.ToString();
                        Supplier_.ActivityName = collection.ActivityOid.ActivityName.ToString();
                        if (collection.SubActivityOid != null)
                        {
                            Supplier_.SubActivityName = collection.SubActivityOid.ActivityName;
                        }
                        if (collection.SubActivityLevelOid != null)
                        {
                            Supplier_.SubActivityLevelName = collection.SubActivityLevelOid.ActivityName;
                        }

                        Supplier_.FinanceYear = collection.FinanceYearOid.YearName.ToString();
                        Supplier_.OrganizationName = collection.OrganizationOid.SubOrganizeName.ToString();
                        if (collection.EmployeeOid != null)
                        {
                            Supplier_.EmployeeName = collection.EmployeeOid.FullName;
                        }

                        Supplier_.Remark = collection.Remark;
                        Supplier_.Status = collection.Status.ToString();
                        Supplier_.ApproveDate = collection.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        Supplier_.ActivityNameOid = collection.ActivityOid.Oid.ToString();
                        Supplier_.ActivityName = collection.ActivityOid.ActivityName.ToString();

                        string TempSubDistrict, TempDistrict;
                        if (collection.RegisCusServiceOid != null)
                        {

                            if (collection.RegisCusServiceOid.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                            {
                                TempSubDistrict = "แขวง";
                            }
                            else
                            {
                                TempSubDistrict = "ตำบล";
                            };

                            if (collection.RegisCusServiceOid.DistrictOid.DistrictNameTH.Contains("กรุงเทพ"))
                            {
                                TempDistrict = "เขต";
                            }
                            else
                            {
                                TempDistrict = "อำเภอ";
                            };

                   

                            Supplier_.FullAddress = "เลขที่" + collection.RegisCusServiceOid.Address + " หมู่ที่" + checknull(collection.RegisCusServiceOid.Moo) + " ถนน" + checknull(collection.RegisCusServiceOid.Road) +
  TempSubDistrict + " " + collection.RegisCusServiceOid.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + collection.RegisCusServiceOid.DistrictOid.DistrictNameTH + " " +
     "จังหวัด" + collection.RegisCusServiceOid.ProvinceOid.ProvinceNameTH + " รหัสไปรษณีย์ " + collection.RegisCusServiceOid.DistrictOid.PostCode;
                        }
      
                        else
                        {
                          
                                if (collection.OrgeServiceOid.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                                {
                                    TempSubDistrict = "แขวง";
                                }
                                else
                                {
                                    TempSubDistrict = "ตำบล";
                                };

                                if (collection.OrgeServiceOid.DistrictOid.DistrictNameTH.Contains("กรุงเทพ"))
                                {
                                    TempDistrict = "เขต";
                                }
                                else
                                {
                                    TempDistrict = "อำเภอ";
                                };
                            if (collection.OrgeServiceOid != null)
                            {
                                Supplier_.FullAddress = "เลขที่" + collection.OrgeServiceOid.Address + " หมู่ที่" + checknull(collection.OrgeServiceOid.Moo) + " ถนน" + checknull(collection.OrgeServiceOid.Road) +
                    TempSubDistrict + " " + collection.OrgeServiceOid.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + collection.OrgeServiceOid.DistrictOid.DistrictNameTH + " " +
                    "จังหวัด" + collection.OrgeServiceOid.ProvinceOid.ProvinceNameTH + " รหัสไปรษณีย์ " + collection.OrgeServiceOid.DistrictOid.PostCode; ;
                            }
                        
                        }

                            Supplier_.ServiceCount = 0;
                        //}
                        if (collection.SubActivityLevelOid != null)
                        {
                            Supplier_.SubActivityLevelOid = collection.SubActivityLevelOid.Oid.ToString();
                            Supplier_.SubActivityLevelName = collection.SubActivityLevelOid.ActivityName;
                        }
                        if (collection.PickupType != EnumPickupType.NoAction || collection.PickupType != EnumPickupType.Myself)
                        {
                            Supplier_.PickupType = collection.PickupType.ToString();
                        }
                          
                            Supplier_.ReceiverCitizenID = collection.ReceiverCitizenID;
                            Supplier_.ReceiverName = collection.ReceiverName;
                            Supplier_.ReceiverAddress = collection.ReceiverAddress;
                            Supplier_.ReceiverRemark = collection.ReceiverRemark;

                            if (collection.SubActivityOid != null)
                            {
                                Supplier_.SubActivityOid = collection.SubActivityLevelOid.Oid.ToString();
                                Supplier_.SubActivityName = collection.SubActivityOid.ActivityName.ToString();
                            }

                            string BudgetSourceName = null;
                            string Amount = "0";
                            //ค้นหา Role Detail
                            List<SupplierUseAnimalDetail_Model> listD = new List<SupplierUseAnimalDetail_Model>();
                            foreach (SupplierUseAnimalProductDetail row2 in collection.SupplierUseAnimalProductDetails)
                            {
                                SupplierUseAnimalDetail_Model item2 = new SupplierUseAnimalDetail_Model();
                                item2.SupplierUseAnimalProductOid = Supplier_.SupplierUseAnimalProductOid.ToString();
                                item2.Oid = row2.Oid.ToString();
                                item2.SupplierUseAnimalProductOid = row2.SupplierUseAnimalProductOid.Oid.ToString();

                                item2.Weight = row2.Weight;

                                if (row2.AnimalSupplieTypeOid != null)
                                {
                                    item2.AnimalSupplieTypeName = row2.AnimalSupplieTypeOid.SupplietypeName;
                                }
                                item2.AnimalSeedName = "";

                                if (row2.BudgetSourceOid != null)
                                {
                                    item2.BudgetSourceName = row2.BudgetSourceOid.BudgetName;
                                }
                                if (row2.QuotaTypeOid != null)
                                {
                                    item2.QuotaTypeOid = row2.QuotaTypeOid.Oid.ToString();
                                    item2.QuotaTypeName = row2.QuotaTypeOid.QuotaName;
                                }
                                item2.Price = row2.Price;
                                item2.PerPrice = row2.PerPrice;
                                item2.StockUsed = row2.StockUsed;
                                item2.StockLimit = row2.StockLimit;
                                item2.AnimalSupplieName = row2.AnimalSupplieOid.AnimalSupplieName;
                                item2.QuotaQTY = row2.QuotaQTY;

                                item2.Amount = row2.Amount;
                                Amount = item2.Amount.ToString();
                                BudgetSourceName = item2.BudgetSourceName;
                                listD.Add(item2);


                            }
                            Supplier_.ReceiptNo = collection.ReceiptNo;

                            Supplier_.Refno = collection.UseNo + "|" + collection.OrganizationOid.OrganizationCode + "|3";
                            Supplier_.details = listD;
                            list_detail.Add(Supplier_);

                            return Ok(list_detail);
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
            finally
            {
               
            }
        }
        /// <summary>
        /// ดึง Detail อย่างเดียว ของการใช้เสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimalDetail/ByOid")]  ///SupplierUseAnimal/ByUseNo
        public IHttpActionResult GetSupplierSupplierUselDetail_ByOid()  ///ยังไม่มีตัวแทน ผู้รับบริการ***
        {
            string OrganizationOid;

            try
            {
                string Oid = HttpContext.Current.Request.Form["supplierUseAnimalProductOid"].ToString();
                //  string  YearName = HttpContext.Current.Request.Form["YearName"].ToString();

                if (Oid != "")
                {


                    // string ActivityOid = "B100C7C1 - 4755 - 4AF0 - 812E-3DD6BA372D45";
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProductDetail));
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SupplierUseAnimalProduct collection = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null  and Oid = '" + Oid + "'", null));

                    if (collection != null)
                    {

                        string BudgetSourceName = null;
                        string Amount = "0";
                        //ค้นหา Role Detail
                        List<SupplierUseAnimalDetail_Model> listD = new List<SupplierUseAnimalDetail_Model>();
                        foreach (SupplierUseAnimalProductDetail row2 in collection.SupplierUseAnimalProductDetails)
                        {
                            SupplierUseAnimalDetail_Model item2 = new SupplierUseAnimalDetail_Model();
                            item2.SupplierUseAnimalProductOid = Oid.ToString();
                            item2.Oid = row2.Oid.ToString();
                            item2.SupplierUseAnimalProductOid = row2.SupplierUseAnimalProductOid.Oid.ToString();

                            item2.Weight = row2.Weight;

                            if (row2.AnimalSupplieTypeOid != null)
                            {
                                item2.AnimalSupplieTypeName = row2.AnimalSupplieTypeOid.SupplietypeName;
                            }
                            item2.AnimalSeedName = "";

                            if (row2.BudgetSourceOid != null)
                            {
                                item2.BudgetSourceName = row2.BudgetSourceOid.BudgetName;
                            }
                            if (row2.QuotaTypeOid != null)
                            {
                                item2.QuotaTypeOid = row2.QuotaTypeOid.Oid.ToString();
                                item2.QuotaTypeName = row2.QuotaTypeOid.QuotaName;
                            }
                            item2.Price = row2.Price;
                            item2.PerPrice = row2.PerPrice;
                            item2.StockUsed = row2.StockUsed;
                            item2.StockLimit = row2.StockLimit;
                            item2.AnimalSupplieName = row2.AnimalSupplieOid.AnimalSupplieName;
                            item2.QuotaQTY = row2.QuotaQTY;
                            item2.PerUnit = row2.PerUnit;
                            if (row2.PackageOid !=null)
                            {
                                item2.PackageOid = row2.PackageOid.Oid.ToString();
                                item2.PackageName = row2.PackageOid.PackageName;
                            }

                            
                           

                            item2.Amount = row2.Amount;
                            Amount = item2.Amount.ToString();
                            BudgetSourceName = item2.BudgetSourceName;
                            listD.Add(item2);

                        }
        

                        return Ok(listD);
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
                    err.message = "กรุณากรอก Oid ";
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
            finally
            {

            }
        }
        /// <summary>
        ///  ลบเสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimal/DeleteDetai")]
        public HttpResponseMessage SupplierUseAnimal_DeleteDetail()  ///SupplierUseAnimal/DeleteDetai
        {

            try
            {

                string Oid = HttpContext.Current.Request.Form["Oid"].ToString(); //เลข OID ของ ตาราง SupplierUseAnimalProductDetail

                if (Oid != "")
                {
                    DataSet ds;
                    ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "SP_MoblieDeleteSupplierUseAnimalProductDetail_ByID", new SqlParameter("@Oid", Oid));

                    UpdateResult ret = new UpdateResult();
                    ret.status = "true";
                    ret.message = "ลบข้อมูลเรียบร้อยแล้ว";
                    return Request.CreateResponse(HttpStatusCode.OK, ret);
                }
                else
                {
                    UpdateResult ret = new UpdateResult();
                    ret.status = "False";
                    ret.message = "ต้องระบุ OID";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ret);
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
        /// เพิ่มข้อมูลเสบียงสัตว์ detail
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimal/InsertDetail")]
        public HttpResponseMessage SupplierUseAnimal_InsertDetail()  ///SupplierUseAnimal_InsertDetail
        {
            inserts_suppile inserts = new inserts_suppile();
            try
            {
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                if (jObject.SelectToken("OrganizationOid") != null)
                {
                    inserts.orgOid = jObject.SelectToken("OrganizationOid").Value<string>();
                }
                if (jObject.SelectToken("packageoid") != null)
                {
                    inserts.PackageOid = jObject.SelectToken("packageoid").Value<string>();
                }
                if (jObject.SelectToken("perunit") != null)
                {
                    inserts.PerUnit = jObject.SelectToken("perunit").Value<Double>();
                }

                if (jObject.SelectToken("quotaQTY") != null)
                {
                    inserts.QuotaQTY = jObject.SelectToken("quotaQTY").Value<Double>();
                }

                inserts.SupplierUseAnimalProductOid = jObject.SelectToken("supplierUseAnimalProductOid").Value<string>();
                inserts.Weight = jObject.SelectToken("weight").Value<string>();
                inserts.AnimalSupplieOid = jObject.SelectToken("animalsupplieoid").Value<string>();
                if (jObject.SelectToken("animalSupplieTypeOid") != null)
                {
                    inserts.AnimalSupplieTypeOid = jObject.SelectToken("animalSupplieTypeOid").Value<string>();
                }

                if (jObject.SelectToken("balancquotaQTY") != null)
                {
                    inserts.StockLimit = jObject.SelectToken("balancquotaQTY").Value<Double>();
                }

                inserts.SeedTypeOid = jObject.SelectToken("seedtypeoid").Value<string>();
                if (jObject.SelectToken("quotatypeoid") != null)
                {
                    inserts.QuotaTypeOid = jObject.SelectToken("quotatypeoid").Value<string>();
                }

                inserts.BudgetSourceOid = jObject.SelectToken("budgetsourceoid").Value<string>();
                inserts.Amount = jObject.SelectToken("balancQTY").Value<Double>();
                if (jObject.SelectToken("stockUsed") != null)
                {
                    inserts.StockUsed = jObject.SelectToken("stockUsed").Value<Double>();
                }
                if (jObject.SelectToken("weightunitoid") != null)
                {
                    inserts.WeightUnitOid = jObject.SelectToken("weightunitoid").Value<string>();
                }






                if (jObject.SelectToken("managesuboid") != null)
                {
                    inserts.managesuboid = jObject.SelectToken("managesuboid").Value<string>();
                }

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageSubAnimalSupplier));
                XafTypesInfo.Instance.RegisterEntity(typeof(QuotaType));
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageAnimalSupplier));
                XafTypesInfo.Instance.RegisterEntity(typeof(SeedType));
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSupplieType));
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSupplie));
                List<ManageAnimalSupplier_Model2> list = new List<ManageAnimalSupplier_Model2>();
                List<ManageQuantityProductDetail> listQuantity = new List<ManageQuantityProductDetail>();
                List<objManageAnimalSupplier> listdetail = new List<objManageAnimalSupplier>();
                ManageQuantityProductDetail listQuantity2 = new ManageQuantityProductDetail();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                double balanceQTY = 0.0;
                double amount = 0.0;
                double objstocklimit = 0.0;

                AnimalSupplie objAnimalSupplie = ObjectSpace.FindObject<AnimalSupplie>(CriteriaOperator.Parse("[oid] =? ", inserts.AnimalSupplieOid));
                if (objAnimalSupplie.AnimalSupplieName == "แห้ง")
                {

                    QuotaType objQuotaType = ObjectSpace.FindObject<QuotaType>(CriteriaOperator.Parse("[Oid]=?", inserts.QuotaTypeOid));
                    if (objQuotaType != null)
                    {
                        if (objQuotaType.QuotaName != "โควตาปศุสัตว์จังหวัด")
                        {
                            ManageAnimalSupplier objManageAnimalSupplier = ObjectSpace.FindObject<ManageAnimalSupplier>(CriteriaOperator.Parse("[OrganizationOid] =? and Status = 1", inserts.orgOid));
                            if (objManageAnimalSupplier != null)
                            {
                                if (objQuotaType.QuotaName == "โควตาปศุสัตว์เขต")
                                {
                                    listQuantity2.QuotaName = objQuotaType.QuotaName;
                                    listQuantity2.QuotaQTY = objManageAnimalSupplier.ZoneQTY;
                                }
                                else if (objQuotaType.QuotaName == "โควตาอื่นๆ")
                                {
                                    listQuantity2.QuotaName = objQuotaType.QuotaName;
                                    listQuantity2.QuotaQTY = objManageAnimalSupplier.OtherQTY;
                                }
                                else if (objQuotaType.QuotaName == "โควตาศูนย์ฯ")
                                {
                                    listQuantity2.QuotaName = objQuotaType.QuotaName;
                                    listQuantity2.QuotaQTY = objManageAnimalSupplier.CenterQTY;
                                }
                                else if (objQuotaType.QuotaName == "โควตาสำนัก")
                                {
                                    listQuantity2.QuotaName = objQuotaType.QuotaName;
                                    SeedType objSeedType = ObjectSpace.FindObject<SeedType>(CriteriaOperator.Parse("[Oid]=?", inserts.SeedTypeOid));
                                    AnimalSupplieType objAnimalSupplieType = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[Oid]=?", inserts.AnimalSupplieTypeOid));
                                    if (objSeedType != null && objAnimalSupplieType != null)
                                    {
                                        if (objSeedType.SeedTypeName == "GAP" && objAnimalSupplieType.SupplietypeName == "หญ้าแห้ง")
                                        {
                                            listQuantity2.QuotaQTY = objManageAnimalSupplier.OfficeGAPQTY;
                                        }
                                        else if (objSeedType.SeedTypeName.ToLower() == "normal" && objAnimalSupplieType.SupplietypeName == "หญ้าแห้ง")
                                        {
                                            listQuantity2.QuotaQTY = objManageAnimalSupplier.OfficeQTY;
                                        }
                                        else if (objSeedType.SeedTypeName.ToLower() == "normal" && objAnimalSupplieType.SupplietypeName == "ถั่วแห้ง")
                                        {
                                            listQuantity2.QuotaQTY = objManageAnimalSupplier.OfficeBeanQTY;
                                        }
                                        else if (objSeedType.SeedTypeName.ToLower() == "oganic")
                                        {
                                            listQuantity2.QuotaQTY = 0;
                                        }
                                    }
                                }
                            }
                        }
                        else //โควตาจังหวัด
                        {
                            listQuantity2.QuotaName = objQuotaType.QuotaName;
                            ManageSubAnimalSupplier objManageSubAnimalSupplier = ObjectSpace.FindObject<ManageSubAnimalSupplier>(CriteriaOperator.Parse("[ProvinceOid]=?", inserts.managesuboid));
                            if (objManageSubAnimalSupplier != null)
                            {
                                listQuantity2.ManageSubAnimalSupplierOid = objManageSubAnimalSupplier.Oid.ToString();
                                listQuantity2.QuotaQTY = objManageSubAnimalSupplier.ProvinceQTY;

                                listQuantity2.Provincename = objManageSubAnimalSupplier.ProvinceOid.ProvinceNameTH;

                            }
                        }

                        //Get StockUsed
                        DataSet Ds = null;
                        if (inserts.managesuboid != null && inserts.managesuboid != "")
                        {
                            Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "GetStockUsed_QuotaHay_Province"
                         , new SqlParameter("@OrganizationOid", inserts.orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", inserts.AnimalSupplieTypeOid)
                         , new SqlParameter("@QuotaTypeOid", inserts.QuotaTypeOid)
                         , new SqlParameter("@BudgetSourceOid", inserts.BudgetSourceOid)
                         , new SqlParameter("@AnimalSupplieOid", inserts.AnimalSupplieOid)
                         , new SqlParameter("@ManageSubAnimalSupplierOid", inserts.managesuboid.ToString())
                         , new SqlParameter("@SeedTypeOid", inserts.SeedTypeOid));
                        }
                        else
                        {
                            Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "GetStockUsed_QuotaHay"
                         , new SqlParameter("@OrganizationOid", inserts.orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", inserts.AnimalSupplieTypeOid)
                         , new SqlParameter("@QuotaTypeOid", inserts.QuotaTypeOid)
                         , new SqlParameter("@BudgetSourceOid", inserts.BudgetSourceOid)
                         , new SqlParameter("@AnimalSupplieOid", inserts.AnimalSupplieOid)
                         , new SqlParameter("@SeedTypeOid", inserts.SeedTypeOid));
                        }
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            //listQuantity2.ba = (double)Ds.Tables[0].Rows[0]["StockUsed"];
                            listQuantity2.balancQuotaQTY = listQuantity2.QuotaQTY - (double)Ds.Tables[0].Rows[0]["StockUsed"];
                            listQuantity2.stockuse = (double)Ds.Tables[0].Rows[0]["StockUsed"];

                        }
                        else
                        {
                            //listQuantity2.QuotaQTY = 0;
                            listQuantity2.balanceQTY = 0;
                            listQuantity2.balancQuotaQTY = 0;
                        }

                        Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo"
                        , new SqlParameter("@OrganizationOid", inserts.orgOid)
                        , new SqlParameter("@AnimalSupplieTypeOid", inserts.AnimalSupplieTypeOid)
                        , new SqlParameter("@BudgetSourceOid", inserts.BudgetSourceOid)
                        , new SqlParameter("@AnimalSupplieOid", inserts.AnimalSupplieOid)
                        , new SqlParameter("@SeedTypeOid", inserts.SeedTypeOid));
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            listQuantity2.balanceQTY = (double)Ds.Tables[0].Rows[0]["Total_StockAnimalInfo"];
                        }
                        else
                        {
                            listQuantity2.balanceQTY = 0;
                        }

                    }
                    else //QuotaType = null
                    {
                        listQuantity2.QuotaQTY = 0; //ปริมาณตามแผนจัดสรร
                        listQuantity2.balanceQTY = 0; //จำนวนคงเหลือตามจริง
                        listQuantity2.balancQuotaQTY = 0; //ปริมาณคงเหลือตามแผนจัดสรร
                    }

                    listQuantity.Add(listQuantity2);
                }
                else
                {
                    var stocklimit = 0.0;
                    if (jObject.SelectToken("seedtypeoid").Value<string>() != null && jObject.SelectToken("seedtypeoid").Value<string>() != "")
                    {

                        DataSet Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo"
                         , new SqlParameter("@OrganizationOid", inserts.orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", inserts.AnimalSupplieTypeOid)
                         , new SqlParameter("@BudgetSourceOid", inserts.BudgetSourceOid)
                         , new SqlParameter("@AnimalSupplieOid", inserts.AnimalSupplieOid)
                         , new SqlParameter("@SeedTypeOid", inserts.SeedTypeOid));
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            balanceQTY = (double)Ds.Tables[0].Rows[0]["Total_StockAnimalInfo"];
                        }
                        else
                        {
                            balanceQTY = 0;
                        }

                        Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo_TMR"
                         , new SqlParameter("@OrganizationOid", inserts.orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", inserts.AnimalSupplieTypeOid)
                         , new SqlParameter("@BudgetSourceOid", inserts.BudgetSourceOid)
                         , new SqlParameter("@AnimalSupplieOid", inserts.AnimalSupplieOid)
                         , new SqlParameter("@SeedTypeOid", inserts.SeedTypeOid));
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            stocklimit = (double)Ds.Tables[0].Rows[0]["Total_Current"];
                        }
                        else
                        {
                            stocklimit = 0;
                        }
                    }
                    else
                    {
                        DataSet Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo"
                         , new SqlParameter("@OrganizationOid", inserts.orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", inserts.AnimalSupplieTypeOid)
                         , new SqlParameter("@BudgetSourceOid", inserts.BudgetSourceOid)
                         , new SqlParameter("@AnimalSupplieOid", inserts.AnimalSupplieOid)
                         , new SqlParameter("@SeedTypeOid", null));
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            balanceQTY = (double)Ds.Tables[0].Rows[0]["Total_StockAnimalInfo"];
                        }
                        else
                        {
                            balanceQTY = 0;
                        }

                        Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo_TMR"
                         , new SqlParameter("@OrganizationOid", inserts.orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", inserts.AnimalSupplieTypeOid)
                         , new SqlParameter("@BudgetSourceOid", inserts.BudgetSourceOid)
                         , new SqlParameter("@AnimalSupplieOid", inserts.AnimalSupplieOid)
                         , new SqlParameter("@SeedTypeOid", null));
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            stocklimit = (double)Ds.Tables[0].Rows[0]["Total_Current"];
                        }
                        else
                        {
                            stocklimit = 0;
                        }
                    }


                    listQuantity2.QuotaQTY = 0;
                    listQuantity2.balanceQTY = balanceQTY;
                    listQuantity2.balancQuotaQTY = 0;
                    listQuantity2.stockuse = stocklimit;


                }
                if (inserts.AnimalSupplieOid != "")
                {
                    DataSet ds;


                    SqlParameter[] prm = new SqlParameter[15];

                    prm[0] = new SqlParameter("@AnimalSupplieOid", inserts.AnimalSupplieOid);
                    //     prm[1] = new SqlParameter("@AnimalSeedOid", null);
                    prm[1] = new SqlParameter("@StockLimit", listQuantity2.balanceQTY); // ใช้ weight อย่างเดียว
                    prm[2] = new SqlParameter("@Weight", inserts.Weight);
                    prm[3] = new SqlParameter("@WeightUnitOid", inserts.WeightUnitOid);
                    prm[4] = new SqlParameter("@SupplierUseAnimalProductOid", inserts.SupplierUseAnimalProductOid);
                    prm[5] = new SqlParameter("@AnimalSupplieTypeOid", inserts.AnimalSupplieTypeOid);
                    prm[6] = new SqlParameter("@ManageSubAnimalSupplierOid", listQuantity2.ManageSubAnimalSupplierOid);
                    prm[7] = new SqlParameter("@QuotaQTY", listQuantity2.QuotaQTY);
                    prm[8] = new SqlParameter("@StockUsed", listQuantity2.stockuse);
                    prm[9] = new SqlParameter("@Amount", listQuantity2.QuotaQTY);
                    prm[10] = new SqlParameter("@QuotaTypeOid", inserts.QuotaTypeOid);
                    prm[11] = new SqlParameter("@SeedTypeOid", inserts.SeedTypeOid);
                    prm[12] = new SqlParameter("@PackageOid", inserts.PackageOid);
                    prm[13] = new SqlParameter("@PerUnit", inserts.PerUnit);
                    prm[14] = new SqlParameter("@BudgetSourceOid", inserts.BudgetSourceOid);
                    ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "SP_ADDSupplierUseAnimalProductDetail", prm);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        UpdateResult ret = new UpdateResult();
                        ret.status = "true";
                        ret.message = "บันทึกข้อมูลเรียบร้อยแล้ว";
                        return Request.CreateResponse(HttpStatusCode.OK, ret);
                    }
                    else
                    {
                        UpdateResult ret = new UpdateResult();
                        ret.status = "0";
                        ret.message = "Nodata";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, ret);
                    }

                }
                else
                {
                    UpdateResult ret = new UpdateResult();
                    ret.status = "-1";
                    ret.message = "ต้องระบุข้อมูลให้ครบ";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ret);
                }







            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            finally
            {
  
            }
        }

        #region แก้ไข Oid กิจกรรม

        /// <summary>
        /// ข้อมูลการใช้เสบียงสัตว์
        /// เผื่อการแข้ไข
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimal/List")]
        public HttpResponseMessage GetSupplierUse()
        {
            try
            {
                string org_oid = HttpContext.Current.Request.Form["Org_Oid"].ToString();
                string type = HttpContext.Current.Request.Form["type"].ToString(); //การแจกจ่าย=1/การจำหน่าย=2/ภัยพิบัติ=3
                if (org_oid != "" && type != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));

                    List<SupplierProductUser> UseACT1 = new List<SupplierProductUser>();
                    List<SupplierProductUser> UseACT2 = new List<SupplierProductUser>();
                    List<SupplierProductUser> UseACT3 = new List<SupplierProductUser>();
                    List<SupplierProductUser> UseACT4 = new List<SupplierProductUser>();
                    SupplierUseAnimalProduct_Model lists = new SupplierUseAnimalProduct_Model();
                    lists.org_oid = org_oid;

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    // อาจต้องแก้ไข ในอนาคต
                    if (type == "4")
                    {  //เพื่อใช้ในกิจกรรมกรมปศุสัตว์ 
                        Activity ActivityOid = ObjectSpace.FindObject<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1  ", null));

                        ActivityOid.ActivityName = "เพื่อใช้ในกิจกรรมกรมปศุสัตว์";
                    //    string ActivityOid = "069CB598-B40E-472A-A386-4F8056FB78D2";
                        IList<SupplierUseAnimalProduct> collection = ObjectSpace.GetObjects<SupplierUseAnimalProduct>(CriteriaOperator.Parse("GCRecord is null and (Status in( 0,1)) and OrganizationOid='" + org_oid + "' and  [ActivityOid.ActivityName] = '" + ActivityOid.ActivityName + "' ", null));
                        if (collection.Count > 0)
                        {
                            foreach (SupplierUseAnimalProduct row in collection)
                            {

                                SupplierProductUser Supplier_ = new SupplierProductUser();
                                Supplier_.TypeMoblie = type;
                                Supplier_.Oid = row.Oid.ToString();
                                Supplier_.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_.UseNo = row.UseNo.ToString();
                                if (row.RegisCusServiceOid == null)
                                {
                                    Supplier_.RegisCusServiceOid = "ไม่พบข้อมูล";
                                    Supplier_.RegisCusServiceName = "ไม่พบข้อมูล";
                                }
                                else
                                {
                                    Supplier_.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                                    Supplier_.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + " " + row.RegisCusServiceOid.LastNameTH;
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
                                Supplier_.OrganizationOid = row.OrganizationOid.Oid.ToString();
                                Supplier_.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                Supplier_.Remark = row.Remark;
                                Supplier_.Status = row.Status.ToString();
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
                                Supplier_.Refno = row.UseNo + "|" + row.OrganizationOid.Oid.ToString() + "|1";
                                Supplier_.Weight = row.SupplierUseAnimalProductDetails.Sum((c => c.Weight)).ToString() + " " + "กิโลกรัม";
                                UseACT1.Add(Supplier_);
                            }
                        }
                        //lists.UseACT1 = null; //UseACT1;
                        return Request.CreateResponse(HttpStatusCode.OK, UseACT1);
                    }
                    else if (type == "2")
                    {  //การจำหน่าย
                        Activity ActivityOid = ObjectSpace.FindObject<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1  ", null));

                        ActivityOid.ActivityName = "เพื่อการจำหน่าย";
                        //string ActivityOid = "1B648296-1105-4216-B4C2-ECEEF6859E96";

                        IList<SupplierUseAnimalProduct> collection2 = ObjectSpace.GetObjects<SupplierUseAnimalProduct>(CriteriaOperator.Parse("GCRecord is null and Status = 1 and OrganizationOid='" + org_oid + "'and  [ActivityOid.ActivityName] = '" + ActivityOid.ActivityName + "'  ", null));
                        if (collection2.Count > 0)
                        {
                            foreach (SupplierUseAnimalProduct row in collection2)
                            {

                                SupplierProductUser Supplier_2 = new SupplierProductUser();
                                Supplier_2.TypeMoblie = type;
                                Supplier_2.Oid = row.Oid.ToString();
                                Supplier_2.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_2.UseNo = row.UseNo.ToString();
                                if (row.RegisCusServiceOid == null)
                                {
                                    Supplier_2.RegisCusServiceOid = "ไม่พบข้อมูล";
                                    Supplier_2.RegisCusServiceName = "ไม่พบข้อมูล";
                                }
                                else
                                {
                                    Supplier_2.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                                    Supplier_2.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + " " + row.RegisCusServiceOid.LastNameTH;
                                }

                                if (row.OrgeServiceOid == null)
                                {
                                    Supplier_2.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
                                }
                                else
                                {
                                    Supplier_2.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                                }
                                Supplier_2.ActivityName = row.ActivityOid.ActivityName.ToString();
                                if (row.SubActivityOid != null)
                                {
                                    Supplier_2.SubActivityName = row.SubActivityOid.ActivityName;
                                }
                                if (row.SubActivityLevelOid != null)
                                {
                                    Supplier_2.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                                }

                                Supplier_2.FinanceYear = row.FinanceYearOid.YearName.ToString();
                                Supplier_2.OrganizationName = row.OrganizationOid.SubOrganizeName.ToString();
                                if (row.EmployeeOid == null)
                                {
                                    Supplier_2.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                                }
                                else
                                {
                                    Supplier_2.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                                }
                                Supplier_2.OrganizationOid = row.OrganizationOid.Oid.ToString();
                                Supplier_2.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                Supplier_2.Remark = row.Remark;
                                Supplier_2.Status = row.Status.ToString();
                                Supplier_2.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_2.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                Supplier_2.ActivityName = row.ActivityOid.ActivityName.ToString();

                                if (row.SubActivityOid == null)
                                {
                                    Supplier_2.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                                }
                                else
                                {
                                    Supplier_2.SubActivityName = row.SubActivityOid.ActivityName.ToString();
                                }
                                Supplier_2.ReceiptNo = row.ReceiptNo;
                                Supplier_2.Refno = row.UseNo + "|" + row.OrganizationOid.Oid.ToString() + "|2";
                                Supplier_2.Weight = row.SupplierUseAnimalProductDetails.Sum((c => c.Weight)).ToString() + " " + "กิโลกรัม";
                                UseACT2.Add(Supplier_2);
                            }
                            //lists.UseACT2 = UseACT2;
                            return Request.CreateResponse(HttpStatusCode.OK, UseACT2);
                        }
                    }
                    else if (type == "3")
                    {
                        //การอนุมัติภัยพิบัติ
                        string ActivityOid = "b100c7c1-4755-4af0-812e-3dd6ba372d45";
                        string SubActivityOid = "";
                        if (SubActivityOid != "58d27cfd - 6f9c - 4e1f - adbc - 4bec48bce531")
                        {
                            SubActivityOid = "2d040ef1-23c3-4c1c-9e59-0d3e2e7ef712";
                        }
                        else
                        {
                            SubActivityOid = "86e8c106 - a176 - 441f - a7e0 - b911e487641f";
                        }

                        IList<SupplierUseAnimalProduct> collection3 = ObjectSpace.GetObjects<SupplierUseAnimalProduct>(CriteriaOperator.Parse("GCRecord is null and (Status in (1 ,0) ) and OrganizationOid='" + org_oid + "'and ActivityOid = '" + ActivityOid + "' ", null));
                        if (collection3.Count > 0)
                        {
                            foreach (SupplierUseAnimalProduct row in collection3)
                            {

                                SupplierProductUser Supplier_3 = new SupplierProductUser();
                                Supplier_3.TypeMoblie = type;
                                Supplier_3.Oid = row.Oid.ToString();
                                Supplier_3.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_3.UseNo = row.UseNo.ToString();
                                if (row.RegisCusServiceOid == null)
                                {
                                    Supplier_3.RegisCusServiceOid = "ไม่พบข้อมูล";
                                    Supplier_3.RegisCusServiceName = "ไม่พบข้อมูล";
                                }
                                else
                                {
                                    Supplier_3.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                                    Supplier_3.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + " " + row.RegisCusServiceOid.LastNameTH;
                                }

                                if (row.OrgeServiceOid == null)
                                {
                                    Supplier_3.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
                                }
                                else
                                {
                                    Supplier_3.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                                }
                                Supplier_3.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                Supplier_3.ActivityName = row.ActivityOid.ActivityName.ToString();
                                Supplier_3.OrganizationOid = row.OrganizationOid.Oid.ToString();
                                if (row.SubActivityOid != null)
                                {
                                    Supplier_3.SubActivityName = row.SubActivityOid.ActivityName;
                                }
                                if (row.SubActivityLevelOid != null)
                                {
                                    Supplier_3.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                                }

                                Supplier_3.FinanceYear = row.FinanceYearOid.YearName.ToString();
                                Supplier_3.OrganizationName = row.OrganizationOid.SubOrganizeName.ToString();
                                if (row.EmployeeOid == null)
                                {
                                    Supplier_3.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                                }
                                else
                                {
                                    Supplier_3.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                                }

                                Supplier_3.Remark = row.Remark;
                                Supplier_3.Status = row.Status.ToString();
                                Supplier_3.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_3.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                Supplier_3.ActivityName = row.ActivityOid.ActivityName.ToString();

                                if (row.SubActivityOid == null)
                                {
                                    Supplier_3.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                                }
                                else
                                {
                                    Supplier_3.SubActivityName = row.SubActivityOid.ActivityName.ToString();
                                }
                                Supplier_3.ReceiptNo = row.ReceiptNo;
                                Supplier_3.Refno = row.UseNo + "|" + row.OrganizationOid.Oid.ToString() + "|3";
                                Supplier_3.Weight = row.SupplierUseAnimalProductDetails.Sum((c => c.Weight)).ToString() + " " + "กิโลกรัม";
                                UseACT3.Add(Supplier_3);
                            }
                            //lists.UseACT2 = UseACT2;
                            return Request.CreateResponse(HttpStatusCode.OK, UseACT3);
                        }
                    }
                    else if (type == "1")
                    {
                        //เพื่อการแจกจ่าย (สนับสนุนเกษตรกร)
                        string ActivityOid = "A29D77A9-4BCB-4774-9744-FF97A373353E";

                        IList<SupplierUseAnimalProduct> collection4 = ObjectSpace.GetObjects<SupplierUseAnimalProduct>(CriteriaOperator.Parse("GCRecord is null and Status = 1 and OrganizationOid='" + org_oid + "'and ActivityOid = '" + ActivityOid + "' ", null));
                        if (collection4.Count > 0)
                        {
                            foreach (SupplierUseAnimalProduct row in collection4)
                            {

                                SupplierProductUser Supplier_4 = new SupplierProductUser();
                                Supplier_4.TypeMoblie = type;
                                Supplier_4.Oid = row.Oid.ToString();
                                Supplier_4.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_4.UseNo = row.UseNo.ToString();
                                if (row.RegisCusServiceOid == null)
                                {
                                    Supplier_4.RegisCusServiceOid = "ไม่พบข้อมูล";
                                    Supplier_4.RegisCusServiceName = "ไม่พบข้อมูล";
                                }
                                else
                                {
                                    Supplier_4.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                                    Supplier_4.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + " " + row.RegisCusServiceOid.LastNameTH;
                                }

                                if (row.OrgeServiceOid == null)
                                {
                                    Supplier_4.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
                                }
                                else
                                {
                                    Supplier_4.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                                }
                                Supplier_4.ActivityName = row.ActivityOid.ActivityName.ToString();
                                Supplier_4.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                if (row.SubActivityOid != null)
                                {
                                    Supplier_4.SubActivityName = row.SubActivityOid.ActivityName;
                                }
                                if (row.SubActivityLevelOid != null)
                                {
                                    Supplier_4.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                                }
                                Supplier_4.OrganizationOid = row.OrganizationOid.Oid.ToString();
                                Supplier_4.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                Supplier_4.FinanceYear = row.FinanceYearOid.YearName.ToString();
                                Supplier_4.OrganizationName = row.OrganizationOid.SubOrganizeName.ToString();
                                if (row.EmployeeOid == null)
                                {
                                    Supplier_4.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                                }
                                else
                                {
                                    Supplier_4.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                                }

                                Supplier_4.Remark = row.Remark;
                                Supplier_4.Status = row.Status.ToString();
                                Supplier_4.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_4.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                Supplier_4.ActivityName = row.ActivityOid.ActivityName.ToString();

                                if (row.SubActivityOid == null)
                                {
                                    Supplier_4.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                                }
                                else
                                {
                                    Supplier_4.SubActivityName = row.SubActivityOid.ActivityName.ToString();
                                }
                                if (row.ReceiptNo != null)
                                {
                                    Supplier_4.ReceiptNo = row.ReceiptNo.ToString();
                                }

                                Supplier_4.Refno = row.UseNo + "|" + row.OrganizationOid.Oid.ToString() + "|1";
                                if (row.SupplierUseAnimalProductDetails != null)
                                {
                                    Supplier_4.Weight = row.SupplierUseAnimalProductDetails.Sum((c => c.Weight)).ToString() + " " + "กิโลกรัม";
                                }
                                UseACT4.Add(Supplier_4);
                            }
                            //lists.UseACT2 = UseACT2;
                            return Request.CreateResponse(HttpStatusCode.OK, UseACT4);
                        }
                    }
                    //invalid
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล  type (type 1 เพื่อการแจกจ่าย (สนับสนุนเกษตรกร)/2 การจำหน่าย /3 ภัยพิบัติ/)  ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);

                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = " กรุณาใส่ข้อมูล  oid และ type (type 1 เพื่อการแจกจ่าย (สนับสนุนเกษตรกร)/2 การจำหน่าย /3 ภัยพิบัติ/)  ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.status = "false";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// การใช้กิจกรรม เมล้ดพันธุ์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseProduct/List")]
        public HttpResponseMessage GetSupplierUse_List()
        {
            try
            {
                string org_oid = HttpContext.Current.Request.Form["Org_Oid"].ToString();
                string type = HttpContext.Current.Request.Form["type"].ToString(); //รับ=1/ส่ง=2

                if (org_oid != "" && type != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));

                    List<SupplierProductUser_Model> UserACT1 = new List<SupplierProductUser_Model>();
                    List<SupplierProductUser_Model> UserACT2 = new List<SupplierProductUser_Model>();
                    List<SupplierProductUser_Model> UserACT3 = new List<SupplierProductUser_Model>();
                    List<SupplierProductUser_Model> UserACT4 = new List<SupplierProductUser_Model>();
                    SendOrderSeedModel lists = new SendOrderSeedModel();
                    lists.org_oid = org_oid;

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    if (type == "1")
                    {  //เพื่อใช้ในกิจกรรมกรมปศุสัตว์ 
                        string ActivityOid = "069CB598-B40E-472A-A386-4F8056FB78D2";
                        IList<SupplierUseProduct> collection = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse("GCRecord is null and Status=1 and OrganizationOid='" + org_oid + "'and ActivityOid = '" + ActivityOid + "' ", null));
                        if (collection.Count > 0)
                        {
                            foreach (SupplierUseProduct row in collection)
                            {
                                SupplierProductUser_Model item = new SupplierProductUser_Model();
                                item.Oid = row.Oid.ToString();
                                item.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                item.UseNo = row.UseNo.ToString();
                                if (row.RegisCusServiceOid == null)
                                {
                                    item.RegisCusService = "ไม่พบข้อมูล";
                                    item.RegisCusServiceName = "ไม่พบข้อมูล";
                                }
                                else
                                {
                                    item.RegisCusService = row.RegisCusServiceOid.Oid.ToString();
                                    item.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + "  " + row.RegisCusServiceOid.LastNameTH;
                                }

                                if (row.OrgeServiceOid == null)
                                {
                                    item.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
                                }
                                else
                                {
                                    item.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                                }
                                item.ActivityName = row.ActivityOid.ActivityName.ToString();
                                if (row.SubActivityOid != null)
                                {
                                    item.SubActivityName = row.SubActivityOid.ActivityName;
                                }
                                if (row.SubActivityLevelOid != null)
                                {
                                    item.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                                }

                                item.FinanceYear = row.FinanceYearOid.YearName.ToString();
                                item.OrganizationName = row.OrganizationOid.SubOrganizeName.ToString();
                                if (row.EmployeeOid == null)
                                {
                                    item.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                                }
                                else
                                {
                                    item.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                                }

                                item.Remark = row.Remark;
                                item.Status = row.Status.ToString();
                                item.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                item.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                item.ActivityName = row.ActivityOid.ActivityName.ToString();

                                if (row.SubActivityOid == null)
                                {
                                    item.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                                }
                                else
                                {
                                    item.SubActivityName = row.SubActivityOid.ActivityName.ToString();
                                }
                                item.Refno = row.UseNo + "|" + row.OrganizationOid.Oid.ToString() + "|1";
                                item.Weight = row.SupplierUseProductDetails.Sum((c => c.Weight)).ToString() + " " + "กิโลกรัม";
                                UserACT1.Add(item);
                            }
                        }
                        //lists.UseACT1 = null; //UseACT1;
                        return Request.CreateResponse(HttpStatusCode.OK, UserACT1);
                    }
                    else if (type == "2")
                    {  //การจำหน่าย
                        string ActivityOid = "1B648296-1105-4216-B4C2-ECEEF6859E96";

                        IList<SupplierUseProduct> collection2 = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse("GCRecord is null and Status = 1 and OrganizationOid='" + org_oid + "'and ActivityOid = '" + ActivityOid + "' ", null));
                        if (collection2.Count > 0)
                        {
                            foreach (SupplierUseProduct row in collection2)
                            {

                                SupplierProductUser_Model item2 = new SupplierProductUser_Model();
                                item2.Oid = row.Oid.ToString();
                                item2.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                item2.UseNo = row.UseNo.ToString();
                                if (row.RegisCusServiceOid == null)
                                {
                                    item2.RegisCusService = "ไม่พบข้อมูล";
                                    item2.RegisCusServiceName = "ไม่พบข้อมูล";
                                }
                                else
                                {
                                    item2.RegisCusService = row.RegisCusServiceOid.Oid.ToString();
                                    item2.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + " " + row.RegisCusServiceOid.LastNameTH;
                                }

                                if (row.OrgeServiceOid == null)
                                {
                                    item2.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
                                }
                                else
                                {
                                    item2.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                                }
                                item2.ActivityName = row.ActivityOid.ActivityName.ToString();
                                if (row.SubActivityOid != null)
                                {
                                    item2.SubActivityName = row.SubActivityOid.ActivityName;
                                }
                                if (row.SubActivityLevelOid != null)
                                {
                                    item2.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                                }

                                item2.FinanceYear = row.FinanceYearOid.YearName.ToString();
                                item2.OrganizationName = row.OrganizationOid.SubOrganizeName.ToString();
                                if (row.EmployeeOid == null)
                                {
                                    item2.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                                }
                                else
                                {
                                    item2.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                                }

                                item2.Remark = row.Remark;
                                item2.Status = row.Status.ToString();
                                item2.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                item2.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                item2.ActivityName = row.ActivityOid.ActivityName.ToString();

                                if (row.SubActivityOid == null)
                                {
                                    item2.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                                }
                                else
                                {
                                    item2.SubActivityName = row.SubActivityOid.ActivityName.ToString();
                                }
                                item2.ReceiptNo = row.ReceiptNo;
                                item2.Refno = row.UseNo + "|" + row.OrganizationOid.Oid.ToString() + "|2";
                                item2.Weight = row.SupplierUseProductDetails.Sum((c => c.Weight)).ToString() + " " + "กิโลกรัม";
                                UserACT2.Add(item2);
                            }
                            //lists.UseACT2 = UseACT2;
                            return Request.CreateResponse(HttpStatusCode.OK, UserACT2);
                        }
                    }
                    else if (type == "3")
                    {
                        //การอนุมัติภัยพิบัติ
                        string ActivityOid = "b100c7c1-4755-4af0-812e-3dd6ba372d45";

                        IList<SupplierUseProduct> collection3 = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse("GCRecord is null and (Status in (1 ,0) )and OrganizationOid='" + org_oid + "'and ActivityOid = '" + ActivityOid + "' ", null));
                        if (collection3.Count > 0)
                        {
                            foreach (SupplierUseProduct row in collection3)
                            {

                                SupplierProductUser_Model item3 = new SupplierProductUser_Model();
                                item3.Oid = row.Oid.ToString();
                                item3.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                item3.UseNo = row.UseNo.ToString();
                                if (row.RegisCusServiceOid == null)
                                {
                                    item3.RegisCusService = "ไม่พบข้อมูล";
                                    item3.RegisCusServiceName = "ไม่พบข้อมูล";
                                }
                                else
                                {
                                    item3.RegisCusService = row.RegisCusServiceOid.Oid.ToString();
                                    item3.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + " " + row.RegisCusServiceOid.LastNameTH;
                                }

                                if (row.OrgeServiceOid == null)
                                {
                                    item3.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
                                }
                                else
                                {
                                    item3.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                                }
                                item3.ActivityName = row.ActivityOid.ActivityName.ToString();
                                if (row.SubActivityOid != null)
                                {
                                    item3.SubActivityName = row.SubActivityOid.ActivityName;
                                }
                                if (row.SubActivityLevelOid != null)
                                {
                                    item3.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                                }

                                item3.FinanceYear = row.FinanceYearOid.YearName.ToString();
                                item3.OrganizationName = row.OrganizationOid.SubOrganizeName.ToString();
                                if (row.EmployeeOid == null)
                                {
                                    item3.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                                }
                                else
                                {
                                    item3.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                                }

                                item3.Remark = row.Remark;
                                item3.Status = row.Status.ToString();
                                item3.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                item3.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                item3.ActivityName = row.ActivityOid.ActivityName.ToString();

                                if (row.SubActivityOid == null)
                                {
                                    item3.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                                }
                                else
                                {
                                    item3.SubActivityName = row.SubActivityOid.ActivityName.ToString();
                                }
                                item3.ReceiptNo = row.ReceiptNo;
                                item3.Refno = row.UseNo + "|" + row.OrganizationOid.Oid.ToString() + "|3";
                                item3.Weight = row.SupplierUseProductDetails.Sum((c => c.Weight)).ToString() + " " + "กิโลกรัม";
                                UserACT3.Add(item3);
                            }
                            //lists.UseACT3 = UseACT3;
                            return Request.CreateResponse(HttpStatusCode.OK, UserACT3);
                        }
                    }
                    else if (type == "4")
                    {
                        //เพื่อการแจกจ่าย (สนับสนุนเกษตรกร)
                        string ActivityOid = "A29D77A9-4BCB-4774-9744-FF97A373353E";

                        IList<SupplierUseProduct> collection4 = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse("GCRecord is null and Status = 1 and OrganizationOid='" + org_oid + "'and ActivityOid = '" + ActivityOid + "' ", null));
                        if (collection4.Count > 0)
                        {
                            foreach (SupplierUseProduct row in collection4)
                            {

                                SupplierProductUser_Model Supplier_4 = new SupplierProductUser_Model();
                                Supplier_4.Oid = row.Oid.ToString();
                                Supplier_4.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_4.UseNo = row.UseNo.ToString();
                                if (row.RegisCusServiceOid == null)
                                {
                                    Supplier_4.RegisCusService = "ไม่พบข้อมูล";
                                    Supplier_4.RegisCusServiceName = "ไม่พบข้อมูล";
                                }
                                else
                                {
                                    Supplier_4.RegisCusService = row.RegisCusServiceOid.Oid.ToString();
                                    Supplier_4.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + " " + row.RegisCusServiceOid.LastNameTH;
                                }

                                if (row.OrgeServiceOid == null)
                                {
                                    Supplier_4.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
                                }
                                else
                                {
                                    Supplier_4.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                                }
                                Supplier_4.ActivityName = row.ActivityOid.ActivityName.ToString();
                                if (row.SubActivityOid != null)
                                {
                                    Supplier_4.SubActivityName = row.SubActivityOid.ActivityName;
                                }
                                if (row.SubActivityLevelOid != null)
                                {
                                    Supplier_4.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                                }

                                Supplier_4.FinanceYear = row.FinanceYearOid.YearName.ToString();
                                Supplier_4.OrganizationName = row.OrganizationOid.SubOrganizeName.ToString();
                                if (row.EmployeeOid == null)
                                {
                                    Supplier_4.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                                }
                                else
                                {
                                    Supplier_4.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                                }

                                Supplier_4.Remark = row.Remark;
                                Supplier_4.Status = row.Status.ToString();
                                Supplier_4.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_4.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                Supplier_4.ActivityName = row.ActivityOid.ActivityName.ToString();

                                if (row.SubActivityOid == null)
                                {
                                    Supplier_4.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                                }
                                else
                                {
                                    Supplier_4.SubActivityName = row.SubActivityOid.ActivityName.ToString();
                                }
                                string ReceiptNo = null;
                                if (row.ReceiptNo != null)
                                {
                                    Supplier_4.ReceiptNo = row.ReceiptNo;
                                }

                                Supplier_4.Refno = row.UseNo + "|" + row.OrganizationOid.Oid.ToString() + "|1";
                                Supplier_4.Weight = row.SupplierUseProductDetails.Sum((c => c.Weight)).ToString() + " " + "กิโลกรัม";
                                UserACT4.Add(Supplier_4);
                            }
                            //lists.UseACT4 = UseACT4;
                            return Request.CreateResponse(HttpStatusCode.OK, UserACT4);
                        }
                    }

                    //invalid
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล  type (type 1 เพื่อการแจกจ่าย (สนับสนุนเกษตรกร)/2 การจำหน่าย /3 ภัยพิบัติ/)  ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);

                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล Oid และ type (type 1 เพื่อการแจกจ่าย (สนับสนุนเกษตรกร)/2 การจำหน่าย /3 ภัยพิบัติ/)  ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.status = "false";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        /// <summary>
        /// ไม่อนุมัติ การช่วยเหลือภัยพิบัติ status 4
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimal/List/NotApprove")]
        public HttpResponseMessage GetSupplierUse_NotApprove()
        {
            try
            {
                string org_oid = HttpContext.Current.Request.Form["Org_Oid"].ToString();
                //การแจกจ่าย=1/การจำหน่าย=2/ภัยพิบัติ=3
                if (org_oid != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));

                    List<SupplierProductUser> UseACT = new List<SupplierProductUser>();
                    SupplierUseAnimalProduct_Model lists = new SupplierUseAnimalProduct_Model();
                    lists.org_oid = org_oid;

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    //เพื่อใช้ในกิจกรรมกรมปศุสัตว์           
                    string ActivityOid = "b100c7c1-4755-4af0-812e-3dd6ba372d45";

                    IList<SupplierUseAnimalProduct> collection3 = ObjectSpace.GetObjects<SupplierUseAnimalProduct>(CriteriaOperator.Parse("GCRecord is null and Status = 4 and OrganizationOid='" + org_oid + "'and ActivityOid = '" + ActivityOid + "' ", null));
                    if (collection3.Count > 0)
                    {
                        foreach (SupplierUseAnimalProduct row in collection3)
                        {

                            SupplierProductUser Supplier_3 = new SupplierProductUser();
                            Supplier_3.Oid = row.Oid.ToString();
                            Supplier_3.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                            Supplier_3.UseNo = row.UseNo.ToString();
                            if (row.RegisCusServiceOid == null)
                            {
                                Supplier_3.RegisCusServiceOid = "ไม่พบข้อมูล";
                                Supplier_3.RegisCusServiceName = "ไม่พบข้อมูล";
                            }
                            else
                            {
                                Supplier_3.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                                Supplier_3.RegisCusServiceName = row.RegisCusServiceOid.FirstNameTH + " " + row.RegisCusServiceOid.LastNameTH;
                            }

                            if (row.OrgeServiceOid == null)
                            {
                                Supplier_3.OrgeServiceName = "ไม่พบข้อมูลหน่วยงานขอรับบริการ";
                            }
                            else
                            {
                                Supplier_3.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                            }
                            Supplier_3.ActivityNameOid = row.ActivityOid.Oid.ToString();
                            Supplier_3.ActivityName = row.ActivityOid.ActivityName.ToString();
                            Supplier_3.OrganizationOid = row.OrganizationOid.Oid.ToString();
                            if (row.SubActivityOid != null)
                            {
                                Supplier_3.SubActivityName = row.SubActivityOid.ActivityName;
                            }
                            if (row.SubActivityLevelOid != null)
                            {
                                Supplier_3.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                            }

                            Supplier_3.FinanceYear = row.FinanceYearOid.YearName.ToString();
                            Supplier_3.OrganizationName = row.OrganizationOid.SubOrganizeName.ToString();
                            if (row.EmployeeOid == null)
                            {
                                Supplier_3.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                            }
                            else
                            {
                                Supplier_3.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                            }

                            Supplier_3.Remark = row.Remark;
                            Supplier_3.Status = row.Status.ToString();
                            Supplier_3.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                            Supplier_3.ActivityNameOid = row.ActivityOid.Oid.ToString();
                            Supplier_3.ActivityName = row.ActivityOid.ActivityName.ToString();

                            if (row.SubActivityOid == null)
                            {
                                Supplier_3.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                            }
                            else
                            {
                                Supplier_3.SubActivityName = row.SubActivityOid.ActivityName.ToString();
                            }
                            Supplier_3.ReceiptNo = row.ReceiptNo;
                            Supplier_3.Refno = row.UseNo + "|" + row.OrganizationOid.Oid.ToString() + "|3";
                            Supplier_3.Weight = row.SupplierUseAnimalProductDetails.Sum((c => c.Weight)).ToString() + " " + "กิโลกรัม";
                            UseACT.Add(Supplier_3);
                        }
                        //lists.UseACT = UseACT;

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, UseACT);
                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล Org_Oid ให้เรียบร้อย";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.status = "false";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        #endregion แก้ไขกิจกรรม
        /// <summary>
        /// รายละเอียดหน้าการใช้เมล็ดพันธุ์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseProduct/Detail")]  ///SupplierUseProduct/Detail 
        public HttpResponseMessage GetSupplierUseProduct_detail()
        {
            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); ///เลขที่ได้จากหน้า list SupplierUseProduct/List
            //    string YearName = HttpContext.Current.Request.Form["YearName"].ToString();

                if (RefNo != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));

                    List<SupplierProductUser> list_detail = new List<SupplierProductUser>();
                    List<SupplierUseProductDetail_Model> detail = new List<SupplierUseProductDetail_Model>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    IList<SupplierUseProduct> collection = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Status = 1 and UseNo = '" + _refno + "'", null));

                    if (collection.Count > 0)
                    {
                        foreach (SupplierUseProduct row in collection)
                        {
                            double WeightAll = 0;
                            double Amout = 0;
                            SupplierProductUser item = new SupplierProductUser();
                            string TempSubDistrict, TempDistrict;
                            if (row.RegisCusServiceOid != null)
                            {

                                if (row.RegisCusServiceOid.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                                {
                                    TempSubDistrict = "แขวง";
                                }
                                else
                                {
                                    TempSubDistrict = "ตำบล";
                                };

                                if (row.RegisCusServiceOid.DistrictOid.DistrictNameTH.Contains("กรุงเทพ"))
                                {
                                    TempDistrict = "เขต";
                                }
                                else
                                {
                                    TempDistrict = "อำเภอ";
                                }          
                            }
                            else
                            {

                                if (row.OrgeServiceOid.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                                {
                                    TempSubDistrict = "แขวง";
                                }
                                else
                                {
                                    TempSubDistrict = "ตำบล";
                                };

                                if (row.OrgeServiceOid.DistrictOid.DistrictNameTH.Contains("กรุงเทพ"))
                                {
                                    TempDistrict = "เขต";
                                }
                                else { TempDistrict = "อำเภอ"; };
                            }

                            item.TypeMoblie = _type;
                            item.Oid = row.Oid.ToString();
                            item.UseNo = row.UseNo;
                            item.UseDate = row.UseDate.ToString("dd/MM/yyyy");
                            item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                            item.FinanceYear = row.FinanceYearOid.YearName;
                            item.OrganizationOid = row.OrganizationOid.Oid.ToString();
                            item.OrganizationName = row.OrganizationOid.OrganizeNameTH;
                            if (row.SubActivityLevelOid != null)
                            {
                                item.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                            }
                          
                            if (row.OrgeServiceOid != null)
                            {
                                item.OrgeServiceOid = row.OrgeServiceOid.Oid.ToString();
                                item.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;

                                item.FullAddress = "เลขที่" + " " + row.OrgeServiceOid.Address + " หมู่ที่" + " " + checknull(row.OrgeServiceOid.Moo) + " ถนน" + checknull(row.OrgeServiceOid.Road)
                                + " " + TempSubDistrict + " " + row.OrgeServiceOid.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + row.OrgeServiceOid.DistrictOid.DistrictNameTH + " " +
                                "จังหวัด" + row.OrgeServiceOid.ProvinceOid.ProvinceNameTH + " รหัสไปรษณีย์ " + row.OrgeServiceOid.DistrictOid.PostCode;
                            }
                            else
                            {
                            item.RegisCusServiceOid = "ไม่พบข้อมูล";
                            item.RegisCusServiceName = "ไม่พบข้อมูลรายชื่อบุคคลรายเดี่ยว";
                                item.FullAddress = "เลขที่" + " " + row.RegisCusServiceOid.Address + " หมู่ที่" + " " + checknull(row.RegisCusServiceOid.Moo) + " ถนน" + " " + checknull(row.RegisCusServiceOid.Road)
                               + " " + TempSubDistrict + " " + row.RegisCusServiceOid.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + row.RegisCusServiceOid.DistrictOid.DistrictNameTH + " " +
                                "จังหวัด" + row.RegisCusServiceOid.ProvinceOid.ProvinceNameTH + " รหัสไปรษณีย์ " + row.RegisCusServiceOid.DistrictOid.PostCode;

                            }
                            if (row.ApproveDate != null)
                            {
                                item.ApproveDate = row.ApproveDate.ToString("dd/MM/yyyy");
                            }

                            item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                            item.OrganizationOid = row.OrganizationOid.Oid.ToString();
                            item.ActivityNameOid = row.ActivityOid.Oid.ToString();
                            item.ActivityName = row.ActivityOid.ActivityName.ToString();
                            item.ServiceCount = row.ServiceCount;
                            item.ActivityNameOid = row.ActivityOid.Oid.ToString();
                            item.ActivityName = row.ActivityOid.ActivityName;
                            if (row.SubActivityOid != null)
                            {
                                item.SubActivityName = row.SubActivityOid.ActivityName;
                            }
                            if (row.SubActivityLevelOid != null)
                            {
                                item.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                            }
                            if (row.ReceiptNo != null)
                            {
                                item.ReceiptNo = row.ReceiptNo;
                            }

                            item.Refno = RefNo;
                            if (row.EmployeeOid != null)
                            {
                                item.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                            }
                            string BudgetSourceName = null;
                            item.Status = row.Status.ToString();
                            double Amount = 0;
                            double Price = 0;
                            double Weight = 0;
                            List<SupplierUseProductDetail_Model> item2 = new List<SupplierUseProductDetail_Model>();
                            foreach (SupplierUseProductDetail row2 in row.SupplierUseProductDetails)
                            {
                                SupplierUseProductDetail_Model D2 = new SupplierUseProductDetail_Model();
                                D2.SupplierUseProductOid = item.Oid.ToString();
                                D2.Oid = row2.Oid.ToString();
                                D2.SupplierUseProductOid = row2.SupplierUseProduct.Oid.ToString();

                                D2.Weight = row2.Weight;
                                if (row2.Weight == 0)
                                {
                                    Weight = 0;
                                }
                                else
                                {
                                    Weight = Weight + row2.Weight;
                                }

                                if (row2.AnimalSeedOid != null)
                                {
                                    D2.AnimalSeedName = row2.AnimalSeedOid.SeedName;
                                }

                                if (row2.BudgetSourceOid != null)
                                {
                                    D2.BudgetSourceName = row2.BudgetSourceOid.BudgetName;
                                }
                                D2.AnimalSeedLevelName = row2.AnimalSeedLevelOid.SeedLevelName;
                                D2.AnimalSeedLevelSubName = row2.AnimalSeedLevelOid.SeedLevelCode;
                                D2.Price = row2.Price;
                                D2.PerPrice = row2.PerPrice;
                                D2.StockUsed = row2.StockLimit;
                                D2.StockLimit = row2.StockLimit;
                                D2.AnimalSupplieName = D2.AnimalSupplieName;
                                D2.Amount = D2.Amount;
                                Price = Price + D2.Price;
                                Amount = Amount + D2.Amount;
                                BudgetSourceName = D2.BudgetSourceName;
                                detail.Add(D2);

                            }
                            item.BudgetSourceName = BudgetSourceName;
                            item.Weight = Weight + " " + "กิโลกรัม";
                            item.ReceiptNo = row.ReceiptNo;

                            item.ServiceCount = row.ServiceCount;
                            item.TotalPrice = Price.ToString() + " " + "บาท";
                            item.TotalAmout = Amount.ToString();

                            item.Refno = RefNo;
                            item.Detail = detail;
                            list_detail.Add(item);

                        }


                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list_detail);
                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "โปรดระบุ RefNo";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }



            catch (Exception ex)
            {
                UserError err = new UserError();
                err.status = "false";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
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
                    if (objStockSeedInfo.Count != 0)
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
                    ObjMaster.SendStatus = EnumSendOrderSeedStatus.Approve;
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

        /// <summary>
        /// ตัดขีด
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string checknull(object val)
        {
            string ret = "-";
            try
            {
                if (val != null || val.ToString() != string.Empty)
                {
                    ret = val.ToString();
                };
            }
            catch (Exception)
            {
                ret = "-";
            }
            return ret;
        }

        #endregion
    }
}




