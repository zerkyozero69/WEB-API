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

namespace WebApi.Jwt.Controllers
{
    public class Approve_Controller : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
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
          

}
