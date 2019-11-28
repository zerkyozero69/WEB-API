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
       // /
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
                    Regi_subscriber.DisPlayName = jObject.SelectToken("DisPlayName").Value<string>();
                    if (jObject.SelectToken("Moo") == null)
                    {
                        Regi_subscriber.Moo = string.Empty;
                    }
                    else
                    {
                        Regi_subscriber.Moo = jObject.SelectToken("Moo").Value<string>();
                    }

                    if (jObject.SelectToken("Soi") == null)
                    {
                        Regi_subscriber.Soi = string.Empty;
                    }
                    else
                    {
                        Regi_subscriber.Soi = jObject.SelectToken("Soi").Value<string>();
                    }

                    if (jObject.SelectToken("Road") == null)
                    {
                        Regi_subscriber.Road = string.Empty;
                    }
                    else
                    {
                        Regi_subscriber.Road = jObject.SelectToken("Road").Value<string>();
                    }

                    Regi_subscriber.ProvinceOid = jObject.SelectToken("ProvinceOid").Value<string>();
                    Regi_subscriber.DistrictOid = jObject.SelectToken("DistrictOid").Value<string>();
                    Regi_subscriber.SubDistrictOid = jObject.SelectToken("SubDistrictOid").Value<string>();
                    Regi_subscriber.ZipCode = jObject.SelectToken("ZipCode").Value<string>();
                    Regi_subscriber.FullAddress = jObject.SelectToken("FullAddress").Value<string>();
     



                }

                XPObjectSpaceProvider osProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace objectSpace = osProvider.CreateObjectSpace();
                DataSet ds = new DataSet();
                SqlParameter[] prm = new SqlParameter[12]; /// parameter นับได้เท่าไร ใส่เท่านั้น c#
                prm[0] = new SqlParameter("@Organization", Regi_subscriber.OrganizationOid); ///แต่ array ต้องนับจาก 0
                prm[1] = new SqlParameter("@Tel", Regi_subscriber.RegisterDate);
                prm[2] = new SqlParameter("@Email", Regi_subscriber.CitizenID);
                prm[3] = new SqlParameter("@Address_No", Regi_subscriber.TitleOid);
                prm[4] = new SqlParameter("@Address_moo", Regi_subscriber.FirstNameTH);
                prm[5] = new SqlParameter("@Address_Soi", Regi_subscriber.LastNameTH);
                prm[6] = new SqlParameter("@Address_Road", Regi_subscriber.Gender);
                prm[7] = new SqlParameter("@Address_provinces", Regi_subscriber.BirthDate);
                prm[8] = new SqlParameter("@Address_districts", Regi_subscriber.Tel);
                prm[9] = new SqlParameter("@Address_subdistricts", Regi_subscriber.Email);      
                prm[10] = new SqlParameter("@OrgeServiceName", Regi_subscriber.Moo);
                prm[11] = new SqlParameter("@OrgeServiceName", Regi_subscriber.Soi);
                prm[12] = new SqlParameter("@OrgeServiceName", Regi_subscriber.Road);
                prm[13] = new SqlParameter("@OrgeServiceName", Regi_subscriber.ProvinceOid);
                prm[14] = new SqlParameter("@OrgeServiceName", Regi_subscriber.DistrictOid);
                prm[15] = new SqlParameter("@OrgeServiceName", Regi_subscriber.SubDistrictOid);
                prm[16] = new SqlParameter("@ZipCode", Regi_subscriber.ZipCode);
                prm[17] = new SqlParameter("@OrgeServiceName", Regi_subscriber.FullAddress);
                prm[18] = new SqlParameter("@OrgeServiceName", Regi_subscriber.CusTomerName);
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieRigisterUser_Service", prm);
                if (ds.Tables[0].Rows[0]["pStatus"].ToString() != "0" || ds.Tables[0].Rows[0]["pStatus"].ToString() == "2")
                {

                    var subscriber_User = new Farmer_Status();
                    subscriber_User.Status = "1";
                    subscriber_User.Message = "บันทึกข้อมูลผู้ขอรับบริการ เรียบร้อยแล้ว";
                    return Request.CreateResponse(HttpStatusCode.OK, subscriber_User);


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
        [AllowAnonymous]
        [HttpPost]
        [Route("Register_Customer")]
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


