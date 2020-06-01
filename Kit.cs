using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using Newtonsoft.Json;
using System.Threading;
using System.Security.Policy;

namespace PTools
{
    public class FailureBlock
    {
        public string Error { get; set; }
        public string Cargo { get; set; }
    }

    public class SuccessBlock
    {
        public string Error { get; set; }
        public DataTable Cargo { get; set; }
    }

    [Guid("5E10370D-B1C1-400B-80C0-481A9E2AD499")]
    [ComVisible(true)]
    public interface IKit
    {
        string TagValue(string tag, string value);
        string TagAttrValue(string tag, string attr, string value);
        string EvaluateSQLReturnJSON(string connection, string sql, int timeout = 60);
    }


    [Guid("E9C9ADFC-57F9-4BE9-9593-38B80E1B1284")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Kit : IKit
    {
        string IKit.TagValue(string tag, string value)
        {
            return $"<{tag}>{value}</{tag}>";
        }
        string IKit.TagAttrValue(string tag, string attr, string value)
        {
            return $"<{tag} {attr}>{value}</{tag}>";
        }

        string IKit.EvaluateSQLReturnJSON(string connection, string sql, int timeout)
        {
            SqlConnection sqlConnection;
            try
            {
                sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new FailureBlock
                {
                    Error = e.Message,
                    Cargo = null
                });
            }

            while (sqlConnection.State == ConnectionState.Connecting)
            {
                Thread.Sleep(1);
            }

            using (DataTable table = new DataTable())
            {
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandTimeout = timeout;
                    command.CommandType = CommandType.Text;
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            table.Load(reader);
                        }
                    }
                    catch (Exception e)
                    {
                        return JsonConvert.SerializeObject(new FailureBlock
                        {
                            Error = e.Message,
                            Cargo = null
                        });
                    }
                }

                return JsonConvert.SerializeObject(new SuccessBlock
                {
                    Error = null,
                    Cargo = table
                });
            }
        }
    }
}

