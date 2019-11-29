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
using System.Globalization;

namespace WebApi.Jwt.Controllers
{
    public class Approval_SendSeedController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        #region การส่งเมล็ด อนุมัติ ไม่อนุมัติ
       
        /// <summary>
        /// อนุมัติการส่ง =3 เมล็ดพันธุ์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
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
                approve_Success.SendStatus = HttpContext.Current.Request.Form["Sendstatus"].ToString();

                DataSet ds = new DataSet();
          ds=  SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_Moblie_Approval_SendSeed", new SqlParameter("@SendNo", approve_Success.Send_No.ToString())
                    , new SqlParameter("@Remark", approve_Success.Remark)
                    ,new SqlParameter("@SendStatus", approve_Success.SendStatus));

                if (ds.Tables[1].Rows[0]["pStatus"].ToString() == "3")
                {
                    approve_Success.SendStatus = "3";
                    approve_Success.Send_Messengr = "อนุมัติการส่งเมล็ด";
                    
                }
                else if (ds.Tables[1].Rows[0]["pStatus"].ToString() == "4"|| ds.Tables[1].Rows[0]["pMessage"].ToString() == "ไม่อนุมัติข้อมูลการส่ง")
                {
                    approve_Success.SendStatus = "4";
                    approve_Success.Send_Messengr = "ไม่อนุมัติการส่งเมล็ด";
                    return Request.CreateResponse(HttpStatusCode.OK, approve_Success);
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

        /// <summary>
        /// หารายละเอียดการส่งด้วย sendOID
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("SendOrder/{SendNo}")] // ใส่ OIDSendOrderSeed ใบนำส่ง
        public IHttpActionResult SendOrderSeedDetail_ByOrderSeedID()
        {
            object SendNo = string.Empty;
            object ReceiveOrgOid = string.Empty;
            Approve_Model sendDetail = new Approve_Model();

            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                if (HttpContext.Current.Request.Form["SendNo"].ToString() != null)
                {
                    SendNo = HttpContext.Current.Request.Form["SendNo"].ToString();
                }
                if (HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString() != null)
                {
                    ReceiveOrgOid = HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString();
                }
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                List<Approve_Model> list = new List<Approve_Model>();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SendOrderSeed sendOrderSeed;
                sendOrderSeed = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and SendNo=? and ReceiveOrgOid=? ", SendNo, ReceiveOrgOid));
                //sendOrderSeed = ObjectSpace.GetObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", null));
                if (SendNo != null)
                {
                    sendDetail.Send_No = sendOrderSeed.SendNo;
                    sendDetail.SendDate = Convert.ToDateTime(sendOrderSeed.SendDate).ToString();
                    sendDetail.SendOrgOid = sendOrderSeed.SendOrgOid.OrganizeNameTH;
                    sendDetail.ReceiveOrgOid = sendOrderSeed.ReceiveOrgOid.OrganizeNameTH;
                    sendDetail.Remark = sendOrderSeed.Remark;
                    sendDetail.SendStatus = sendOrderSeed.SendStatus.ToString();
                    if (sendOrderSeed.CancelMsg == null)
                    {
                        sendDetail.CancelMsg = "";
                    }
                    else
                    {
                        sendDetail.CancelMsg = sendOrderSeed.CancelMsg;
                    }

                    foreach (SendOrderSeedDetail row in sendOrderSeed.SendOrderSeedDetails)
                    {
                        SendOrderSeed_Model send_Detail = new SendOrderSeed_Model();
                        send_Detail.LotNumber = row.LotNumber.LotNumber;
                        send_Detail.WeightUnitOid = row.WeightUnitOid.UnitName;
                        send_Detail.AnimalSeedCode = row.AnimalSeedCode;
                        send_Detail.AnimalSeedLevel = row.AnimalSeedLevel;
                        send_Detail.AnimalSeeName = row.AnimalSeeName;
                        send_Detail.BudgetSourceOid = row.BudgetSourceOid.BudgetName;
                        send_Detail.Weight = row.Weight;
                        send_Detail.Used = row.Used.ToString();
                        send_Detail.SendOrderSeed = row.SendOrderSeed.SendNo;
                        send_Detail.AnimalSeedOid = row.AnimalSeedOid.SeedName;
                        send_Detail.AnimalSeedLevelOid = row.AnimalSeedLevelOid.SeedLevelName;
                        send_Detail.SeedTypeOid = row.SeedTypeOid.SeedTypeName;
                        send_Detail.Amount = row.Amount;
                        list_detail.Add(send_Detail);
                    }
                    sendDetail.objSeed = list_detail;
                    return Ok(sendDetail);
                }
                else
                {
                    return BadRequest("NoData");
                }
            }

            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// เรียกรายละเอียดการส่ง เบิกจำหน่ายเมล็ดพันธุ์ในหน่วยงาน
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ApprovalSend/SupplierUseProduct")]
        public HttpResponseMessage ApprovalSend_SupplierUseProduct()
        {
            Approve_Model approve_Success = new Approve_Model();
            try
            {
                if (HttpContext.Current.Request.Form["UseNo"].ToString() != null)
                {
                    approve_Success.Send_No = HttpContext.Current.Request.Form["UseNo"].ToString();
                }

                approve_Success.Remark = HttpContext.Current.Request.Form["Remark"].ToString();
                approve_Success.SendStatus = HttpContext.Current.Request.Form["Sendstatus"].ToString();

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_Mobile_Approval_SupplierUseProduct", new SqlParameter("@UseNo", approve_Success.Send_No.ToString())
                          , new SqlParameter("@Remark", approve_Success.Remark)
                          , new SqlParameter("@SendStatus", approve_Success.SendStatus));

                if (ds.Tables[1].Rows[0]["pStatus"].ToString() == "3")
                {
                    approve_Success.SendStatus = "3";
                    approve_Success.Send_Messengr = "อนุมัติการเบิกเมล็ด";

                }
                else if (ds.Tables[1].Rows[0]["pStatus"].ToString() == "4" || ds.Tables[1].Rows[0]["pMessage"].ToString() == "ไม่อนุมัติข้อมูลการส่ง")
                {
                    approve_Success.SendStatus = "4";
                    approve_Success.Send_Messengr = "ไม่อนุมัติ";
                    return Request.CreateResponse(HttpStatusCode.OK, approve_Success);
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
       
        #endregion
        //}
    }
}

   


