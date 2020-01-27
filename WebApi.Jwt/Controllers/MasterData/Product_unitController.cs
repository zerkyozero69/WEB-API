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
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class Product_unitController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// หน่วยวัด
        /// </summary>
        /// <returns></returns>
        //[JwtAuthentication]
        [AllowAnonymous]
        [HttpGet]
        [Route("Product_unit")]
        public HttpResponseMessage get_Product_unit()
        {
        
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Unit));
                List<Unit_Model> list = new List<Unit_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<Unit> collection = ObjectSpace.GetObjects<Unit>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1 ", null));
                foreach (Unit row in collection)
                {
                    Unit_Model model = new Unit_Model();
                    model.UnitOid = row.Oid.ToString();
                    model.UnitCode = row.UnitCode;
                    model.UnitName = row.UnitName;
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
    


        [AllowAnonymous]
        [HttpGet]
        [Route("Package_List")]
        public HttpResponseMessage loadPackage_Info() ///ข้อมูลบรรจุภัณท์
        {
            try
            { 
                   XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Package));
                    List<Package_Model> list = new List<Package_Model>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    IList<Package> collection = ObjectSpace.GetObjects<Package>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1 ", null));
                    foreach (Package row in collection)
                    {
                    Package_Model model = new Package_Model();
                       model.PackageOid = row.Oid.ToString();
                        model.PackageName = row.PackageName;     
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

    

