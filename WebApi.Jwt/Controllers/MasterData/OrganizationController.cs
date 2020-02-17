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
using nutrition.Module;
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class OrganizationController : ApiController

    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();


        /// <summary>
        /// 
        /// </summary>
        /// 
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("Organization_info")]
        public HttpResponseMessage get_Organization( )
        {
            try
            {
                string Oid = null;
                Oid = HttpContext.Current.Request.Form["Oid"];
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_GetOrganization", new SqlParameter("@Oid", Oid));
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
                return Request.CreateResponse(HttpStatusCode.OK);
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
        [Route("getDLDArea/List")]
        public HttpResponseMessage getDLDarea()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Organization));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                ///รอ debug
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                
                List<DLDArea_Model> list = new List<DLDArea_Model>();
                IList<nutrition.Module.Organization> collection = ObjectSpace.GetObjects<nutrition.Module.Organization>(CriteriaOperator.Parse(" IsActive = 1  and [OrganizeNameTH] like 'เขต%' ", null));
                {
                    var query = from Q in collection orderby Q.OrganizeNameTH select Q;
                    if (collection.Count > 0)
                        
                    {
                        
                        foreach (nutrition.Module.Organization row in query)
                        {
                            DLDArea_Model Item = new DLDArea_Model();
                            Item.DLDAreaOid = row.Oid.ToString();
                            Item.DLDAreaName = row.OrganizeNameTH;
                            Item.IsActive = row.IsActive;

                            list.Add(Item);

                        }

                    }
                    else
                    {          //invalid
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "0";
                        err.message = "NoData";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
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


        //[AllowAnonymous]
        //// [JwtAuthentication]
        //[HttpPost]
        //[Route("Organization_info")]
        //public  HttpResponseMessage getquotaOrganization()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

        //        err.message = ex.Message;
        //        //  Return resual
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }


        //}
    }
}