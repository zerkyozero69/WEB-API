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
using static WebApi.Jwt.Models.MasterData;
using System.Data;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.Security;
using WebApi.Jwt.Models;
using WebApi.Jwt.Filters;
using WebApi.Jwt.helpclass;
using NTi.CommonUtility;
using System.IO;
using nutrition.Module;
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
#region จังหวัด อำเภอ ตำบล
{/// <summary>
 /// ใช้เรียกจังหวัด อำเภอ ตำบล
 /// </summary>
    public class ProvinceController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();


        [AllowAnonymous]
        [HttpPost]

        [Route("Province")]
        public HttpResponseMessage loadProvince() /// get จังหวัด
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieLoadProvinces");
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
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
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, err);
            }
        }
    
        [AllowAnonymous]
        [HttpPost]
        [Route("Districts")]
        public HttpResponseMessage getDistricts_ByProvince() ///โหลดอำเภอ by จังหวัด
        {
            try
            {
                string Oid = null; // Oid จังหวัด

                if (HttpContext.Current.Request.Form["Oid"].ToString() != null)
                {
                    if (HttpContext.Current.Request.Form["Oid"].ToString() != "")
                    {
                        Oid = HttpContext.Current.Request.Form["Oid"].ToString();
                    }
                }
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieGetDistricts_ByProvince", new SqlParameter("@Oid", Oid)
                   );

                _Districts districts = new _Districts();
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
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
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("SubDistricts")]
        public HttpResponseMessage getSubDistricts_ByDistricts() ///โหลดตำบล by อำเภอ
        {
            try
            {
                string Oid = null; // Oid อำเภอ

                if (HttpContext.Current.Request.Form["Oid"].ToString() != null)
                {
                    if (HttpContext.Current.Request.Form["Oid"].ToString() != "")
                    {
                        Oid = HttpContext.Current.Request.Form["Oid"].ToString();
                    }
                }

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieGetSubDistricts_ByDistricts", new SqlParameter("@Oid", Oid)
                   );

                _Districts districts = new _Districts();
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
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
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
    }
    #endregion
}
