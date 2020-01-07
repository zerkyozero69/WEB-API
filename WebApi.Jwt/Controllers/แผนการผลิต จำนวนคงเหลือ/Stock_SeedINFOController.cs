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

namespace WebApi.Jwt.Controllers.แผนการผลิต_จำนวนคงเหลือ
{
    public class Stock_SeedINFOController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Stockseedanimal()
        {
            try
            {
                string OrganizeOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                string YearName = HttpContext.Current.Request.Form["YearName"].ToString();
                DataSet ds  = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_Stock_count", new SqlParameter("@OrganizationOid", OrganizeOid)
                    ,new SqlParameter("@Yearname", YearName));

            }
            catch (Exception ex)
            {

            }
        }
    }
}
