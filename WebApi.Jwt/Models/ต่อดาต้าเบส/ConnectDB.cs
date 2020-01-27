using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.ต่อดาต้าเบส
{
    public class ConnectDB
    {
        string ConnectionString = "";
        SqlConnection scc;
        public void OpenConection()
        {
            scc = new SqlConnection(ConnectionString);
            scc.Open();
        }
        public void CloseConnection()
        {
            scc.Close();
        }
    }
}