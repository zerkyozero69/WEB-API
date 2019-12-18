using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.Controllers.แผนการผลิต
{
    public class PlanSeedInfo_SumController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        public HttpResponseMessage PlanSeedInfo_Detail()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(PlanSeedInfo));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus=2 and SendOrgOid.Oid='" + org_oid + "'", null));
                if (collection.Count > 0)

            }
        }
    }
}
