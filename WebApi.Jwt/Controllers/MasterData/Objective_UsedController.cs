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
using static WebApi.Jwt.Models.MasterData;
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
    public class Objective_UsedController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        [HttpGet]
        [Route("Objective_Used")]
        public HttpResponseMessage loadObjective_Used ()
        {
        try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.ObjectType));
                List<Objective_Used_Model> list = new List<Objective_Used_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<ObjectType> collection = ObjectSpace.GetObjects<ObjectType>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1", null));
                foreach (ObjectType row in collection)
                {
                    Objective_Used_Model model = new Objective_Used_Model();
                    model.ObjectTypeName = row.ObjectTypeName;
                    model.IsActive = row.IsActive;
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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
        [Route("Objective_Usedinfo")]
        public HttpResponseMessage get_ProductionObjective()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.ProductionObjective));
                List<ProductionObjective_Model> list = new List<ProductionObjective_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<ProductionObjective> collection = ObjectSpace.GetObjects<ProductionObjective>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1", null));
                foreach (ProductionObjective row in collection)
                {
                    ProductionObjective_Model model = new ProductionObjective_Model();
                    model.ProductObjectiveName = row.ProductObjectiveName;
                    model.IsActive = row.IsActive;
                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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

