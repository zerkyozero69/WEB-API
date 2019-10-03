using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class Farmer
    {

        public class Farmer_Status

        {
          
            public string Message { get; set; }
            public string Fullname { get; set; }
            //public string LastNameTH { get; set; }
            public string CitizenID { get; set; }
            //public string Phone { get; set; }
            //public string Email { get; set; }
        }
        public class Profile_Farmer

        {
            public object Oid { get; set; }
            public string Organization { get; set; }
            public string CitizenID { get; set; }
            public string Title { get; set; }
            public string FirstNameTH { get; set; }
            public string LastNameTH { get; set; }
            public string Gender { get; set; }
            public string BirthDate { get; set; }
            public string Tel { get; set; }
            public string Email { get; set; }
            public string DisPlayName { get; set; }
            public string Address { get; set; }
            public string Moo { get; set; }
            public string Soi { get; set; }
            public string Road { get; set; }
            public string Province { get; set; }
            public string District { get; set; }
            public string SubDistrict { get; set; }
            public string ZipCode { get; set; }
            public string IsActive { get; set; }

        }
        public class _Registerfarmer

        {
            public object Oid { get; set; }
            public string Username { get; set; }
            public object OrganizationOid { get; set; }
            public Int64 CitizenID { get; set; }
            public object TitleOid { get; set; }
            public string FirstNameTH { get; set; }
            public string LastNameTH { get; set; }
            public DateTime BirthDay { get; set; }
            public object Gender { get; set; }
            public string Tel { get; set; }
            public string Email { get; set; }
            public string Address_No { get; set; }
            public string Address_buildingName { get; set; }
            public string Address_moo { get; set; }
            public string Address_Soi { get; set; }
            public string Address_Road { get; set; }
            // string Address_FullAddress = "";
            public object Address_provinces { get; set; }
            public object Address_districts { get; set; }
            public object Address_subdistricts { get; set; }
            public string ZipCode { get; set; }
            public object AnimalSupplie { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }
    }

}