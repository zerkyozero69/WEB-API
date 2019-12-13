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
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using nutrition.Module;
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class ServiceTypeController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
      


        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpGet]
        [Route("LoadServiceType")]
        public HttpResponseMessage LoadServiceType()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.ServiceType));
                List<ServiceType_Model> list = new List<ServiceType_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<ServiceType> collection = ObjectSpace.GetObjects<ServiceType>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1 ", null));
                foreach (ServiceType row in collection)
                {
                    ServiceType_Model model = new ServiceType_Model();
                    model.Oid = row.Oid.ToString();
                    model.ServiceTypeName = row.ServiceTypeName;
                    model.Remark = row.Remark;
                    model.IsActive = row.IsActive;
                    list.Add(model);

    }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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
    }
}
