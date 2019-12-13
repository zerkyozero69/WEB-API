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
using nutrition.Module;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class PositionController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        [AllowAnonymous]
        [HttpGet]

        [Route("Position")]/// เรียกตำแหน่งเจ้าหน้าที่
        public HttpResponseMessage loadPosition()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Position));
                List<Position_Model> list = new List<Position_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<Position> collection = ObjectSpace.GetObjects<Position>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1", null));
                foreach (Position row in collection)
                {
                    Position_Model model = new Position_Model();
                    model.PositionName = row.PositionName;
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
                //return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, err);
            }
            
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Position/Level")] /// เรียกลำดับชั้นของตำแหน่ง
        public HttpResponseMessage loadPosition_Tier()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.PositionLevel));
                List<PositionLevel_Model> list = new List<PositionLevel_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<PositionLevel> collection = ObjectSpace.GetObjects<PositionLevel>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1", null));
                foreach (PositionLevel row in collection)
                {
                    PositionLevel_Model model = new PositionLevel_Model();
                    model.PositionLevelName = row.PositionLevelName;
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
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, err);
            }
        }
        
    }
}
