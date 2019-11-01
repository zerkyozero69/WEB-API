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
            RegisterSubscriber_User subscriber = new RegisterSubscriber_User();

            try
            {
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                if (jObject != null)
                {
                    subscriber.OrgeServiceID = jObject.SelectToken("OrgeServiceID").Value<string>();
                    subscriber.OrgeServiceName = jObject.SelectToken("OrgeServiceName").Value<string>();
                    subscriber.Tel = jObject.SelectToken("Tel").Value<string>();
                    subscriber.E_Mail = jObject.SelectToken("Email").Value<string>();
                    subscriber.Address_No = jObject.SelectToken("Address").Value<string>();
                    if (jObject.SelectToken("Moo") == null)
                    {
                        subscriber.Moo = string.Empty;
                    }
                    else
                    {
                        subscriber.Moo = jObject.SelectToken("Moo").Value<string>();
                    }

                    if (jObject.SelectToken("Soi") == null)
                    {
                        subscriber.Soi = string.Empty;
                    }
                    else
                    {
                        subscriber.Soi = jObject.SelectToken("Soi").Value<string>();
                    }

                    if (jObject.SelectToken("Road") == null)
                    {
                        subscriber.Road = string.Empty;
                    }
                    else
                    {
                        subscriber.Road = jObject.SelectToken("Road").Value<string>();
                    }

                    subscriber.ProvinceNameTH = jObject.SelectToken("ProvinceOid").Value<string>();
                    subscriber.DistrictNameTH = jObject.SelectToken("DistrictOid").Value<string>();
                    subscriber.SubDistrictNameTH = jObject.SelectToken("SubDistrictOid").Value<string>();
                    subscriber.ZipCode = jObject.SelectToken("ZipCode").Value<string>();


                }

                XPObjectSpaceProvider osProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace objectSpace = osProvider.CreateObjectSpace();
                DataSet ds = new DataSet();
                SqlParameter[] prm = new SqlParameter[12]; /// parameter นับได้เท่าไร ใส่เท่านั้น c#
                prm[0] = new SqlParameter("@OrgeServiceID", subscriber.OrgeServiceID); ///แต่ array ต้องนับจาก 0
                prm[1] = new SqlParameter("@Tel", subscriber.OrgeServiceName);
                prm[2] = new SqlParameter("@Email", subscriber.E_Mail);
                prm[3] = new SqlParameter("@Address_No", subscriber.Address_No);
                prm[4] = new SqlParameter("@Address_moo", subscriber.Moo);
                prm[5] = new SqlParameter("@Address_Soi", subscriber.Soi);
                prm[6] = new SqlParameter("@Address_Road", subscriber.Road);
                prm[7] = new SqlParameter("@Address_provinces", subscriber.ProvinceNameTH);
                prm[8] = new SqlParameter("@Address_districts", subscriber.SubDistrictNameTH);
                prm[9] = new SqlParameter("@Address_subdistricts", subscriber.SubDistrictNameTH);
                prm[10] = new SqlParameter("@ZipCode", subscriber.ZipCode);
                prm[11] = new SqlParameter("@OrgeServiceName", subscriber.OrgeServiceName);
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieRigisterUser_Service", prm);
                if (ds.Tables[0].Rows[0]["pStatus"].ToString() != "0" || ds.Tables[0].Rows[0]["pStatus"].ToString() == "2")
                {

                    var subscriber_User = new Farmer_Status();
                    subscriber.Status = 1;
                    subscriber_User.Message = "บันทึกข้อมูลผู้ขอรับบริการ เรียบร้อยแล้ว";
                    return Request.CreateResponse(HttpStatusCode.OK, subscriber);


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
        //[Route("Register_CustomerXAF")]
        //public IHttpActionResult RegisterCustomerXAF()
        //{
        //    string TempService_ = string.Empty;
        //    RegisterCustomer customer = new RegisterCustomer();

        //    try
        //    {

        //        string requestString = Request.Content.ReadAsStringAsync().Result;
        //        JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
        //        if (jObject != null)
        //        {
        //            customer.DateTime = jObject.SelectToken("DateTime").Value<DateTime>();
        //            customer.Service_info = jObject.SelectToken("Service_info").Value<object>();

        //            JArray Service_info = (JArray)jObject_Service_info["Service_info"];

        //            customer.Get_ServiceUser_Name = jObject.SelectToken("User_Name").Value<string>();
        //            customer.Organization_ServiceName = jObject.SelectToken("Organization_Service").Value<string>();
        //            customer.Address = jObject.SelectToken("Address").Value<string>();
        //            customer.CustomerTypeOid = jObject.SelectToken("CustomerType").Value<object>();
        //            if (jObject.SelectToken("Remark") == null)
        //            {
        //                customer.Remark = string.Empty;
        //            }
        //            else
        //            {
        //                customer.Remark = jObject.SelectToken("Remark").Value<string>();
        //            }
        //        }



        //        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //        XPObjectSpaceProvider osProvider = new XPObjectSpaceProvider(scc, null);
        //        IObjectSpace objectSpace = osProvider.CreateObjectSpace();
        //        nutrition.Module.RegisterCustomer _Customer;

        //        XafTypesInfo.Instance.RegisterEntity(typeof(RegisterCustomer));
        //        _Customer = objectSpace.CreateObject<nutrition.Module.RegisterCustomer>();
        //        _Customer.RegisterDate = Convert.ToDateTime(10 - 25 - 2000);
        //        _Customer.CustomerTypeOid = objectSpace.FindObject<nutrition.Module.CustomerType>
        //                                                        (new BinaryOperator("Oid", "F3A44910-5FE9-496A-9B66-368432ED16AD"));

        //        _Customer.CustomerTypeOid = objectSpace.FindObject<nutrition.Module.CustomerType>
        //                                                        (new BinaryOperator("Oid", customer.CustomerTypeOid));
        //        _Customer.OrgeServiceOid = objectSpace.FindObject<nutrition.Module.OrgeService>
        //                                                         (new BinaryOperator("Oid", customer.Organization_ServiceName));

        //        _Customer.CustomerOid = objectSpace.FindObject<nutrition.Module.Farmer>
        //                                                                  (new BinaryOperator("Oid", customer.Get_ServiceUser_Name));

        //        _Customer.Address = customer.Address;
        //        _Customer.Remark = customer.Remark;

        //        objectSpace.CommitChanges();

        //        return Ok(_Customer);
        //        {
        //            return BadRequest();
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.status = "ผิดพลาด";
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

        //        err.message = ex.Message;
        //        //   Return resual
        //        return BadRequest(ex.Message);
        //    }

        //}
        #endregion
    }

}


