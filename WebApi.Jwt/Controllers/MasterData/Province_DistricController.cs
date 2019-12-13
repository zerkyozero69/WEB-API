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
#region จังหวัด อำเภอ ตำบล
{/// <summary>
/// ใช้เรียกจังหวัด อำเภอ ตำบล
/// </summary>
    public class Province_DistricController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        [HttpPost]

        [Route("Province")]
        public HttpResponseMessage loadProvince() /// get จังหวัด
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Province));
                List<Province_Model> list = new List<Province_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<Province> collection = ObjectSpace.GetObjects<Province>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1 ", null));
                foreach (Province row in collection)
                {
                    Province_Model model = new Province_Model();
                    model.Oid = row.Oid.ToString();
                    model.ProvinceCode = row.ProvinceCode;
                    model.ProvinceNameTH = row.ProvinceNameTH;
                    model.ProvinceNameENG = row.ProvinceNameENG;
                    model.IsActive = row.IsActive;
                    model.Latitude =row.Latitude;
                    model.Longitude =row.Longitude;
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
        [AllowAnonymous]
        [HttpPost]

        [Route("Province/Oid")]
        public HttpResponseMessage loadProvince_ByID(string Oid) /// get จังหวัด ด้วย Oid
        {
        
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(Province));

                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<Province_Model> list_detail = new List<Province_Model>();
                IList<Province> collection = ObjectSpace.GetObjects<Province>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1  and Oid=? ", Oid));
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select Oid from Province where GCRecord is null and IsActive = 1 and Oid= '" + Oid + "'");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (Province row in collection)
                    {
                        Province_Model model = new Province_Model();
                        model.Oid = row.Oid.ToString();
                        model.ProvinceCode = row.ProvinceCode;
                        model.ProvinceNameTH = row.ProvinceNameTH;
                        model.ProvinceNameENG = row.ProvinceNameENG;
                        model.IsActive = row.IsActive;
                        model.Latitude = row.Latitude;
                        model.Longitude = row.Longitude;
               
                        list_detail.Add(model);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list_detail);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
                }

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
                    [AllowAnonymous]
                    [HttpPost]
                    [Route("Districts")]
                    public HttpResponseMessage loadDistricts() // โหลดอำเภอ
                    {
                        try
                        {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(District));
                List<District_Model> list = new List<District_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<District> collection = ObjectSpace.GetObjects<District>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1 ", null));
                foreach (District row in collection)
                {
                    District_Model model = new District_Model();
                    model.Oid = row.Oid.ToString();
                    model.DistrictCode = row.DistrictCode;
                    model.DistrictNameTH = row.DistrictNameTH;
                    model.DistrictNameENG = row.DistrictNameENG;
                    model.PostCode = row.PostCode;
                    model.IsActive = row.IsActive;
                    model.Latitude = row.Latitude;
                    model.Longitude = row.Longitude;
                   
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
                [AllowAnonymous]      
                [HttpPost]
                [Route("SubDistricts")]
        public HttpResponseMessage loadSubDistricts() // โหลดอำเภอ
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SubDistrict));
                List<SubDistrict_Model> list = new List<SubDistrict_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SubDistrict> collection = ObjectSpace.GetObjects<SubDistrict>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1 ", null));
                foreach (SubDistrict row in collection)
                {
                    SubDistrict_Model model = new SubDistrict_Model();
                    model.Oid = row.Oid.ToString();
                    model.SubDistrictCode = row.SubDistrictCode;
                    model.SubDistrictNameTH = row.SubDistrictNameTH;
                    model.SubDistrictNameENG = row.SubDistrictNameENG;
       
                    model.IsActive = row.IsActive;
                    model.Latitude = row.Latitude;
                    model.Longitude = row.Longitude;

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
        [AllowAnonymous]
        [HttpPost]
        [Route("Districts/Oid")]
        public HttpResponseMessage getDistricts_ByProvince(string Oid) ///โหลดอำเภอ by จังหวัด
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(District));

                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<District_Model> list_detail = new List<District_Model>();
                IList<District> collection = ObjectSpace.GetObjects<District>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1  and Oid=? ", Oid));
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select Oid from District where GCRecord is null and IsActive = 1 and Oid= '" + Oid + "'");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (District row in collection)
                    {
                        District_Model model = new District_Model();
                        model.Oid = row.Oid.ToString();
                        model.DistrictCode = row.DistrictCode;
                        model.DistrictNameTH = row.DistrictNameTH;
                        model.DistrictNameENG = row.DistrictNameENG;
                        model.IsActive = row.IsActive;
                        model.Latitude = row.Latitude;
                        model.Longitude = row.Longitude;

                        list_detail.Add(model);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list_detail);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
                }
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("SubDistricts/Oid")]
        public HttpResponseMessage getSubDistricts_ByDistricts(string Oid) ///โหลดตำบล by อำเภอ
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SubDistrict));

                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<SubDistrict_Model> list_detail = new List<SubDistrict_Model>();
                IList<SubDistrict> collection = ObjectSpace.GetObjects<SubDistrict>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1  and Oid=? ", Oid));
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select Oid from SubDistrict where GCRecord is null and IsActive = 1 and Oid= '" + Oid + "'");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (SubDistrict row in collection)
                    {
                        SubDistrict_Model model = new SubDistrict_Model();
                        model.Oid = row.Oid.ToString();
                        model.SubDistrictCode = row.SubDistrictCode;
                        model.SubDistrictNameTH = row.SubDistrictNameTH;
                        model.SubDistrictNameENG = row.SubDistrictNameENG;
                        model.IsActive = row.IsActive;
                        model.Latitude = row.Latitude;
                        model.Longitude = row.Longitude;

                        list_detail.Add(model);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list_detail);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
                }
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, err);
            }
        }
        #endregion
    }
}
