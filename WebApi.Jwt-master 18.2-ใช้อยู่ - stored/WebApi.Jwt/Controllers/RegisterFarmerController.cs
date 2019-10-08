using System;
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
using static WebApi.Jwt.Models.Farmer;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
                            public HttpResponseMessage RegisterFarmer()
                            {
                                _Registerfarmer Registerfarmer = new _Registerfarmer();
                                try
                                {
                                    string requestString = Request.Content.ReadAsStringAsync().Result;
                                    JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);

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
                                            Registerfarmer.BirthDay = jObject.SelectToken("BirthDay").Value<DateTime>();

                                            Registerfarmer.Gender = jObject.SelectToken("Gender").Value<string>();
                                            Registerfarmer.Tel = jObject.SelectToken("Tel").Value<string>();
                                            Registerfarmer.Email = jObject.SelectToken("Email").Value<string>();
                                            if (jObject.SelectToken("Email") == null)
                                            {
                                                Registerfarmer.Email = null;
                                            }
                                            Registerfarmer.Address_No = jObject.SelectToken("Address_No").Value<string>();                                     
                                         
                                            Registerfarmer.Address_moo = jObject.SelectToken("Address_moo").Value<string>();
                                            Registerfarmer.Address_Soi = jObject.SelectToken("Address_Soi").Value<string>();
                                             if (jObject.SelectToken("Address_Soi")==null) {
                                            Registerfarmer.Address_Soi = null;
                                        }
                                            Registerfarmer.Address_Road = jObject.SelectToken("Address_Road").Value<string>();
                                            Registerfarmer.Address_provinces = jObject.SelectToken("Address_provinces").Value<string>();
                                            Registerfarmer.Address_districts = jObject.SelectToken("Address_districts").Value<string>();
                                            Registerfarmer.Address_subdistricts = jObject.SelectToken("Address_subdistricts").Value<string>();
                                            Registerfarmer.ZipCode = jObject.SelectToken("ZipCode").Value<string>();
                                            Registerfarmer.AnimalSupplie = jObject.SelectToken("AnimalSupplie").Value<string>();
                                            Registerfarmer.Latitude = jObject.SelectToken("Latitude").Value<float>();
                                            Registerfarmer.Longitude = jObject.SelectToken("Longitude").Value<float>();
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
                                    prm[5] = new SqlParameter("@Birthdate", Registerfarmer.BirthDay);
                                    prm[6] = new SqlParameter("@Gender", Registerfarmer.Gender);
                                    prm[7] = new SqlParameter("@Tel", Registerfarmer.Tel);
                                    prm[8] = new SqlParameter("@Email", Registerfarmer.Email);
                                    prm[9] = new SqlParameter("@Address_No", Registerfarmer.Address_No);
                                    prm[10] = new SqlParameter("@Address_moo", Registerfarmer.Address_moo);
                                    prm[11] = new SqlParameter("@Address_Soi", Registerfarmer.Address_Soi);
                                    prm[12] = new SqlParameter("@Address_Road", Registerfarmer.Address_Road);
                                    prm[13] = new SqlParameter("@Address_provinces", Registerfarmer.Address_provinces);
                                    prm[14] = new SqlParameter("@Address_districts", Registerfarmer.Address_districts);
                                    prm[15] = new SqlParameter("@Address_subdistricts", Registerfarmer.Address_subdistricts);
                                    prm[16] = new SqlParameter("@ZipCode", Registerfarmer.ZipCode);
                                    prm[17] = new SqlParameter("@animalSupplie", Registerfarmer.AnimalSupplie);
                                    prm[18] = new SqlParameter("@Latitude", Registerfarmer.Latitude);
                                    prm[19] = new SqlParameter("@Longitude", Registerfarmer.Longitude);
                                     prm[20] = new SqlParameter("@Register_Type", Registerfarmer.Register_Type);
                            
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_Moblieinsert_RegisterFarmer", prm);
                                    DataTable dt = new DataTable();
                                    dt = ds.Tables[0];

                                    //    using (DataSet ds = SqlHelper.ExecuteDataset(scc, "spt_Moblieinsert_RegisterFarmer", ))

                                    if (ds.Tables[0].Rows[0]["pStatus"].ToString() != "0")
                                    {

                                        Farmer_Status Farmer = new Farmer_Status();
                                        Farmer.Message = "ลงทะเบียนสำเร็จ ";

                                        Farmer.Fullname = @Registerfarmer.FirstNameTH + "  " + @Registerfarmer.LastNameTH;
                                        Farmer.CitizenID = "รหัสบัตรประชาชน" + " = " + @Registerfarmer.CitizenID;


                                        return Request.CreateResponse(HttpStatusCode.OK, Farmer);
                                    }
                                    else if (ds.Tables[0].Rows[0]["pStatus"].ToString() == "0")
                                    {
                                        UserError err = new UserError();
                                        err.code = "2"; // กรอกข้อมูลซ้ำ
                                        err.message = "บุคคลนี้เคยลงทะเบียนไปแล้ว โปรดตรวจสอบ ";
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                                    }

                                    else
                                    {
                                        UserError err = new UserError();
                                        err.code = "3"; // กรอกข้อมูลไม่ครบ
                                        err.message = "กรอกข้อมูลไม่ครบ ";
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
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
              
                if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != null)
                {
                    Updatefarmer.OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                }
                if (HttpContext.Current.Request.Form["CitizenID"] != null)
                {
                    Updatefarmer.CitizenID = Convert.ToInt64(HttpContext.Current.Request.Form["CitizenID"]);
                }
                if (HttpContext.Current.Request.Form["TitleOid"].ToString() != null)
                {
                    Updatefarmer.TitleOid = HttpContext.Current.Request.Form["TitleOid"].ToString();
                }
                if (HttpContext.Current.Request.Form["FirstNameTH"].ToString() != null)
                {
                    Updatefarmer.FirstNameTH = HttpContext.Current.Request.Form["FirstNameTH"].ToString();
                }
                if (HttpContext.Current.Request.Form["LastNameTH"].ToString() != null)
                {
                    Updatefarmer.LastNameTH = HttpContext.Current.Request.Form["LastNameTH"].ToString();
                }
                if (HttpContext.Current.Request.Form["BirthDay"] != null)
                {
                    Updatefarmer.BirthDay = Convert.ToDateTime(HttpContext.Current.Request.Form["BirthDay"]);
                }
                if (HttpContext.Current.Request.Form["Gender"].ToString() != null)
                {
                    Updatefarmer.Gender = HttpContext.Current.Request.Form["Gender"].ToString();
                }
                if (HttpContext.Current.Request.Form["Tel"].ToString() != null)
                {
                    Updatefarmer.Tel = HttpContext.Current.Request.Form["Tel"].ToString();
                }
                if (HttpContext.Current.Request.Form["Email"].ToString() != null)
                {
                    Updatefarmer.Email = HttpContext.Current.Request.Form["Email"].ToString();
                }
                ///
                if (HttpContext.Current.Request.Form["Address_No"].ToString() != null)
                {
                    Updatefarmer.Address_No = HttpContext.Current.Request.Form["Address_No"].ToString();
                }
    
                if (HttpContext.Current.Request.Form["Address_moo"].ToString() != null)
                {
                    Updatefarmer.Address_moo = HttpContext.Current.Request.Form["Address_moo"].ToString();
                }
                if (HttpContext.Current.Request.Form["Address_Soi"].ToString() != null)
                {
                    Updatefarmer.Address_Soi = HttpContext.Current.Request.Form["Address_Soi"].ToString();
                }
                if (HttpContext.Current.Request.Form["Address_Road"].ToString() != null)
                {
                    Updatefarmer.Address_Road = HttpContext.Current.Request.Form["Address_Road"].ToString();
                }
                if (HttpContext.Current.Request.Form["Address_provinces"].ToString() != null)
                {
                    Updatefarmer.Address_provinces = HttpContext.Current.Request.Form["Address_provinces"].ToString();
                }
                if (HttpContext.Current.Request.Form["Address_districts"].ToString() != null)
                {
                    Updatefarmer.Address_districts = HttpContext.Current.Request.Form["Address_districts"].ToString();
                }
                if (HttpContext.Current.Request.Form["Address_subdistricts"].ToString() != null)
                {
                    Updatefarmer.Address_subdistricts = HttpContext.Current.Request.Form["Address_subdistricts"].ToString();
                }
                if (HttpContext.Current.Request.Form["ZipCode"].ToString() != null)
                {
                    Updatefarmer.ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                }
                if (HttpContext.Current.Request.Form["AnimalSupplie"].ToString() != null)
                {
                    Updatefarmer.AnimalSupplie = HttpContext.Current.Request.Form["AnimalSupplie"].ToString();
                }
                if (HttpContext.Current.Request.Form["Latitude"] != null)
                {
                    Updatefarmer.Latitude = float.Parse(HttpContext.Current.Request.Form["Latitude"]);
                }
                if (HttpContext.Current.Request.Form["Longitude"]!=null)
                {
                    Updatefarmer.Longitude = float.Parse(HttpContext.Current.Request.Form["Longitude"]);
                }
                Updatefarmer.Register_Type = 2;
                

                DataSet ds = new DataSet();
                SqlParameter[] prm = new SqlParameter[21];                
                prm[0] = new SqlParameter("@OrganizationOid", Updatefarmer.OrganizationOid);
                prm[1] = new SqlParameter("@Citizen_ID", Updatefarmer.CitizenID);
                prm[2] = new SqlParameter("@TitleOid", Updatefarmer.TitleOid);
                prm[3] = new SqlParameter("@FirstName_TH", Updatefarmer.FirstNameTH);
                prm[4] = new SqlParameter("@LastName_TH", Updatefarmer.LastNameTH);
                prm[5] = new SqlParameter("@Birthdate", Updatefarmer.BirthDay);
                prm[6] = new SqlParameter("@Gender", Updatefarmer.Gender);
                prm[7] = new SqlParameter("@Tel", Updatefarmer.Tel);
                prm[8] = new SqlParameter("@Email", Updatefarmer.Email);
                prm[9] = new SqlParameter("@Address_No", Updatefarmer.Address_No);
                prm[10] = new SqlParameter("@Address_moo", Updatefarmer.Address_moo);
                prm[11] = new SqlParameter("@Address_Soi", Updatefarmer.Address_Soi);
                prm[12] = new SqlParameter("@Address_Road", Updatefarmer.Address_Road);
                prm[13] = new SqlParameter("@Address_provinces", Updatefarmer.Address_provinces);
                prm[14] = new SqlParameter("@Address_districts", Updatefarmer.Address_districts);
                prm[15] = new SqlParameter("@Address_subdistricts", Updatefarmer.Address_subdistricts);
                prm[16] = new SqlParameter("@ZipCode", Updatefarmer.ZipCode);
                prm[17] = new SqlParameter("@animalSupplie", Updatefarmer.AnimalSupplie);
                prm[18] = new SqlParameter("@Latitude", Updatefarmer.Latitude);
                prm[19] = new SqlParameter("@Longitude", Updatefarmer.Longitude);
                prm[20] = new SqlParameter("@Register_Type", Updatefarmer.Register_Type );
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_Moblieinsert_RegisterFarmer", prm);
                if (prm[1].Value != null)
                {
                    if (ds.Tables[0].Rows[0]["pStatus"].ToString() != "0")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "[" + Updatefarmer.@FirstNameTH + "  " + Updatefarmer.@LastNameTH + "] " + "แก้ไขข้อมูลสำเร็จแล้ว");
                    }
                    else if (ds.Tables[0].Rows[0]["pStatus"].ToString() == "0")
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "กรอกข้อมูลผิด");
                    }
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่มีข้อมูล");
                    }
                }
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "กรุณาใส่เลขบัตร");
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
                return Request.CreateResponse(HttpStatusCode.BadRequest,"ว๊ายไม่ลง");            }

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