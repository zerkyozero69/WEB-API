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
using DevExpress.Xpo;
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
                IList<SendOrderSupplierAnimal> collection = ObjectSpace.GetObjects<SendOrderSupplierAnimal>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 2 and SendOrgOid=?", SendOrgOid));
                if (collection.Count > 0)
                {
                    foreach (SendOrderSupplierAnimal row in collection)
                    {
                        SendOrderSupplierAnimal_info SupplierAnimal = new SendOrderSupplierAnimal_info();
                        SupplierAnimal.SendNo = row.SendNo;
                        SupplierAnimal.SendDate = row.SendDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); ;
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
                        SupplierAnimal.SendDate = row.SendDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); ;
                        SupplierAnimal.FinanceYear = row.FinanceYearOid.YearName;
                        SupplierAnimal.SendOrgOid = row.SendOrgOid.Oid;
                        SupplierAnimal.SendOrgName = row.SendOrgOid.SubOrganizeName;
                        SupplierAnimal.ReceiveOrgOid = row.ReceiveOrgOid.Oid;
                        SupplierAnimal.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                        SupplierAnimal.CancelMsg = row.CancelMsg;
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
        [HttpPost  ]
        [Route("SupplierAnimalProduct/accept")]
        public IHttpActionResult LoadSupplierAnimalProduct()
        {
            object OrganizationOid;
            try
            {

                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();


                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierAnimalProduct));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSendDetail));
                List<SupplierAnimalProduct_info> list_detail = new List<SupplierAnimalProduct_info>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SupplierAnimalProduct> collection = ObjectSpace.GetObjects<SupplierAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null and Stauts = 2 and OrganizationOid=?", OrganizationOid));
                double Weight = 0;
                if (OrganizationOid != null)
                {
                    foreach (SupplierAnimalProduct row in collection)
                    {

                        SupplierAnimalProduct_info SupplierAnimal = new SupplierAnimalProduct_info();
                        SupplierAnimal.SupplierAnimalNumber = row.SupplierAnimalNumber;
                        SupplierAnimal.FinanceYear = row.FinanceYearOid.YearName;
                        SupplierAnimal.BudgetSource = row.BudgetSourceOid.BudgetName;
                        SupplierAnimal.OrganizationName = row.OrganizationOid.SubOrganizeName;
                        SupplierAnimal.AnimalSupplie = row.AnimalSupplieOid.AnimalSupplieName;
                        SupplierAnimal.AnimalSeed = row.AnimalSeedOid.SeedName;
                        SupplierAnimal.PlotInfoOid = row.PlotInfoOidOid.PlotCode;
                        SupplierAnimal.Weight = row.Weight.ToString();
                        SupplierAnimal.Unit = row.UnitOid.UnitName;
                        SupplierAnimal.AnimalSupplieType = row.AnimalSupplieTypeOid.SupplietypeName;
                        SupplierAnimal.Area = row.Area.ToString();
                        SupplierAnimal.ManufactureDate = row.ManufactureDate.ToString();

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
        [Route("LoadSupplierUseProduct/{No}")] // ใส่ OIDSendOrderSeed ใบนำส่ง /SendOrder/226-0011
        public IHttpActionResult SendOrderSeedDetail_ByOrderSeedID()
        {
            object SupplierAnimalNumber = string.Empty;
            object OrganizationOid = string.Empty;
            SupplierAnimalProduct_info sendDetail = new SupplierAnimalProduct_info();

            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                if (HttpContext.Current.Request.Form["No"].ToString() != null)
                {
                    SupplierAnimalNumber = HttpContext.Current.Request.Form["No"].ToString();
                }
                if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != null)
                {
                    OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                }
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                List<Approve_Model> list = new List<Approve_Model>();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SupplierAnimalProduct SupplierUseProduct_;
                SupplierUseProduct_ = ObjectSpace.FindObject<SupplierAnimalProduct>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and SupplierAnimalNumber=? and OrganizationOid=? ", SupplierAnimalNumber, OrganizationOid));
                //sendOrderSeed = ObjectSpace.GetObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", null));
                if (SupplierAnimalNumber != null)
                {
                    sendDetail.SupplierAnimalNumber = SupplierUseProduct_.SupplierAnimalNumber;
                    sendDetail.FinanceYear = SupplierUseProduct_.FinanceYearOid.YearName;
                    sendDetail.BudgetSource = SupplierUseProduct_.BudgetSourceOid.BudgetName;
                    sendDetail.OrganizationName = SupplierUseProduct_.OrganizationOid.SubOrganizeName;
                    sendDetail.AnimalSupplie = SupplierUseProduct_.AnimalSupplieOid.AnimalSupplieName;
                    sendDetail.AnimalSeed = SupplierUseProduct_.AnimalSeedOid.SeedName.ToString();
                    sendDetail.PlotInfoOid = SupplierUseProduct_.PlotInfoOidOid.PlotName.ToString();
                    sendDetail.Weight = SupplierUseProduct_.Weight.ToString();
                    sendDetail.Unit = SupplierUseProduct_.UnitOid.UnitName.ToString();
                    sendDetail.Area = SupplierUseProduct_.Area.ToString();
                    sendDetail.ManufactureDate = SupplierUseProduct_.ManufactureDate.ToString();
                    //if (sendOrderSeed.CancelMsg == null)
                    //{
                    //    sendDetail.CancelMsg = "";
                    //}
                    //else
                    //{
                    //    sendDetail.CancelMsg = sendOrderSeed.CancelMsg;
                    //}

                    //foreach (SendOrderSeedDetail row in sendOrderSeed.SendOrderSeedDetails)
                    //{
                    //    SendOrderSeed_Model send_Detail = new SendOrderSeed_Model();
                    //    send_Detail.LotNumber = row.LotNumber.LotNumber;
                    //    send_Detail.WeightUnitOid = row.WeightUnitOid.UnitName;
                    //    send_Detail.AnimalSeedCode = row.AnimalSeedCode;
                    //    send_Detail.AnimalSeedLevel = row.AnimalSeedLevel;
                    //    send_Detail.AnimalSeeName = row.AnimalSeeName;
                    //    send_Detail.BudgetSourceOid = row.BudgetSourceOid.BudgetName;
                    //    send_Detail.Weight = row.Weight;
                    //    send_Detail.Used = row.Used.ToString();
                    //    send_Detail.SendOrderSeed = row.SendOrderSeed.SendNo;
                    //    send_Detail.AnimalSeedOid = row.AnimalSeedOid.SeedName;
                    //    send_Detail.AnimalSeedLevelOid = row.AnimalSeedLevelOid.SeedLevelName;
                    //    send_Detail.SeedTypeOid = row.SeedTypeOid.SeedTypeName;
                    //    send_Detail.Amount = row.Amount;
                    //    list_detail.Add(send_Detail);
                    //}
                    //sendDetail.objSeed = list_detail;
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
