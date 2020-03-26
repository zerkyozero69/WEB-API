using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.ท่อนพันธุ์_กล้าพันธุ์;

namespace WebApi.Jwt.Controllers.ท่อนพันธุ์_กล้าพันธุ์
{
    public class Sprout_Controller : ApiController

    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        // GET: api/Sprout_
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Sprout_/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Sprout_
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Sprout_/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sprout_/5
        [AllowAnonymous]
        [HttpPost]
        [Route("SproutUpdate/Approve")]
        public HttpResponseMessage UpdateSprout()
        {

            //  object objStockAnimalInfo = null;
            HistoryWork ObjHistory = null;
            IList<StockSeedInfo> objStockSeedInfo = null;
            string Username = " ";

            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
              //  string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                string CancelMsg = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                Username = HttpContext.Current.Request.Form["Username"].ToString();

                if (RefNo != "")
                {

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierSproutUseProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockSproutInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockSproutInfo_Report));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.HistoryWork));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModifyDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModify));
                    XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveLotNumber));

                    List<SupplierSproutUseProduct> list = new List<SupplierSproutUseProduct>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    UserInfo objUserInfo = ObjectSpace.FindObject<UserInfo>(CriteriaOperator.Parse("[UserName]=?", Username));

                    SupplierSproutUseProduct ObjMaster = ObjectSpace.FindObject<SupplierSproutUseProduct>(CriteriaOperator.Parse("UseNo=?", RefNo));


                    foreach (SupplierSproutUseProductDetail row in ObjMaster.SupplierSproutUseProductDetails)
                    {

                        string TempDescription = "";
                        var switchExpr = ObjMaster.ActivityOid.ActivityName;
                        switch (switchExpr)
                        {
                            case "เพื่อใช้ในกิจกรรมของศูนย์ฯ":
                                {

                                    EmployeeInfo objEmployees = ObjectSpace.FindObject<EmployeeInfo>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.EmployeeOid));
                                    if (objEmployees is object)
                                    {
                                        TempDescription = "ใช้ในกิจกรรมของศูนย์ฯ-" + ObjMaster.SubActivityOid.ActivityName + " : " + objEmployees.FullName + " (Mobile Application)";
                                    }
                                    else
                                    {
                                        TempDescription = "ใช้ในกิจกรรมของศูนย์ฯ-" + ObjMaster.SubActivityOid.ActivityName + " : " + " (Mobile Application)";
                                    }

                                    break;
                                }

                            case "เพื่อช่วยเหลือภัยพิบัติ":
                                {
                                    if (ObjMaster.RegisCusServiceOid is object)
                                    {
                                        RegisterCusService objRegisCusService = ObjectSpace.FindObject<RegisterCusService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.RegisCusServiceOid));
                                        if (objRegisCusService is object)
                                        {
                                            TempDescription = "ช่วยเหลือภัยพิบัติ-" + ObjMaster.SubActivityLevelOid.ActivityName + " : " + objRegisCusService.DisPlayName + " (Mobile Application)";
                                        }
                                        else
                                        {
                                            TempDescription = "ช่วยเหลือภัยพิบัติ-" + ObjMaster.SubActivityLevelOid.ActivityName + " : " + " (Mobile Application)";
                                        }
                                    }
                                    else if (ObjMaster.OrgeServiceOid is object)
                                    {
                                        OrgeService objOrgeService = ObjectSpace.FindObject<OrgeService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.OrgeServiceOid));
                                        if (objOrgeService is object)
                                        {
                                            TempDescription = "ช่วยเหลือภัยพิบัติ-" + ObjMaster.SubActivityLevelOid.ActivityName + " : " + objOrgeService.OrgeServiceName + " (Mobile Application)";
                                        }
                                        else
                                        {
                                            TempDescription = "ช่วยเหลือภัยพิบัติ (Mobile Application)-" + ObjMaster.SubActivityLevelOid.ActivityName + " : " + " (Mobile Application)";
                                        }
                                    }

                                    break;
                                }

                            case "เพื่อใช้ในกิจกรรมกรมปศุสัตว์":
                                {
                                    EmployeeInfo objEmployees = ObjectSpace.FindObject<EmployeeInfo>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.EmployeeOid));
                                    if (objEmployees is object)
                                    {
                                        TempDescription = "ใช้ในกิจกรรมกรม-" + ObjMaster.SubActivityOid.ActivityName + " : " + objEmployees.FullName + " (Mobile Application)";
                                    }
                                    else
                                    {
                                        TempDescription = "ใช้ในกิจกรรมกรม-" + ObjMaster.SubActivityOid.ActivityName + " : " + " (Mobile Application)";
                                    }

                                    break;
                                }

                            case "เพื่อสนับสนุนหน่วยงานภายนอกกรมปศุสัตว์":
                                {
                                    TempDescription = "สนับสนุนหน่วยงานภายนอก: " + ObjMaster.OrgeServiceOid.OrgeServiceName + " (Mobile Application) ";
                                    break;
                                }

                            case "พัฒนาความมั่นคงด้านเสบียงสัตว์":
                                {
                                    TempDescription = "พัฒนาความมั่นคงด้านเสบียงสัตว์" + "(Mobile Application)";
                                    break;
                                }

                            case "เพื่อการจำหน่าย":
                                {
                                    if (ObjMaster.RegisCusServiceOid is object)
                                    {
                                        RegisterCusService objRegisCusService = ObjectSpace.FindObject<RegisterCusService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.RegisCusServiceOid));
                                        if (objRegisCusService is object)
                                        {
                                            TempDescription = "จำหน่ายให้ : " + objRegisCusService.DisPlayName + "(Mobile Application)";
                                        }
                                        else
                                        {
                                            TempDescription = "จำหน่ายให้ : " + "(Mobile Application)";
                                        }
                                    }
                                    else if (ObjMaster.OrgeServiceOid is object)
                                    {
                                        OrgeService objOrgeService = ObjectSpace.FindObject<OrgeService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.OrgeServiceOid));
                                        if (objOrgeService is object)
                                        {
                                            TempDescription = "จำหน่ายให้ : " + objOrgeService.OrgeServiceName + "(Mobile Application)";
                                        }
                                        else
                                        {
                                            TempDescription = "จำหน่ายให้ : " + "(Mobile Application)";
                                        }
                                    }

                                    break;
                                }

                            case "เพื่อการแจกจ่าย (สนับสนุนเกษตรกร)":
                                {
                                    if (ObjMaster.RegisCusServiceOid is object)
                                    {
                                        RegisterCusService objRegisCusService = ObjectSpace.FindObject<RegisterCusService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.RegisCusServiceOid));
                                        if (objRegisCusService is object)
                                        {
                                            TempDescription = "แจกจ่ายให้ : " + objRegisCusService.DisPlayName + "(Mobile Application)";
                                        }
                                        else
                                        {
                                            TempDescription = "แจกจ่ายให้ : " + "(Mobile Application)";
                                        }
                                    }
                                    else if (ObjMaster.OrgeServiceOid is object)
                                    {
                                        OrgeService objOrgeService = ObjectSpace.FindObject<OrgeService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.OrgeServiceOid));
                                        if (objOrgeService is object)
                                        {
                                            TempDescription = "แจกจ่ายให้ : " + objOrgeService.OrgeServiceName + "(Mobile Application)";
                                        }
                                        else
                                        {
                                            TempDescription = "แจกจ่ายให้ : " + "(Mobile Application)";
                                        }
                                    }

                                    break;
                                }
                        }
                        //''Stock สำหรับ กปศ4ว
                        var objCheckStockSproutInfo = ObjectSpace.GetObjects<StockSproutInfo>(CriteriaOperator.Parse("SupplierSproutNumber=? and FinanceYearOid=? and AnimalSeedOid=? and OrganizationOid=? ", row.SupplierSproutUseProduct.UseNo, row.SupplierSproutUseProduct.FinanceYearOid.Oid, row.AnimalSeedOid.Oid, row.SupplierSproutUseProduct.OrganizationOid.Oid));

                        if (objCheckStockSproutInfo.Count > 0)
                        {
                            var ObjSubStockCardSource = (from item in objCheckStockSproutInfo orderby item.TransactionDate descending select item).First();

                            var withBlock = objCheckStockSproutInfo[0];
                            withBlock.Weight = 0 - row.Weight;

                        }
                        // View.ObjectSpace.CommitChanges()
                        else
                        {
                            objCheckStockSproutInfo = ObjectSpace.GetObjects<StockSproutInfo>(CriteriaOperator.Parse("FinanceYearOid=? and AnimalSeedOid=? and OrganizationOid=? and [SeedTypeOid]=?", row.SupplierSproutUseProduct.FinanceYearOid.Oid, row.AnimalSeedOid.Oid, row.SupplierSproutUseProduct.OrganizationOid.Oid, row.SeedTypeOid.Oid));
                            var objStockSproutInfo = ObjectSpace.CreateObject<StockSproutInfo>();
                            objStockSproutInfo.TransactionDate = DateTime.Now;
                            objStockSproutInfo.SupplierSproutNumber = row.SupplierSproutUseProduct.UseNo;
                            objStockSproutInfo.FinanceYearOid = row.SupplierSproutUseProduct.FinanceYearOid;
                            objStockSproutInfo.BudgetSourceOid = objCheckStockSproutInfo[0].BudgetSourceOid;
                            objStockSproutInfo.AnimalSeedOid = row.AnimalSeedOid;
                            objStockSproutInfo.OrganizationOid = row.SupplierSproutUseProduct.OrganizationOid;
                            objStockSproutInfo.Weight = 0 - row.Weight;
                            objStockSproutInfo.Remark = "อนุมัติใช้กล้าพันธุ์ (Mobile Application)";
                            objStockSproutInfo.SeedTypeOid = row.SeedTypeOid;
                            objStockSproutInfo.Description = TempDescription;
                            // View.ObjectSpace.CommitChanges()
                        }

                        // 'Stock สำหรับ กปศ4ว
                        var objStockSproutInfo_Detail = ObjectSpace.GetObjects<StockSproutInfo_Report>(CriteriaOperator.Parse("[FinanceYearOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", ObjMaster.FinanceYearOid, ObjMaster.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
                        var objStockSproutInfo_DetailNew = ObjectSpace.CreateObject<StockSproutInfo_Report>();
                        // ==========================================
                        if (objStockSproutInfo_Detail.Count > 0)
                        {
                            var ObjStockSproutInfo_DetailSource = (from Item in objStockSproutInfo_Detail
                                                                   orderby Item.TransactionDate descending
                                                                   select Item).First();
                            objStockSproutInfo_DetailNew.TransactionDate = DateTime.Now;
                            objStockSproutInfo_DetailNew.SproutProductNumber = ObjMaster.UseNo;
                            objStockSproutInfo_DetailNew.FinanceYearOid = ObjMaster.FinanceYearOid;
                            objStockSproutInfo_DetailNew.BudgetSourceOid = ObjStockSproutInfo_DetailSource.BudgetSourceOid;
                            objStockSproutInfo_DetailNew.OrganizationOid = ObjMaster.OrganizationOid;
                            objStockSproutInfo_DetailNew.AnimalSeedOid = row.AnimalSeedOid;
                            objStockSproutInfo_DetailNew.TotalForward = ObjStockSproutInfo_DetailSource.TotalWeight;
                            objStockSproutInfo_DetailNew.TotalChange = 0 - row.Weight;
                            objStockSproutInfo_DetailNew.SeedTypeOid = row.SeedTypeOid;
                            objStockSproutInfo_DetailNew.Description = TempDescription;
                            // ==========================================

                        };


                    }
                    ObjMaster.Status = EnumRodBreedProductSeedStatus.Approve;
                    ObjMaster.ApproveDate = DateTime.Now;
                    ObjectSpace.CommitChanges();

                    ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                    // ประวัติ
                    ObjHistory.RefOid = ObjMaster.Oid.ToString();
                    ObjHistory.FormName = "กล้าพันธุ์";
                    ObjHistory.Message = "อนุมัติ (ขอใช้กล้าพันธุ์ (Mobile Application) ) ลำดับที่ : " + ObjMaster.UseNo;
                    ObjHistory.CreateBy = objUserInfo.DisplayName;
                    ObjHistory.CreateDate = DateTime.Now;
                    ObjectSpace.CommitChanges();


                    UpdateResult ret = new UpdateResult();
                    ret.status = "true";
                    ret.message = "บันทึกข้อมูลอนุมัติเรียบร้อยแล้ว";
                    return Request.CreateResponse(HttpStatusCode.OK, ret);

                }

                else
                {
                    UpdateResult ret = new UpdateResult();
                    ret.status = "False";
                    ret.message = "กรุณากรอก RefNo";
                    return Request.CreateResponse(HttpStatusCode.NoContent, ret);
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
        [AllowAnonymous]
        [HttpPost]
        [Route("SproutUpdate/Eject")]
        public HttpResponseMessage UpdateSproutUseEject()
        {
            //  object objStockAnimalInfo = null;
            HistoryWork ObjHistory = null;
            IList<StockSeedInfo> objStockSeedInfo = null;
            string Username = " ";

            try
            {
                //string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                //string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                //string CancelMsg = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                //Username = HttpContext.Current.Request.Form["Username"].ToString();

                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                                                                                     // string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                string CancelMsg = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                Username = HttpContext.Current.Request.Form["Username"].ToString();
                if (RefNo != "")
                {

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierSproutUseProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockSproutInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockSproutInfo_Report));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.HistoryWork));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModifyDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModify));
                    XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveLotNumber));

                    List<SupplierSproutUseProduct> list = new List<SupplierSproutUseProduct>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    UserInfo objUserInfo = ObjectSpace.FindObject<UserInfo>(CriteriaOperator.Parse("[UserName]=?", Username));

                    SupplierSproutUseProduct ObjMaster = ObjectSpace.FindObject<SupplierSproutUseProduct>(CriteriaOperator.Parse("UseNo=?", RefNo));

                    foreach (SupplierSproutUseProductDetail row in ObjMaster.SupplierSproutUseProductDetails)
                    {
                        var objCheckStockSproutInfo = ObjectSpace.GetObjects<StockSproutInfo>(CriteriaOperator.Parse("SupplierSproutNumber=? and FinanceYearOid=? and AnimalSeedOid=? and OrganizationOid=? ", row.SupplierSproutUseProduct.UseNo, row.SupplierSproutUseProduct.FinanceYearOid.Oid, row.AnimalSeedOid.Oid, row.SupplierSproutUseProduct.OrganizationOid.Oid));
                        if (objCheckStockSproutInfo.Count > 0)
                        {
                            objCheckStockSproutInfo = ObjectSpace.GetObjects<StockSproutInfo>(CriteriaOperator.Parse("FinanceYearOid=? and AnimalSeedOid=? and OrganizationOid=? ", row.SupplierSproutUseProduct.FinanceYearOid.Oid, row.AnimalSeedOid.Oid, row.SupplierSproutUseProduct.OrganizationOid.Oid));
                            var objStockSproutInfo = ObjectSpace.CreateObject<StockSproutInfo>();
                            objStockSproutInfo.TransactionDate = DateTime.Now;
                            objStockSproutInfo.SupplierSproutNumber = row.SupplierSproutUseProduct.UseNo;
                            objStockSproutInfo.FinanceYearOid = row.SupplierSproutUseProduct.FinanceYearOid;
                            objStockSproutInfo.BudgetSourceOid = objCheckStockSproutInfo[0].BudgetSourceOid;
                            objStockSproutInfo.AnimalSeedOid = row.AnimalSeedOid;
                            objStockSproutInfo.OrganizationOid = row.SupplierSproutUseProduct.OrganizationOid;
                            objStockSproutInfo.Weight = row.Weight;
                            objStockSproutInfo.Remark = "คืนสต๊อกสินค้าเนื่องจากไม่อนุมัติ (Mobile Application) ";
                            objStockSproutInfo.SeedTypeOid = row.SeedTypeOid;
                        }
                        //                ''Stock สำหรับ กปศ4ว

                        if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Approve)
                        {
                            var objStockSproutInfo_Detail = ObjectSpace.GetObjects<StockSproutInfo_Report>(CriteriaOperator.Parse("[FinanceYearOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", ObjMaster.FinanceYearOid, ObjMaster.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
                            var objStockSproutInfo_DetailNew = ObjectSpace.CreateObject<StockSproutInfo_Report>();
                            if (objStockSproutInfo_Detail.Count > 0)
                            {
                                var ObjStockSproutInfo_DetailSource = (from Item in objStockSproutInfo_Detail
                                                                       orderby Item.TransactionDate descending
                                                                       select Item).First();
                                objStockSproutInfo_DetailNew.TransactionDate = DateTime.Now;
                                objStockSproutInfo_DetailNew.SproutProductNumber = ObjMaster.UseNo;
                                objStockSproutInfo_DetailNew.FinanceYearOid = ObjMaster.FinanceYearOid;
                                objStockSproutInfo_DetailNew.BudgetSourceOid = ObjStockSproutInfo_DetailSource.BudgetSourceOid;
                                objStockSproutInfo_DetailNew.OrganizationOid = ObjMaster.OrganizationOid;
                                objStockSproutInfo_DetailNew.AnimalSeedOid = row.AnimalSeedOid;
                                objStockSproutInfo_DetailNew.TotalForward = ObjStockSproutInfo_DetailSource.TotalWeight;
                                objStockSproutInfo_DetailNew.TotalChange = row.Weight;
                                objStockSproutInfo_DetailNew.SeedTypeOid = row.SeedTypeOid;
                                objStockSproutInfo_DetailNew.Description = "คืนสต๊อกเนื่องจากไม่อนุมัติการใช้กล้าพันธุ์ (Mobile Application): " + ObjMaster.OrganizationOid.SubOrganizeName;
                            }
                        }
                    }
                    SupplierSproutUseProduct TmpObjMaster ;
                    ObjMaster.CancelMsg = CancelMsg;
                    ObjMaster.Status = EnumRodBreedProductSeedStatus.Eject;
                    ObjMaster.CancelBy = objUserInfo.DisplayName;
                    ObjMaster.ActionType = EnumAction.Eject;
                    ObjMaster.CancelDate = DateTime.Now;
                    ObjectSpace.CommitChanges();

                    ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                    // ประวัติ
                    ObjHistory.RefOid = ObjMaster.Oid.ToString();
                    ObjHistory.FormName = "กล้าพันธุ์";
                    ObjHistory.Message = "ไม่อนุมัติ (ขอใช้กล้าพันธุ์ (Mobile Application)) ลำดับที่ : " + ObjMaster.UseNo;
                    ObjHistory.CreateBy = objUserInfo.DisplayName;
                    ObjHistory.CreateDate = DateTime.Now;
                    ObjectSpace.CommitChanges();
                    UpdateResult ret = new UpdateResult();
                    ret.status = "true";
                    ret.message = "บันทึกข้อมูลไม่อนุมัติเรียบร้อยแล้ว";
                   // ret.message = "บันทึกข้อมูลไม่อนุมัติเรียบร้อยแล้ว"+ "(Mobile Application)";
                    return Request.CreateResponse(HttpStatusCode.OK, ret);

                }
                else
                {
                    UpdateResult ret = new UpdateResult();
                    ret.status = "False";
                    ret.message = "กรุณากรอก RefNo";
                    return Request.CreateResponse(HttpStatusCode.NotFound, ret);
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
        [AllowAnonymous]
        [HttpPost]
        [Route("Sprout/list")]
        public HttpResponseMessage  Sproutlist()
            {
            try
            {
                string Org = HttpContext.Current.Request.Form["organizationOid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSproutUseProduct));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<SproutUseProduct_Model> Detail = new List<SproutUseProduct_Model>();
                IList<SupplierSproutUseProduct> collection = ObjectSpace.GetObjects<SupplierSproutUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Status = 1 and OrganizationOid = ? ", Org));
                var query = from Q in collection orderby Q.UseNo select Q;
                if (collection.Count > 0)
                {
                    foreach (SupplierSproutUseProduct row in query)
                    {
                        SproutUseProduct_Model item = new SproutUseProduct_Model();
                        item.SproutUseProductOid = row.Oid.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        item.ActivityOid = row.ActivityOid.Oid.ToString();
                        item.ActivityName = row.ActivityOid.ActivityName;
                        if (row.SubActivityLevelOid != null)
                        {
                            item.SubActivityLevelOid = row.SubActivityLevelOid.Oid.ToString();
                            item.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                        }
                       
                        item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        item.UseNo = row.UseNo;
                        item.UseDate = row.UseDate.ToString("dd/MM/yy");
                        item.OrganizationOid = row.OrganizationOid.Oid.ToString();
                        item.OrganizationName = row.OrganizationOid.SubOrganizeName;
                        if (row.EmployeeOid != null)
                        {
                            item.EmployeeOid = row.EmployeeOid.Oid.ToString();
                            item.FullName = row.EmployeeOid.FullName;
                            
                        }
                        if (row.RegisCusServiceOid != null)
                        {
                            item.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                            item.FullName = row.RegisCusServiceOid.DisPlayName;
                            item.FullName = row.RegisCusServiceOid.FullAddress;
                        }
                        if(row.OrgeServiceOid != null)
                        {
                            item.OrgeServiceOid = row.OrgeServiceOid.Oid.ToString();
                            item.FullName = row.OrgeServiceOid.OrgeServiceName;
                            item.FullAddress = row.OrgeServiceOid.FullAddress;
                        }

                        
                        item.ServiceCount = row.ServiceCount;
                        item.TotalWeight = row.SupplierSproutUseProductDetails.Sum((c => c.Weight)).ToString() + " กิโลกรัม";
                        Detail.Add(item);

                    }
                    directProvider.Dispose();
                    ObjectSpace.Dispose();
                    //lists.Receive = ReceiveItems;
                    return Request.CreateResponse(HttpStatusCode.OK, Detail);
                }
                else
                {
                    UserError err2 = new UserError();
                    err2.status = "false";
                    err2.code = "-9";
                    err2.message = "ไม่มีข้อมูลรายการ การใช้กล้าพันธุ์";
                    return Request.CreateResponse(HttpStatusCode.NotFound, err2);

                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.status = "false";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.ToString();
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Sprout/Detail")]
        public HttpResponseMessage  Sproutlisttdetail()
        {
            try
            {
                string UseNo = HttpContext.Current.Request.Form["useNo"].ToString();

                if (UseNo != "")
                {


                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierSproutUseProduct));
                    RodbreedSproutUseProduct_model_Detail item = null;
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    IList<SupplierSproutUseProduct> collection = ObjectSpace.GetObjects<SupplierSproutUseProduct>(CriteriaOperator.Parse("GCRecord is null and UseNo='" + UseNo + "'", null));

                    foreach (SupplierSproutUseProduct row in collection)
                    {
                        item = new RodbreedSproutUseProduct_model_Detail();
                        item.SproutUseProductOid = row.Oid.ToString();
                        item.UseNo = row.UseNo;
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        item.UseDate = row.UseDate.ToString("dd/MM/yyyy");
                        item.OrganizationOid = row.OrganizationOid.Oid.ToString();
                        item.OrganizationName = row.OrganizationOid.SubOrganizeName;
                        item.ActivityOid = row.ActivityOid.Oid.ToString();
                        item.Remark = row.Remark;
                        item.ActivityName = row.ActivityOid.ActivityName;
                        item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;

                        if (row.OrgeServiceOid != null)
                        {
                            item.OrgeServiceOid = row.OrgeServiceOid.Oid.ToString();
                            item.FullName = row.OrgeServiceOid.OrgeServiceName;
                            item.FullAddress = row.OrgeServiceOid.FullAddress;
                        }
                        if (row.RegisCusServiceOid != null)
                        {
                            item.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                            item.FullName = row.RegisCusServiceOid.DisPlayName;
                            item.FullAddress = row.RegisCusServiceOid.FullAddress;
                        }
                        if (row.SubActivityOid != null)
                        {
                            item.SubActivityOid = row.SubActivityOid.Oid.ToString();
                            item.SubActivityName = row.SubActivityOid.ActivityName;
                        }
                        if (row.SubActivityLevelOid != null)
                        {
                            item.SubActivityLevelOid = row.SubActivityLevelOid.Oid.ToString();
                            item.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                        }
     
                        item.Status = row.Status.ToString();
                        item.ServiceCount = row.ServiceCount;
                        item.TotalWeight = row.SupplierSproutUseProductDetails.Sum((c => c.Weight)).ToString()+""+"กิโลกกรัม";
                        item.TotalPrice = row.SupplierSproutUseProductDetails.Sum((c => c.Price)).ToString()+""+"บาท ";
                        item.CancelMsg = row.CancelMsg;
                        if (row.ActivityOid.ActivityName == "เพื่อการจำหน่าย")
                        {
                            item.ReceiptNo = row.ReceiptNo;
                         
                        }
                        List<SupplierSproutUseProductDetail_Model> details = new List<SupplierSproutUseProductDetail_Model>();
                        SupplierSproutUseProductDetail_Model _dt = null;

                        foreach (SupplierSproutUseProductDetail  rw in row.SupplierSproutUseProductDetails)
                        {
                            _dt = new SupplierSproutUseProductDetail_Model();
                            _dt.AnimalSeedOid = rw.AnimalSeedOid.Oid.ToString();
                            _dt.SupplierSproutUseProductDetailOid = rw.Oid.ToString();
                            _dt.StockLimit = rw.StockLimit;
                            _dt.Weight = rw.Weight;
                            _dt.SeedTypeOid = rw.SeedTypeOid.Oid.ToString();
                            _dt.SeedTypeName = rw.SeedTypeOid.SeedTypeName;
                            _dt.AnimalSeedName = rw.AnimalSeedOid.SeedName;
                            _dt.Price = rw.Price;
                            _dt.PerPrice = rw.PerPrice;
                            _dt.SupplierSproutUseProductOid = rw.SupplierSproutUseProduct.Oid.ToString();
                            _dt.PackageOid = rw.PackageOid.Oid.ToString();
                            _dt.PackageName = rw.PackageOid.PackageName;
                            details.Add(_dt);
                        }

                        item.Details = details;

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล RefNo ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
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
                SqlConnection.ClearAllPools();
            }
        }

    }
}
