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
using nutrition.Module;
using System.Globalization;
using static WebApi.Jwt.Models.Supplier;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class SendorderSuppilerAnimal_Controller : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// หน้าส่งเสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost ]
        [Route("SendOrderAnimal/accept")]
        public IHttpActionResult LoadSendAnimal_accept()
        {
            object SendOrgOid;
            try
            {

                SendOrgOid = HttpContext.Current.Request.Form["SendOrgOid"].ToString();


                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSupplierAnimal));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
                List<SendOrderSupplierAnimal_info> list = new List<SendOrderSupplierAnimal_info>();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SendOrderSupplierAnimal> collection = ObjectSpace.GetObjects<SendOrderSupplierAnimal>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 5 and SendOrgOid=?", SendOrgOid));
                if (collection.Count > 0)
                {
                                foreach (SendOrderSupplierAnimal row in collection)
                                {
                                    SendOrderSupplierAnimal_info SupplierAnimal = new SendOrderSupplierAnimal_info();
                                    SupplierAnimal.SendNo = row.SendNo;
                                    SupplierAnimal.SendDate = row.SendDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); 
                                    SupplierAnimal.FinanceYear = row.FinanceYearOid.YearName;
                                    SupplierAnimal.SendOrgOid = row.SendOrgOid.Oid;
                                    SupplierAnimal.SendOrgName = row.SendOrgOid.SubOrganizeName;
                                    SupplierAnimal.ReceiveOrgOid = row.ReceiveOrgOid.Oid;
                                    SupplierAnimal.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;                      
                                    SupplierAnimal.Remark = row.Remark;
                                    SupplierAnimal.Send_Messengr = row.SendStatus.ToString();
                                    SupplierAnimal.Weight = row.QTY;
                                    list.Add(SupplierAnimal);
                                }
                    return Ok(list);
                }

                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest("NoData");
                }
               
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// หน้ารับเสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ReceiveOrderAnimal/accept")]
        public IHttpActionResult LoadReceiveAnimal_accept()
        {
            object ReceiveOrgOid;
            try
            {
                ReceiveOrgOid = HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSupplierAnimal));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
                List<ReceiveOrderAnimal_info> list = new List<ReceiveOrderAnimal_info>();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SendOrderSupplierAnimal> collection = ObjectSpace.GetObjects<SendOrderSupplierAnimal>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 2 and ReceiveOrgOid=?", ReceiveOrgOid));
                if (collection.Count > 0)
                {
                    foreach (SendOrderSupplierAnimal row in collection)
                    {
                        ReceiveOrderAnimal_info SupplierAnimal = new ReceiveOrderAnimal_info();
                        SupplierAnimal.SendNo = row.SendNo;
                        SupplierAnimal.SendDate = row.SendDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); 
                        SupplierAnimal.FinanceYear = row.FinanceYearOid.YearName;                     
                        SupplierAnimal.ReceiveOrgOid = row.ReceiveOrgOid.Oid;
                        SupplierAnimal.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                        SupplierAnimal.SendOrgOid = row.SendOrgOid.Oid;
                        SupplierAnimal.SendOrgName = row.SendOrgOid.SubOrganizeName;
                        SupplierAnimal.CancelMsg = row.CancelMsg;
                        SupplierAnimal.Send_Messengr = row.SendStatus.ToString();
                        SupplierAnimal.Package =row.PackageOid.PackageName;
                        SupplierAnimal.Weight = row.QTY;
                        list.Add(SupplierAnimal);
                    }
                    return Ok(list);
                }

                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest(err.message);
                }

            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost ]
        [Route("SupplierUseAnimalProduct/accept")] ///การใช้เสบียงสัตว์
        public IHttpActionResult Get_SupplierUseAnimalProduct()
        {
            object OrganizationOid;
            string ActivityOid;
            try
            {

                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                ActivityOid = HttpContext.Current.Request.Form["ActivityOid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
                List<SupplierAnimalUseProduct_Model> list_detail = new List<SupplierAnimalUseProduct_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SupplierUseAnimalProduct> collection = ObjectSpace.GetObjects<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null and Stauts = 1  and OrganizationOid=? and ActivityOid = ? ", OrganizationOid,ActivityOid));
                double Weight = 0;
                if (OrganizationOid != null)
                {
                    foreach (SupplierUseAnimalProduct row in collection)
                    {

                        SupplierAnimalUseProduct_Model SupplierAnimal = new SupplierAnimalUseProduct_Model();
                        SupplierAnimal.Oid = row.Oid.ToString();
                        SupplierAnimal.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        SupplierAnimal.UseNo = row.UseNo;
                        SupplierAnimal.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        SupplierAnimal.FinanceYear = row.FinanceYearOid.YearName;
                        SupplierAnimal.OrganizationOid = row.OrganizationOid.Oid.ToString();
                        SupplierAnimal.Organization = row.OrganizationOid.SubOrganizeName;
                        if (row.EmployeeOid == null)
                        {
                            SupplierAnimal.EmployeeOid = "ไม่มีข้อมูลรายชื่อ";
                        }
                        else
                        {
                            SupplierAnimal.EmployeeOid = row.EmployeeOid.ToString();
                        }
                        
                        if (row.EmployeeOid  == null)
                        {
                            SupplierAnimal.Employee = "ไม่มีข้อมูลรายชื่อ";
                        }
                        else
                        {
                            SupplierAnimal.Employee = row.EmployeeOid.EmployeeFirstName + row.EmployeeOid.EmployeeLastName;
                        }
                            
                        SupplierAnimal.ActivityOid = row.ActivityOid.Oid.ToString();
                        SupplierAnimal.ActivityName = row.ActivityOid.ActivityName;
                       // SupplierAnimal.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        SupplierAnimal.SubActivityOid = row.SubActivityOid.Oid.ToString();
                        SupplierAnimal.SubActivityName = row.SubActivityOid.ActivityName;
                        SupplierAnimal.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                        SupplierAnimal.RegisCusService = row.RegisCusServiceOid.FirstNameTH + row.RegisCusServiceOid.LastNameTH;
                        if (row.OrgeServiceOid == null)
                        {
                            SupplierAnimal.OrgeServiceOid = "ไม่พบข้อมูลองค์กร";
                            SupplierAnimal.OrgeService = "ไม่พบข้อมูลองค์กร";
                        }
                        else
                        {
                            SupplierAnimal.OrgeServiceOid = row.OrgeServiceOid.Oid.ToString();

                        }
                        

                        

                        //foreach (SupplierUseProductDetail row2 in row.SupplierUseProductDetails)
                        //{
                        //    Weight = row2.Weight;
                        //}
                        //Supplier.Weight = Weight + " " + "กิโลกรัม";

                        list_detail.Add(SupplierAnimal);
                    }

                    return Ok(list_detail);
                }

                else if (list_detail == null)
                {
                    UserError err = new UserError();
                    err.code = "3"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest("รายการอนุมัติไม่สมบูรณ์");
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest(err.message);
                }
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("LoadSupplierUseProduct/{UseNo}")] // ใส่ OIDSendOrderSeed ใบนำส่ง
        public IHttpActionResult SendSupplierSeedDetail_ByOrderSeedID()
        {
            object UseNo = string.Empty;
            object OrganizationOid = string.Empty;
            string ActivityOid = string.Empty;
            SupplierAnimalUseProduct_Model sendDetail = new SupplierAnimalUseProduct_Model();

            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                if (HttpContext.Current.Request.Form["UseNo"].ToString() != null)
                {
                    UseNo = HttpContext.Current.Request.Form["UseNo"].ToString();
                }
                if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != null)
                {
                    OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                }
                if (HttpContext.Current.Request.Form["ActivityOid"].ToString()!=null)
                {
                    ActivityOid = HttpContext.Current.Request.Form["ActivityOid"].ToString();
                }
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                List<SupplierAnimalUseProduct_Model> list = new List<SupplierAnimalUseProduct_Model>();
                List<SupplierAnimalUseProductDetail_Model> list_detail = new List<SupplierAnimalUseProductDetail_Model>();
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SupplierUseAnimalProduct SupplierUseAnimalProduct_;
                SupplierUseAnimalProduct_ = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse("GCRecord is null and Stauts = 1  and OrganizationOid=? and ActivityOid = ? ", UseNo, OrganizationOid, ActivityOid));
                //sendOrderSeed = ObjectSpace.GetObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", null));
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select UseNo from SupplierUseAnimalProduct where UseNo = '" + UseNo + "'");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    sendDetail.Oid = SupplierUseAnimalProduct_.Oid.ToString();
                    sendDetail.UseNo = SupplierUseAnimalProduct_.UseNo;
                    sendDetail.UseDate = SupplierUseAnimalProduct_.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                    sendDetail.FinanceYearOid = SupplierUseAnimalProduct_.FinanceYearOid.ToString();
                    sendDetail.FinanceYear = SupplierUseAnimalProduct_.FinanceYearOid.YearName;
                    sendDetail.OrganizationOid = SupplierUseAnimalProduct_.OrganizationOid.ToString();
                    sendDetail.Organization = SupplierUseAnimalProduct_.OrganizationOid.SubOrganizeName;
                    if (SupplierUseAnimalProduct_.EmployeeOid == null)
                    {
                        sendDetail.EmployeeOid = "ไม่มีข้อมูลรายชื่อ";
                        sendDetail.Employee = "ไม่พบข้อมูล";
                    }
                    else
                    {
                        sendDetail.EmployeeOid = SupplierUseAnimalProduct_.EmployeeOid.ToString();
                        sendDetail.Employee = SupplierUseAnimalProduct_.EmployeeOid.EmployeeFirstName + SupplierUseAnimalProduct_.EmployeeOid.EmployeeLastName;
                    }
                  
                    sendDetail.ActivityOid = SupplierUseAnimalProduct_.ActivityOid.ToString();
                    sendDetail.ActivityName = SupplierUseAnimalProduct_.ActivityOid.ActivityName;
                    sendDetail.SubActivityOid = SupplierUseAnimalProduct_.SubActivityOid.ToString();
                    sendDetail.SubActivityName = SupplierUseAnimalProduct_.SubActivityOid.ActivityName;
                    sendDetail.Remark = SupplierUseAnimalProduct_.Remark;
                    sendDetail.ApproveDate = SupplierUseAnimalProduct_.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                    //SupplierAnimalUseProductDetail_Model list_detail = new SupplierAnimalUseProductDetail_Model();
                    //foreach (SupplierUseProductDetail row in SupplierUseProduct_.SupplierUseAnimalProductDetails)
                    {

                    }

                   

                    return Ok(sendDetail);
                }
                else
                {
                    return BadRequest("NoData");
                }
            }

            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }

        }

    }
}
