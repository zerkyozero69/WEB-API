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

namespace WebApi.Jwt.Controllers.MasterData
{
    public class Subscriber_UserController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// พักไว้ก่อน
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Register/Subscriber")]
        public HttpResponseMessage Register_Subscriber()
        {
            try
            {
                string Username = "";
                string Datetime = "";
                string Subscriber_Type = "";
                string Subscriber_User = "";
                string Subscriber_Government = "";
                string Address = "";
                string Note = "";

                if (HttpContext.Current.Request.Form["Username"].ToString() != null)
                {
                    Username = HttpContext.Current.Request.Form["Username"].ToString();
                }
                if (HttpContext.Current.Request.Form["Datetime"].ToString() != null)
                {
                    Datetime = HttpContext.Current.Request.Form["Datetime"].ToString();
                }
                if (HttpContext.Current.Request.Form["Subscriber_Type"].ToString() != null)
                {
                    Subscriber_Type = HttpContext.Current.Request.Form["Subscriber_Type"].ToString();
                }
                if (HttpContext.Current.Request.Form["Subscriber_User"].ToString() != null)
                {
                    Subscriber_User = HttpContext.Current.Request.Form["Subscriber_User"].ToString();
                }
                if (HttpContext.Current.Request.Form["Subscriber_Government"].ToString() != null)
                {
                    Subscriber_Government = HttpContext.Current.Request.Form["Subscriber_Government"].ToString();
                }
                if (HttpContext.Current.Request.Form["Address"].ToString() != null)
                {
                    Address = HttpContext.Current.Request.Form["Address"].ToString();
                }
                if (HttpContext.Current.Request.Form["Note"].ToString() != null)
                {
                    Note = HttpContext.Current.Request.Form["Note"].ToString();
                }
               
                var ds = SqlHelper.ExecuteNonQuery(scc, CommandType.StoredProcedure, "spt_MoblieRegisterSubscribe", new SqlParameter("@Username ", Username)
                     , new SqlParameter("@Datetime", Datetime)
                     , new SqlParameter("@Subscriber_Type", Subscriber_Type)
                     , new SqlParameter("@Subscriber_User", Subscriber_User)
                     , new SqlParameter("@Subscriber_Government", Subscriber_Government)
                     , new SqlParameter("@Address", Address)
                     , new SqlParameter("@Note", Note));
           
                {
                    if (ds.ToString() == "1")
                    {
                        AddSubscriber_User userRet = new AddSubscriber_User();

                        userRet.code = "1";
                        userRet.Message = "ลงทะเบียนของรับบริการสำเร็จ";
                        return Request.CreateResponse(HttpStatusCode.OK, userRet);

                    }


                    {

                        UserError err = new UserError();
                        err.code = "2";
                        err.message = "กรอกข้อมูลไม่ครบ";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                        // return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "naughty");
                    }
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
        [Route("Update/Subscriber")]
        public HttpResponseMessage Update_Subscriber()
        {
            try
            {
                string Username = "";
                string Datetime = "";
                string Subscriber_Type = "";
                string Subscriber_User = "";
                string Subscriber_Government = "";
                string Address = "";
                string Note = "";

                if (HttpContext.Current.Request.Form["Username"].ToString() != null)
                {
                    Username = HttpContext.Current.Request.Form["Username"].ToString();
                }
                if (HttpContext.Current.Request.Form["Datetime"].ToString() != null)
                {
                    Datetime = HttpContext.Current.Request.Form["Datetime"].ToString();
                }
                if (HttpContext.Current.Request.Form["Subscriber_Type"].ToString() != null)
                {
                    Subscriber_Type = HttpContext.Current.Request.Form["Subscriber_Type"].ToString();
                }
                if (HttpContext.Current.Request.Form["Subscriber_User"].ToString() != null)
                {
                    Subscriber_User = HttpContext.Current.Request.Form["Subscriber_User"].ToString();
                }
                if (HttpContext.Current.Request.Form["Subscriber_Government"].ToString() != null)
                {
                    Subscriber_Government = HttpContext.Current.Request.Form["Subscriber_Government"].ToString();
                }
                if (HttpContext.Current.Request.Form["Address"].ToString() != null)
                {
                    Address = HttpContext.Current.Request.Form["Address"].ToString();
                }
                if (HttpContext.Current.Request.Form["Note"].ToString() != null)
                {
                    Note = HttpContext.Current.Request.Form["Note"].ToString();
                }
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileUpdateSubscribe",
                    new SqlParameter("@Datetime", Datetime)
                    , new SqlParameter("@Subscriber_Type", Subscriber_Type)
                    , new SqlParameter("@Subscriber_User", Subscriber_User)
                    , new SqlParameter("@Subscriber_Government", Subscriber_Government)
                    , new SqlParameter("@Address", Address)
                    , new SqlParameter("@Note", Note)
                    ,new SqlParameter ("@Username", Username));
                if (ds.Tables.Count ==0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,"แก้ไขข้อมูลสำเร็จแล้ว");
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "แก้ไขไม่สำเร็จ");
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            
        }
    }
}
