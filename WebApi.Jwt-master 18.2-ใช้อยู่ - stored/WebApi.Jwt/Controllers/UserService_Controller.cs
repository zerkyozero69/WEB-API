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
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using DevExpress.Utils.Extensions;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using nutrition.Module;

namespace WebApi.Jwt.Controllers
{
    public class UserService_Controller : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        //public HttpResponseMessage Approve_sendID()
        //{
        //    Approve_Model approve_Success = new Approve_Model();
        //    try
        //    {
        //        if (HttpContext.Current.Request.Form["Send_Code"].ToString() != null)
        //        {
        //            approve_Success.Send_Code = HttpContext.Current.Request.Form["Send_Code"].ToString();
        //        }

        //        if (HttpContext.Current.Request.Form["Send_No"].ToString() != null)
        //        {
        //            approve_Success.Send_No = HttpContext.Current.Request.Form["Send_No"].ToString();
        //        }
        //        if (HttpContext.Current.Request.Form["Send_Messengr"].ToString() != null)
        //        {
        //            approve_Success.Send_Messengr = HttpContext.Current.Request.Form["Send_Messengr"].ToString();
        //        }

        //        DataSet ds = new DataSet();
        //        ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieApproval_SendSeed", new SqlParameter("@SendNo", approve_Success.Send_No));
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            approve_Success.Send_No = 1;
        //            approve_Success.Send_Messengr = "อนุมัติ";

        //        }

        //    }
        //    catch (Exception)
        //    {
        //    }


        //}
        
        public Customer_Service Register_UserService(string Type_Name)
        {
            Customer_Service Customer_Info = new Customer_Service();
            try

            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(OrgeService));
                XafTypesInfo.Instance.RegisterEntity(typeof(RoleInfo));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                OrgeService OrgeServiceinfo;
                nutrition.Module.Province DLD;
                OrgeServiceinfo = ObjectSpace.FindObject<OrgeService>(new BinaryOperator("OrgeServiceName", Type_Name));
                if (Type_Name != null)
                {
                    Customer_Info.OrgeOid = OrgeServiceinfo.Oid;
                    Customer_Info.Name_Group = OrgeServiceinfo.OrgeServiceName;
                    Customer_Info.Address = OrgeServiceinfo.FullAddress;
                    Customer_Info.ProvinceOid = OrgeServiceinfo.ProvinceOid;
                    Customer_Info.ProvinceNameTH = OrgeServiceinfo.ProvinceOid.ProvinceNameTH;
                    Customer_Info.OrgeID = OrgeServiceinfo.OrgeServiceID;
                    DLD = ObjectSpace.FindObject<nutrition.Module.Province>(new BinaryOperator("Oid", OrgeServiceinfo.ProvinceOid.DLDZone));
                    Customer_Info.DLD = OrgeServiceinfo.ProvinceOid.DLDZone.DLDAreaName;
                    Customer_Info.DistrictOid = OrgeServiceinfo.DistrictOid;
                    Customer_Info.DistrictNameTH = OrgeServiceinfo.DistrictOid.DistrictNameTH;
                    Customer_Info.SubDistrictOid = OrgeServiceinfo.SubDistrictOid;
                    Customer_Info.SubDistrictNameTH = OrgeServiceinfo.SubDistrictOid.SubDistrictNameTH;
                    Customer_Info.ZipCode = OrgeServiceinfo.ZipCode;
                    Customer_Info.Tel = OrgeServiceinfo.Tel;
                    Customer_Info.Status = 1;
                    

                }
                else
                {
                    Customer_Info.Status = 0;
                   
                }


            }
            catch (Exception ex)
            {
                Customer_Info.Status = 6;
                Customer_Info.Message = ex.Message;
            }
            return Customer_Info;
        }

        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ
        // [HttpPost] หน้าโมบาย
        [HttpPost]
        [Route("UserService")]
        public HttpResponseMessage Get_UserService()
        {
            Customer_Service Customer = new Customer_Service();
            try
            {
                string Type_Name =" ";
                if (HttpContext.Current.Request.Form["Type_Name"].ToString() != null)
                {
                    Type_Name = HttpContext.Current.Request.Form["Type_Name"].ToString();
                }
                UserService_Controller result = new UserService_Controller();

                Customer = result.Register_UserService( Type_Name);
               
                if (Customer.Status == 1)
                {
                    Customer.Message = "แสดงข้อมูลผู้รับบริการ";
                    return Request.CreateResponse(HttpStatusCode.OK, Customer);
                }
                else if (Customer.Status == 0)
                {
                    Customer.Message = "NoData";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, Customer);
                }



            }
            catch (Exception ex)
            {
                Customer.Status = 6;
                Customer.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, Customer);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Customer);
        }
    }
//        public  HttpResponseMessage  Approve_sendID2()
//        {
//            try
//            {


//                SelectedData data =   session.ExecuteSproc("TestProc", new OperandValue(123), new OperandValue("abc"));

//                XpoTypesInfoHelper.GetXpoTypeInfoSource();
//                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSend));

//                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
//                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
//                IList<SupplierSend> collection = ObjectSpace.GetObjects<SupplierSend>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));
//                if (collection.Count > 0)
//            }
//            catch
//        }
          

}
