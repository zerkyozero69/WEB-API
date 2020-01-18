using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using nutrition.Module;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class Get_listAnimalDetailController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        [AllowAnonymous]
        [HttpPost]
        [Route("AnimalSupplieTypeList")]
        public HttpResponseMessage AnimalSupplieType_list()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSupplieType));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<AnimalSupplieType> collection = ObjectSpace.GetObjects<AnimalSupplieType>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));

                if (collection.Count > 0)
                {
                    List<AnimalSupplieType_Model> list = new List<AnimalSupplieType_Model>();
                    foreach (AnimalSupplieType row in collection)
                    {
                        AnimalSupplieType_Model item = new AnimalSupplieType_Model();
                        item.AnimalSupplieTypeOid = row.Oid.ToString();
                        item.SupplietypeName = row.SupplietypeName;
                        item.AnimalSupplie = row.AnimalSupplie.AnimalSupplieName;
                        item.SalePrice = row.SalePrice;
                        list.Add(item);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "-1"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                    err.message = "ไม่พบข้อมูล";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
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
        /// list โควตาจัดสรร
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("QuotaType_list")]
        public HttpResponseMessage QuotaType_list()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(QuotaType));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<QuotaType> collection = ObjectSpace.GetObjects<QuotaType>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));

                if (collection.Count > 0)
                {
                    List<QuotaType_Model> list = new List<QuotaType_Model>();
                    foreach (QuotaType row in collection)
                    {
                        QuotaType_Model item = new QuotaType_Model();
                        item.QuotaTypeOid = row.Oid.ToString();
                        item.QuotaName = row.QuotaName;

                        list.Add(item);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "-1"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                    err.message = "ไม่พบข้อมูล";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
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
        #region แก้โควตา หญ้าแห้ง
        /// <summary>
        /// ///
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ManageSubAnimalSupplierList")]
        public HttpResponseMessage ManageSubAnimalSupplierList()
        {
            List<AnimalProductDetail> animalDatail = new List<AnimalProductDetail>();
            AnimalProductDetail animal = new AnimalProductDetail();
            try
            {
                string QuotaName = HttpContext.Current.Request.Form["QuotaName"].ToString();
                string OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                string AnimalSupplieOid = HttpContext.Current.Request.Form["AnimalSupplieOid"].ToString();
                string AnimalSupplieTypeOid = HttpContext.Current.Request.Form["AnimalSupplieTypeOid"].ToString(); 
                string QuotaTypeOid = HttpContext.Current.Request.Form["QuotaTypeOid"].ToString();

                List<QuotaType_Model> listquo = new List<QuotaType_Model>();
                QuotaType_Model quota = new QuotaType_Model();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(QuotaType));
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageAnimalSupplier));
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageSubAnimalSupplier));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                List<ManageAnimalSupplier_Model2> list = new List<ManageAnimalSupplier_Model2>();
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                QuotaType quotaType;
                //  var ManageSubAnimalSupplierOid = null;
                quotaType = ObjectSpace.FindObject<QuotaType>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and QuotaName  = '" + QuotaName + "' ", null));
                if (quotaType.QuotaName != "โควตาปศุสัตว์จังหวัด")
                {
                    ManageAnimalSupplier ObjManageAnimalSupplier = ObjectSpace.FindObject<ManageAnimalSupplier>(CriteriaOperator.Parse("[OrganizationOid]=? and Status=1", OrganizationOid));
                    if (ObjManageAnimalSupplier != null)
                    {
                        switch (quotaType.QuotaName)
                        {
                            case "โควตาสำนัก":
                                animal.QuotaName = "โควตาสำนัก";
                                animal.AnimalSupplieTypeOid = null;
                                animal.AnimalSupplieTypeName = "";
                                break;
                            case "โควตาศูนย์ฯ":
                                animal.QuotaName = "โควตาศูนย์ฯ";
                                animal.AnimalSupplieTypeOid = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).Oid.ToString();
                                animal.AnimalSupplieTypeName = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).SupplietypeName;
                                animal.QuotaQTY = ObjManageAnimalSupplier.CenterQTY;

                                break;
                            case "โควตาปศุสัตว์เขต":
                                animal.QuotaName = "โควตาปศุสัตว์เขต";
                                animal.AnimalSupplieTypeOid = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).Oid.ToString();
                                animal.AnimalSupplieTypeName = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).SupplietypeName;
                                animal.QuotaQTY = ObjManageAnimalSupplier.ZoneQTY;
                                break;
                            //case "โควตาปศุสัตว์จังหวัด":
                            //    animal.QuotaName = "โควตาปศุสัตว์จังหวัด";
                            //    animal.AnimalSupplieTypeOid = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).Oid.ToString();
                            //    animal.AnimalSupplieTypeName = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).SupplietypeName;
                            //    animal.QuotaQTY = ObjManageAnimalSupplier.ZoneQTY;
                            //    break;
                            case "โควตาอื่นๆ":
                                animal.QuotaName = "โควตาอื่นๆ";
                                animal.AnimalSupplieTypeOid = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).Oid.ToString();
                                animal.AnimalSupplieTypeName = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).SupplietypeName;
                                animal.QuotaQTY = ObjManageAnimalSupplier.OtherQTY;
                                break;
                                //    default: return animalDatail.QuotaName;                   
                        }
                        XPCollection<StockAnimalUseInfo> objStockAnimalUseInfo = new XPCollection<StockAnimalUseInfo>();
                        //var objOrganizationOid = ObjectSpace.FindObject<Organization>(CriteriaOperator.Parse("Oid=?", OrganizationOid));
                        //var objAnimalSupplieOid = AnimalSupplieOid;
                        //var objAnimalSupplieTypeOid = AnimalSupplieTypeOid;
                        //var objQuotaTypeOid = QuotaTypeOid;
                        //if (quotaType.Oid != null)
                        //{
                        //    objStockAnimalUseInfo.Criteria = CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and QuotaTypeOid=? and BudgetSourceOid=?", objOrganizationOid.Oid, objAnimalSupplieOid, objAnimalSupplieTypeOid, objQuotaTypeOid, BudgetSourceOid);
                        //}
                        //else
                        //{
                        //    objStockAnimalUseInfo.Criteria = CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and BudgetSourceOid=?", objOrganizationOid.Oid, objAnimalSupplieOid, objAnimalSupplieTypeOid, BudgetSourceOid.Oid);
                        //}
                        //List<GetBlance> GetStockRodBreedInfo = objStockAnimalUseInfo.GroupBy(Filed => Filed.AnimalSupplieOid).Select(TmpStockRodBreedInfo => new GetBlance() { Name = TmpStockRodBreedInfo.First().AnimalSupplieOid.AnimalSupplieName, Total = TmpStockRodBreedInfo.Sum(c => c.Weight).ToString() }).ToList();
                        if (quotaType.QuotaName == "โควตาสำนัก")
                        {
                            if (animal.AnimalSupplieTypeOid != null)
                            {
                                // ManageAnimalSupplier ObjManageAnimalSupplier = ObjectSpace.FindObject<ManageAnimalSupplier>(CriteriaOperator.Parse("[OrganizationOid]=? and Status=1", OrganizationOid));
                                switch (animal.AnimalSupplieTypeOid)
                                {
                                    case "หญ้าแห้ง":
                                        animal.QuotaQTY = ObjManageAnimalSupplier.OfficeQTY;
                                        break;
                                    case "หญ้าแห้ง GAP":
                                        animal.QuotaQTY = ObjManageAnimalSupplier.OfficeGAPQTY;
                                        break;
                                    case "ถั่วแห้ง":
                                        animal.QuotaQTY = ObjManageAnimalSupplier.OfficeBeanQTY;
                                        break;

                                }
                            }
                            else
                                animal.QuotaQTY = 0;
                            animal.Amount = 0;

                            quota.QuotaTypeOid = quotaType.Oid.ToString();
                            quota.QuotaName = quotaType.QuotaName;
                            // listquo.Add(quota);
                            animalDatail.Add(animal);
                            ManageAnimalSupplier collection = ObjectSpace.FindObject<ManageAnimalSupplier>(CriteriaOperator.Parse(" GCRecord is null and AnimalSupplieOid.AnimalSupplieName ='แห้ง' and OrganizationOid = '" + OrganizationOid + "' ", null));
                            ManageAnimalSupplier_Model2 item = new ManageAnimalSupplier_Model2();
                            item.ManageAnimalSupplierOid = collection.Oid.ToString();
                            item.FinanceYearOid = collection.FinanceYearOid.Oid.ToString();
                            item.FinanceYearName = collection.FinanceYearOid.YearName;
                            item.OrgZoneOid = collection.OrgZoneOid.Oid.ToString();
                            item.OrgZoneName = collection.OrgZoneOid.OrganizeNameTH;
                            item.ZoneQTY = collection.ZoneQTY.ToString();
                            item.CenterQTY = collection.CenterQTY.ToString();
                            item.OfficeQTY = collection.OfficeQTY.ToString();
                            item.OfficeGAPQTY = collection.OfficeGAPQTY.ToString();
                            item.OfficeBeanQTY = collection.OfficeBeanQTY.ToString();
                            item.OrganizationOid = collection.OrganizationOid.Oid.ToString();
                            item.objquota = listquo;
                            return Request.CreateResponse(HttpStatusCode.OK, animalDatail);
                        }
                        else
                        {
                            UserError err2 = new UserError();
                            err2.code = "0"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                            err2.message = "ไม่พบข้อมูล";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, err2);

                        }
                    }
                    else
                    {
                        UserError err2 = new UserError();
                        err2.code = "3"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                        err2.message = "ไม่พบข้อมูล";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err2);

                    }


                }
                else
                {
                    UserError err2 = new UserError();
                    err2.code = "2"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err2.message = "กรุณาระบุโควตา";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err2);

                }
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
        //public void  GetStockUsed()
        //{
        //    XPCollection<StockAnimalUseInfo> objStockAnimalUseInfo = new XPCollection<StockAnimalUseInfo>();
        //    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //    var objOrganizationOid = ObjectSpace.FindObject<Organization>(CriteriaOperator.Parse("[OrganizationOid] = ? ", OrganizationOid));
        //    var objAnimalSupplieOid = objAnimalSupplieOid;
        //    var objAnimalSupplieTypeOid = AnimalSupplieTypeOid;
        //    var objQuotaTypeOid = QuotaTypeOid;
        //    var objManageSubAnimalSupplierOid = ManageSubAnimalSupplierOid;
        //    return objgetStockUsed;
        //}
        #endregion
        public class GetBlance
        {
            public GetBlance()
            {
            }

            public decimal Total { get; set; }
            public string Name { get; set; }
        }

    }
}
