using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;

namespace DataAccessLayer
{
    public static class DBManger
    {
        private static readonly string _connectionString;

        static DBManger()
        {
            _connectionString = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build()
                .GetConnectionString("con1")
                ?? throw new InvalidOperationException("Connection string 'con1' not found.");
        }

        public static DataTable GetQueryResult(string cmdText, SqlParameter[] parameters = null)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(cmdText, con))
            {
                try
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Query failed: {cmdText}. Error: {ex.Message}", ex);
                }
            }
        }

        public static int ExecuteNonQuery(string cmdText, SqlParameter[] parameters = null)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(cmdText, con))
            {
                try
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Non-query failed: {cmdText}. Error: {ex.Message}", ex);
                }
            }
        }

        public static T ExecuteScalar<T>(string cmdText, SqlParameter[] parameters = null)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(cmdText, con))
            {
                try
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    con.Open();
                    var result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        return default;

                    return (T)Convert.ChangeType(result, typeof(T));
                }
                catch (InvalidCastException ex)
                {
                    throw new Exception($"Type conversion failed for query: {cmdText}. Expected type: {typeof(T).Name}", ex);
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Scalar query failed: {cmdText}. Error: {ex.Message}", ex);
                }
            }
        }
    }
}