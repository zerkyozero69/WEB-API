﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class user
    {
        public class member_info_Shot
        {

            // Public id As String
            public string User_name { get; set; }
            public string User_Password { get; set; }
            public object Token_key { get; set; }
            // Public Property GU_id As Object
            // Public Property token As String
            // Public Property Created_by As String
            // Public Property Created_date As DateTime
            public string message { get; set; }
            public object status { get; set; }

            internal bool ComparePassword(string password)
            {
                throw new NotImplementedException();
            }
        }

        public class Roles_info
        {
            public string Role_Name { get; set; }

            public string Role_display { get; set; }

        }

        public class User_info
        {

            public string User_Name { get; set; }
            // public string User_Password { get; set; }
            public List<Roles_info> objRoles_info;
            public string DisplayName { get; set; }
            public string Organization { get; set; }
            public string SubOrganizeName { get; set; }

            public string Address { get; set; }
            // Public Property Created_date As DateTime

            public string Address_No { get; set; }
            public string Moo { get; set; }
            public string Soi { get; set; }
            public string Road { get; set; }
            public string ProvinceNameTH { get; set; }
            public string DistrictNameTH { get; set; }
            public string SubDistrictNameTH { get; set; }
            public string DLD { get; set; }
            public string E_Mail { get; set; }
            public string Tel { get; set; }
            public string Website { get; set; }
            public double Mapcoordinates_Latitude { get; set; }
            public double Mapcoordinates_Longitude { get; set; }
            public object Description { get; set; }
            public string Message { get; set; }
            public int Status { get; set; }
            public string Token_key { get; set; }
        }

        public class get_role_byuser
        {
            public string User_Name { get; set; }
            public string Display_name { get; set; }
                 
            public List<Roles_info> objRoles_info;
            public int Status { get; set; }
            public string Message { get; set; }

        }

        public class Log_info //รายละเอียด log  
        {
            public int LogID { get; set; }
            public string EventName { get; set; } 
            public string Description { get; set; }
            // Public Property IPAddress As String
            public string name { get; set; } 
            public string DisplayName { get; set; }
        }

        public class profile
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public class UserInfoShort
            {
                public bool status { get; set; }
                public string token { get; set; } 
                public bool complete_register { get; set; }
                public string name { get; set; }
                public string device_token { get; set; } 
            }
        }

    }
}