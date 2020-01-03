using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using static WebApi.Jwt.Models.Farmerinfo;
using static WebApi.Jwt.Models.แผนการผลิตโมเดล.การใช้เมล็ดพันธุ์อนุมัติภัยพิบัติ;

namespace WebApi.Jwt.Controllers.อนุมัติภัยพิบัตื
{
    public class CalamityController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// อนุมัติ-ไม่อนุมัติการช่วยเหลือภัยพิบัติ เมล็ดพันธุ์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ApprovalDisasterSupplierUseProduct/Update")]
        public HttpResponseMessage UpdateDisaster()  ///SupplierUseAnimalProduct/Update
        {

            try
            {

                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                
                string RefNo = jObject.SelectToken("RefNo").Value<string>();//ข้อมูลเลขที่อ้างอิง
                string Status = jObject.SelectToken("Status").Value<string>(); //สถานะ 1 บันทึกการส่งแต่ไม่เปลี่ยนสถานนะ/ 2 ยืนยันการส่ง
                string Remark = jObject.SelectToken("Remark").Value<string>(); //หมายเหตุ
                string Type = jObject.SelectToken("Type").Value<string>(); //1 รายเดี่ยว /2 รายกลุ่ม
                string activityNameOid = jObject.SelectToken("activityNameOid").Value<string>();

                
                int ServiceCount = jObject.SelectToken("ServiceCount").Value<int>();

                if (RefNo != "" && Status != "" && activityNameOid != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseProduct));

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SupplierUseProduct objSupplierUseProduct = ObjectSpace.FindObject<SupplierUseProduct>(CriteriaOperator.Parse("UseNo=? and ActivityOid= ?  ", _refno, activityNameOid));


                    //nutrition.Module.SupplierUseProduct SupplierUseProduct_inserts = new SupplierUseProduct(Session.DefaultSession) ;
                    //XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct_inserts));
                  //  SupplierUseProduct_inserts = ObjectSpace.FindObject<SupplierUseProduct>(CriteriaOperator.Parse("CitizenID=? and GCRecord is null ", CitizenID));

                    //

                    if (objSupplierUseProduct != null)
                    {

                        if (Status == "1")  //1 บันทึก
                        { //บันทึกการส่ง
                            if (Type == "1") //หน่วยงาน OrgeService
                            {
                                //   OrgeServiceOid = Session.FindObject<OrgeService>(CriteriaOperator.Parse("Oid=?", OrgeServiceOid))OrgeServiceOid
                                //objSupplierUseProduct = ObjectSpace.CreateObject<SupplierUseProduct>();
                                objSupplierUseProduct.ServiceCount = ServiceCount;
                                if (Remark != "")
                                {
                                    objSupplierUseProduct.Remark = Remark;
                                }

                                ObjectSpace.CommitChanges();
                            }
                            else if (Type == "2")
                            {
                                //objSupplierUseProduct = ObjectSpace.CreateObject<SupplierUseProduct>();
                                objSupplierUseProduct.ServiceCount = ServiceCount;
                                if (Remark != "")
                                {
                                    objSupplierUseProduct.Remark = Remark;
                                }

                                ObjectSpace.CommitChanges();

                            }
                        }
                        if (Status == "2")
                        { //ยืนยันการส่ง
                            if (Type == "1") //หน่วยงาน OrgeService
                            {
                                //   OrgeServiceOid = Session.FindObject<OrgeService>(CriteriaOperator.Parse("Oid=?", OrgeServiceOid))
                              //  objSupplierUseProduct = ObjectSpace.CreateObject<SupplierUseProduct>();

                                objSupplierUseProduct.ServiceCount = ServiceCount;
                                if (Remark != "")
                                {
                                    objSupplierUseProduct.Remark = Remark;
                                }

                                ObjectSpace.CommitChanges();
                            }
                            objSupplierUseProduct.Stauts = EnumSupplierUseStatus.Approved; //4

                            ObjectSpace.CommitChanges();
                        }
                        else if (Type == "2")
                        {
                           // objSupplierUseProduct = ObjectSpace.CreateObject<SupplierUseProduct>();
                            objSupplierUseProduct.ServiceCount = ServiceCount;
                            if (Remark != "")
                            {
                                objSupplierUseProduct.Remark = Remark;
                            }
                            objSupplierUseProduct.Stauts = EnumSupplierUseStatus.Approved; //4
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
        ///  sp อัพเดทเมล็ดพันธุ์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("insertsCalamity/SupplierUserProduct")]
        public HttpResponseMessage inserts_Calamity_SupplierUserProduct()
        {
            SupplierProductUser_Model2 productUser = new SupplierProductUser_Model2();

            try
            {
               
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                string TempForageType = string.Empty;
                if (jObject != null)
                {
                   
                    //string[] arr = RefNo.Split('|');
                    //string _refno = arr[0]; //เลขที่อ้างอิง
                    //string _org_oid = arr[1]; //oid หน่วยงาน
                    //string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    string iDate = jObject.SelectToken("UseDate").Value<string>();
                    DateTime oDate = Convert.ToDateTime(iDate);
                    productUser.UseNo = jObject.SelectToken("UseNo").Value<string>();
                    productUser.UseDate = oDate.Year + "-" + oDate.Month + "-" + oDate.Day;
                    productUser.FinanceYearOid = jObject.SelectToken("FinanceYearOid").Value<string>();
                    productUser.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    productUser.Remark = jObject.SelectToken("Remark").Value<string>();
                    productUser.ActivityNameOid = jObject.SelectToken("ActivityNameOid").Value<string>();
                    productUser.CitizenID = jObject.SelectToken("CitizenID").Value<string>();
                   
       
                    productUser.YearName = jObject.SelectToken("YearName").Value<string>();

                    if (jObject.SelectToken("SubActivityOid") != null)
                    {
                        productUser.SubActivityOid = jObject.SelectToken("SubActivityOid").Value<string>();
                    }
                        if (jObject.SelectToken("SubActivityLevelName") != null)
                    { productUser.SubActivityLevelName = jObject.SelectToken("SubActivityLevelName").Value<string>(); }
         
                    if (jObject.SelectToken("RegisCusServiceOid ") != null)
                    {
                        productUser.RegisCusServiceOid = jObject.SelectToken("RegisCusServiceOid ").Value<string>();
                    }
                    if (jObject.SelectToken("OrgeServiceOid ") != null)
                    {
                        productUser.OrgeServiceOid = jObject.SelectToken("OrgeServiceOid ").Value<string>();
                    }

                    productUser.ServiceCount = jObject.SelectToken("ServiceCount").Value<int>();


                    if (productUser.UseNo == "")
                    {
                        XpoTypesInfoHelper.GetXpoTypeInfoSource();
                        XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.RunNumber));
                        XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Organization));

                        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                        Organization objORG = ObjectSpace.FindObject<Organization>(CriteriaOperator.Parse("Oid=?", productUser.OrganizationOid));
                        //SendOrderSeed objSupplierProduct = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("SendNo=?", _refno));
                        RunNumber runningNumber = ObjectSpace.FindObject<RunNumber>(CriteriaOperator.Parse("FormType ='UseProduct' and BudgetYear =? and OrgCode=? ", productUser.YearName, objORG.OrganizationCode));
                        if (runningNumber != null)
                        {
                            string customerNumberFormat = string.Empty;
                            string Postfix = "000" + runningNumber.LastNumber + 1;
                            var FullNumber = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-" + (runningNumber.LastNumber + 1).ToString().PadLeft(6, '0');

                            productUser.UseNo = FullNumber;
                        }
                        else
                        {
                            DataSet ds2;
                            SqlParameter[] prm2 = new SqlParameter[10];

                            prm2[0] = new SqlParameter("@orgcode", objORG.OrganizationCode);
                            prm2[1] = new SqlParameter("@BudgetYear", productUser.YearName);
                            prm2[2] = new SqlParameter("@LastNumber", 1);
                            prm2[3] = new SqlParameter("@FormType", "UseProduct");

                            ds2 = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Insert_RuningNumber", prm2);

                            productUser.UseNo = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-000001";
                        }
                    }
                  
                    DataSet ds;
                    SqlParameter[] prm = new SqlParameter[10];

                    prm[0] = new SqlParameter("@UseNo", productUser.UseNo);
                    prm[1] = new SqlParameter("@UseDate", productUser.UseDate);  
                    prm[2] = new SqlParameter("@YearName", productUser.YearName);
                    prm[3] = new SqlParameter("@OrganizationOid", productUser.OrganizationOid);
                    prm[4] = new SqlParameter("@Remark", productUser.Remark);
                    prm[5] = new SqlParameter("@ActivityOid", productUser.ActivityNameOid);
                    prm[6] = new SqlParameter("@RegisCusServiceOid", productUser.RegisCusServiceOid);
                    prm[7] = new SqlParameter("@OrgeServiceOid", productUser.OrgeServiceOid);
                    prm[8] = new SqlParameter("@ServiceCount", productUser.ServiceCount);       
                    prm[9] = new SqlParameter("@CitizenID",productUser.CitizenID);
                    

                    ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieInserts_Calamity_SupplierUseProduct", prm);
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    //    using (DataSet ds = SqlHelper.ExecuteDataset(scc, "spt_Moblieinsert_RegisterFarmer", ))

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        
                        //return Request.CreateResponse(HttpStatusCode.OK);
                      return Request.CreateResponse(HttpStatusCode.OK, productUser);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,productUser);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,"NoData");
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
        /// อัพเดทภัยพิบัติ เสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("insertsCalamity/SupplierUseAnimalProduct")]
        public HttpResponseMessage inserts_Calamity_SupplierUseAnimalProductt()
        {
            SupplierProductUser_Model2 productUser = new SupplierProductUser_Model2();

            try
            {

                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                string TempForageType = string.Empty;
                if (jObject != null)
                {

                    //string[] arr = RefNo.Split('|');
                    //string _refno = arr[0]; //เลขที่อ้างอิง
                    //string _org_oid = arr[1]; //oid หน่วยงาน
                    //string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    string iDate = jObject.SelectToken("UseDate").Value<string>();
                    DateTime oDate = Convert.ToDateTime(iDate);
                    productUser.UseNo = jObject.SelectToken("UseNo").Value<string>();
                    productUser.UseDate = oDate.Year + "-" + oDate.Month + "-" + oDate.Day;
                    productUser.FinanceYearOid = jObject.SelectToken("FinanceYearOid").Value<string>();
                    productUser.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    productUser.Remark = jObject.SelectToken("Remark").Value<string>();
                    productUser.ActivityNameOid = jObject.SelectToken("ActivityNameOid").Value<string>();
                    productUser.CitizenID = jObject.SelectToken("CitizenID").Value<string>();


                    productUser.YearName = jObject.SelectToken("YearName").Value<string>();

                    if (jObject.SelectToken("SubActivityOid") != null)
                    {
                        productUser.SubActivityOid = jObject.SelectToken("SubActivityOid").Value<string>();
                    }
                    if (jObject.SelectToken("SubActivityLevelName") != null)
                    { productUser.SubActivityLevelName = jObject.SelectToken("SubActivityLevelName").Value<string>(); }

                    if (jObject.SelectToken("RegisCusServiceOid ") != null)
                    {
                        productUser.RegisCusServiceOid = jObject.SelectToken("RegisCusServiceOid ").Value<string>();
                    }
                    if (jObject.SelectToken("OrgeServiceOid ") != null)
                    {
                        productUser.OrgeServiceOid = jObject.SelectToken("OrgeServiceOid ").Value<string>();
                    }

                    productUser.ServiceCount = jObject.SelectToken("ServiceCount").Value<int>();


                    if (productUser.UseNo == "")
                    {
                        XpoTypesInfoHelper.GetXpoTypeInfoSource();
                        XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.RunNumber));
                        XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Organization));

                        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                        Organization objORG = ObjectSpace.FindObject<Organization>(CriteriaOperator.Parse("Oid=?", productUser.OrganizationOid));
                        //SendOrderSeed objSupplierProduct = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("SendNo=?", _refno));
                        /// รอเปลี่ยน
                        RunNumber runningNumber = ObjectSpace.FindObject<RunNumber>(CriteriaOperator.Parse("FormType ='UseProduct' and BudgetYear =? and OrgCode=? ", productUser.YearName, objORG.OrganizationCode));
                        if (runningNumber != null)
                        {
                            string customerNumberFormat = string.Empty;
                            string Postfix = "000" + runningNumber.LastNumber + 1;
                            var FullNumber = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-" + (runningNumber.LastNumber + 1).ToString().PadLeft(6, '0');

                            productUser.UseNo = FullNumber;
                        }
                        else
                        {
                            DataSet ds2;
                            SqlParameter[] prm2 = new SqlParameter[10];

                            prm2[0] = new SqlParameter("@orgcode", objORG.OrganizationCode);
                            prm2[1] = new SqlParameter("@BudgetYear", productUser.YearName);
                            prm2[2] = new SqlParameter("@LastNumber", 1);
                            prm2[3] = new SqlParameter("@FormType", "UseProduct");

                            ds2 = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Insert_RuningNumber", prm2);

                            productUser.UseNo = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-000001";
                        }
                    }

                    DataSet ds;
                    SqlParameter[] prm = new SqlParameter[10];

                    prm[0] = new SqlParameter("@UseNo", productUser.UseNo);
                    prm[1] = new SqlParameter("@UseDate", productUser.UseDate);
                    prm[2] = new SqlParameter("@YearName", productUser.YearName);
                    prm[3] = new SqlParameter("@OrganizationOid", productUser.OrganizationOid);
                    prm[4] = new SqlParameter("@Remark", productUser.Remark);
                    prm[5] = new SqlParameter("@ActivityOid", productUser.ActivityNameOid);
                    prm[6] = new SqlParameter("@RegisCusServiceOid", productUser.RegisCusServiceOid);
                    prm[7] = new SqlParameter("@OrgeServiceOid", productUser.OrgeServiceOid);
                    prm[8] = new SqlParameter("@ServiceCount", productUser.ServiceCount);
                    prm[9] = new SqlParameter("@CitizenID", productUser.CitizenID);


                    ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieInserts_Calamity_SupplierUseAnimalProduct", prm);
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    //    using (DataSet ds = SqlHelper.ExecuteDataset(scc, "spt_Moblieinsert_RegisterFarmer", ))

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        //return Request.CreateResponse(HttpStatusCode.OK);
                        return Request.CreateResponse(HttpStatusCode.OK, productUser);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, productUser);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
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

        public HttpResponseMessage AddSupplierUseAnimalProductDetail_ByUseNo()
        {
            try
            {


                return Request.CreateResponse(HttpStatusCode.OK);
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
        /// อนุมัติ-ไม่อนุมัติการช่วยเหลือภัยพิบัติ เสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ApprovalDisasterSupplierUseAnimalProduct/Update")]
        public HttpResponseMessage UpdateDisasterSupplierUseAnimalProduct()  ///SupplierUseAnimalProduct/Update
        {
            _Registerfarmer Registerfarmer = new _Registerfarmer();
            try
            {
                
                    string UseNo = HttpContext.Current.Request.Form["UseNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                    string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                    string Remark = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                    string activityNameOid = HttpContext.Current.Request.Form["activityNameOid"].ToString();

                    if (UseNo != "" && Status != "" && activityNameOid != "")
                    {
                       

                        XpoTypesInfoHelper.GetXpoTypeInfoSource();
                        XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseAnimalProduct));

                        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                        SupplierUseAnimalProduct objSupplierUseAnimalProduct = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null  and Stauts = 1 and UseNo=? and ActivityOid= ? ", UseNo, activityNameOid));

                        if (objSupplierUseAnimalProduct != null)
                        {

                            if (Status == "1")
                            { //อนุมัติ                      
                            objSupplierUseAnimalProduct.UseNo = objSupplierUseAnimalProduct.UseNo;

                                objSupplierUseAnimalProduct.Stauts = EnumRodBreedProductSeedStatus.Accept; //1
                                if (Remark != "")
                                {
                                    objSupplierUseAnimalProduct.Remark = Remark;
                                }
                                ObjectSpace.CommitChanges();

                            UpdateResult Accept = new UpdateResult();
                            Accept.status = "true";
                            Accept.message = "บันทึกข้อมูลเสร็จเรียบร้อยแล้ว";
                            return Request.CreateResponse(HttpStatusCode.OK, Accept);
                        }
                            else if (Status == "2")
                            { // อนุมัติให้ ผ.อ.
                            objSupplierUseAnimalProduct.UseNo = objSupplierUseAnimalProduct.UseNo;
                            objSupplierUseAnimalProduct.Stauts = EnumRodBreedProductSeedStatus.Approve; //2
                                if (Remark != "")
                                {
                                    objSupplierUseAnimalProduct.Remark = Remark;
                                }
                                ObjectSpace.CommitChanges();
                            }

                            UpdateResult ret = new UpdateResult();
                            ret.status = "true";
                            ret.message = "ยืนยันการส่งให้ ผอ.อนุมัติเรียบร้อยแล้ว";
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
        /// อัพเดทอนุมัติการใช้เสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimal/Update")] //SupplierUseAnimal/Update
        public HttpResponseMessage SupplierUseAnimal_Update()  ///SupplierUseAnimalProduct/Update
        {
            _Registerfarmer Registerfarmer = new _Registerfarmer();
            try
            {

                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                string Remark = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                string activityNameOid = HttpContext.Current.Request.Form["activityNameOid"].ToString();

                if (RefNo != "" && Status != "" && activityNameOid != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseAnimalProduct));

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SupplierUseAnimalProduct objSupplierUseAnimalProduct = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse("UseNo=? and ActivityOid= ? ", _refno, activityNameOid));

                    if (objSupplierUseAnimalProduct != null)
                    {

                        if (Status == "1")
                        {
                            objSupplierUseAnimalProduct.Stauts = EnumRodBreedProductSeedStatus.Approve;
                            objSupplierUseAnimalProduct.Remark = Remark;
                        }
                        else
                        {
                            objSupplierUseAnimalProduct.Stauts = EnumRodBreedProductSeedStatus.NoApprove;
                            objSupplierUseAnimalProduct.Remark = Remark;
                        }

                        ObjectSpace.CommitChanges();
                        UpdateResult ret = new UpdateResult();
                        ret.status = "true";
                        ret.message = "บันทึกข้อมูลเสร็จเรียบร้อยแล้ว";
                        return Request.CreateResponse(HttpStatusCode.OK, ret);
                    }
                    else
                    {
                        UpdateResult ret = new UpdateResult();
                        ret.status = "False";
                        ret.message = "ไม่พบข้อมูล";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, ret);
                    }
                }
                else {
                    UpdateResult ret = new UpdateResult();
                    ret.status = "False";
                    ret.message = "กรุณากรอก RefNo";
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
        /// อนุมัติ-ไม่อนุมัติการใช้เมล็ดพันธุ์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseProduct/Update")]
        public HttpResponseMessage UpdateSupplierUseProduct()
        {
            try
            {
                string Remark = "";
               
                    Remark = HttpContext.Current.Request.Form["Remark"].ToString();
                
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ


                if (RefNo != "" && Status != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseProduct));
                    List<SendOrderSeed> list = new List<SendOrderSeed>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SupplierUseProduct objSupplierUseProduct = ObjectSpace.FindObject<SupplierUseProduct>(CriteriaOperator.Parse("UseNo=?", _refno));
                    if (objSupplierUseProduct != null)
                    {

                        if (Status == "1")
                        { //อนุมัติ
                            objSupplierUseProduct.Stauts = EnumSupplierUseStatus.Approved; //2
                            objSupplierUseProduct.Remark = Remark;
                            ObjectSpace.CommitChanges();
                        }
                        else if (Status == "2")
                        { //ไม่อนุมัติ
                            objSupplierUseProduct.Stauts = EnumSupplierUseStatus.Eject; //4
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
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
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
        ///// <summary>
        ///// อนุมัติ-ไม่อนุมัติการใช้เสบียงสัตว์
        ///// </summary>
        ///// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimalProduct/Update")]
        public HttpResponseMessage UpdateSupplierUseAnimalProduct()  ///SupplierUseAnimalProduct/Update
        {
            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ


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
                            objSupplierUseProduct.Stauts = EnumRodBreedProductSeedStatus.Approve; //2
                            ObjectSpace.CommitChanges();
                        }
                        else if (Status == "2")
                        { //ไม่อนุมัติ
                            objSupplierUseProduct.Stauts = EnumRodBreedProductSeedStatus.NoApprove; //4

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

    }
}
