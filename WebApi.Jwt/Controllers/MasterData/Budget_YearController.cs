﻿using System;
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

namespace WebApi.Jwt.Controllers.MasterData
{
    public class Budget_YearController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();


        /// <summary>
        /// รายละเอียดงบประมาณ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Budget/Year")]
        public HttpResponseMessage GetBudget_Year()
        {

            try

            {

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(FinanceYear));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<FinanceYear> collection = ObjectSpace.GetObjects<FinanceYear>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1", null));

                if (collection.Count > 0)
                {
                    List<FinanceYearModel> list = new List<FinanceYearModel>();
                    foreach (FinanceYear row in collection)
                    {
                        FinanceYearModel Finance = new FinanceYearModel();
                        Finance.FinanceYearOid = row.Oid.ToString();
                        Finance.FinanceYear = row.YearName;
                        list.Add(Finance);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
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
        /// รายละเอียด งบประมาณ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost   ]
        [Route("Budget/info")]
        public HttpResponseMessage  GetBudget_Info()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(BudgetSource));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<BudgetSource> collection = ObjectSpace.GetObjects<BudgetSource>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));

                if (collection.Count > 0)
                {
                    List<BudgetSourceModel> list = new List<BudgetSourceModel>();
                    foreach (BudgetSource row in collection)
                    {
                        BudgetSourceModel budget_type = new BudgetSourceModel();
                        budget_type.Oid = row.Oid;
                        budget_type.BudgetName = row.BudgetName;
                        list.Add(budget_type);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
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
    }
}

        
    




