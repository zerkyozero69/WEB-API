using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using Microsoft.ApplicationBlocks.Data;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.นับจำนวนกิจกรรม;

namespace WebApi.Jwt.Controllers.API_นับรายการ_ที่ให้ผ.อ._อนุมัติ
{

    public class Count_RequestController : ApiController
    {

        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// นับจำนวนกิจกรรม ใช้เสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierAnimalUseProduct/Count")]
        public HttpResponseMessage CountSupplierUseAnimalProduct()  ///SupplierUseAnimalProduct/Update
        {
            try
            {

                string OrganizeOid = HttpContext.Current.Request.Form["OrganizeOid"].ToString();
                DataSet ds;
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "SP_GetStatusName", new SqlParameter("@OrganizationOid", OrganizeOid));
                List<Titile_Group> titile_Groups = new List<Titile_Group>();
                Titile_Group Group_ = new Titile_Group();
                List<Status_count> status = new List<Status_count>();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string Temp_Group_Name = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Temp_Group_Name == dr["GroupName"].ToString())
                        {
                            Status_count item = new Status_count();
                            item.ActivityName = dr["StatusName"].ToString();
                            item.CountActivityName = dr["CountStatus"].ToString();
                            //status.Add(item);
                            Group_.Status_List.Add(item);
                        }
                        else
                        {
                            Temp_Group_Name = dr["GroupName"].ToString();
                            Group_ = new Titile_Group();
                            Group_.GroupName = dr["GroupName"].ToString();
                            Status_count item = new Status_count();
                            item.ActivityName = dr["StatusName"].ToString();
                            item.CountActivityName = dr["CountStatus"].ToString();
                            //status.Add(item);
                            //Group_.Status_List = status;
                            Group_.Status_List.Add(item);
                            titile_Groups.Add(Group_);
                        }
                    }
                    UserError err = new UserError();
                    err.code = ""; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "OK";
                    return Request.CreateResponse(HttpStatusCode.OK, titile_Groups);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = ""; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "Nodata";
                    return Request.CreateResponse(HttpStatusCode.OK, err);
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





