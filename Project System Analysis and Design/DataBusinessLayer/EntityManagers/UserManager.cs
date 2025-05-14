using DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DataBusinessLayer
{
    public static class UserManager
    {
        public static UserList GetAll()
        {
            DataTable dt = DBManger.GetQueryResult("SELECT * FROM [User]");
            return MapFromDTtoUserList(dt);
        }
        public static bool UserExists(string username, string email)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@Email", email)
            };

            string query = "SELECT COUNT(*) FROM [User] WHERE Fname = @Username OR Email = @Email";
            int count = DBManger.ExecuteScalar<int>(query, parameters);

            return count > 0;
        }

        public static User GetById(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ID", id);
            DataTable dt = DBManger.GetQueryResult("SELECT top 1* FROM [User] WHERE ID=@ID", parameters);
            User user = MapFromDataRowtoUser(dt.Rows[0]);
            return user;
        }
        public static string GetRole(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ID", id);
            DataTable dt = DBManger.GetQueryResult("SELECT top 1* FROM [User] WHERE ID=@ID", parameters);
            User user = MapFromDataRowtoUser(dt.Rows[0]);
            return user.Role;
        }
        public static UserList GetByAll(string email, string number, string role)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder query = new StringBuilder("SELECT * FROM [User] WHERE 1=1"); // Fix here

            if (!string.IsNullOrEmpty(email))
            {
                query.Append(" AND Email LIKE @Email");
                parameters.Add(new SqlParameter("@Email", "%" + email + "%"));
            }

            if (!string.IsNullOrEmpty(number))
            {
                query.Append(" AND Number = @Number"); // Ensure correct case
                parameters.Add(new SqlParameter("@Number", number));
            }

            if (!string.IsNullOrEmpty(role))
            {
                query.Append(" AND [Role] = @Role"); 
                parameters.Add(new SqlParameter("@Role", role));
            }

            DataTable dt = DBManger.GetQueryResult(query.ToString(), parameters.ToArray());
            return MapFromDTtoUserList(dt);
        }
        public static User GetByEmail(string email)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Email", email);
            DataTable dt = DBManger.GetQueryResult("SELECT top 1* FROM [User] WHERE Email=@Email", parameters);
            User user = MapFromDataRowtoUser(dt.Rows[0]);
            return user;
        }
        public static int UserLogin(string email, string password)
        {
            string cmd = "SELECT id FROM [user] WHERE email=@email AND password=@password";
            SqlParameter[] sqlParameter = new SqlParameter[]
            {
                new SqlParameter("@email", email),
                new SqlParameter("@password", password)
            };

            object result = DBManger.ExecuteScalar<object>(cmd, sqlParameter);

            return result != null ? Convert.ToInt32(result) : 0;
        }
        public static int Insert(string fname, string lname, string email, string password, string number, string Role)
        {

            string cmdText = "INSERT INTO [User] (FName, LName, Email, Password, Number, Role) VALUES (@FName, @LName, @Email, @Password, @Number, @Role)";

            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@FName", fname),
        new SqlParameter("@LName", lname),
        new SqlParameter("@Email", email),
        new SqlParameter("@Password", password),
        new SqlParameter("@Number", number),
        new SqlParameter("@Role", Role)
            };

            return DBManger.ExecuteNonQuery(cmdText, parameters);
        }


        public static int Update(int id,string fname,string lname,string password,string email,string number)
        {
            string cmdText = "UPDATE [User] SET FName=@FName, LName=@LName, Password=@Password, Email=@Email, Number=@Number WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", id),
                new SqlParameter("@FName", fname),
                new SqlParameter("@LName", lname),
                new SqlParameter("@Password", password),
                new SqlParameter("@Email", email),
                new SqlParameter("@Number", number)
            };
            return DBManger.ExecuteNonQuery(cmdText, parameters);
        }
        public static int Delete(int id)
        {
            string cmdText = "DELETE FROM [User] WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", id)
            };
            return DBManger.ExecuteNonQuery(cmdText, parameters);
        }
        static UserList MapFromDTtoUserList(DataTable dt)
        {
            UserList userList = new UserList();
            foreach (DataRow dr in dt.Rows)
            {
                userList.Add(MapFromDataRowtoUser(dr));
            }
            return userList;
        }
        static User MapFromDataRowtoUser(DataRow dr)
        {
            User user = new User();
            user.ID = (int)dr["ID"];
            user.FName = dr["FName"].ToString();
            user.LName = dr["LName"].ToString();
            user.Email = dr["Email"].ToString();
            user.Password = dr["Password"].ToString();
            user.Role = dr["Role"].ToString();
            user.Number = dr["Number"].ToString();
            return user;
        }
    }
}
