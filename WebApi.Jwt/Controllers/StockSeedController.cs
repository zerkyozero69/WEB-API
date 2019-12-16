using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.Controllers
{
    public class StockSeedController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// เรียกสต็อคคงเหลือ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("GetStockSeed")]
        public IHttpActionResult GetStockSeed()
        {
            string OrganizationOid;
            string FinanceYearOid;
            try
            {


                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();

                FinanceYearOid = HttpContext.Current.Request.Form["FinanceYearOid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(StockSeedInfo));
                List<StockSeedInfo_Model> list_detail = new List<StockSeedInfo_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                IList<StockSeedInfo> collection = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse(" GCRecord is null and StockType = 1 and OrganizationOid=? and FinanceYearOid = ?", OrganizationOid,FinanceYearOid));
                double Weight = 0;

                foreach (StockSeedInfo row in collection)
                {
                    StockSeedInfo_Model stock = new StockSeedInfo_Model();
                    stock.Oid = row.Oid.ToString();
                    stock.StockDate = row.StockDate.ToString();
                    stock.OrganizationOid = row.OrganizationOid.Oid.ToString();
                    stock.Organization = row.OrganizationOid.SubOrganizeName;
                    stock.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                    stock.FinanceYear = row.FinanceYearOid.YearName;
                    stock.BudgetSourceOid = row.BudgetSourceOid.Oid.ToString();
                    stock.BudgetSource = row.BudgetSourceOid.BudgetName;
                    stock.AnimalSeedOid = row.AnimalSeedOid.Oid.ToString();
                    stock.AnimalSeed = row.AnimalSeedOid.SeedName;
                    stock.AnimalSeedLevelOid = row.AnimalSeedLevelOid.Oid.ToString();
                    stock.AnimalSeedLevel = row.AnimalSeedLevelOid.SeedLevelName;
                    stock.StockDetail = row.StockDetail;
                    stock.TotalForward = row.TotalForward;
                    stock.TotalChange = row.TotalChange;
                    stock.TotalWeight = row.TotalWeight;
                    stock.ReferanceCode = row.ReferanceCode;
                    stock.SeedTypeOid = row.SeedTypeOid.Oid.ToString();
                    stock.SeedType = row.SeedTypeOid.SeedTypeName;
                    list_detail.Add(stock);
                }
                return Ok(list_detail);



                //else
                // {
                // UserError err = new UserError();
                // err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                // err.message = "No data";
                // //  Return resual
                // return BadRequest(err.message);
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