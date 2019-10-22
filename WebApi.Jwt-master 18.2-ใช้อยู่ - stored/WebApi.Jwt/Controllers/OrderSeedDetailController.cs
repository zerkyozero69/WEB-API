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
                    return BadRequest();
                }
               
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
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
                IList<SendOrderSeedDetail> collection = ObjectSpace.GetObjects<SendOrderSeedDetail>(CriteriaOperator.Parse("GCRecord is null and IsActive=1 ") );
                if (collection.Count > 0)
                {
                    List<OrderSeedDetail> list = new List<OrderSeedDetail>();
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

                    return Ok(list);
                }
                else
                {

                    return BadRequest("NoData");

                }
            }
                     catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
