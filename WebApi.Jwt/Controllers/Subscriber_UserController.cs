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
using static WebApi.Jwt.Models.AddSubscriber_User;
using System.Data;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.Security;
using WebApi.Jwt.Filters;
using WebApi.Jwt.helpclass;
using NTi.CommonUtility;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using WebApi.Jwt.Models;
using Newtonsoft.Json.Linq;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using static WebApi.Jwt.Models.Farmerinfo;
using nutrition.Module;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class Subscriber_UserController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
       // /ลงทะเบียนผู้รับบริการ รายใหม่ และอัพเดทข้อมูล
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Register_Subscriber")]
        public HttpResponseMessage Register_Subscriber()
        {
            RegisterSubscriber_User Regi_subscriber = new RegisterSubscriber_User();

            try
            {
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                if (jObject != null)
                {
                    if (jObject.SelectToken("Organization") == null)
                    {
                        Regi_subscriber.OrganizationOid = "ไม่มีข้อมูลศูนย์";
                    }
                    else
                    {
                        Regi_subscriber.OrganizationOid = jObject.SelectToken("Organization").Value<string>();
                    }
                  
                    Regi_subscriber.RegisterDate = jObject.SelectToken("RegisterDate").Value<string>();
                    Regi_subscriber.CitizenID = jObject.SelectToken("CitizenID").Value<string>();
                    Regi_subscriber.TitleOid = jObject.SelectToken("TitleOid").Value<string>();
                    Regi_subscriber.FirstNameTH = jObject.SelectToken("FirstNameTH").Value<string>();
                    Regi_subscriber.LastNameTH = jObject.SelectToken("LastNameTH").Value<string>();
                    Regi_subscriber.Gender = jObject.SelectToken("Gender").Value<string>();
                    Regi_subscriber.BirthDate = jObject.SelectToken("BirthDate").Value<string>();
                    Regi_subscriber.Tel = jObject.SelectToken("Tel").Value<string>();
                    Regi_subscriber.Email = jObject.SelectToken("Email").Value<string>();
                    Regi_subscriber.Address = jObject.SelectToken("Address_No").Value<string>();

                    if (jObject.SelectToken("Moo") == null)
                    {
                        Regi_subscriber.Moo = null;
                    }
                    else
                    {
                        Regi_subscriber.Moo = jObject.SelectToken("Moo").Value<string>();
                    }

                    if (jObject.SelectToken("Soi") == null)
                    {
                        Regi_subscriber.Soi = null;
                    }
                    else
                    {
                        Regi_subscriber.Soi = jObject.SelectToken("Soi").Value<string>();
                    }

                    if (jObject.SelectToken("Road") == null)
                    {
                        Regi_subscriber.Road = null;
                    }
                    else
                    {
                        Regi_subscriber.Road = jObject.SelectToken("Road").Value<string>();
                    }

                    Regi_subscriber.ProvinceOid = jObject.SelectToken("ProvinceOid").Value<string>();
                    Regi_subscriber.DistrictOid = jObject.SelectToken("DistrictOid").Value<string>();
                    Regi_subscriber.SubDistrictOid = jObject.SelectToken("SubDistrictOid").Value<string>();
                    Regi_subscriber.ZipCode = jObject.SelectToken("ZipCode").Value<string>();
                
     



                }

                XPObjectSpaceProvider osProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace objectSpace = osProvider.CreateObjectSpace();
                DataSet ds = new DataSet();
                SqlParameter[] prm = new SqlParameter[19]; /// parameter นับได้เท่าไร ใส่เท่านั้น c#
                prm[0] = new SqlParameter("@OrganizationOid", Regi_subscriber.OrganizationOid); ///แต่ array ต้องนับจาก 0
                prm[1] = new SqlParameter("@RegisterDate", Regi_subscriber.RegisterDate);
                prm[2] = new SqlParameter("@Citizen_ID", Regi_subscriber.CitizenID);
                prm[3] = new SqlParameter("@TitleOid", Regi_subscriber.TitleOid);
                prm[4] = new SqlParameter("@FirstName_TH", Regi_subscriber.FirstNameTH);
                prm[5] = new SqlParameter("@LastName_TH", Regi_subscriber.LastNameTH);
                prm[6] = new SqlParameter("@Gender", Regi_subscriber.Gender);
                prm[7] = new SqlParameter("@Birthdate", Regi_subscriber.BirthDate);
                prm[8] = new SqlParameter("@Tel", Regi_subscriber.Tel);
                prm[9] = new SqlParameter("@Email", Regi_subscriber.Email);      
                prm[10] = new SqlParameter("@Remark", Regi_subscriber.Remark);
                prm[11] = new SqlParameter("@Address_No", Regi_subscriber.Address);
                prm[12] = new SqlParameter("@Address_moo", Regi_subscriber.Moo);
                prm[13] = new SqlParameter("@Address_Soi", Regi_subscriber.Soi);
                prm[14] = new SqlParameter("@Address_Road", Regi_subscriber.Road);
                prm[15] = new SqlParameter("@Address_provinces", Regi_subscriber.ProvinceOid);
                prm[16] = new SqlParameter("@Address_districts", Regi_subscriber.DistrictOid);
                prm[17] = new SqlParameter("@Address_subdistricts", Regi_subscriber.SubDistrictOid);
                prm[18] = new SqlParameter("@ZipCode", Regi_subscriber.ZipCode);
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieRigisterUser_Service", prm);
                if (ds.Tables[0].Rows[0]["pStatus"].ToString() != "0" || ds.Tables[0].Rows[0]["pStatus"].ToString() == "2")
                {

                    var subscriber_User = new Farmer_Status();
                    subscriber_User.Status = "1";
                    subscriber_User.Message = "บันทึกข้อมูลผู้ขอรับบริการ เรียบร้อยแล้ว";
                    return Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);


                }
                else
                {

                    UserError err = new UserError();

                    err.code = "2";
                    err.message = "ผิดพลาด กรอกข้อมูลไม่ครบ";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    // return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "naughty");
                }
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// ลงทะเบียนขอรับบริการ
        /// </summary>
        /// <returns></returns>
        /*   [AllowAnonymous]
           [HttpPost]
           [Route("Register_OrgeService")]
           public HttpResponseMessage RegisterCustomer()
           {
               democlass democlass = new democlass();
               RegisterCustomer customer = new RegisterCustomer();
               try
               {
                   string requestString = Request.Content.ReadAsStringAsync().Result;
                   JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                   if (jObject != null)
                   {
                       customer.DateTime = jObject.SelectToken("DateTime").Value<DateTime>();
                       customer.CustomerTypeOid = jObject.SelectToken("Service_info").Value<string>();
                       customer.Get_ServiceUser_Name = jObject.SelectToken("Name").Value<string>();
                       customer.Organization_ServiceName = jObject.SelectToken("Organization_Service").Value<string>();
                       customer.Address = jObject.SelectToken("Address").Value<string>();
                       if (jObject.SelectToken("Remark") == null)
                       {
                           customer.Remark = string.Empty;
                       }
                       else
                       {
                           customer.Remark = jObject.SelectToken("Remark").Value<string>();
                       }
                   }

                   DataSet ds = new DataSet();
                   ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieRegisterCustomer",
                       new SqlParameter("@datetime", customer.DateTime)
                       , new SqlParameter("@CustomerTypeOid,", customer.CustomerTypeOid)
                       , new SqlParameter("@CustomerOid", customer.Get_ServiceUser_Name)
                       , new SqlParameter("@OrgeServiceID", customer.Organization_ServiceName)
                       , new SqlParameter("@Address", customer.Address)
                       , new SqlParameter("@Remark", customer.Remark));

                   if (ds.Tables.Count == 0)
                   {

                       return Request.CreateResponse(HttpStatusCode.OK, "ลงทะเบียนสำเร็จ");
                   }
                   return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่สามารถลงทะเบียนได้");
               }


               catch (Exception ex)
               {
                   // Error case เกิดข้อผิดพลาด
                   UserError err = new UserError();
                   err.status = "ผิดพลาด";
                   err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                   err.message = ex.Message;
                   //     Return resual
                   return Request.CreateResponse(HttpStatusCode.BadRequest, err);
               }
           }===============================*/
        /// <summary>
        // /แก้ไขข้อมูลผู้รับบริการ 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Subscriber/Edit")]
        public HttpResponseMessage Edit_Subscriber()
        {
            RegisterSubscriber_User Update_subscriber = new RegisterSubscriber_User();

            try
            {
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                if (jObject != null)
                {

                    Update_subscriber.OrganizationOid = jObject.SelectToken("Organization").Value<string>();


                    Update_subscriber.RegisterDate = jObject.SelectToken("RegisterDate").Value<string>();
                    Update_subscriber.CitizenID = jObject.SelectToken("CitizenID").Value<string>();
                    Update_subscriber.TitleOid = jObject.SelectToken("TitleOid").Value<string>();
                    Update_subscriber.FirstNameTH = jObject.SelectToken("FirstNameTH").Value<string>();
                    Update_subscriber.LastNameTH = jObject.SelectToken("LastNameTH").Value<string>();
                    Update_subscriber.Gender = jObject.SelectToken("Gender").Value<string>();
                    Update_subscriber.BirthDate = jObject.SelectToken("BirthDate").Value<string>();
                    Update_subscriber.Tel = jObject.SelectToken("Tel").Value<string>();
                    Update_subscriber.Email = jObject.SelectToken("Email").Value<string>();
                    Update_subscriber.Address = jObject.SelectToken("Address_No").Value<string>();

                    Update_subscriber.Moo = jObject.SelectToken("Moo").Value<string>();
                    Update_subscriber.Soi = jObject.SelectToken("Soi").Value<string>();
                    Update_subscriber.Road = jObject.SelectToken("Road").Value<string>();
                    Update_subscriber.ProvinceOid = jObject.SelectToken("ProvinceOid").Value<string>();
                    Update_subscriber.DistrictOid = jObject.SelectToken("DistrictOid").Value<string>();
                    Update_subscriber.SubDistrictOid = jObject.SelectToken("SubDistrictOid").Value<string>();
                    Update_subscriber.ZipCode = jObject.SelectToken("ZipCode").Value<string>();

                }

                XPObjectSpaceProvider osProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace objectSpace = osProvider.CreateObjectSpace();
                DataSet ds = new DataSet();
                SqlParameter[] prm = new SqlParameter[19]; /// parameter นับได้เท่าไร ใส่เท่านั้น c#
                prm[0] = new SqlParameter("@OrganizationOid", Update_subscriber.OrganizationOid); ///แต่ array ต้องนับจาก 0
                prm[1] = new SqlParameter("@RegisterDate", Update_subscriber.RegisterDate);
                prm[2] = new SqlParameter("@Citizen_ID", Update_subscriber.CitizenID);
                prm[3] = new SqlParameter("@TitleOid", Update_subscriber.TitleOid);
                prm[4] = new SqlParameter("@FirstName_TH", Update_subscriber.FirstNameTH);
                prm[5] = new SqlParameter("@LastName_TH", Update_subscriber.LastNameTH);
                prm[6] = new SqlParameter("@Gender", Update_subscriber.Gender);
                prm[7] = new SqlParameter("@Birthdate", Update_subscriber.BirthDate);
                prm[8] = new SqlParameter("@Tel", Update_subscriber.Tel);
                prm[9] = new SqlParameter("@Email", Update_subscriber.Email);
                prm[10] = new SqlParameter("@Remark", Update_subscriber.Remark);
                prm[11] = new SqlParameter("@Address_No", Update_subscriber.Address);
                prm[12] = new SqlParameter("@Address_moo", Update_subscriber.Moo);
                prm[13] = new SqlParameter("@Address_Soi", Update_subscriber.Soi);
                prm[14] = new SqlParameter("@Address_Road", Update_subscriber.Road);
                prm[15] = new SqlParameter("@Address_provinces", Update_subscriber.ProvinceOid);
                prm[16] = new SqlParameter("@Address_districts", Update_subscriber.DistrictOid);
                prm[17] = new SqlParameter("@Address_subdistricts", Update_subscriber.SubDistrictOid);
                prm[18] = new SqlParameter("@ZipCode", Update_subscriber.ZipCode);
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieRigisterUser_Service", prm);
                if (ds.Tables[0].Rows[0]["pStatus"].ToString() != "0" || ds.Tables[0].Rows[0]["pStatus"].ToString() == "2")
                {

                    var subscriber_User = new Farmer_Status();
                    subscriber_User.Status = "1";
                    subscriber_User.Message = "บันทึกข้อมูลผู้ขอรับบริการ เรียบร้อยแล้ว";
                    return Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
                }
                else
                {

                    UserError err = new UserError();

                    err.code = "2";
                    err.message = "ผิดพลาด กรอกข้อมูลไม่ครบ";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    // return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "naughty");
                }
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("GetCusService")]  
        public HttpResponseMessage GetCusService()
        {
            List<FarmerCitizen> farmerCitizenList = new List<FarmerCitizen>();
            FarmerCitizen farmer_info = new FarmerCitizen();

            try
            {
                string CitizenID = string.Empty;
                if (HttpContext.Current.Request.Form["CitizenID"].ToString() != null)
                {
                    CitizenID = HttpContext.Current.Request.Form["CitizenID"].ToString();
                }
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileGetCusService", new SqlParameter("@CitizenID", CitizenID));
                if (ds.Tables[0].Rows.Count > 0)
                {



                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, rows);
                }
                else if (ds.Tables[0].Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่พบบัตรประจำตัวประชาชน");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
                }
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Register_OrgeService")]
        public HttpResponseMessage RigisteOrgeService()
        {
            democlass democlass = new democlass();
            RigisteOrgeService OrgeService = new RigisteOrgeService();
            try
            {
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                if (jObject != null)
                {
                    if (jObject.SelectToken("OrganizationOid") == null)
                    {
                        OrgeService.OrganizationOid = string.Empty;
                    }
                    else
                    {
                        OrgeService.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    }
                }
                OrgeService.OrgeServiceID = jObject.SelectToken("OrgeServiceID").Value<string>();
                OrgeService.OrgeServiceName = jObject.SelectToken("OrgeServiceName").Value<string>();
                OrgeService.Email = jObject.SelectToken("Email").Value<string>();
                OrgeService.Tel = jObject.SelectToken("Tel").Value<string>();
                OrgeService.Address = jObject.SelectToken("Address").Value<string>();
                if (jObject.SelectToken("Moo") == null)
                {
                    OrgeService.Moo = null;
                }
                else
                {
                    OrgeService.Moo = jObject.SelectToken("Moo").Value<string>();
                }

                if (jObject.SelectToken("Soi") == null)
                {
                    OrgeService.Soi = null;
                }
                else
                {
                    OrgeService.Soi = jObject.SelectToken("Soi").Value<string>();
                }

                if (jObject.SelectToken("Road") == null)
                {
                    OrgeService.Road = null ;
                }
                else
                {
                    OrgeService.Road = jObject.SelectToken("Road").Value<string>();
                }

                OrgeService.Province = jObject.SelectToken("Province").Value<string>();
                OrgeService.District = jObject.SelectToken("District").Value<string>();
                OrgeService.SubDistrict = jObject.SelectToken("SubDistrict ").Value<string>();
                OrgeService.Zipcode = jObject.SelectToken("Zipcode").Value<string>();


                DataSet ds = new DataSet();
                SqlParameter[] prm = new SqlParameter[13]; /// parameter นับได้เท่าไร ใส่เท่านั้น c#
                prm[0] = new SqlParameter("@OrganizationOid", OrgeService.OrganizationOid); ///แต่ array ต้องนับจาก 0
                prm[1] = new SqlParameter("@OrgeServiceID", OrgeService.OrgeServiceID);
                prm[2] = new SqlParameter("@OrgeServiceName", OrgeService.OrgeServiceName);
                prm[3] = new SqlParameter("@Tel", OrgeService.Tel);
                prm[4] = new SqlParameter("@Email", OrgeService.Email);
                prm[5] = new SqlParameter("@Address_No", OrgeService.Address);
                prm[6] = new SqlParameter("@Address_moo", OrgeService.Moo);
                prm[7] = new SqlParameter("@Address_Soi", OrgeService.Soi);
                prm[8] = new SqlParameter("@Address_Road", OrgeService.Road);
                prm[9] = new SqlParameter("@Address_provinces", OrgeService.Province);
                prm[10] = new SqlParameter("@Address_districts", OrgeService.District);
                prm[11] = new SqlParameter("@Address_subdistricts", OrgeService.SubDistrict);
                prm[12] = new SqlParameter("@ZipCode", OrgeService.Zipcode);
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieRigisteOrgeService", prm);
                if (ds.Tables[0].Rows[0]["pStatus"].ToString() != "0" || ds.Tables[0].Rows[0]["pStatus"].ToString() == "2")
                {

                    var subscriber_User = new Farmer_Status();
                    subscriber_User.Status = "1";
                    subscriber_User.Message = "บันทึกข้อมูลผู้ขอรับบริการ เรียบร้อยแล้ว";
                    return Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);


                }
                else
                {

                    UserError err = new UserError();

                    err.code = "2";
                    err.message = "ผิดพลาด กรอกข้อมูลไม่ครบ";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    // return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "naughty");
                }

            }
            catch (Exception ex)
            {
                // Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //     Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("GetOrgeService")]
        public HttpResponseMessage GetOrgeService()
        {
            List<FarmerCitizen> farmerCitizenList = new List<FarmerCitizen>();
            FarmerCitizen farmer_info = new FarmerCitizen();

            try
            {
                string CitizenID = string.Empty;
                if (HttpContext.Current.Request.Form["CitizenID"].ToString() != null)
                {
                    CitizenID = HttpContext.Current.Request.Form["CitizenID"].ToString();
                }
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileGetCusService", new SqlParameter("@CitizenID", CitizenID));
                if (ds.Tables[0].Rows.Count > 0)
                {



                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, rows);
                }
                else if (ds.Tables[0].Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่พบบัตรประจำตัวประชาชน");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
                }
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }





        #region รอปรับแก้

        //[AllowAnonymous]
        //[HttpPost]
        [Route("Register_CustomerXAF")]
        public IHttpActionResult RegisterCustomerXAF()
        {
            string TempService_ = string.Empty;
            RegisterSubscriber_User Regi_subscriber = new RegisterSubscriber_User();

            try
            {

                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                if (jObject != null)
                {
                    if (jObject.SelectToken("Organization") == null)
                    {
                        Regi_subscriber.OrganizationOid = "ไม่มีข้อมูลศูนย์";
                    }
                    else
                    {
                        Regi_subscriber.OrganizationOid = jObject.SelectToken("Organization").Value<string>();
                    }

                    Regi_subscriber.RegisterDate = jObject.SelectToken("RegisterDate").Value<string>();
                    Regi_subscriber.CitizenID = jObject.SelectToken("CitizenID").Value<string>();
                    Regi_subscriber.TitleOid = jObject.SelectToken("TitleOid").Value<string>();
                    Regi_subscriber.FirstNameTH = jObject.SelectToken("FirstNameTH").Value<string>();
                    Regi_subscriber.LastNameTH = jObject.SelectToken("LastNameTH").Value<string>();
                    Regi_subscriber.Gender = jObject.SelectToken("Gender").Value<string>();
                    Regi_subscriber.BirthDate = jObject.SelectToken("BirthDate").Value<string>();
                    //Regi_subscriber.Tel = jObject.SelectToken("Tel").Value<string>();
                    //Regi_subscriber.Email = jObject.SelectToken("Email").Value<string>();
                    //Regi_subscriber.DisPlayName = jObject.SelectToken("DisPlayName").Value<string>();
                    //if (jObject.SelectToken("Moo") == null)
                    //{
                    //    Regi_subscriber.Moo = string.Empty;
                    //}
                    //else
                    //{
                    //    Regi_subscriber.Moo = jObject.SelectToken("Moo").Value<string>();
                    //}

                    //if (jObject.SelectToken("Soi") == null)
                    //{
                    //    Regi_subscriber.Soi = string.Empty;
                    //}
                    //else
                    //{
                    //    Regi_subscriber.Soi = jObject.SelectToken("Soi").Value<string>();
                    //}

                    //if (jObject.SelectToken("Road") == null)
                    //{
                    //    Regi_subscriber.Road = string.Empty;
                    //}
                    //else
                    //{
                    //    Regi_subscriber.Road = jObject.SelectToken("Road").Value<string>();
                    //}

                    Regi_subscriber.ProvinceOid = jObject.SelectToken("ProvinceOid").Value<string>();
                    Regi_subscriber.DistrictOid = jObject.SelectToken("DistrictOid").Value<string>();
                    Regi_subscriber.SubDistrictOid = jObject.SelectToken("SubDistrictOid").Value<string>();
                    Regi_subscriber.ZipCode = jObject.SelectToken("ZipCode").Value<string>();
                    //Regi_subscriber.FullAddress = jObject.SelectToken("FullAddress").Value<string>();




                }

                
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XPObjectSpaceProvider osProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace objectSpace = osProvider.CreateObjectSpace();
                nutrition.Module.RegisterCusService Regi_subscriber_;

                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.RegisterCusService));
                Regi_subscriber_ = objectSpace.CreateObject<RegisterCusService>();
                Regi_subscriber.OrganizationOid = "F97EF626-FDB7-4361-A9F3-1C9D14FAC27E";



                objectSpace.CommitChanges();

                return Ok(Regi_subscriber);
                {
                    return BadRequest();
                }
            }

            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //   Return resual
                return BadRequest(ex.Message);
            }

        }
        #endregion
    }

}


