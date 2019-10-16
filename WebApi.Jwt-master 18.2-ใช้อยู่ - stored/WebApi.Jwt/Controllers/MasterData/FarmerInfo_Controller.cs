using System.Web.Http;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp;
using WebApi.Jwt.Models;
using DevExpress.Data.Filtering;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Base;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;
using DevExpress.ExpressApp.Model;
using System.Security.Cryptography;
using DevExpress.Persistent.Validation;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using DevExpress.ExpressApp.Security.Strategy;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using System.Collections.Generic;
using WebApi.Jwt.Controllers;
using nutrition.Module;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class FarmerInfo_Controller : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ
        // [HttpPost] หน้าโมบาย
        [HttpPost]
        [Route("XAF2")]
        public HttpResponseMessage RigisterFarmer_XAF()
        {
            XpoTypesInfoHelper.GetTypesInfo();
            XafTypesInfo.Instance.RegisterEntity(typeof(Farmer));
            XafTypesInfo.Instance.RegisterEntity(typeof(Organization));
            Organization org;
            user._farmerinfo farmerinfo = new user._farmerinfo();
            //Farmer _Farmer = new Farmer();
            try
            {
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                if (jObject != null)
                {
                    farmerinfo.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    farmerinfo.CitizenID = jObject.SelectToken("CitizenID").Value<Int64>();
                    farmerinfo.FirstNameTH = jObject.SelectToken("FirstNameTH").Value<string>();
                    }      
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();         
                Session session = new Session();
                ISessionProvider session_ = session;
                //   IList<Farmer> collection = ObjectSpace.GetObjects<Farmer>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));

                //     XPCollection collection = (XPCollection)ObjectSpace.CreateCollection(typeof(Farmer));
                Farmer farmer_ = null;
                farmer_ = ObjectSpace.CreateObject<Farmer>();
                {
                    var farmer = farmer_;
                    farmer.FirstNameTH = farmer_.FirstNameTH;


                }



                //List<user._farmerinfo> list = new List<user._farmerinfo>();
                //foreach (Farmer row in collection)
                //{
                //    user._farmerinfo farmerinfo_ = new user._farmerinfo();
                //    farmerinfo_.OrganizationOid = row.OrganizationOid;
                //    farmerinfo_.CitizenID = Convert.ToInt64(row.CitizenID);
                //    farmerinfo_.FirstNameTH = row.FirstNameTH;
                //    list.Add(farmerinfo_);
                //}
                ObjectSpace.CommitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "ok");


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
