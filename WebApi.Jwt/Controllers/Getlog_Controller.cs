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
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WebApi.Jwt.Controllers
{
    public class Getlog_Controller : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        
        // [JwtAuthentication]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetLog")]
        public HttpResponseMessage GetLog()
        {
            try
            {

                DataSet ds = new DataSet();

                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Get_LogEvent");
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        row = new Dictionary<string, object>();
                //        foreach (DataColumn col in dt.Columns)
                //        {
                //            row.Add(col.ColumnName, dr[col]);
                //        }
                //        rows.Add(row);
                //    }
                //    return Request.CreateResponse(HttpStatusCode.OK, rows);

                //}
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ToString() == "No")
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        else if (col.ToString() == "EventName")
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                    }
                    rows.Add(row);
                }
                return Request.CreateResponse(HttpStatusCode.OK, rows);

            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        /// <summary>
        /// เรียกรายชื่อเกษตรกร จากเลขบัตรประชาชนทั้งหมด
        /// </summary>
        ///
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("FarmerCitizenID/all")]
        public HttpResponseMessage get_AllCitizenID()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileGetFarmer_ALL"); 
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
        /// เรียกรายชื่อเกษตรกร จากเลขบัตรประชาชน 1 คน
        /// </summary>
        /// <param name="citizenID"></param>
        /// <returns></returns>
        /// 
       






        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpGet]
        [Route("Organization")]
        public HttpResponseMessage get_Organization()
        {
            try
            {

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_GetOrganization");
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
                if (rows != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, rows);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No Data");
            

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

    }
}
        
    








