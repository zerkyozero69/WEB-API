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
using Organization = nutrition.Module.Organization;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class OrganizationController : ApiController

    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();


        /// <summary>
        /// เลือกหน่วยงาน
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
               
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
        /// ค้นหา DLD Zone 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("getDLDArea/List")]
        public HttpResponseMessage getDLDarea()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(DLDArea));
                XafTypesInfo.Instance.RegisterEntity(typeof(Organization));
                List<listdetail> DLD = new List<listdetail>();



                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);

                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                //IList<Organization> collection = ObjectSpace.GetObjects<Organization>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and OrganizeNameTH LIKE 'เขต%'", null));
                IList<Organization> collection = ObjectSpace.GetObjects<Organization>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and OrganizeNameTH like 'เขต%'", null));

                var query = from Q in collection orderby Q.OrganizeNameTH select Q;
                {
                    foreach (Organization row in query)
                    {
                        listdetail listsa = new listdetail();
                        listsa.OId = row.Oid.ToString();
                        listsa.DLDName = row.OrganizeNameTH;

                        IList<Organization> collection2 = ObjectSpace.GetObjects<Organization>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and MasterOrganization ='" + row.Oid + "' ", null));
                        List<listDLD> listDLDs = new List<listDLD>();
                        foreach (Organization row2 in collection2)
                        {
                            listDLD item = new listDLD();
                            item.ORGOid = row2.Oid.ToString();
                            item.OrganizationName = row2.OrganizeNameTH;
                            listDLDs.Add(item);
                        }
                        listsa.Detail = listDLDs;
                        DLD.Add(listsa);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, DLD);

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