using DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{
    public static class ProductOrderManager
    {
        public static int Insert(int ProductID,int OrderID,int Quantity)
        {
            string cmdText = "INSERT INTO ProductOrder (ProductID, OrderID, Quantity) VALUES (@ProductID, @OrderID, @Quantity)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", ProductID),
                new SqlParameter("@OrderID", OrderID),
                new SqlParameter("@Quantity", Quantity)
            };
            return DBManger.ExecuteNonQuery(cmdText, parameters);
        }
    }
}
