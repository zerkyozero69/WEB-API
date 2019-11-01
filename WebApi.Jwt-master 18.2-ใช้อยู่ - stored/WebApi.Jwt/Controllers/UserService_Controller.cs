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

        /// <summary>
        /// ใช้ในการเรียกหน่วยงานที่ขอรับบริการ
        /// </summary>
        /// <param name="Type_Name"></param>
        /// <returns></returns>
        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ
        // [HttpPost] หน้าโมบาย
        [HttpPost]
        [Route("OrgeService")]
        public HttpResponseMessage OrgeService(string Type_Name )
        {
        
            try

            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(OrgeService));
                XafTypesInfo.Instance.RegisterEntity(typeof(RoleInfo));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();            
                nutrition.Module.Province DLD;
                IList<OrgeService> collection = ObjectSpace.GetObjects <OrgeService> (CriteriaOperator.Parse("GCRecord is null and IsActive=1 and OrgeServiceName='"+ Type_Name + "' "
                    , Type_Name));
                if (collection.Count > 0)
                {
                    List<Customer_Service> list = new List<Customer_Service>();
                    foreach (OrgeService row in collection)
                    {
                        Customer_Service Customer_Info = new Customer_Service();
                        Customer_Info.OrgeOid = row.Oid;
                        Customer_Info.Name_Group = row.OrgeServiceName;
                        Customer_Info.Address = row.FullAddress;
                        Customer_Info.ProvinceOid = row.ProvinceOid.Oid;
                        Customer_Info.ProvinceNameTH = row.ProvinceOid.ProvinceNameTH;
                        Customer_Info.OrgeID = row.OrgeServiceID;
                        DLD = ObjectSpace.FindObject<nutrition.Module.Province>(new BinaryOperator("Oid", row.ProvinceOid.DLDZone));
                        Customer_Info.DLD = row.ProvinceOid.DLDZone.DLDAreaName;
                        Customer_Info.DistrictOid = row.DistrictOid.Oid;
                        Customer_Info.DistrictNameTH = row.DistrictOid.DistrictNameTH;
                        Customer_Info.SubDistrictOid = row.SubDistrictOid.Oid;
                        Customer_Info.SubDistrictNameTH = row.SubDistrictOid.SubDistrictNameTH;
                        Customer_Info.ZipCode = row.ZipCode;
                        Customer_Info.Tel = row.Tel;
                        Customer_Info.Status = 1;

                        list.Add(Customer_Info);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, list);
                    //string TempSubDistrict, TempDistrict;
                    //if (Customer_Info.Organization.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                    //{ TempSubDistrict = "แขวง"; }
                    //else
                    //{ TempSubDistrict = "ตำบล"; };

                    //if (Customer_Info.Organization.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                    //{ TempDistrict = "เขต"; }
                    //else { TempDistrict = "อำเภอ"; };

                    //Customer_Info.FullAddress = Customer_Info.Organization.Address + " หมู่ที่" + checknull(Customer_Info.Organization.Moo) + " ถนน" + checknull(Customer_Info.Organization.Road) + " " +
                    //TempSubDistrict + Customer_Info.Organization.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + Customer_Info.Organization.DistrictOid.DistrictNameTH + " " +
                    //"จังหวัด" + Customer_Info.Organization.ProvinceOid.ProvinceNameTH + " " + Customer_Info.Organization.DistrictOid.PostCode;

                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");

                }


            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// ใช้ในการเรียกหน่วยงานที่ขอรับบริการ
        /// </summary>
        /// <param name="Type_Name"></param>
        /// <returns></returns>
        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ
        // [HttpPost] หน้าโมบาย
        [HttpPost]
        [Route("Customer/info")]
        public HttpResponseMessage RegisterCustomer(string Type_Name)
        {

            try

            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(CustomerType));
                XafTypesInfo.Instance.RegisterEntity(typeof(RoleInfo));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
             
                IList<CustomerType> collection = ObjectSpace.GetObjects<CustomerType>(CriteriaOperator.Parse("GCRecord is null and IsActive=1 and TypeName='" + Type_Name + "' ", Type_Name));
                if (collection.Count > 0)
                {
                    List<Customer_Service> list = new List<Customer_Service>();
                    foreach (CustomerType row in collection)
                    {
                        //Customer_Service Customer_Info = new Customer_Service();
                        //Customer_Info.OrgeOid = row.Oid;
                        //Customer_Info.Name_Group = row.OrgeServiceName;
                        //Customer_Info.Address = row.FullAddress;
                        //Customer_Info.ProvinceOid = row.ProvinceOid.Oid;
                        //Customer_Info.ProvinceNameTH = row.ProvinceOid.ProvinceNameTH;
                        //Customer_Info.OrgeID = row.OrgeServiceID;
                        //DLD = ObjectSpace.FindObject<nutrition.Module.Province>(new BinaryOperator("Oid", row.ProvinceOid.DLDZone));
                        //Customer_Info.DLD = row.ProvinceOid.DLDZone.DLDAreaName;
                        //Customer_Info.DistrictOid = row.DistrictOid.Oid;
                        //Customer_Info.DistrictNameTH = row.DistrictOid.DistrictNameTH;
                        //Customer_Info.SubDistrictOid = row.SubDistrictOid.Oid;
                        //Customer_Info.SubDistrictNameTH = row.SubDistrictOid.SubDistrictNameTH;
                        //Customer_Info.ZipCode = row.ZipCode;
                        //Customer_Info.Tel = row.Tel;
                        //Customer_Info.Status = 1;

                       // list.Add(Customer_Info);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, list);
                    //string TempSubDistrict, TempDistrict;
                    //if (Customer_Info.Organization.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                    //{ TempSubDistrict = "แขวง"; }
                    //else
                    //{ TempSubDistrict = "ตำบล"; };

                    //if (Customer_Info.Organization.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                    //{ TempDistrict = "เขต"; }
                    //else { TempDistrict = "อำเภอ"; };

                    //Customer_Info.FullAddress = Customer_Info.Organization.Address + " หมู่ที่" + checknull(Customer_Info.Organization.Moo) + " ถนน" + checknull(Customer_Info.Organization.Road) + " " +
                    //TempSubDistrict + Customer_Info.Organization.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + Customer_Info.Organization.DistrictOid.DistrictNameTH + " " +
                    //"จังหวัด" + Customer_Info.Organization.ProvinceOid.ProvinceNameTH + " " + Customer_Info.Organization.DistrictOid.PostCode;

                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");

                }


            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        //[AllowAnonymous]
        ////[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ
        //// [HttpPost] หน้าโมบาย
        //[HttpPost]
        //[Route("UserService")]
        //public HttpResponseMessage Get_UserService()
        //{
        //    Customer_Service Customer = new Customer_Service();
        //    try
        //    {
        //        string Type_Name =" ";

        //        Type_Name = HttpContext.Current.Request.Form["Type_Name"].ToString();

        //        UserService_Controller result = new UserService_Controller();

        //        Customer = result.Register_UserService(Type_Name);

        //        if (Customer.Status == 1)
        //        {
        //            Customer.Message = "แสดงข้อมูลผู้รับบริการ";
        //            return Request.CreateResponse(HttpStatusCode.OK, Customer);
        //        }
        //        else if (Customer.Status == 0)
        //        {
        //            Customer.Message = "NoData";
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, Customer);
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        Customer.Status = 6;
        //        Customer.Message = ex.Message;
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, Customer);
        //    }
        //    return Request.CreateResponse(HttpStatusCode.BadRequest, Customer);
        //}
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


}
