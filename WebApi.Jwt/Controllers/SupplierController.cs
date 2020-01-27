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
using static WebApi.Jwt.Models.Supplier;

namespace WebApi.Jwt.Controllers
{
    public class SupplierController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// ส่งเสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("SupplierSend")]
        public IHttpActionResult SupplierSend()
        {

            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSend));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<SupplierSend_Model> list = new List<SupplierSend_Model>();
                IList<SupplierSend> collection = ObjectSpace.GetObjects<SupplierSend>(CriteriaOperator.Parse("GCRecord is null "));
                if (collection.Count > 0)
                {

                    foreach (SupplierSend row in collection)
                    {
                        SupplierSend_Model Model = new SupplierSend_Model();
                        Model.SendNo = row.SendNo;
                        Model.CreateDate = row.CreateDate;
                        Model.FinanceYearOid = row.FinanceYearOid.YearName;
                        Model.OrganizationSendOid = row.OrganizationSendOid.OrganizeNameTH;
                        Model.OrganizationReceiveOid = row.OrganizationReceiveOid.OrganizeNameTH;
                        Model.Remark = row.Remark;
                        Model.SendStatusOid = row.SendStatusOid;
                        list.Add(Model);
                    }

                }

                else
                {
                    return BadRequest("NoData");
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public  IHttpActionResult SupplierProduct()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierProduct));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<SupplierProduct_Model> list = new List<SupplierProduct_Model>();
                IList<SupplierProduct> collection = ObjectSpace.GetObjects<SupplierProduct>(CriteriaOperator.Parse("GCRecord is null "));
                if (collection.Count > 0)
                {
                    foreach (SupplierProduct row in collection)
                    {
                        SupplierProduct_Model supplier = new SupplierProduct_Model();
                        supplier.LotNumber = row.LotNumber;
                        supplier.FinanceYearOid = row.FinanceYearOid.YearName;
                        supplier.BudgetSourceOid = row.BudgetSourceOid.BudgetName;
                        supplier.AnimalSeedOid = row.AnimalSeedOid.SeedName;
                        supplier.AnimalSeedLevelOid = row.AnimalSeedLevelOid.SeedLevelName;
                        supplier.PlotHeaderOid = row.PlotInfoOidOid.PlotName;
                        supplier.Weight = Convert.ToDouble(row.Weight);
                        supplier.UnitOid = row.UnitOid.UnitName;
                        supplier.LastCleansingDate = row.LastCleansingDate;
                        supplier.Stauts = row.Status.ToString();
                        supplier.Used = row.Used;
                        supplier.ReferanceUsed = row.ReferanceUsed;
                        supplier.PlotInfoOidOid = row.PlotInfoOidOid.PlotName;
                        supplier.FormType = row.FormType.ToString();
                        supplier.SeedTypeOid = row.SeedTypeOid.SeedTypeName;
                        list.Add(supplier);
                    }
                    return Ok(list);
                }
               
                else
                {
                    return BadRequest("NoData");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        }
    }

