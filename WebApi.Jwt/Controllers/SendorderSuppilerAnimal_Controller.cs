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

namespace WebApi.Jwt.Controllers.MasterData
{
    //public class SendorderSuppilerAnimal_Controller : ApiController
    //{
    //    string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
    //    [AllowAnonymous]
    //    [HttpGet]
    //    [Route("LoadSendSeed/accept")]
    //    public IHttpActionResult LoadSendSeed_accept()
    //    { 
    //        object ReceiveOrgOid;
    //        try
    //        {

    //            ReceiveOrgOid = HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString();


    //            XpoTypesInfoHelper.GetXpoTypeInfoSource();
    //            XafTypesInfo.Instance.RegisterEntity(typeof(sendordersup));
    //            XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
    //            List<Approve_Model> list = new List<Approve_Model>();             
    //            List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
    //            XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
    //            IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
    //            IList<SendOrderSupplierAnimal> collection = ObjectSpace.GetObjects<SendOrderSupplierAnimal>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 2 and ReceiveOrgOid=?", ReceiveOrgOid));
    //            if (collection.Count > 0)
    //            {
    //                foreach (SendOrderSupplierAnimal row in collection)
    //                {
    //                    Approve_Model SupplierAnimal = new Approve_Model();
    //                    SupplierAnimal.Send_No = row.;
    //                    SupplierAnimal.SendDate = row.SendDate.ToString();
    //                    SupplierAnimal.FinanceYearOid = row.FinanceYearOid.YearName;
    //                    SupplierAnimal.SendOrgOid = row.SendOrgOid.OrganizeNameTH;
    //                    SupplierAnimal.ReceiveOrgOid = row.ReceiveOrgOid.OrganizeNameTH;
    //                    SupplierAnimal.Remark = row.Remark;
    //                    SupplierAnimal.CancelMsg = row.CancelMsg;
    //                    SupplierAnimal.SendStatus = row.SendStatus.ToString();

    //                    SupplierAnimal.objSeed = list_detail;
    //                    list.Add(SupplierAnimal);
    //                }

    //            }

    //            else
    //            {
    //                UserError err = new UserError();
    //                err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
    //                err.message = "No data";
    //                //  Return resual
    //                return BadRequest();
    //            }
    //            return Ok(list);
    //        }
    //        catch (Exception ex)
    //        { //Error case เกิดข้อผิดพลาด
    //            UserError err = new UserError();
    //            err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
    //            err.message = ex.Message;
    //            //  Return resual
    //            return BadRequest();
    //        }
    //    }
    //}
}
