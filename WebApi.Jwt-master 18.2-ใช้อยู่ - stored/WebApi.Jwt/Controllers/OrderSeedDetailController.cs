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
using nutrition.Module;
using DevExpress.Xpo;

namespace WebApi.Jwt.Controllers
{
    public class OrderSeedDetailController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// หน้ารายละเอียดการส่งแบบระบุเลขที่นำส่ง
        /// </summary>
        /// <param name="SendOrderSeed"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("SendSeed/Order")]
        public IHttpActionResult OrderseedDetail(string SendOrderSeed)
        {
            OrderSeedDetail OrderSeedDetail = new OrderSeedDetail();
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeedDetail));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SendOrderSeedDetail OrderSeed;

                OrderSeed = ObjectSpace.FindObject<SendOrderSeedDetail>(new BinaryOperator("SendOrderSeed", SendOrderSeed));
                if (SendOrderSeed != null)
                {

                    OrderSeedDetail.Oid = OrderSeed.Oid;
                    OrderSeedDetail.LotNumber = OrderSeed.LotNumber.Oid;
                    OrderSeedDetail.WeightUnitOid = OrderSeed.WeightUnitOid.Oid;
                    OrderSeedDetail.AnimalSeedCode = OrderSeed.AnimalSeedCode;
                    OrderSeedDetail.AnimalSeeName = OrderSeed.AnimalSeeName;
                    OrderSeedDetail.AnimalSeedLevel = OrderSeed.AnimalSeedLevel;
                    OrderSeedDetail.BudgetSourceOid = OrderSeed.BudgetSourceOid.Oid;
                    OrderSeedDetail.Weight = OrderSeed.Weight;
                    OrderSeedDetail.Used = OrderSeed.Used;
                    OrderSeedDetail.SendOrderSeed = OrderSeed.SendOrderSeed.Oid;
                    return Ok(OrderSeedDetail);
                }
                {
                    return BadRequest("NoData");
                }

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// หน้ารายละเอียดการส่งเมล็ด
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("SendSeed/Detail_List")]
        public IHttpActionResult OrderseedDetail_List()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeedDetail));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<OrderSeedDetail> list = new List<OrderSeedDetail>();
                IList<SendOrderSeedDetail> collection = ObjectSpace.GetObjects<SendOrderSeedDetail>(CriteriaOperator.Parse("GCRecord is null  "));
                if (collection.Count > 0)
                {

                    foreach (SendOrderSeedDetail row in collection)
                    {
                        OrderSeedDetail OrderSeedDetail = new OrderSeedDetail();
                        OrderSeedDetail.Oid = row.Oid;
                        OrderSeedDetail.LotNumber = row.LotNumber.Oid;
                        OrderSeedDetail.WeightUnitOid = row.WeightUnitOid.Oid;
                        OrderSeedDetail.AnimalSeedCode = row.AnimalSeedCode;
                        OrderSeedDetail.AnimalSeeName = row.AnimalSeeName;
                        OrderSeedDetail.AnimalSeedLevel = row.AnimalSeedLevel;
                        OrderSeedDetail.BudgetSourceOid = row.BudgetSourceOid.Oid;
                        OrderSeedDetail.Weight = row.Weight;
                        OrderSeedDetail.Used = row.Used;
                        OrderSeedDetail.SendOrderSeed = row.SendOrderSeed.Oid;

                        list.Add(OrderSeedDetail);
                    }
                }
                else
                {

                    return BadRequest("NoData");

                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region
        /// <summary>
        /// แสดงรายละเอียดข้อมูลการรับเมล็ด
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("ReceiveOrderSeed")]
        public IHttpActionResult ReceiveOrderSeed()
        {

            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveOrderSeed));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
          
                List<ReceiveOrderSeed_Model> list = new List<ReceiveOrderSeed_Model>();
                IList<ReceiveOrderSeed> collection = ObjectSpace.GetObjects<ReceiveOrderSeed>(CriteriaOperator.Parse("GCRecord is null  "));
                if (collection.Count > 0)
                {
                    foreach (ReceiveOrderSeed row in collection)
                    {
                        ReceiveOrderSeed_Model Model = new ReceiveOrderSeed_Model();
                        Model.ReceiveNo = row.ReceiveNo;
                        Model.SendDate = row.SendDate;
                        //    FinanceYear = ObjectSpace.GetObject<nutrition.Module.FinanceYear>(CriteriaOperator.Parse(nameof"Oid = @FinanceYearOid ", null));
                        Model.FinanceYearOid = row.FinanceYearOid.YearName;
                        Model.SendOrgOid = row.SendOrgOid.OrganizeNameTH;
                        Model.ReceiveOrgOid = row.ReceiveOrgOid.OrganizeNameTH;
                        Model.Remark = row.Remark;
                        Model.SendStatus = (int)row.SendStatus;
                        list.Add(Model);
                    }
                }

                else
                {
                    return BadRequest("NoData");
                }
                return Ok(list);

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
        /// รายละเอียดใบรับเมล็ด
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("ReceiveOrder/SeedDetail")]
        public IHttpActionResult ReceiveSeedDetail()
        {

            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.ReceiveOrderSeedDetail));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
         
                List<ReceiveOrderSeedDetail_Model> list = new List<ReceiveOrderSeedDetail_Model>();
                IList<nutrition.Module.ReceiveOrderSeedDetail> collection = ObjectSpace.GetObjects<nutrition.Module.ReceiveOrderSeedDetail>(CriteriaOperator.Parse("GCRecord is null  "));
                if (collection.Count > 0)
                {

                    foreach (nutrition.Module.ReceiveOrderSeedDetail row in collection)
                    {

                        ReceiveOrderSeedDetail_Model Model = new ReceiveOrderSeedDetail_Model();
                        Model.Oid = row.Oid;
                        Model.LotNumber = row.LotNumber.FactoryName;
                        //    FinanceYear = ObjectSpace.GetObject<nutrition.Module.FinanceYear>(CriteriaOperator.Parse(nameof"Oid = @FinanceYearOid ", null));
                        Model.AnimalSeedCode = row.AnimalSeedCode;
                        Model.AnimalSeedLevel = row.AnimalSeedLevel;
                        Model.AnimalSeeName = row.AnimalSeeName;
                        Model.BudgetSourceOid = row.BudgetSourceOid.BudgetName;
                        Model.Weight = row.Weight;
                        Model.Used = row.Used.ToString();
                        Model.SendNo = row.SendNo;
                        Model.ReceiveOrderSeed = row.ReceiveOrderSeed.SendDate;
                        list.Add(Model);
                    }
                }
                else

                {
                    return BadRequest("NoData");
                }
                return Ok(list);
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
        [AllowAnonymous]
        [HttpGet]
        [Route("ReceiveOrder/SeedDetailID")]
        public IHttpActionResult ReceiveSeedDetail_ByID()
        {
            ReceiveOrderSeedDetail_Model Model = new ReceiveOrderSeedDetail_Model();
            object ReceiveOrderSeed = string.Empty;
            try
            {
                if (HttpContext.Current.Request.Form["ReceiveID"].ToString() != null)
                {
                    ReceiveOrderSeed = HttpContext.Current.Request.Form["ReceiveID"].ToString();
                }
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.ReceiveOrderSeedDetail));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                ReceiveOrderSeedDetail ReceiveOrderSeedDetail;
                ReceiveOrderSeedDetail = ObjectSpace.FindObject<ReceiveOrderSeedDetail>(new BinaryOperator("ReceiveOrderSeed", ReceiveOrderSeed));
                if (ReceiveOrderSeedDetail != null)
                {
                    Model.LotNumber = ReceiveOrderSeedDetail.LotNumber.LotNumber ;
                    Model.AnimalSeedCode = ReceiveOrderSeedDetail.AnimalSeedCode;
                    Model.AnimalSeeName = ReceiveOrderSeedDetail.AnimalSeeName;
                    Model.AnimalSeedLevel = ReceiveOrderSeedDetail.AnimalSeedLevel;
                    Model.BudgetSourceOid = ReceiveOrderSeedDetail.BudgetSourceOid.BudgetName;
                    Model.Weight = ReceiveOrderSeedDetail.Weight;
                    Model.Used = ReceiveOrderSeedDetail.Used.ToString();
                    Model.SendNo = ReceiveOrderSeedDetail.SendNo;
                    Model.ReceiveOrderSeed = ReceiveOrderSeedDetail.ReceiveOrderSeed.Oid;
                    return Ok(Model);

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
        }/// <summary>
        /// หารายละเอียดการส่งด้วย sendOID
        /// </summary>
        /// <returns></returns>
             [AllowAnonymous]
        [HttpGet]
        [Route("SendOrder/SeedDetailID")]
        public IHttpActionResult SendOrderSeedDetail_ByOrderSeedID()
        {
            object SendOrderSeed = string.Empty;

            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                if (HttpContext.Current.Request.Form["SendID"].ToString() != null)
                {
                    SendOrderSeed = HttpContext.Current.Request.Form["SendID"].ToString();
                }
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeedDetail));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SendOrderSeedDetail SendOrderSeedDetail;
                SendOrderSeedDetail = ObjectSpace.FindObject<SendOrderSeedDetail>(new BinaryOperator("SendOrderSeed", SendOrderSeed));
                //  IList<SendOrderSeedDetail> collection = ObjectSpace.GetObject<SendOrderSeedDetail>(CriteriaOperator.Parse(" GCRecord is null and WeightUnitOid=+Oid+", null));
                if (SendOrderSeedDetail != null)
                {
                    Model.LotNumber = SendOrderSeedDetail.LotNumber.LotNumber;
                    Model.WeightUnitOid = SendOrderSeedDetail.WeightUnitOid.UnitName;
                    Model.AnimalSeedCode = SendOrderSeedDetail.AnimalSeedCode;
                    Model.AnimalSeeName = SendOrderSeedDetail.AnimalSeeName;
                    Model.BudgetSourceOid = SendOrderSeedDetail.BudgetSourceOid.BudgetName;
                    Model.Weight = SendOrderSeedDetail.Weight;
                    Model.Used = SendOrderSeedDetail.Used.ToString();
                    Model.SendOrderSeed = SendOrderSeedDetail.SendOrderSeed.SendNo;
                    return Ok(Model);
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
        #endregion

    }

}
