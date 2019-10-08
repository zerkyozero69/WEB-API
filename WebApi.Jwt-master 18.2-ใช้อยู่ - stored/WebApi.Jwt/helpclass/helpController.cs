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

namespace WebApi.Jwt.helpclass
{
    public class helpController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();


        /// <summary>
        /// ฟังชั่นเช็ค user password ให้ถูกต้องตามรหัส xaf ที่สมัครไว้
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public WebApi.Jwt.Models.user.User_info CheckLogin_XAF(string Username, string Password) // value1 = Username, value2 = Password จาก class อื่น
        {
            user.User_info objUser_info = new user.User_info();
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                XafTypesInfo.Instance.RegisterEntity(typeof(RoleInfo));
                WebApi.Jwt.Models.user.member_info_Shot user2 = new WebApi.Jwt.Models.user.member_info_Shot();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                UserInfo User;
               nutrition.Module.Organization DLD;
                User = ObjectSpace.FindObject<UserInfo>(new BinaryOperator("UserName", Username));
                // UserInfo = ObjectSpace.FindObject<RoleInfo>(new BinaryOperator("Name", Username));
                PasswordCryptographer.EnableRfc2898 = true;
                PasswordCryptographer.SupportLegacySha512 = false;
                if (User.ComparePassword(Password) == true)
                {
                    objUser_info.User_Name = User.UserName;
                    objUser_info.DisplayName = User.DisplayName;
                    objUser_info.Organization = User.Organization.OrganizeNameTH;
                    objUser_info.SubOrganizeName = User.Organization.SubOrganizeName;
                    objUser_info.Tel = User.Organization.Tel;
                    objUser_info.E_Mail = User.Organization.Email;
                    objUser_info.Address = User.Organization.Address;
                    objUser_info.Moo = User.Organization.Moo;
                    objUser_info.Soi = User.Organization.Soi;
                    objUser_info.Road = User.Organization.Road;
                    if (objUser_info.ProvinceNameTH == "") {
                        objUser_info.ProvinceNameTH = "ไม่มีข้อมูลศูนย์";
                    }
                    else if (objUser_info.ProvinceNameTH != "")
                    {
                        objUser_info.ProvinceNameTH = User.Organization.ProvinceOid.ProvinceNameTH;
                    }
                    if (objUser_info.DistrictNameTH == "")
                    {
                        objUser_info.DistrictNameTH = "ไม่มีข้อมูลศูนย์";
                    }
                    else if (objUser_info.DistrictNameTH != "")
                    {
                        objUser_info.DistrictNameTH = User.Organization.DistrictOid.DistrictNameTH;
                    }
                    if (objUser_info.SubDistrictNameTH == "")
                    {
                        objUser_info.SubDistrictNameTH = "ไม่มีข้อมูลศูนย์";
                    }
                    else if (objUser_info.SubDistrictNameTH != "")
                    {
                        objUser_info.SubDistrictNameTH = User.Organization.SubDistrictOid.SubDistrictNameTH;
                    }
                   

                    DLD = ObjectSpace.FindObject<nutrition.Module.Organization>(new BinaryOperator("Oid", User.Organization.MasterOrganization));
                  
                    if (DLD == null) {
                        objUser_info.DLD = "ไม่มีเขต";
                    }
                    else if(DLD != null) {
                        objUser_info.DLD = DLD.OrganizeNameTH;
                    }
                    objUser_info.Mapcoordinates_Latitude = User.Organization.Latitude;
                    objUser_info.Mapcoordinates_Longitude = User.Organization.Longitude;
                    TokenController token = new TokenController();
                    objUser_info.Description = "ระบบ Login";
                    objUser_info.Token_key = token.Get(Username, Password);
                    objUser_info.Status = 1;
                    objUser_info.Message = "เข้าสู่ระบบสำเร็จ";


                    List<WebApi.Jwt.Models.user.Roles_info> objListRoles_info = new List<WebApi.Jwt.Models.user.Roles_info>();

                    foreach (RoleInfo row in User.UserRoles)
                    {
                        user.Roles_info objRoles_info = new user.Roles_info();
                        objRoles_info.Role_Name = row.Name;
                        objListRoles_info.Add(objRoles_info);
                  
                
                    }
                    objUser_info.objRoles_info = objListRoles_info;
                }
                else if (User.ComparePassword(Password) == false)
                {
                    objUser_info.User_Name = User.UserName;
                    objUser_info.DisplayName = User.DisplayName;
                    objUser_info.Organization = User.Organization.OrganizeNameTH;
                    objUser_info.Tel = User.Organization.Tel;
                    objUser_info.Status = 0;
                    objUser_info.Message = "เข้าสู่ระบบไม่สำเร็จ";

                }
            }
            catch (Exception ex)
            {
                objUser_info.Status = 6;
                objUser_info.Message = ex.Message;
            }

