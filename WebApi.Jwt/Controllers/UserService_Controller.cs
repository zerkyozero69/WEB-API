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
        [Route("Customer/info")]
        public HttpResponseMessage RegisterCustomer()
        {

            try

            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(OrgeService));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<OrgeService_info> list = new List<OrgeService_info>();
                IList<OrgeService> collection = ObjectSpace.GetObjects<OrgeService>(CriteriaOperator.Parse("GCRecord is null and IsActive=1 ", null));
                if (collection.Count > 0)
                {
                    foreach (OrgeService row in collection)
                    {
                        OrgeService_info Customer_Info = new OrgeService_info();
                        Customer_Info.OrgeOid = row.Oid;
                        //Customer_Info.OrganizationOid = row.OrganizationOid.OrganizeNameTH;
                        Customer_Info.OrgeServiceID = row.OrganizationOid.SubOrganizeName;
                        Customer_Info.OrgeServiceName = row.OrgeServiceName;
                        Customer_Info.Tel = row.Tel;
                        Customer_Info.Email = row.Email;
                        Customer_Info.Address = row.Address;
                        Customer_Info.Moo = row.Moo;
                        Customer_Info.Soi = row.Soi;
                        Customer_Info.Road = row.Road;
                        Customer_Info.ProvinceOid = row.ProvinceOid.ProvinceNameTH;
                        Customer_Info.DistrictOid = row.DistrictOid.DistrictNameTH;
                        Customer_Info.SubDistrictOid = row.SubDistrictOid.SubDistrictNameTH;
                        Customer_Info.ZipCode = row.ZipCode;
                        Customer_Info.FullAddress = row.FullAddress;
                        Customer_Info.IsActive = row.Tel;
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
