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
        /// <param name=</param>
        /// <returns></returns>
        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ /*ติดปัญหา*/
        // [HttpPost] 
        [HttpPost]
        [Route("SeachCustomer/info")]
        public HttpResponseMessage OrgeCustomer()
        {

            try

            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(OrgeService));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<OrgeService_info> list = new List<OrgeService_info>();
                List<OrgeServiceDetail_Model> list_detail = new List<OrgeServiceDetail_Model>();
                IList<OrgeService> collection = ObjectSpace.GetObjects<OrgeService>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 ", null));
                if (collection.Count > 0)
                {
                    foreach (OrgeService row in collection)
                    {
                        OrgeService_info Customer_Info = new OrgeService_info();

                        //Customer_Info.OrganizationOid = row.OrganizationOid.OrganizeNameTH;
                        //   Customer_Info.OrgeServiceID = row.or
                        Customer_Info.OrgeServiceName = row.OrgeServiceName;
                        Customer_Info.Tel = row.Tel;
                        if (row.Email == null)
                        {
                            Customer_Info.Email = "ไม่มีข้อมูลอีเมล์";
                        }
                        else
                        {
                            Customer_Info.Email = row.Email;
                        }
                        if (row.Address == null)
                        {
                            Customer_Info.Address = "ไม่มีข้อมูลบ้านเลขที่";
                        }
                        else
                        {
                            Customer_Info.Address = row.Address;
                        }

                        if (row.Moo == null)
                        {
                            Customer_Info.Moo = "ไม่มีข้อมูลหมู่";
                        }
                        else
                        {
                            Customer_Info.Moo = row.Moo;
                        }
                        if (row.Soi == null)
                        {

                            Customer_Info.Soi = "ไม่มีข้อมูลซอย";
                        }
                        else
                        {

                            Customer_Info.Soi = row.Soi;
                        }

                        if (row.Road == null)
                        {

                            Customer_Info.Road = "ไม่มีข้อมูลถนน";
                        }
                        else
                        {

                            Customer_Info.Road = row.Road;
                        }
                        if (row.ProvinceOid.ProvinceNameTH == null)
                        {

                            Customer_Info.ProvinceName = "ไม่มีข้อมูลจังหวัด";
                        }
                        else
                        {

                            Customer_Info.ProvinceName = row.ProvinceOid.ProvinceNameTH;
                        }
                        if (row.DistrictOid == null)
                        {

                            Customer_Info.DistrictName = "ไม่มีข้อมูลอำเภอ";
                        }
                        else
                        {

                            Customer_Info.DistrictName = row.DistrictOid.DistrictNameTH;
                        }
                        if (row.SubDistrictOid == null)
                        {

                            Customer_Info.SubDistrictName = "ไม่มีข้อมูลตำบล";
                        }
                        else
                        {

                            Customer_Info.SubDistrictName = row.SubDistrictOid.SubDistrictNameTH;
                        }

                        if (row.ZipCode == null)
                        {

                            Customer_Info.ZipCode = "ไม่มีข้อมูลรหัสไปรษณีย์";
                        }
                        else
                        {

                            Customer_Info.ZipCode = row.ZipCode;
                        }


                        string TempSubDistrict, TempDistrict;
                        if (row.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                        { TempSubDistrict = "แขวง"; }
                        else
                        { TempSubDistrict = "ตำบล"; };

                        if (row.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                        { TempDistrict = "เขต"; }
                        else { TempDistrict = "อำเภอ"; };

                        Customer_Info.FullAddress = row.Address + " หมู่ที่" + checknull(row.Moo) + " ถนน" + checknull(row.Road) + " " +
                        TempSubDistrict + row.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + row.DistrictOid.DistrictNameTH + " " +
                        "จังหวัด" + row.ProvinceOid.ProvinceNameTH + " " + row.DistrictOid.PostCode;



                        list.Add(Customer_Info);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, list);


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
        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ 
        ///ค้นหาด้วยชื่อ
        // [HttpPost] 
        [HttpPost]
        [Route("SeachCustomer/ID")]
        public HttpResponseMessage OrgeCustomer_All()
        {
            string OrgeServiceName = string.Empty;
            try


            {
                if (HttpContext.Current.Request.Form["OrgeServiceName"].ToString() != null)
                {
                    OrgeServiceName = HttpContext.Current.Request.Form["OrgeServiceName"].ToString();
                }


                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(OrgeService));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                OrgeService_info Customer_Info = new OrgeService_info();
                List<OrgeService_info> list = new List<OrgeService_info>();
                List<OrgeServiceDetail_Model> list_detail = new List<OrgeServiceDetail_Model>();
                OrgeService OrgeService_;
                OrgeService_ = ObjectSpace.FindObject<OrgeService>(CriteriaOperator.Parse("GCRecord is null and OrgeServiceName = ? ", OrgeServiceName));
                if (OrgeServiceName != null)
                {
                    

                    //Customer_Info.OrganizationOid = row.OrganizationOid.OrganizeNameTH;
                    //   Customer_Info.OrgeServiceID = row.or
                    Customer_Info.OrgeServiceName = OrgeService_.OrgeServiceName;
                    Customer_Info.Tel = OrgeService_.Tel;
                    if (OrgeService_.Email == null)
                    {
                        Customer_Info.Email = "ไม่มีข้อมูลอีเมล์";
                    }
                    else
                    {
                        Customer_Info.Email = OrgeService_.Email;
                    }
                    if (OrgeService_.Address == null)
                    {
                        Customer_Info.Address = "ไม่มีข้อมูลบ้านเลขที่";
                    }
                    else
                    {
                        Customer_Info.Address = OrgeService_.Address;
                    }

                    if (OrgeService_.Moo == null)
                    {
                        Customer_Info.Moo = "ไม่มีข้อมูลหมู่";
                    }
                    else
                    {
                        Customer_Info.Moo = OrgeService_.Moo;
                    }
                    if (OrgeService_.Soi == null)
                    {

                        Customer_Info.Soi = "ไม่มีข้อมูลซอย";
                    }
                    else
                    {

                        Customer_Info.Soi = OrgeService_.Soi;
                    }

                    if (OrgeService_.Road == null)
                    {

                        Customer_Info.Road = "ไม่มีข้อมูลถนน";
                    }
                    else
                    {

                        Customer_Info.Road = OrgeService_.Road;
                    }
                    if (OrgeService_.ProvinceOid == null)
                    {

                        Customer_Info.ProvinceName = "ไม่มีข้อมูลจังหวัด";
                    }
                    else
                    {

                        Customer_Info.ProvinceName = OrgeService_.ProvinceOid.ProvinceNameTH;
                    }
                    if (OrgeService_.DistrictOid == null)
                    {

                        Customer_Info.DistrictName = "ไม่มีข้อมูลอำเภอ";
                    }
                    else
                    {

                        Customer_Info.DistrictName = OrgeService_.DistrictOid.DistrictNameTH;
                    }
                    if (OrgeService_.SubDistrictOid == null)
                    {

                        Customer_Info.SubDistrictName = "ไม่มีข้อมูลตำบล";
                    }
                    else
                    {

                        Customer_Info.SubDistrictName = OrgeService_.SubDistrictOid.SubDistrictNameTH;
                    }

                    if (OrgeService_.ZipCode == null)
                    {

                        Customer_Info.ZipCode = "ไม่มีข้อมูลรหัสไปรษณีย์";
                    }
                    else
                    {

                        Customer_Info.ZipCode = OrgeService_.ZipCode;
                    }

                    string TempSubDistrict, TempDistrict;
                    if (OrgeService_.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                    { TempSubDistrict = "แขวง"; }
                    else
                    { TempSubDistrict = "ตำบล"; };

                    if (OrgeService_.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                    { TempDistrict = "เขต"; }
                    else { TempDistrict = "อำเภอ"; };

                    Customer_Info.FullAddress = OrgeService_.Address + " หมู่ที่" + checknull(OrgeService_.Moo) + " ถนน" + checknull(OrgeService_.Road) + " " +
                    TempSubDistrict + OrgeService_.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + OrgeService_.DistrictOid.DistrictNameTH + " " +
                    "จังหวัด" + OrgeService_.ProvinceOid.ProvinceNameTH + " " + OrgeService_.DistrictOid.PostCode;
                    foreach (OrgeServiceDetail row in OrgeService_.OrgeServiceDetails)
                    {
                        OrgeServiceDetail_Model Model = new OrgeServiceDetail_Model();
                        Model.OrgeServiceOid = row.OrgeServiceOid.OrgeServiceName;
                        Model.ServiceTypeOid = row.ServiceTypeOid.ServiceTypeName;
                        Model.SubServiceTypeOid = row.SubServiceTypeOid.ServiceTypeName;
                        list_detail.Add(Model);
                    }
                    Customer_Info.OrgeServiceDetails = list_detail;
                    return Request.CreateResponse(HttpStatusCode.OK, Customer_Info);
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

        public string checknull(object val)
        {
            string ret = "-";
            try
            {
                if (val != null || val.ToString() != string.Empty)
                {
                    ret = val.ToString();
                };
            }
            catch (Exception ex)
            {
                ret = "-";
            }
            return ret;
        }

    }
}
