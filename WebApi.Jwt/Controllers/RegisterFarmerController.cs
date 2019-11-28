﻿using System;
using System.Transactions.Configuration;
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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static WebApi.Jwt.Models.Farmerinfo;

namespace WebApi.Jwt.Controllers
{
    public class RegisterFarmerController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();


        /// <summary>
        /// ลงทะเบียนเกษตรกร
        /// </summary>
        /// <returns></returns>
        //  [JwtAuthentication]
        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterFarmer")]



        #region ใช้เพิ่มข้อมูล แก้ไข ข้อมูลเกษตรกร
        public HttpResponseMessage RegisterFarmer()
        {
            _Registerfarmer Registerfarmer = new _Registerfarmer();
            try
            {
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                string TempForageType = string.Empty;
                if (jObject != null)
                {
                    Registerfarmer.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    Registerfarmer.CitizenID = jObject.SelectToken("CitizenID").Value<Int64>();
                    if (jObject.SelectToken("CitizenID") != null)
                    {
                        int intCitizenID;
                        if (int.TryParse(jObject.SelectToken("CitizenID").ToString(), out intCitizenID))
                        {
                            Registerfarmer.CitizenID = intCitizenID;
                        }
                        Registerfarmer.TitleOid = jObject.SelectToken("TitleOid").Value<string>();
                        Registerfarmer.FirstNameTH = jObject.SelectToken("FirstNameTH").Value<string>();
                        Registerfarmer.LastNameTH = jObject.SelectToken("LastNameTH").Value<string>();
                        Registerfarmer.BirthDate = jObject.SelectToken("BirthDate").Value<DateTime>();

                        Registerfarmer.GenderOid = jObject.SelectToken("GenderOid").Value<string>();
                        Registerfarmer.Tel = jObject.SelectToken("Tel").Value<string>();

                        if (jObject.SelectToken("Email") == null)
                        {
                            Registerfarmer.Email = string.Empty;
                        }
                        else {
                            Registerfarmer.Email = jObject.SelectToken("Email").Value<string>();
                        }

                        Registerfarmer.Address = jObject.SelectToken("Address").Value<string>();

                        if (jObject.SelectToken("Moo") == null)
                        {
                            Registerfarmer.Moo = string.Empty;
                        }
                        else
                        {
                            Registerfarmer.Moo = jObject.SelectToken("Moo").Value<string>();
                        }
                      
                        if (jObject.SelectToken("Soi") == null)
                        {
                            Registerfarmer.Soi = string.Empty;
                        }
                        else {
                            Registerfarmer.Soi = jObject.SelectToken("Soi").Value<string>();
                        }

                        if (jObject.SelectToken("Road") == null)
                        {
                            Registerfarmer.Road = string.Empty;
                        }
                        else
                        {
                            Registerfarmer.Road = jObject.SelectToken("Road").Value<string>();
                        }
                        
                        Registerfarmer.ProvinceOid = jObject.SelectToken("ProvinceOid").Value<string>();
                        Registerfarmer.DistrictOid = jObject.SelectToken("DistrictOid").Value<string>();
                        Registerfarmer.SubDistrictOid = jObject.SelectToken("SubDistrictOid").Value<string>();
                        Registerfarmer.ZipCode = jObject.SelectToken("ZipCode").Value<string>();
                        //Registerfarmer.ForageTypeOid = TempForageType;

                        JObject jObject_Forage =  JObject.Parse(jObject.ToString());
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
                            Registerfarmer.Latitude = 0;
                        }
                        else
                        {
                            Registerfarmer.Latitude = jObject.SelectToken("Latitude").Value<float>();
                        }

                        if (jObject.SelectToken("Longitude") == null || jObject.SelectToken("Longitude").ToString() == string.Empty)
                        {
                            Registerfarmer.Longitude = 0;
                        }
                        else
                        {
                            Registerfarmer.Longitude = jObject.SelectToken("Longitude").Value<float>();
                        }
                        
                        Registerfarmer.Register_Type = 2;
                    }
                }
                DataSet ds;
                SqlParameter[] prm = new SqlParameter[21];

                prm[0] = new SqlParameter("@OrganizationOid", Registerfarmer.OrganizationOid);
                prm[1] = new SqlParameter("@Citizen_ID", Registerfarmer.CitizenID);
                prm[2] = new SqlParameter("@TitleOid", Registerfarmer.TitleOid);
                prm[3] = new SqlParameter("@FirstName_TH", Registerfarmer.FirstNameTH);
                prm[4] = new SqlParameter("@LastName_TH", Registerfarmer.LastNameTH);
                prm[5] = new SqlParameter("@Birthdate", Registerfarmer.BirthDate);
                prm[6] = new SqlParameter("@Gender", Registerfarmer.GenderOid);
                prm[7] = new SqlParameter("@Tel", Registerfarmer.Tel);
                prm[8] = new SqlParameter("@Email", Registerfarmer.Email);
                prm[9] = new SqlParameter("@Address_No", Registerfarmer.Address);
                prm[10] = new SqlParameter("@Address_moo", Registerfarmer.Moo);
                prm[11] = new SqlParameter("@Address_Soi", Registerfarmer.Soi);
                prm[12] = new SqlParameter("@Address_Road", Registerfarmer.Road);
                prm[13] = new SqlParameter("@Address_provinces", Registerfarmer.ProvinceOid);
                prm[14] = new SqlParameter("@Address_districts", Registerfarmer.DistrictOid);
                prm[15] = new SqlParameter("@Address_subdistricts", Registerfarmer.SubDistrictOid);
                prm[16] = new SqlParameter("@ZipCode", Registerfarmer.ZipCode);
                prm[17] = new SqlParameter("@ForageTypeOid", TempForageType); // Registerfarmer.ForageTypeOid
                prm[18] = new SqlParameter("@Latitude", Registerfarmer.Latitude);
                prm[19] = new SqlParameter("@Longitude", Registerfarmer.Longitude);
                prm[20] = new SqlParameter("@Register_Type", Registerfarmer.Register_Type);

                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_Moblieinsert_RegisterFarmer", prm);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                //    using (DataSet ds = SqlHelper.ExecuteDataset(scc, "spt_Moblieinsert_RegisterFarmer", ))

                if (ds.Tables[0].Rows[0]["pStatus"].ToString() != "0" || ds.Tables[0].Rows[0]["pStatus"].ToString() == "2")
                {

                    Farmer_Status Farmer = new Farmer_Status();
                    Farmer.Message = "ลงทะเบียนสำเร็จ ";

                    Farmer.Fullname = @Registerfarmer.FirstNameTH + "  " + @Registerfarmer.LastNameTH;
                    Farmer.CitizenID = "รหัสบัตรประชาชน" + " = " + @Registerfarmer.CitizenID;


                    //return Request.CreateResponse(HttpStatusCode.OK);
                    return Request.CreateResponse(true);
                }
                else if (ds.Tables[0].Rows[0]["pStatus"].ToString() == "99")
                {
                    UserError err = new UserError();
                    err.code = "2"; // กรอกข้อมูลซ้ำ
                    err.message = "บุคคลนี้เคยลงทะเบียนไปแล้ว โปรดตรวจสอบ ";
                    //return Request.CreateResponse(HttpStatusCode.BadRequest,err);
                    return Request.CreateResponse(false);
                }

                else
                {
                    UserError err = new UserError();
                    err.code = "3"; // กรอกข้อมูลไม่ครบ
                    err.message = "กรอกข้อมูลไม่ครบ ";
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    return Request.CreateResponse(false);

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
        /// <summary>
        /// แก้ไขข้อมูล
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Farmer/Update")]
        public HttpResponseMessage UpdateRegisterFarmer()
        {
            _Registerfarmer Updatefarmer = new _Registerfarmer();
       
            try
            {

                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                string TempForageType = string.Empty;
                if (jObject != null)
                {
                    Updatefarmer.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    Updatefarmer.CitizenID = jObject.SelectToken("CitizenID").Value<Int64>();
                    if (jObject.SelectToken("CitizenID") != null)
                    {
                        int intCitizenID;
                        if (int.TryParse(jObject.SelectToken("CitizenID").ToString(), out intCitizenID))
                        {
                            Updatefarmer.CitizenID = intCitizenID;
                        }
                        Updatefarmer.TitleOid = jObject.SelectToken("TitleOid").Value<string>();
                        Updatefarmer.FirstNameTH = jObject.SelectToken("FirstNameTH").Value<string>();
                        Updatefarmer.LastNameTH = jObject.SelectToken("LastNameTH").Value<string>();
                        Updatefarmer.BirthDate = jObject.SelectToken("BirthDate").Value<DateTime>();

                        Updatefarmer.GenderOid = jObject.SelectToken("GenderOid").Value<string>();
                        Updatefarmer.Tel = jObject.SelectToken("Tel").Value<string>();

                        if (jObject.SelectToken("Email") == null)
                        {
                            Updatefarmer.Email = null;
                        }
                        else
                        {
                            Updatefarmer.Email = jObject.SelectToken("Email").Value<string>();
                        }

                        Updatefarmer.Address = jObject.SelectToken("Address").Value<string>();

                        if (jObject.SelectToken("Moo") == null)
                        {
                            Updatefarmer.Moo = null;
                        }
                        else
                        {
                            Updatefarmer.Moo = jObject.SelectToken("Moo").Value<string>();
                        }

                        if (jObject.SelectToken("Soi") == null)
                        {
                            Updatefarmer.Soi = null;
                        }
                        else
                        {
                            Updatefarmer.Soi = jObject.SelectToken("Soi").Value<string>();
                        }

                        if (jObject.SelectToken("Road") == null)
                        {
                            Updatefarmer.Road = null;
                        }
                        else
                        {
                            Updatefarmer.Road = jObject.SelectToken("Road").Value<string>();
                        }

                        Updatefarmer.ProvinceOid = jObject.SelectToken("ProvinceOid").Value<string>();
                        Updatefarmer.DistrictOid = jObject.SelectToken("DistrictOid").Value<string>();
                        Updatefarmer.SubDistrictOid = jObject.SelectToken("SubDistrictOid").Value<string>();
                        Updatefarmer.ZipCode = jObject.SelectToken("ZipCode").Value<string>();
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
                            Updatefarmer.Latitude = 0;
                        }
                        else
                        {
                            Updatefarmer.Latitude = jObject.SelectToken("Latitude").Value<float>();
                        }

                        if (jObject.SelectToken("Longitude") == null || jObject.SelectToken("Longitude").ToString() == string.Empty)
                        {
                            Updatefarmer.Longitude = 0;
                        }
                        else
                        {
                            Updatefarmer.Longitude = jObject.SelectToken("Longitude").Value<float>();
                        }

                        Updatefarmer.Register_Type = 2;
                    }
                }


                DataSet ds = new DataSet();
                SqlParameter[] prm = new SqlParameter[21];
                prm[0] = new SqlParameter("@OrganizationOid", Updatefarmer.OrganizationOid);
                prm[1] = new SqlParameter("@Citizen_ID", Updatefarmer.CitizenID);
                prm[2] = new SqlParameter("@TitleOid", Updatefarmer.TitleOid);
                prm[3] = new SqlParameter("@FirstName_TH", Updatefarmer.FirstNameTH);
                prm[4] = new SqlParameter("@LastName_TH", Updatefarmer.LastNameTH);
                prm[5] = new SqlParameter("@Birthdate", Updatefarmer.BirthDate);
                prm[6] = new SqlParameter("@Gender", Updatefarmer.GenderOid);
                prm[7] = new SqlParameter("@Tel", Updatefarmer.Tel);
                prm[8] = new SqlParameter("@Email", Updatefarmer.Email);
                prm[9] = new SqlParameter("@Address_No", Updatefarmer.Address);
                prm[10] = new SqlParameter("@Address_moo", Updatefarmer.Moo);
                prm[11] = new SqlParameter("@Address_Soi", Updatefarmer.Soi);
                prm[12] = new SqlParameter("@Address_Road", Updatefarmer.Road);
                prm[13] = new SqlParameter("@Address_provinces", Updatefarmer.ProvinceOid);
                prm[14] = new SqlParameter("@Address_districts", Updatefarmer.DistrictOid);
                prm[15] = new SqlParameter("@Address_subdistricts", Updatefarmer.SubDistrictOid);
                prm[16] = new SqlParameter("@ZipCode", Updatefarmer.ZipCode);
                prm[17] = new SqlParameter("@ForageTypeOid", TempForageType);//Updatefarmer.ForageTypeOid);
                prm[18] = new SqlParameter("@Latitude", Updatefarmer.Latitude);
                prm[19] = new SqlParameter("@Longitude", Updatefarmer.Longitude);
                prm[20] = new SqlParameter("@Register_Type", Updatefarmer.Register_Type);
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_Moblieinsert_RegisterFarmer", prm);
                if (prm[1].Value != null)
                {
                    if (ds.Tables[0].Rows[0]["pStatus"].ToString() != "0")
                    {
                        //return Request.CreateResponse(HttpStatusCode.OK, "[" + Updatefarmer.@FirstNameTH + "  " + Updatefarmer.@LastNameTH + "] " + "แก้ไขข้อมูลสำเร็จแล้ว");
                        return Request.CreateResponse(true);
                    }
                    else if (ds.Tables[0].Rows[0]["pStatus"].ToString() == "0")
                    {
                        //return Request.CreateResponse(HttpStatusCode.BadRequest, "กรอกข้อมูลผิด");
                        return Request.CreateResponse(false);
                    }
                    {
                        //return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่มีข้อมูล");
                        return Request.CreateResponse(false);
                    }
                }
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, "กรุณาใส่เลขบัตร");
                    return Request.CreateResponse(false);
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
            //ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieUpdateFarmer"
            //        , new SqlParameter("@Oid", Updatefarmer.Oid)
            //        , new SqlParameter("@OrganizationOid", Updatefarmer.OrganizationOid)
            //         , new SqlParameter("@Citizen_ID", Updatefarmer.CitizenID)
            //         , new SqlParameter("@TitleOid", Updatefarmer.TitleOid)
            //         , new SqlParameter("@FirstName_TH", Updatefarmer.FirstNameTH)
            //         , new SqlParameter("@LastName_TH", Updatefarmer.LastNameTH)
            //        , new SqlParameter("@Birthdate", Updatefarmer.BirthDay)
            //        , new SqlParameter("@Gender", Updatefarmer.Gender)
            //         , new SqlParameter("@Tel", Updatefarmer.Tel)
            //         , new SqlParameter("@Email", Updatefarmer.Email)
            //       , new SqlParameter("@Address_No", Updatefarmer.Address_No)
            //       , new SqlParameter("@Address_buildingName", Updatefarmer.Address_buildingName)
            //       , new SqlParameter("@Address_moo", Updatefarmer.Address_moo)
            //       , new SqlParameter("@Address_Soi", Updatefarmer.Address_Soi)
            //       , new SqlParameter("@Address_Road", Updatefarmer.Address_Road)
            //       , new SqlParameter("@Address_provinces", Updatefarmer.Address_provinces)
            //       , new SqlParameter("@Address_districts", Updatefarmer.Address_districts)
            //       , new SqlParameter("@Address_subdistricts", Updatefarmer.Address_subdistricts)
            //        , new SqlParameter("@ZipCode", Updatefarmer.ZipCode)
            //       , new SqlParameter("@animalSupplie", Updatefarmer.AnimalSupplie)
            //       , new SqlParameter("@Latitude", Updatefarmer.Latitude)
            //       , new SqlParameter("@Longitude", Updatefarmer.Longitude));

        }


        /// <summary>
        /// ค้นหาเรียกรายชื่อเกษตรกร
        /// </summary>
        /// <param name="CitizenID"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("get/profileFarmer")]

        public HttpResponseMessage loadprofileFarmer_ByCitizenID()
        {
            try
            {


                string CitizenID = null;
                if (HttpContext.Current.Request.Form["CitizenID"].ToString() != null)
                {
                    CitizenID = HttpContext.Current.Request.Form["CitizenID"].ToString();
                }

                DataSet ds = new DataSet();

                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileGetCitizenID", new SqlParameter("@CitizenID", CitizenID)
                   );

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Profile_Farmer profile = new Profile_Farmer();
                    profile.Oid = ds.Tables[0].Rows[0]["Oid"].ToString();
                    profile.CitizenID = ds.Tables[0].Rows[0]["OrganizeNameTH"].ToString();
                    profile.Title = ds.Tables[0].Rows[0]["CitizenID"].ToString();
                    profile.FirstNameTH = ds.Tables[0].Rows[0]["TitleName"].ToString();
                    profile.LastNameTH = ds.Tables[0].Rows[0]["FirstNameTH"].ToString();
                    profile.Gender = ds.Tables[0].Rows[0]["LastNameTH"].ToString();
                    profile.Gender = ds.Tables[0].Rows[0]["GenderName"].ToString();
                    profile.BirthDate = ds.Tables[0].Rows[0]["BirthDate"].ToString();
                    profile.Tel = ds.Tables[0].Rows[0]["Tel"].ToString();
                    profile.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    profile.IsActive = ds.Tables[0].Rows[0]["IsActive"].ToString();
                    profile.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    profile.Moo = ds.Tables[0].Rows[0]["Moo"].ToString();
                    profile.Soi = ds.Tables[0].Rows[0]["Soi"].ToString();
                    profile.Road = ds.Tables[0].Rows[0]["Road"].ToString();
                    profile.Province = ds.Tables[0].Rows[0]["ProvinceNameTH"].ToString();
                    profile.District = ds.Tables[0].Rows[0]["DistrictNameTH"].ToString();
                    profile.SubDistrict = ds.Tables[0].Rows[0]["SubDistrictNameTH"].ToString();
                    profile.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    return Request.CreateResponse(HttpStatusCode.OK, profile);
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่มีเลขบัตรประชาชน");
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
            // return Request.CreateResponse(HttpStatusCode.GatewayTimeout, 0);


        }


        [AllowAnonymous]
        [HttpPost]
        [Route("TestPostMethod")]
        public HttpResponseMessage TestPostMethod()
        {
            string requestString = Request.Content.ReadAsStringAsync().Result;
            JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
            //FirstName 
            string Oid = jObject.SelectToken("Oid").Value<string>();
            string firstName = jObject.SelectToken("FirstName").Value<string>();
            //Lastname
            string lastName = jObject.SelectToken("LastName").Value<string>();
            //Age
            int age = jObject.SelectToken("Age").Value<int>();
            //Title
            string title = jObject.SelectToken("Title").Value<string>();
            //if (jObject.SelectToken("Title") != null)
            //{
            //    string titleName = jObject.SelectToken("Title").SelectToken("TitleName").Value<string>();
            //    int titleID = jObject.SelectToken("Title").SelectToken("ID").Value<int>();
            //}
            DataSet ds;
            SqlParameter[] prm = new SqlParameter[4];

            prm[0] = new SqlParameter("@FirstName_TH", firstName);
            prm[1] = new SqlParameter("@LastName_TH", lastName);
            prm[2] = new SqlParameter("@age", age);
            prm[3] = new SqlParameter("@title", title);


            ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "testinsertTBL", prm);
            if (ds.Tables[0].Rows[0]["pMessage"].ToString() == "success")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "พร้อมแล้ว");
            }
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ว๊ายไม่ลง");
            }

        }
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("FarmerCitizenID")]
        public HttpResponseMessage get_CitizenID()
        {
            List<FarmerCitizen> farmerCitizenList = new List<FarmerCitizen>();
            FarmerCitizen farmer_info = new FarmerCitizen();

            try
            {
                string CitizenID = string.Empty;
                if (HttpContext.Current.Request.Form["CitizenID"].ToString() != null)
                {
                    CitizenID = HttpContext.Current.Request.Form["CitizenID"].ToString();
                }
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileGetCitizenID", new SqlParameter("@CitizenID", CitizenID));
                if (ds.Tables[0].Rows.Count > 0)
                {



                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, rows);
                }
                else if (ds.Tables[0].Rows.Count == 0)
                {
                    string param = "username=regislive01&password=password&grant_type=password";//เพื่อทำการขอ access_token 
                    byte[] dataStream = Encoding.UTF8.GetBytes(param);
                    string AuthParam = "regislive:password";
                    string authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(AuthParam));
                    var request = (HttpWebRequest)WebRequest.Create("http://regislives.dld.go.th:9080/regislive_authen/oauth/token");
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = dataStream.Length;
                    request.Headers.Add("Authorization", "Basic " + authInfo);
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(dataStream, 0, dataStream.Length);
                    }
                    var response = (HttpWebResponse)request.GetResponse();
                    string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();
                    var jsonResulttodict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
                    var access_token = jsonResulttodict["access_token"];

                    //    'Get Data By Token
                    request = (HttpWebRequest)WebRequest.Create("http://regislives.dld.go.th:9080/regislive_webservice/farmer/findbyPid?pid=" + CitizenID);
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Headers.Add("Authorization", "Bearer " + access_token);
                    response = (HttpWebResponse)request.GetResponse();
                    var xreader = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var farmerResul = JsonConvert.DeserializeObject<Dictionary<string, object>>(xreader);
                    farmer_info.CitizenID = farmerResul["pid"];
                    farmer_info.titleName = farmerResul["prefixNameTh"];
                    farmer_info.FirstNameTH = farmerResul["firstName"];
                    farmer_info.LastNameTH = farmerResul["lastName"];
                    if (farmerResul["genderName"] == null)
                    {
                        farmer_info.genderName = "";
                    }
                    else
                    {
                        farmer_info.genderName = farmerResul["genderName"];
                    }

                    farmer_info.birthDate = farmerResul["birthDay"];
                    farmer_info.tel = farmerResul["phone"];
                    farmer_info.email = farmerResul["email"];
                    farmer_info.address = farmerResul["homeNo"];
                    farmer_info.moo = farmerResul["moo"];
                    farmer_info.soi = farmerResul["soi"];
                    farmer_info.road = farmerResul["road"];
                    farmer_info.provinceNameTH = farmerResul["provinceName"];
                    farmer_info.districtNameTH = farmerResul["amphurName"];
                    farmer_info.subDistrictNameTH = farmerResul["tambolName"];
                    farmer_info.zipCode = farmerResul["postCode"];
                    farmer_info.latitude = farmerResul["latitude"];
                    farmer_info.longitude = farmerResul["longitude"];

                    farmerCitizenList.Add(farmer_info);
                    return Request.CreateResponse(HttpStatusCode.OK, farmerCitizenList);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่มีเลขบัตรประชาชน");

                }
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

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("RAW/TestPostMethod")]
        //public HttpResponseMessage TestgetMethod()
        //{
        //    string requestString = Request.Content.ReadAsStringAsync().Result;
        //    JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
        //    //FirstName 
        //    string firstName = jObject.SelectToken("FirstName").Value<string>();
        //    //Lastname
        //    string lastName = jObject.SelectToken("LastName").Value<string>();
        //    //Age
        //    int age = jObject.SelectToken("Age").Value<int>();
        //    //Title
        //    object title = jObject.SelectToken("Title").Value<object>();
        //     if (jObject.SelectToken("Title") != null)
        //    {
        //       string titleName = jObject.SelectToken("Title").SelectToken("TitleName").Value<string>();
        //       int titleID = jObject.SelectToken("Title").SelectToken("ID").Value<int>();
        //    }
        //    DataSet ds;
        //    SqlParameter[] prm = new SqlParameter[4];

        //    prm[0] = new SqlParameter("@FirstName_TH", firstName);
        //    prm[1] = new SqlParameter("@LastName_TH", lastName);
        //    prm[2] = new SqlParameter("@age", age);
        //    prm[3] = new SqlParameter("@title", title);

        //    ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select * from testTBL where FirstName_TH='" + firstName + "' " );

        //    return Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
        //}
        /// <summary>
        ///  CitizenID ใช้ในการลบ farmer
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete]
        [Route("Delete/FarmerOid")]
        public HttpResponseMessage Delete_Farmer()
        {
            _Delete_Farmer delete_Farmer = new _Delete_Farmer();
            try

            {
                if (HttpContext.Current.Request.Form["Oid"].ToString() != null)
                {
                    delete_Farmer.Oid = HttpContext.Current.Request.Form["Oid"].ToString();
                }
                DataSet ds = new DataSet();

                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieDeleteFarmer_byOid", new SqlParameter("@Oid", delete_Farmer.Oid)
                   );
                if (ds.Tables[0].Rows[0]["pMessage"].ToString() == "ลบข้อมูลสำเร็จ")
                {

                    return Request.CreateResponse(HttpStatusCode.OK, " ลบข้อมูลแล้ว");
                }
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่เจอเลขบัตรประชาชนหรือไม่มีข้อมูล");

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

    }
}