            return objUser_info;
        }


        public WebApi.Jwt.Models.user.get_role_byuser get_Roles(string Username)
        {
            user.get_role_byuser roles = new user.get_role_byuser();
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                XafTypesInfo.Instance.RegisterEntity(typeof(RoleInfo));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                UserInfo User;
                User = ObjectSpace.FindObject<UserInfo>(new BinaryOperator("UserName", Username));
                //nutrition.Module.CtlShowDispyName ctlshow;
                //ctlshow = ObjectSpace.FindObject<CtlShowDispyName>(new BinaryOperator("Oid", Username));

                if (User.UserName != null)
                {
                    roles.User_Name = User.UserName;
                    roles.Display_name = User.DisplayName;
                  //  roles.Role_name = User.UserRoles;

                    //{
                       List<user.Roles_info> get_Role_Byusers = new List<user.Roles_info>();

                    foreach (RoleInfo row in User.UserRoles)
                    {
                        user.Roles_info Userget = new user.Roles_info();
                        Userget.Role_display = row.DisplayName;
                        Userget.Role_Name = row.Name;
                        get_Role_Byusers .Add(Userget);
                    }
                    roles.objRoles_info = get_Role_Byusers;

                        roles.Status = 1;
                    roles.Message = "แสดงรายชื่อ User";
                }
                else{
                    roles.Status = 0;
                    roles.Message = "ไม่แสดงรายชื่อ User";
                }


            }
            catch (Exception ex)
            {
                roles.Status = 6;
                roles.Message = ex.Message;
            }
            return roles;
        }
        //public user.get_role_byuser Userget_ByRole(string DisplayName)
        //{
        //    user.get_role_byuser user = new user.get_role_byuser();
        //    try
        //    {
        //        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //        XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
        //        XafTypesInfo.Instance.RegisterEntity(typeof(RoleInfo));
        //        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //        RoleInfo User;
        //        User = ObjectSpace.FindObject <RoleInfo>(new BinaryOperator("DisplayName", DisplayName))
        //          if (User.DisplayName != null)
        //        {
        //            user.User_Name = User.DisplayName;
        //            roles.Display_name = User.DisplayName;
        //            //  roles.Role_name = User.UserRoles;

        //            //{
        //            List<user.Roles_info> get_Role_Byusers = new List<user.Roles_info>();

        //            foreach (RoleInfo row in User.UserRoles)
        //            {
        //                user.Roles_info Userget = new user.Roles_info();
        //                Userget.Role_display = row.DisplayName;
        //                Userget.Role_Name = row.Name;
        //                get_Role_Byusers.Add(Userget);
        //            }
        //            roles.objRoles_info = get_Role_Byusers;

        //            roles.Status = 1;
        //            roles.Message = "แสดงรายชื่อ User";
        //        }
        //        else
        //        {
        //            roles.Status = 0;
        //            roles.Message = "ไม่แสดงรายชื่อ User";
        //        }

        //    }
        //}







        public static string GetClientIp(HttpRequestMessage request)
            {
                string ip = string.Empty;
                if (request.Properties.ContainsKey("MS_HttpContext"))
                {
                    HttpContextBase context = ((HttpContextBase)request.Properties["MS_HttpContext"]);
                    if (context.Request.ServerVariables["HTTP_VIA"] != null)
                        ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    else
                        ip = context.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
                return ip;
            }




        }
    }










///เก็บไว้

//    public class CustomAuthentication : 
//    {
//        private CustomLogonParameters customLogonParameters;
//        public CustomAuthentication()
//        {
//            customLogonParameters = new CustomLogonParameters();
//        }
//        public override void Logoff()
//        {
//            base.Logoff();
//            customLogonParameters = new CustomLogonParameters();
//        }
//        public override void ClearSecuredLogonParameters()
//        {
//            customLogonParameters.Password = "";
//            base.ClearSecuredLogonParameters();
//        }
//        public override object Authenticate(IObjectSpace objectSpace)
//        {

//            Employee employee = objectSpace.FindObject<Employee>(
//                new BinaryOperator("UserName", customLogonParameters.UserName));

//            if (employee == null)
//                throw new ArgumentNullException("Employee");

//            if (!employee.ComparePassword(customLogonParameters.Password))
//                throw new AuthenticationException(
//                    employee.UserName, "Password mismatch.");

//            return employee;
//        }

//        public override void SetLogonParameters(object logonParameters)
//        {
//            this.customLogonParameters = (CustomLogonParameters)logonParameters;
//        }

//        public override IList<Type> GetBusinessClasses()
//        {
//            return new Type[] { typeof(CustomLogonParameters) };
//        }
//        public override bool AskLogonParametersViaUI
//        {
//            get { return true; }
//        }
//        public override object LogonParameters
//        {
//            get { return customLogonParameters; }
//        }
//        public override bool IsLogoffEnabled
//        {
//            get { return true; }
//        }
//    }
//}










//       string Userinfo = "";


//       UserInfo = ObjectSpace.FindObject<UserInfo>(new BinaryOperator("StoredPassword", Password));














//public class RestorePasswordParameters : LogonActionParametersBase
//{
//    public override void ExecuteBusinessLogic(IObjectSpace objectSpace)
//    {
//        if (string.IsNullOrEmpty(Email))
//        {
//            throw new ArgumentException("Email address is not specified!");
//        }
//        IAuthenticationStandardUser user = objectSpace.FindObject(SecurityExtensionsModule.SecuritySystemUserType, CriteriaOperator.Parse("Email = ?", Email)) as IAuthenticationStandardUser;
//        IAuthenticationStandardUser user = objectSpace.FindObject(SecurityExtensionsModule.SecuritySystemUserType, CriteriaOperator.Parse("Email = ?", Email)) as IAuthenticationStandardUser;
//        if (user == null)
//        {
//            throw new ArgumentException("Cannot find registered users by the provided email address!");
//        }
//        byte[] randomBytes = new byte[6];
//        new RNGCryptoServiceProvider().GetBytes(randomBytes);
//        string password = Convert.ToBase64String(randomBytes);
//        Dennis: Resets the old password and generates a new one.
//        user.SetPassword(password);
//        user.ChangePasswordOnFirstLogon = true;
//        objectSpace.CommitChanges();
//        EmailLoginInformation(Email, password);
//    }
//    public static void EmailLoginInformation(string email, string password)
//    {
//        Dennis:
//        if (success)
//        {
//            Send an email with the login details here. Refer to http://msdn.microsoft.com/en-us/library/system.net.mail.mailmessage.aspx for more details.
//        }
//        else
//        {
//            throw new Exception("Failed!");
//        }
//    }
//}





