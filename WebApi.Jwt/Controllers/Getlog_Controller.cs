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
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WebApi.Jwt.Controllers
{
    public class Getlog_Controller : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        
        // [JwtAuthentication]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetLog")]
        public HttpResponseMessage GetLog()
        {
            try
            {

                DataSet ds = new DataSet();

                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Get_LogEvent");
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        row = new Dictionary<string, object>();
                //        foreach (DataColumn col in dt.Columns)
                //        {
                //            row.Add(col.ColumnName, dr[col]);
                //        }
                //        rows.Add(row);
                //    }
                //    return Request.CreateResponse(HttpStatusCode.OK, rows);

                //}
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ToString() == "No")
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        else if (col.ToString() == "EventName")
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                    }
                    rows.Add(row);
                }
                return Request.CreateResponse(HttpStatusCode.OK, rows);

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
        /// <summary>
        /// เรียกรายชื่อเกษตรกร จากเลขบัตรประชาชนทั้งหมด
        /// </summary>
        ///
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("FarmerCitizenID/all")]
        public HttpResponseMessage get_AllCitizenID()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileGetFarmer_ALL"); 
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
        /// <summary>
        /// เรียกรายชื่อเกษตรกร จากเลขบัตรประชาชน 1 คน
        /// </summary>
        /// <param name="citizenID"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("FarmerCitizenID")]
        public HttpResponseMessage get_CitizenID()
        {
            FarmerCitizen farmer_info = new FarmerCitizen();
            
            try
            {
                string CitizenID=string.Empty ;
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
                    farmer_info.genderName = farmerResul["genderName"];
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
       


                    return Request.CreateResponse(HttpStatusCode.OK, farmer_info);

                }
                else {
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






        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpGet]
        [Route("Organization")]
        public HttpResponseMessage get_Organization()
        {
            try
            {

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_GetOrganization");
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
                if (rows != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, rows);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No Data");
            

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

    }
}
        
    








