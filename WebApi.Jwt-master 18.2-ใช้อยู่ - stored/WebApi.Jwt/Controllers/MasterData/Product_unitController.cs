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
    public class Product_unitController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// หน่วยวัด
        /// </summary>
        /// <returns></returns>
        //[JwtAuthentication]
        [AllowAnonymous]
        [HttpGet]
        [Route("Product_unit")]
        public HttpResponseMessage loadProduct_unit()
        {
            Unit_Modle unit_model = new Unit_Modle();
            try
            {
                //    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                //    XafTypesInfo.Instance.RegisterEntity(typeof(Unit));
                //    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                //    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                //    Unit unit_type;
                //    unit_type = ObjectSpace.FindObject<Unit>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1"));
                //    if (unit_type != null)
                //    {

                //        unit_model.Oid = unit_type.Oid;
                //        unit_model.UnitCode = unit_type.UnitCode;
                //        unit_model.UnitName = unit_type.UnitName;
                //        return Request.CreateResponse(HttpStatusCode.OK, unit_model);
                //    }
                //    else
                //    {
                //        UserError err = new UserError();
                //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                //        err.message = "No data";
                //        //  Return resual
                //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                //    }
                //}
                //catch (Exception ex)
                //{ //Error case เกิดข้อผิดพลาด
                //    UserError err = new UserError();
                //    err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                //    err.message = ex.Message;
                //    //  Return resual
                //    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                //}
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieGetunit");
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
    


        [AllowAnonymous]
        [HttpGet]
        [Route("Package_Info")]
        public HttpResponseMessage loadPackage_Info() ///ข้อมูลบรรจุภัณท์
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieGetPackage_Info");
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        }
        }

    

