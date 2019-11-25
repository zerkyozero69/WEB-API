using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class FinanceYearModel
    {
        public object Oid { get; set; }
        public string YearName { get; set; }
      
    }
    public class BudgetSourceModel
    {
        public object Oid { get; set; }
        public string BudgetName { get; set; }
       // public List<BudgetSourceModel> BudgetSourcelist;
    }
}