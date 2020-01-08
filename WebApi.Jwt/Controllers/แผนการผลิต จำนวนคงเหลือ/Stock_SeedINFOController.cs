using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.นับจำนวนกิจกรรม;


namespace WebApi.Jwt.Controllers.แผนการผลิต_จำนวนคงเหลือ
{
    public class Stock_SeedINFOController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();


        /// <summary>
        /// เรียกเสบียงสัตว์คงเหลือ ตามศูนย์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierProductAmount/Count")]   ///SupplierProductAmount/Count
        public HttpResponseMessage Stockseedanimal()
        {
            try
            {
       

                string OrganizeOid = null; // Oid จังหวัด

                if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != null)
                {
                    if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != "")
                    {
                        OrganizeOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                    }
                }
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_Stock_count", new SqlParameter("@OrganizationOid", OrganizeOid));
             
                    List<Stock_info> titile_Groups = new List<Stock_info>();
                    Stock_info stock_Info = new Stock_info();
                    List<SeedAnimal_info> detail = new List<SeedAnimal_info>();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string Temp_Group_Name = "";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (Temp_Group_Name == dr["SeedLevelCode"].ToString())
                            {
                                SeedAnimal_info item = new SeedAnimal_info();
                                item.SeedName = dr["SeedName"].ToString();
                                item.TotalWeight = dr["TotalWeight"].ToString();
                                item.WeightUnit = dr["WeightUnit"].ToString();
                                //status.Add(item);
                                stock_Info.Detail.Add(item);
                            }
                            else
                            {
                                Temp_Group_Name = dr["SeedLevelCode"].ToString();
                                stock_Info = new Stock_info();
                                stock_Info.SeedLevelCode = dr["SeedLevelCode"].ToString();
                                SeedAnimal_info item = new SeedAnimal_info();
                                item.SeedName = dr["SeedName"].ToString();

                                item.TotalWeight = dr["TotalWeight"].ToString();
                                item.WeightUnit = dr["WeightUnit"].ToString();
                                //status.Add(item);
                                //Group_.Status_List = status;
                                stock_Info.Detail.Add(item);
                                titile_Groups.Add(stock_Info);
                            }

                        }
                        UserError err = new UserError();
                        err.code = ""; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                        err.message = "OK";
                        return Request.CreateResponse(HttpStatusCode.OK, titile_Groups);
                    }
                

                UserError err2 = new UserError();
                    err2.code = "0"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err2.message = "กรุณาระบุศูนย์";
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                

            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
    }
}
