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
using WebApi.Jwt.Models.Models_Masters;
using WebApi.Jwt.Models.ภัยภิบัติ;
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
                            objSupplierUseProduct.Status = EnumSupplierUseStatus.Approve; //4

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
                            objSupplierUseProduct.Status = EnumSupplierUseStatus.Approve; //4
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
                    prm[9] = new SqlParameter("@CitizenID", productUser.CitizenID);


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
        /// <summary>
        /// เจนเลข oid ใบลงทะเบียน
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("getOid/SupplierUseAnimalProduct")]
        public HttpResponseMessage GetList_cataclysm()
            {
            GenOidcalamity genOid = new GenOidcalamity();
            string OrganizationOid = HttpContext.Current.Request.Form["Organizationoid"].ToString();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "GenOidSupplierUseAnimalProduc",new SqlParameter("@OrganizationOid", OrganizationOid));
                DataTable Dt = ds.Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow obj in Dt.Rows)

                        genOid.SupplierUseAnimalProductOid = obj["Oid"].ToString();
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                return Request.CreateResponse(HttpStatusCode.OK, genOid);
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
            return_OidSupplierUseAnimalProductOid item = new return_OidSupplierUseAnimalProductOid();
            SupplierProductUser_Model2 productUser = new SupplierProductUser_Model2();
            int? Type = 0;


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
                    if (jObject.SelectToken("ActivityNameOid") != null)
                    {
                        productUser.SubActivityOid = jObject.SelectToken("ActivityNameOid").Value<string>();
                    }

                    productUser.SupplierUseAnimalProductOid = jObject.SelectToken("supplierUseAnimalProductOid").Value<string>();
                    productUser.UseNo = "";
                    productUser.UseDate = oDate.Year + "-" + oDate.Month + "-" + oDate.Day;
                    productUser.FinanceYearOid = jObject.SelectToken("FinanceYearOid").Value<string>();
                    productUser.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    productUser.Remark = jObject.SelectToken("Remark").Value<string>();
                    productUser.ActivityNameOid = "b100c7c1-4755-4af0-812e-3dd6ba372d45";
                    productUser.CitizenID = jObject.SelectToken("CitizenID").Value<string>();
                    productUser.YearName = jObject.SelectToken("FinanceYear").Value<string>(); ///ใช้สำหรับเจนเลข ออโต้
                   
                    if (jObject.SelectToken("SubActivityLevelOid") != null)
                    { productUser.SubActivityLevelName = jObject.SelectToken("SubActivityLevelOid").Value<string>(); }

                    if (jObject.SelectToken("RegisCusServiceOid") != null)
                    {
                        productUser.RegisCusServiceOid = jObject.SelectToken("RegisCusServiceOid").Value<string>();
                    }
                    if (jObject.SelectToken("OrgeServiceOid") != null)
                    {
                        productUser.OrgeServiceOid = jObject.SelectToken("OrgeServiceOid").Value<string>();
                    }

                    productUser.ServiceCount = jObject.SelectToken("ServiceCount").Value<int>();

                    productUser.PickUp_Type = jObject.SelectToken("PickUpType").Value<string>();
                    if (jObject.SelectToken("FullName") != null)
                    {
                        productUser.ReceiverName = jObject.SelectToken("FullName").Value<string>();
                    }

                    if (jObject.SelectToken("ReceiverNumber") != null)
                    {
                        productUser.ReceiverNumber = jObject.SelectToken("ReceiverNumber").Value<string>();
                    }
                    if (jObject.SelectToken("ReceiverRemark") != null)
                    {
                        productUser.ReceiverRemark = jObject.SelectToken("ReceiverRemark").Value<string>();
                    }
                    productUser.ReceiverAddress = jObject.SelectToken("FullAddress").Value<string>();
                    //if (jObject.SelectToken("Type") != null || jObject.SelectToken("type") != null)
                    //{
                      Type= jObject.SelectToken("type").Value<int>();
                   
                   // }
                  



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
                        RunNumber runningNumber = ObjectSpace.FindObject<RunNumber>(CriteriaOperator.Parse("FormType ='nutrition' and BudgetYear =? and OrgCode=? ", productUser.YearName, objORG.OrganizationCode));
                        if (runningNumber != null)
                        {
                            string customerNumberFormat = string.Empty;
                            int numberfix = Convert.ToInt32(runningNumber.LastNumber) + 1;
                            string Postfix = "000" + numberfix;
                            var FullNumber = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-" + (runningNumber.LastNumber + 1).ToString().PadLeft(6, '0');

                            productUser.UseNo = FullNumber;
                            SqlParameter[] prm2 = new SqlParameter[5];

                            prm2[0] = new SqlParameter("@orgcode", objORG.OrganizationCode);
                            prm2[1] = new SqlParameter("@BudgetYear", productUser.YearName);
                            prm2[2] = new SqlParameter("@LastNumber", numberfix);
                            prm2[3] = new SqlParameter("@FormType", "nutrition");
                            prm2[4] = new SqlParameter("@Type", 1);

                            SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Insert_RuningNumber", prm2);

                        }
                        else
                        {
                            DataSet ds2;
                            SqlParameter[] prm2 = new SqlParameter[5];

                            prm2[0] = new SqlParameter("@orgcode", objORG.OrganizationCode);
                            prm2[1] = new SqlParameter("@BudgetYear", productUser.YearName);
                            prm2[2] = new SqlParameter("@LastNumber", 1);
                            prm2[3] = new SqlParameter("@FormType", "nutrition");
                            prm2[4] = new SqlParameter("@Type", 0);

                            ds2 = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Insert_RuningNumber", prm2);


                            productUser.UseNo = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-000001";
                        }
                    }
                    if (productUser.PickUp_Type == "เป็นตัวแทนรับ")
                    {
                        productUser.PickUp_Type = "2";
                    }
                    else
                    {
                        productUser.PickUp_Type = "1";
                    }
                    DataSet ds;
                    SqlParameter[] prm = new SqlParameter[17];
                    prm[0] = new SqlParameter("@UseNo", productUser.UseNo);
                    prm[1] = new SqlParameter("@UseDate", productUser.UseDate);
                    prm[2] = new SqlParameter("@YearName", productUser.YearName);
                    prm[3] = new SqlParameter("@OrganizationOid", productUser.OrganizationOid);
                    prm[4] = new SqlParameter("@Remark", productUser.Remark);
                    prm[5] = new SqlParameter("@ActivityOid", productUser.ActivityNameOid);
                    prm[6] = new SqlParameter("@RegisCusServiceOid", productUser.RegisCusServiceOid);
                    prm[7] = new SqlParameter("@OrgeServiceOid", productUser.OrgeServiceOid);
                   // prm[8] = new SqlParameter("@ServiceCount", productUser.ServiceCount);
                    prm[8] = new SqlParameter("@CitizenID", productUser.CitizenID);
                    prm[9] = new SqlParameter("@SubActivityOid", productUser.SubActivityOid);
                    prm[10] = new SqlParameter("@SubActivityLevelOid", productUser.SubActivityLevelName);
                    prm[11] = new SqlParameter("@PickUp_Type", productUser.PickUp_Type);
                    prm[12] = new SqlParameter("@oid", productUser.SupplierUseAnimalProductOid);
                    prm[13] = new SqlParameter("@ReceiverName", productUser.ReceiverName);
                    prm[14] = new SqlParameter("@ReceiverAddress", productUser.ReceiverAddress);
                    prm[15] = new SqlParameter("@ReceiverNumber", productUser.ReceiverNumber);
                    prm[16] = new SqlParameter("@ReceiverRemark", productUser.ReceiverRemark);

                    ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieInserts_Calamity_SupplierUseAnimalProduct_Update", prm);
                    DataTable dt = new DataTable();
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseAnimalProduct));
                    XPObjectSpaceProvider directProvider2 = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace2 = directProvider2.CreateObjectSpace();
                    SupplierUseAnimalProduct objSupplierUseAnimalProduct = ObjectSpace2.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null  and Oid=?  ", productUser.SupplierUseAnimalProductOid));

                    if (Type == 2)// เซฟข้อมูลตามปกติ
              {
                       objSupplierUseAnimalProduct.Status = EnumRodBreedProductSeedStatus.Accepet; //1

                       ObjectSpace2.CommitChanges();

                        UpdateResult ret = new UpdateResult();
                       ret.status = "true";
                        ret.message = "ยืนยันการส่งให้ ผอ.อนุมัติเรียบร้อยแล้ว";
                       return Request.CreateResponse(HttpStatusCode.OK, ret);


                    }
                    else
                   {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //productUser.SupplierUseAnimalProductOid ;
                            item.supplieruseanimalproductoid = ds.Tables[1].Rows[0]["oid"].ToString();
                            item.useno = ds.Tables[1].Rows[0]["UseNo"].ToString();
                            productUser.UseNo = productUser.UseNo;
                            //return Request.CreateResponse(HttpStatusCode.OK);
                            return Request.CreateResponse(HttpStatusCode.OK, item);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, productUser);
                        }

                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "กรอกข้อมูลไม่ครบ");
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
        #region ไม่ใช้

        [AllowAnonymous]
        [HttpPost]
        [Route("UpdateCalamity/SupplierUseAnimalProduct")]
        public HttpResponseMessage Update_Calamity_SupplierUseAnimalProduct()
        {
            return_OidSupplierUseAnimalProductOid item = new return_OidSupplierUseAnimalProductOid();
            SupplierProductUser_Model2 productUser = new SupplierProductUser_Model2();
            inserts_suppile inserts = new inserts_suppile();
            try
            {
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                string TempForageType = string.Empty;
                if (jObject != null)
                {
                    string iDate = jObject.SelectToken("UseDate").Value<string>();
                    DateTime oDate = Convert.ToDateTime(iDate);
                    if (jObject.SelectToken("SubActivityOid") != null)
                    {
                        productUser.SubActivityOid = jObject.SelectToken("SubActivityOid").Value<string>();
                    }
                    //       productUser.SupplierUseAnimalProductOid = jObject.SelectToken("supplieruseanimalproductOid").Value<string>();
                    productUser.UseNo = "";
                    productUser.UseDate = oDate.Year + "-" + oDate.Month + "-" + oDate.Day;
                    productUser.FinanceYearOid = jObject.SelectToken("FinanceYearOid").Value<string>();
                    productUser.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    productUser.Remark = jObject.SelectToken("Remark").Value<string>();
                    productUser.ActivityNameOid = jObject.SelectToken("ActivityNameOid").Value<string>();
                    productUser.CitizenID = jObject.SelectToken("CitizenID").Value<string>();
                    productUser.YearName = jObject.SelectToken("FinanceYear").Value<string>(); ///ใช้สำหรับเจนเลข ออโต้

                    if (jObject.SelectToken("SubActivityLevelOid") != null)
                    { productUser.SubActivityLevelName = jObject.SelectToken("SubActivityLevelOid").Value<string>(); }

                    if (jObject.SelectToken("RegisCusServiceOid") != null)
                    {
                        productUser.RegisCusServiceOid = jObject.SelectToken("RegisCusServiceOid").Value<string>();
                    }
                    if (jObject.SelectToken("OrgeServiceOid") != null)
                    {
                        productUser.OrgeServiceOid = jObject.SelectToken("OrgeServiceOid").Value<string>();
                    }

                    productUser.ServiceCount = jObject.SelectToken("ServiceCount").Value<int>();

                    productUser.PickUp_Type = jObject.SelectToken("PickUpType").Value<string>();

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
                    //inserts.SupplierUseAnimalProductOid = jObject.SelectToken("supplieruseanimalproductoid").Value<string>();
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
                            int numberfix = Convert.ToInt32(runningNumber.LastNumber) + 1;
                            string Postfix = "000" + numberfix;
                            var FullNumber = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-" + (runningNumber.LastNumber + 1).ToString().PadLeft(6, '0');

                            productUser.UseNo = FullNumber;
                            SqlParameter[] prm2 = new SqlParameter[5];

                            prm2[0] = new SqlParameter("@orgcode", objORG.OrganizationCode);
                            prm2[1] = new SqlParameter("@BudgetYear", productUser.YearName);
                            prm2[2] = new SqlParameter("@LastNumber", numberfix);
                            prm2[3] = new SqlParameter("@FormType", "UseProduct");
                            prm2[4] = new SqlParameter("@Type", 1);

                            SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Insert_RuningNumber", prm2);

                        }

                        else
                        {
                            DataSet ds2;
                            SqlParameter[] prm2 = new SqlParameter[5];

                            prm2[0] = new SqlParameter("@orgcode", objORG.OrganizationCode);
                            prm2[1] = new SqlParameter("@BudgetYear", productUser.YearName);
                            prm2[2] = new SqlParameter("@LastNumber", 1);
                            prm2[3] = new SqlParameter("@FormType", "UseProduct");
                            prm2[4] = new SqlParameter("@Type", 0);

                            ds2 = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Insert_RuningNumber", prm2);


                            productUser.UseNo = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-000001";
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
                        DataSet ds;
                        SqlParameter[] prm = new SqlParameter[27];
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
                        prm[10] = new SqlParameter("@SubActivityOid", productUser.SubActivityOid);
                        prm[11] = new SqlParameter("@SubActivityLevelOid", productUser.SubActivityLevelName);
                        prm[12] = new SqlParameter("@PickUp_Type", productUser.PickUp_Type);
                        //  prm[13] = new SqlParameter("@oid", productUser.SupplierUseAnimalProductOid);
                        prm[13] = new SqlParameter("@AnimalSupplieOid", inserts.AnimalSupplieOid);
                        prm[14] = new SqlParameter("@StockLimit", listQuantity2.balanceQTY); // ใช้ weight อย่างเดียว
                        prm[15] = new SqlParameter("@Weight", inserts.Weight);
                        prm[16] = new SqlParameter("@WeightUnitOid", inserts.WeightUnitOid);
                        //  prm[18] = new SqlParameter("@SupplierUseAnimalProductOid", inserts.SupplierUseAnimalProductOid);
                        prm[17] = new SqlParameter("@AnimalSupplieTypeOid", inserts.AnimalSupplieTypeOid);
                        prm[18] = new SqlParameter("@ManageSubAnimalSupplierOid", listQuantity2.ManageSubAnimalSupplierOid);
                        prm[19] = new SqlParameter("@QuotaQTY", listQuantity2.QuotaQTY);
                        prm[20] = new SqlParameter("@StockUsed", listQuantity2.stockuse);
                        prm[21] = new SqlParameter("@Amount", listQuantity2.stockuse);
                        prm[22] = new SqlParameter("@QuotaTypeOid", inserts.QuotaTypeOid);
                        prm[23] = new SqlParameter("@SeedTypeOid", inserts.SeedTypeOid);
                        prm[24] = new SqlParameter("@PackageOid", inserts.PackageOid);
                        prm[25] = new SqlParameter("@PerUnit", inserts.PerUnit);
                        prm[26] = new SqlParameter("@BudgetSourceOid", inserts.BudgetSourceOid);

                        ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieUpdate_Calamity_SupplierUseAnimalProduct2", prm);
                        DataTable dt = new DataTable();

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //productUser.SupplierUseAnimalProductOid ;

                            item.supplieruseanimalproductoid = ds.Tables[0].Rows[0]["Oid"].ToString();
                            productUser.UseNo = productUser.UseNo;
                            //return Request.CreateResponse(HttpStatusCode.OK);
                            return Request.CreateResponse(HttpStatusCode.OK, item);
                        }
                        else
                        {

                            return Request.CreateResponse(HttpStatusCode.BadRequest, "ลงทะเบียนไม่สำเร็จ");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "ข้อมูลไม่ครบ");
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
#endregion

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
        /// อนุมัติ-ไม่อนุมัติการช่วยเหลือภัยพิบัติ เสบียงสัตว์ ฟั่งชั่นของปุ่มในหน้าช่วยเหลือภัยพิบัติ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ApprovalDisasterSupplierUseAnimalProduct/Update")]/// ใช้สร้างใหม่
        public HttpResponseMessage UpdateDisasterSupplierUseAnimalProduct()  ///SupplierUseAnimalProduct/Update
        {
            string Remark = "";
            _Registerfarmer Registerfarmer = new _Registerfarmer();
            try
            {
                string SupplierUseAnimalProductOid = HttpContext.Current.Request.Form["supplierUseAnimalProductOid"].ToString();
                
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                if (HttpContext.Current.Request.Form["Remark"] != null)
                {
                     Remark = HttpContext.Current.Request.Form["Remark"].ToString();
                    //หมายเหตุ
                }
                

                if (SupplierUseAnimalProductOid != "" && Status != "" )
                {


                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseAnimalProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseProductDetail));

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SupplierUseAnimalProduct objSupplierUseAnimalProduct = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null  and Oid=?  ", SupplierUseAnimalProductOid));
                    //SupplierUseProductDetail objSupplierUseProductDetail;
                    SupplierUseProductDetail objSupplierUseProductDetail    = ObjectSpace.FindObject<SupplierUseProductDetail>(CriteriaOperator.Parse(" GCRecord is null   ", null));

                    ///    spt_UpdateOidSupplierUseProductDetail
                    if (objSupplierUseAnimalProduct.Oid != null)
                    {
  
                        if (Status == "0") //บันทึก
                        { //รอการตรวจสอบ                    
                            
                            //objSupplierUseProductDetail.SupplierUseProduct = ;
                            objSupplierUseAnimalProduct.Status = EnumRodBreedProductSeedStatus.Accepet; //0
                            if (Remark != "")
                            {
                                objSupplierUseAnimalProduct.Remark = Remark;
                            }
                            ObjectSpace.CommitChanges();

                            UpdateResult Accept = new UpdateResult();
                            Accept.status = "true";
                            Accept.message = "บันทึกข้อมูลรออนุมัติเสร็จเรียบร้อยแล้ว";
                            return Request.CreateResponse(HttpStatusCode.OK, Accept);
                        }
                        else if (Status == "1")
                        { // อนุมัติให้ ผ.อ.
                            
                            objSupplierUseAnimalProduct.Status = EnumRodBreedProductSeedStatus.Accepet; //1
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
                string CancelMsg = "";
                string Remark = "";
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                if (CancelMsg != null)
                {
                    CancelMsg = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                }



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

                    SupplierUseAnimalProduct objSupplierUseAnimalProduct = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse("UseNo=?  ", _refno));

                    if (objSupplierUseAnimalProduct != null)
                    {

                        if (Status == "1")
                        {
                            objSupplierUseAnimalProduct.Status = EnumRodBreedProductSeedStatus.Approve;//2
                            objSupplierUseAnimalProduct.Remark = CancelMsg;
                            ObjectSpace.CommitChanges();
                        }
                        else
                        {
                            objSupplierUseAnimalProduct.Status = EnumRodBreedProductSeedStatus.Eject;//4
                            objSupplierUseAnimalProduct.CancelMsg = CancelMsg;
                            ObjectSpace.CommitChanges();
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
                else
                {
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
        #region ตัดสต้อค
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
                string CancelMsg = HttpContext.Current.Request.Form["Remark"].ToString();
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
                            objSupplierUseProduct.Status = EnumSupplierUseStatus.Approve; //2
                            objSupplierUseProduct.Remark = CancelMsg;
                            ObjectSpace.CommitChanges();
                        }
                        else if (Status == "2")
                        { //ไม่อนุมัติ
                            objSupplierUseProduct.Status = EnumSupplierUseStatus.Eject; //4
                                                                                        //if( Remark != " ")
                                                                                        // {
                            objSupplierUseProduct.Remark = CancelMsg;
                            //}

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
                string CancelMsg = "";
                CancelMsg = HttpContext.Current.Request.Form["CancelMsg"].ToString();
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
                            objSupplierUseProduct.Status = EnumRodBreedProductSeedStatus.Approve; //2
                            objSupplierUseProduct.Remark = CancelMsg.ToString();
                            ObjectSpace.CommitChanges();
                        }
                        else if (Status == "2")
                        { //ไม่อนุมัติ
                            objSupplierUseProduct.Status = EnumRodBreedProductSeedStatus.Eject; //4
                            objSupplierUseProduct.Remark = CancelMsg.ToString();
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
        #endregion ตัดสต็อค
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimalProductDetail/List")] //SupplierUseProductDetail
        public HttpResponseMessage SupplierUseAnimalProduct_Detail()  ///SupplierUseAnimalProduct/Update
        {
            _Registerfarmer Registerfarmer = new _Registerfarmer();
            try
            {

                //string SupplierUseAnimalProductOid = HttpContext.Current.Request.Form["supplieruseanimalproductoid"].ToString(); //ข้อมูลเลขที่อ้างอิง

                //    string org_oid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                string org_oid = "";
                string  type = "";
                org_oid = HttpContext.Current.Request.Form["OrganizationOid"];
                type = HttpContext.Current.Request.Form["type"];

                //การแจกจ่าย=1/การจำหน่าย=2/ภัยพิบัติ=3
                if (org_oid != ""  && type != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));

                    List<SupplierProductUser> UseACT1 = new List<SupplierProductUser>();
                    List<SupplierProductUser> UseACT2 = new List<SupplierProductUser>();
                    List<SupplierAnimalUsecalarity_Model> UseACT3 = new List<SupplierAnimalUsecalarity_Model>();
                    List<SupplierProductUser> UseACT4 = new List<SupplierProductUser>();
                    SupplierAnimalUsecalarity_Model lists = new SupplierAnimalUsecalarity_Model();
                    lists.SupplierUseAnimalProductOid = org_oid;

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();              
                    if (org_oid != null)
                    {
                        //การอนุมัติภัยพิบัติ
                        Activity ActivityOid = ObjectSpace.FindObject<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1  ", null));
        
                         ActivityOid.ActivityName = "เพื่อช่วยเหลือภัยพิบัติ";
                    
                        string SubActivityOid = "";
                        if (SubActivityOid != "58d27cfd - 6f9c - 4e1f - adbc - 4bec48bce531")
                        {
                            SubActivityOid = "2d040ef1-23c3-4c1c-9e59-0d3e2e7ef712";
                        }
                        else
                        {
                            SubActivityOid = "86e8c106 - a176 - 441f - a7e0 - b911e487641f";
                        }

                        IList<SupplierUseAnimalProduct> collection3 = ObjectSpace.GetObjects<SupplierUseAnimalProduct>(CriteriaOperator.Parse("GCRecord is null and Status  in (0,1) and OrganizationOid='" + org_oid + "' and [ActivityOid.ActivityName] = '" + ActivityOid.ActivityName + "'  ", null));
                        if (collection3 != null)
                        {
                            foreach (SupplierUseAnimalProduct row in collection3)
                            {

                                SupplierAnimalUsecalarity_Model Supplier_3 = new SupplierAnimalUsecalarity_Model();
                                Supplier_3.TypeMoblie = type;
                                Supplier_3.SupplierUseAnimalProductOid = row.Oid.ToString();
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
                        else
                        {
                            UpdateResult ret = new UpdateResult();
                            ret.status = "False";
                            ret.message = "ไม่พบข้อมูล";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, ret);
                        }
                    }
                    else
                    {
                        UpdateResult ret = new UpdateResult();
                        ret.status = "False";
                        ret.message = "ไม่พบข้อมูล";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, ret);
                    }
                }
                else
                {
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


    }
}
