using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.นับจำนวนกิจกรรม
{

        public class Stock_info
        {
            public Stock_info()
            {
                Detail = new List<SeedAnimal_info>();
            }

            public string SeedLevelCode { get; set; }
            
             public List<SeedAnimal_info> Detail { get; set; }
        }
        public class SeedAnimal_info
        {
            
            public string SeedName { get; set; }
            public string TotalWeight { get; set; }
              public string WeightUnit { get; set; }

        }
    
}