//ใส่พารามิเตอร์
//  DataSet ds = new DataSet();
//    SqlDataReader dr; //ถ้าใช้ datareader จะอ่าน  role เดียว



//ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_Moblieinsert_RegisterFarmer"            
//     , new SqlParameter("@OrganizationOid", registerfarmer.OrganizationOid)
//    , new SqlParameter("@Citizen_ID", registerfarmer.CitizenID)
//   , new SqlParameter("@TitleOid", registerfarmer.TitleOid)
//   , new SqlParameter("@FirstName_TH", registerfarmer.FirstNameTH)
//   , new SqlParameter("@LastName_TH", registerfarmer.LastNameTH)
//   , new SqlParameter("@Birthdate", registerfarmer.BirthDay)
//   , new SqlParameter("@Gender", registerfarmer.Gender)
//   , new SqlParameter("@Tel", registerfarmer.Tel)
//   , new SqlParameter("@Email", registerfarmer.Email)
//   , new SqlParameter("@Address_No", registerfarmer.Address_No)
//   , new SqlParameter("@Address_buildingName", registerfarmer.Address_buildingName)
//   , new SqlParameter("@Address_moo", registerfarmer.Address_moo)
//   , new SqlParameter("@Address_Soi", registerfarmer.Address_Soi)
//   , new SqlParameter("@Address_Road", registerfarmer.Address_Road)
//   , new SqlParameter("@Address_provinces", registerfarmer.Address_provinces)
//   , new SqlParameter("@Address_districts", registerfarmer.Address_districts)
//   , new SqlParameter("@Address_subdistricts", registerfarmer.Address_subdistricts)
//    , new SqlParameter("@ZipCode", registerfarmer.ZipCode)                   
//   , new SqlParameter("@animalSupplie", registerfarmer.AnimalSupplie));
#endregion 