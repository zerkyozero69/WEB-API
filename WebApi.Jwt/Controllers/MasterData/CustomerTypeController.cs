using System.Web.Http;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp;
using WebApi.Jwt.Models;
using DevExpress.Data.Filtering;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Base;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;
using DevExpress.ExpressApp.Model;
using System.Security.Cryptography;
using DevExpress.Persistent.Validation;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using DevExpress.ExpressApp.Security.Strategy;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using System.Collections.Generic;
using WebApi.Jwt.Controllers;
using nutrition.Module;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class CustomerTypeController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// ประเภทผู้รับบริการ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpGet]
        [Route("GetCustomerType")]
        public HttpResponseMessage GetCustomerType()
        {
          
            try


            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(CustomerType));            
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
           
                IList<CustomerType> collection = ObjectSpace.GetObjects<CustomerType>(CriteriaOperator.Parse(" IsActive = 1 and GCRecord is null", null));
                List<_CustomerType> list = new List<_CustomerType>();
                foreach (CustomerType row in collection)
                {
                    _CustomerType customerType = new _CustomerType();
                    customerType.Oid = row.Oid;
                    customerType.TypeName = row.TypeName;                         
                    customerType.Remark = row.Remark;
                    customerType.Message = "แสดงรายละเอียดผู้รับบริการ";
                    customerType.IsActive = row.IsActive;

                    list.Add(customerType);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);


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
            
}
        
        
}
