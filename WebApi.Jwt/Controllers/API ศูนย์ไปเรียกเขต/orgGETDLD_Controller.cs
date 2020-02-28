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
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.API_ศูนย์ไปเรียกเขต
{
    public class orgGETDLD_Controller : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        // GET: api/orgGETDLD_
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/orgGETDLD_/5
        public string Get(int id)
        {
            return "value";
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("GetDLD_byORG")]
        public HttpResponseMessage GetDLD_byORG([FromBody]string OrganizationOid)
        {
            try
            {
                if (OrganizationOid != null)
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(DLDArea));
                    XafTypesInfo.Instance.RegisterEntity(typeof(Province));
                    List<GetProvinceDLD> DLD = new List<GetProvinceDLD>();



                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);

                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    IList<Province> collection = ObjectSpace.GetObjects<Province>(CriteriaOperator.Parse("GCRecord is null and OrganizationOid='" + OrganizationOid + "'", null));
                    var query = from Q in collection orderby Q.OrganizationOid select Q;
                    foreach (Province row in collection)
                    {
                        GetProvinceDLD listsa = new GetProvinceDLD();
                        listsa.OId = row.Oid.ToString();
                        listsa.ProvinceOid = row.ProvinceCode;
                        listsa.ProvinceNameTH = row.ProvinceNameTH;
                        listsa.OrganizationOid = row.OrganizationOid.OrganizeNameTH;
                        listsa.DLDZoneOid = row.DLDZone.Oid.ToString();
                        listsa.DLDZone = row.DLDZone.DLDAreaName;
                        DLD.Add(listsa);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, DLD);
                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ OrganizationOid ";
                    return Request.CreateResponse(HttpStatusCode.NoContent, err);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
                throw;
               
            }



        }
    }
}