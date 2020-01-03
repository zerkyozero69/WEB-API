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
using static WebApi.Jwt.Models.Farmerinfo;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class RegisterFarmerXAF_Controller : ApiController
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
            Organization org = null;
            Farmerinfo._Registerfarmer farmerinfo = new Farmerinfo._Registerfarmer();
            string TempForageType = string.Empty;
            //Farmer _Farmer = new Farmer();
            try
            {
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                if (jObject != null)
                {
                    farmerinfo.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    farmerinfo.CitizenID = jObject.SelectToken("CitizenID").Value<Int64>();
                    if (jObject.SelectToken("CitizenID") != null)
                    {
                        int intCitizenID;
                        if (int.TryParse(jObject.SelectToken("CitizenID").ToString(), out intCitizenID))
                        {
                            farmerinfo.CitizenID = intCitizenID;
                        }
                        farmerinfo.TitleOid = jObject.SelectToken("TitleOid").Value<string>();
                        farmerinfo.FirstNameTH = jObject.SelectToken("FirstNameTH").Value<string>();
                        farmerinfo.LastNameTH = jObject.SelectToken("LastNameTH").Value<string>();
                        farmerinfo.BirthDate = jObject.SelectToken("BirthDate").Value<DateTime>();

                        farmerinfo.GenderOid = jObject.SelectToken("GenderOid").Value<string>();
                        farmerinfo.Tel = jObject.SelectToken("Tel").Value<string>();

                        if (jObject.SelectToken("Email") == null)
                        {
                            farmerinfo.Email = string.Empty;
                        }
                        else
                        {
                            farmerinfo.Email = jObject.SelectToken("Email").Value<string>();
                        }

                        farmerinfo.Address = jObject.SelectToken("Address").Value<string>();

                        if (jObject.SelectToken("Moo") == null)
                        {
                            farmerinfo.Moo = string.Empty;
                        }
                        else
                        {
                            farmerinfo.Moo = jObject.SelectToken("Moo").Value<string>();
                        }

                        if (jObject.SelectToken("Soi") == null)
                        {
                            farmerinfo.Soi = string.Empty;
                        }
                        else
                        {
                            farmerinfo.Soi = jObject.SelectToken("Soi").Value<string>();
                        }

                        if (jObject.SelectToken("Road") == null)
                        {
                            farmerinfo.Road = string.Empty;
                        }
                        else
                        {
                            farmerinfo.Road = jObject.SelectToken("Road").Value<string>();
                        }

                        farmerinfo.ProvinceOid = jObject.SelectToken("ProvinceOid").Value<string>();
                        farmerinfo.DistrictOid = jObject.SelectToken("DistrictOid").Value<string>();
                        farmerinfo.SubDistrictOid = jObject.SelectToken("SubDistrictOid").Value<string>();
                        farmerinfo.ZipCode = jObject.SelectToken("ZipCode").Value<string>();
                        //Registerfarmer.ForageTypeOid = TempForageType;

                        JObject jObject_Forage = JObject.Parse(jObject.ToString());
                        JArray Arr_Forage = (JArray)jObject_Forage["ForageTypes"];

                        IList<ForageTypeModel> ForageT = Arr_Forage.ToObject<IList<ForageTypeModel>>();
                        foreach (ForageTypeModel row in ForageT)
                        {
                            if (TempForageType == string.Empty)
                            {
                                TempForageType = row.Oid;
                            }
                            else
                            {
                                TempForageType = TempForageType + ',' + row.Oid;
                            }
                        }

                        if (jObject.SelectToken("Latitude") == null || jObject.SelectToken("Latitude").ToString() == string.Empty)
                        {
                            farmerinfo.Latitude = 0;
                        }
                        else
                        {
                            farmerinfo.Latitude = jObject.SelectToken("Latitude").Value<float>();
                        }

                        if (jObject.SelectToken("Longitude") == null || jObject.SelectToken("Longitude").ToString() == string.Empty)
                        {
                            farmerinfo.Longitude = 0;
                        }
                        else
                        {
                            farmerinfo.Longitude = jObject.SelectToken("Longitude").Value<float>();
                        }

                        farmerinfo.Register_Type = 2;
                    }
                }
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();         
                Session session = new Session();
                ISessionProvider session_ = session;
                //   IList<Farmer> collection = ObjectSpace.GetObjects<Farmer>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));

                //     XPCollection collection = (XPCollection)ObjectSpace.CreateCollection(typeof(Farmer));



                DataSet ds = new DataSet();
                Farmer farmer_ = ObjectSpace.CreateObject< Farmer >();  // new Farmer(session);
             /*   ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, sql)*/;
                SqlParameter[] css = new SqlParameter[21];
                css[0] = new SqlParameter("@OrganizationOid", farmerinfo.OrganizationOid);
                css[1] = new SqlParameter("@Citizen_ID", farmerinfo.CitizenID);
                css[2] = new SqlParameter("@TitleOid", farmerinfo.TitleOid);
                css[3] = new SqlParameter("@FirstName_TH", farmerinfo.FirstNameTH);
                css[4] = new SqlParameter("@LastName_TH", farmerinfo.LastNameTH);
                css[5] = new SqlParameter("@Birthdate", farmerinfo.BirthDate);
                css[6] = new SqlParameter("@Gender", farmerinfo.GenderOid);
                css[7] = new SqlParameter("@Tel", farmerinfo.Tel);
                css[8] = new SqlParameter("@Email", farmerinfo.Email);
                css[9] = new SqlParameter("@Address_No", farmerinfo.Address);
                css[10] = new SqlParameter("@Address_moo", farmerinfo.Moo);
                css[11] = new SqlParameter("@Address_Soi", farmerinfo.Soi);
                css[12] = new SqlParameter("@Address_Road", farmerinfo.Road);
                css[13] = new SqlParameter("@Address_provinces", farmerinfo.ProvinceOid);
                css[14] = new SqlParameter("@Address_districts", farmerinfo.DistrictOid);
                css[15] = new SqlParameter("@Address_subdistricts", farmerinfo.SubDistrictOid);
                css[16] = new SqlParameter("@ZipCode", farmerinfo.ZipCode);
                css[17] = new SqlParameter("@ForageTypeOid", TempForageType);//Updatefarmer.ForageTypeOid);
                css[18] = new SqlParameter("@Latitude", farmerinfo.Latitude);
                css[19] = new SqlParameter("@Longitude", farmerinfo.Longitude);
                css[20] = new SqlParameter("@Register_Type", farmerinfo.Register_Type);
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
        [AllowAnonymous]
        [HttpGet]
        [Route("farmerGET")]
        public IHttpActionResult xafclass()
        {
          //  Farmerinfo.Profile_Farmer farmerinfo = new Farmerinfo.Profile_Farmer();
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(Farmer));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
          
                List<Farmerinfo.Profile_Farmer> ilist = new List<Farmerinfo.Profile_Farmer>();
                IList<Farmer> collection = ObjectSpace.GetObjects<Farmer>(CriteriaOperator.Parse(" GCRecord is null and IsActive = true", null));
                if (collection != null)
                {
                   
                    foreach (Farmer row in collection)
                    {
                        Farmerinfo.Profile_Farmer _farmerinfo = new Farmerinfo.Profile_Farmer();
                        _farmerinfo.Oid = row.Oid;
                        _farmerinfo.CitizenID = row.CitizenID;
                        _farmerinfo.Title = row.TitleOid.TitleName;
                        _farmerinfo.FirstNameTH = row.FirstNameTH;
                        _farmerinfo.LastNameTH = row.LastNameTH;
                        ilist.Add(_farmerinfo);
                    
                    }
                 
                }
                else
                {
                    return BadRequest( "Any object");
                }
                return Ok(ilist);
            }

            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                //   return BadRequest(ex.Message);
                return null;
            }
        }
    }
}
