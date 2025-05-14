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
    public static class ProductManager
    {
        public static ProductList GetAll()
        {
            DataTable dt = DBManger.GetQueryResult("SELECT * FROM Product");
            return MapFromDTtoProductList(dt);
        }
        public static Product GetById(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ID", id);
            DataTable dt = DBManger.GetQueryResult("SELECT top 1* FROM Product WHERE ID=@ID", parameters);
            Product product = MapFromDataRowtoProduct(dt.Rows[0]);
            return product;
        }
        public static int GetIDbyName(string name)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Name", name);
            DataTable dt = DBManger.GetQueryResult("SELECT top 1* FROM Product WHERE Name=@Name", parameters);
            Product product = MapFromDataRowtoProduct(dt.Rows[0]);
            return product.ID;
        }
        public static ProductList GetByName(string name)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Name", "%" + name + "%");
            DataTable dt = DBManger.GetQueryResult("SELECT * FROM Product WHERE Name LIKE @Name", parameters);
            return MapFromDTtoProductList(dt);
        }
        public static ProductList GetByCategoryID(int categoryID)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@CategoryID", categoryID);
            DataTable dt = DBManger.GetQueryResult("SELECT * FROM Product WHERE CategoryID=@CategoryID", parameters);
            return MapFromDTtoProductList(dt);
        }
        public static ProductList GetByAll(string name, int categoryID, int supplierID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder query = new StringBuilder("SELECT * FROM Product WHERE 1=1");

            if (!string.IsNullOrEmpty(name))
            {
                query.Append(" AND Name LIKE @Name");
                parameters.Add(new SqlParameter("@Name", "%" + name + "%"));
            }

            if (categoryID > 0)
            {
                query.Append(" AND CategoryID=@CategoryID");
                parameters.Add(new SqlParameter("@CategoryID", categoryID));
            }

            if (supplierID > 0)
            {
                query.Append(" AND SupplierID=@SupplierID");
                parameters.Add(new SqlParameter("@SupplierID", supplierID));
            }

            DataTable dt = DBManger.GetQueryResult(query.ToString(), parameters.ToArray());
            return MapFromDTtoProductList(dt);
        }
        public static int Insert(Product product)
        {
            string cmdText = "INSERT INTO Product (Name, Price,  NumOfStock, CategoryID,SupplierID) VALUES (@Name, @Price,  @NumOfStock, @CategoryID,@SupplierID)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@NumOfStock", product.NumOfStock),
                new SqlParameter("@CategoryID", product.CategoryID),
                new SqlParameter("@SupplierID", product.SupplierID)
            };            
            int count =  DBManger.ExecuteNonQuery(cmdText, parameters);
            return count;
        }
        public static int Update(Product product)
        {
            string cmdText = "UPDATE Product SET Name=@Name, Price=@Price, NumOfStock=@NumOfStock, CategoryID=@CategoryID WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", product.ID),
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@NumOfStock", product.NumOfStock),
                new SqlParameter("@CategoryID", product.CategoryID)
            };
            return DBManger.ExecuteNonQuery(cmdText, parameters);
        }
        public static void UpdateByProductIDAndQuantity(int productId,int quantity)
        {
            string cmdText = "UPDATE Product SET NumOfStock=NumOfStock-@quantity WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", productId),
                new SqlParameter("@quantity", quantity)
            };
            DBManger.ExecuteNonQuery(cmdText, parameters);
        }
        public static int Delete(int id)
        {
            string cmdText = "DELETE FROM Product WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", id)
            };
            return DBManger.ExecuteNonQuery(cmdText, parameters);
        }

        public static int MaxPrice()
        {
            int maxPrice = DBManger.ExecuteScalar<int>("SELECT MAX(Price) FROM Product");
            return maxPrice;
        }
        public static int MinPrice()
        {
            int minPrice = DBManger.ExecuteScalar<int>("SELECT MIN(Price) FROM Product");
            return minPrice;
        }
        public static int GetCount() {
            int count = DBManger.ExecuteScalar<int>("SELECT COUNT(*) FROM Product");
            return count;
        }
        public static int GetCountByCategoryID(int categoryID)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@CategoryID", categoryID);
            int count = DBManger.ExecuteScalar<int>("SELECT COUNT(*) FROM Product WHERE CategoryID=@CategoryID", parameters);
            return count;
        }
        static ProductList MapFromDTtoProductList(DataTable dt)
        {
            ProductList productList = new ProductList();
            foreach (DataRow dr in dt.Rows)
            {
                productList.Add(MapFromDataRowtoProduct(dr));
            }
            return productList;
        }
        static Product MapFromDataRowtoProduct(DataRow dr)
        {
            Product product = new Product();
            product.ID = Convert.ToInt32(dr["ID"]);
            product.Name = dr["Name"].ToString();
            product.Price = Convert.ToInt32(dr["Price"]);
            product.Image = dr["Image"].ToString();
            product.NumOfStock = Convert.ToInt32(dr["NumOfStock"]);
            product.CategoryID = Convert.ToInt32(dr["CategoryID"]);
            product.SupplierID = Convert.ToInt32(dr["SupplierID"]);
            product.CategoryName = CategoryManager.GetById(Convert.ToInt32(dr["CategoryID"]));
            product.SupplierName = SupplierManager.GetById(Convert.ToInt32(dr["SupplierID"]));
            return product;
        }
    }
}
