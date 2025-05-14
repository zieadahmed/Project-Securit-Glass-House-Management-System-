using DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{
    public static class SupplierManager
    {
        public static SupplierList GetAll()
        {
            DataTable dt = DBManger.GetQueryResult("SELECT * FROM [Supplier]");
            return MapFromDTtoSupplierList(dt);
        }

        public static List<KeyValuePair<int, string>> GetAllNames()
        {
            DataTable dt = DBManger.GetQueryResult("SELECT Id, Name FROM Supplier");
            List<KeyValuePair<int, string>> names = new List<KeyValuePair<int, string>>();

            foreach (DataRow dr in dt.Rows)
            {
                names.Add(new KeyValuePair<int, string>(Convert.ToInt32(dr["Id"]), dr["Name"].ToString()));
            }

            return names;
        }

        public static string GetById(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ID", id);
            DataTable dt = DBManger.GetQueryResult("SELECT top 1* FROM Supplier WHERE ID=@ID", parameters);
            Supplier supplier = MapFromDataRowtoSupplier(dt.Rows[0]);
            return supplier.Name;
        }

        public static SupplierList GetByPhone(string phone)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@phone", "%" + phone + "%");
            DataTable dt = DBManger.GetQueryResult("SELECT * FROM Supplier WHERE phone LIKE @phone", parameters);
            return MapFromDTtoSupplierList(dt);
        }

        public static int insert(string name, string phone,string address)
        {
            string cmdText = "INSERT INTO Supplier(Name, Phone, Address) VALUES(@Name, @Phone, @Address)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", name),
                new SqlParameter("@Phone", phone),
                new SqlParameter("@Address", address)
            };
            return DBManger.ExecuteNonQuery(cmdText, parameters);
        }

        public static int Update(int id, string name,string phone,string address)
        {
            string cmdText = "UPDATE Supplier SET Name=@Name, Phone=@Phone, Address=@Address WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", id),
                new SqlParameter("@Name", name),
                new SqlParameter("@Phone", phone),
                new SqlParameter("@Address", address)
            };
            return DBManger.ExecuteNonQuery(cmdText, parameters);
        }

        public static int Delete(int id)
        {
            string cmdText = "DELETE FROM Supplier WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", id)
            };
            return DBManger.ExecuteNonQuery(cmdText, parameters);
        }

        static SupplierList MapFromDTtoSupplierList(DataTable dt)
        {
            SupplierList supplierList = new SupplierList();
            foreach (DataRow dr in dt.Rows)
            {
                supplierList.Add(MapFromDataRowtoSupplier(dr));
            }
            return supplierList;
        }

        static Supplier MapFromDataRowtoSupplier(DataRow dr)
        {
            Supplier supplier = new Supplier();
            supplier.ID = Convert.ToInt32(dr["ID"]);
            supplier.Name = dr["Name"].ToString();
            supplier.Address = dr["Address"].ToString();
            supplier.Phone = dr["Phone"].ToString();
            return supplier;
        }
    }
}