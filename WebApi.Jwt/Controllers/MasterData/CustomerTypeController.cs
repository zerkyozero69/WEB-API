﻿using System.Web.Http;
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
using System.Globalization;

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
        [Route("GetCustomerList")]
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
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpGet    ]
        [Route("GetCusServiceList")]
        public IHttpActionResult RegisterCusService()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(RegisterCusService));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                IList<RegisterCusService> collection = ObjectSpace.GetObjects<RegisterCusService>(CriteriaOperator.Parse(" IsActive = 1 and GCRecord is null", null));
                List<RegisterCusService_Model> list = new List<RegisterCusService_Model>();
                foreach (RegisterCusService row in collection)
                {
                    RegisterCusService_Model Model = new RegisterCusService_Model();
                    Model.Oid = row.Oid.ToString();
                    Model.Organization = row.OrganizationOid.SubOrganizeName;
                    Model.RegisterDate = row.RegisterDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                    Model.CitizenID = row.CitizenID;
                    Model.Title = row.TitleOid.TitleName;
                    Model.FirstNameTH = row.FirstNameTH;
                    Model.LastNameTH = row.LastNameTH;
                    Model.Gender = row.GenderOid.GenderName;
                    if (row.BirthDate == null)
                    {
                        Model.BirthDate = "ไม่พบข้อมูลวันเกิด";
                    }
                    else
                    {
                        Model.BirthDate = row.BirthDate.ToString();
                    }
                    if (row.Tel == null)
                    {
                        Model.Tel = "ไม่พบข้อมูลเบอร์โทร";
                    }
                    else
                    {
                        Model.Tel = row.Tel.ToString();
                    }
                    if (row.Email == null)
                    {
                        Model.Email = "ไม่พบข้อมูลอีเมลล์";
                    }
                    else
                    {
                        Model.Email = row.Email.ToString();
                    }

                    if (row.Address == null)
                    {
                        Model.Address = "ไม่พบข้อมูลบ้านเลขที่";
                    }
                    else
                    {
                        Model.Address = row.Address.ToString();
                    }

                    if (row.Moo == null)
                    {
                        Model.Moo = "ไม่พบข้อมูลหมู่";
                    }
                    else
                    {
                        Model.Moo = row.Moo.ToString();
                    }
                    if (row.Soi == null)
                    {
                        Model.Soi = "ไม่พบข้อมูลซอย";
                    }
                    else
                    {
                        Model.Soi = row.Soi.ToString();
                    }

                    if (row.Road == null)
                    {
                        Model.Road = "ไม่พบข้อมูลถนน";
                    }
                    else
                    {
                        Model.Road = row.Road.ToString();
                    }

                    Model.Province = row.ProvinceOid.ProvinceNameTH;
                    if (row.DistrictOid == null)
                    {
                        Model.District = "ไม่พบข้อมูลอำเภอ";
                    }
                    else
                    {
                        Model.District = row.DistrictOid.DistrictNameTH;
                    }
                    if (row.SubDistrictOid == null)
                    {

                        Model.SubDistrict  = "ไม่พบข้อมูลตำบล";
                    }
                    else
                    {

                        Model.SubDistrict = row.SubDistrictOid.SubDistrictNameTH;
                    }
                    if (row.ZipCode == null)
                    {

                        Model.ZipCode = "ไม่พบข้อมูลรหัสไปรษณีย์";
                    }
                    else
                    {

                        Model.ZipCode = row.ZipCode;
                    }


              
                    list.Add(Model);

                }
                return Ok(list);

                
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }
        //public IHttpActionResult OrgeService()
        //{
        //    try
        //    {
        //    }
        //}


}
        
        
}
