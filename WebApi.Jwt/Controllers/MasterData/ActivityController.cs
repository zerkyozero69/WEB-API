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
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class ActivityController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// เรียกกิจกรรม
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Activity/List")]
        public HttpResponseMessage Activity_info()

        {
            try
            {
                string ObjectTypeOid =  HttpContext.Current.Request.Form["ObjectTypeOid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(Activity));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<activity_Model> list = new List<activity_Model>();
                List<activityDetails_Model> detail = new List<activityDetails_Model>();
                IList<Activity> collection = ObjectSpace.GetObjects<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and ObjectTypeOid = '" + ObjectTypeOid + "' ", null));
                {
                    if (collection.Count > 0)

                    {
                        foreach (Activity row in collection)
                        {
                            activity_Model Item = new activity_Model();
                            Item.Oid = row.Oid.ToString();
                            Item.ActivityName = row.ActivityName;
                            Item.ObjectTypeOid = row.ObjectTypeOid.Oid.ToString();
                            Item.ObjectTypeName = row.ObjectTypeOid.ObjectTypeName;
 
                            list.Add(Item);

                        }

                    }
                    else
                    {          //invalid
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "0";
                        err.message = "กรุณาใส่ข้อมูลให้เรียบร้อย";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
                 
    }
}
