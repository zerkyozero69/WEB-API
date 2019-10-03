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
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using DevExpress.Utils.Extensions;

namespace WebApi.Jwt.Controllers
{
    public class Get_RolesController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// เช็ค User ในระบบ
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("Get_roles")]
        public HttpResponseMessage Get_roles (string Username ) ///ใส่หรือไม่ใส่ username ก็ได้ ทำงานได้ 2 แบบ แสดงชื่อทั้งหมดกับตาม user ที่ใส่
            {
                get_role_byuser objUser_info = new get_role_byuser();
            try

            {
                //DataSet ds;
                //ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieGetRoles_ByUser", new SqlParameter("@Username", Username)); ///อย่าลืมเปลี่ยน คอนเนคชั่นสติง
                //DataTable dt = new DataTable();
                //dt = ds.Tables[0];
                helpController result = new helpController();
                objUser_info = result.get_Roles(Username);

                if (objUser_info.Status != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, objUser_info);
                }
                {
                    objUser_info.Status = 2;
                    objUser_info.Message = "ใส่ Username ผิด";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, objUser_info);
                }


            }

            //else {
            //    objUser_info.status = "0";
            //}
            //return  Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่เจอ User");

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
