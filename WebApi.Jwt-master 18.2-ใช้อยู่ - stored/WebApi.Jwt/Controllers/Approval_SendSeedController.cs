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
using nutrition.Module;

namespace WebApi.Jwt.Controllers
{
    public class Approval_SendSeedController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        #region การส่งเมล็ด อนุมัติ ไม่อนุมัติ
       
        /// <summary>
        /// อนุมัติการส่ง =1
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPatch]
        [Route("SendSeed/ApprovalSend")]
        public HttpResponseMessage ApprovalSend()
        {
            Approve_Model approve_Success = new Approve_Model();
            try
            {
                if (HttpContext.Current.Request.Form["Send_No"].ToString() != null)
                {
                    approve_Success.Send_No = HttpContext.Current.Request.Form["Send_No"].ToString();
                }

                approve_Success.Remark = HttpContext.Current.Request.Form["Remark"].ToString();


                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_Moblie_Approval_SendSeed", new SqlParameter("@SendNo", approve_Success.Send_No)
                    , new SqlParameter("@Remark", approve_Success.Remark));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    approve_Success.SendStatus = "1";
                    approve_Success.Send_Messengr = "อนุมัติการส่งเมล็ด";
                }

                return Request.CreateResponse(HttpStatusCode.OK, approve_Success);
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
        /// ไม่อนุมัติการส่ง +เหตุผล=2
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPatch]
        [Route("SendSeed/Not_ApprovalSend")]
        public HttpResponseMessage Not_ApprovalSend()
        {
            Approve_Model Not_approve = new Approve_Model();
            try
            {
                if (HttpContext.Current.Request.Form["Send_No"].ToString() != null)
                {
                    Not_approve.Send_No = HttpContext.Current.Request.Form["Send_No"].ToString();
                }
                if (HttpContext.Current.Request.Form["Remark"].ToString() != null)
                {
                    Not_approve.Remark = HttpContext.Current.Request.Form["Remark"].ToString();
                }

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_Moblie_NotApproval_SendSeed", new SqlParameter("@SendNo", Not_approve.Send_No)
                    , new SqlParameter("@Remark", Not_approve.Remark));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Not_approve.SendStatus = "1";
                    Not_approve.Send_Messengr = "ไม่อนุมัติการส่งเมล็ด โปรดตรวจสอบใหม่";
                }

                return Request.CreateResponse(HttpStatusCode.OK, Not_approve);
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);

            }
            #endregion การส่งเมล็ด อนุมัติ ไม่อนุมัติ
        }

        #region แบบใช้ฟังค์ชั่นของ xaf 
        [AllowAnonymous]
        [HttpGet]
        [Route("LoadSendSeed")]
        public IHttpActionResult LoadSendSeed()
        {
            try
            { XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<Approve_Model> list = new List<Approve_Model>();
                IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse(" GCRecord is null", null));

                if (collection.Count > 0)
                {
            
                    foreach (SendOrderSeed  row in collection)
                    {
                        Approve_Model Approve = new Approve_Model();
                        Approve.Send_No = row.SendNo;
                        Approve.SendDate = row.SendDate.ToString();
                        Approve.FinanceYearOid = row.FinanceYearOid.YearName;
                        Approve.SendOrgOid = row.SendOrgOid.OrganizeNameTH;
                        Approve.ReceiveOrgOid = row.ReceiveOrgOid.OrganizeNameTH;
                        Approve.Remark = row.Remark;
                        Approve.SendStatus = row.SendStatus.ToString();
                        list.Add(Approve);
                    }
                }
             
                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest();
                }
                return Ok(list);
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest();
            }

        }
        /// <summary>
        /// เรียกรายละเอียดการส่ง
        /// </summary>
        /// <returns></returns>
       
 
        #endregion
        //}
    }
}

   


