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
using System.Diagnostics;

namespace PTools
{
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

        string IKit.EvaluateSQL(string connection, string sql, int timeout = 60)
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
                        command.ExecuteNonQuery();
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

                return JsonConvert.SerializeObject(new SuccessDataTableBlock
                {
                    Error = null,
                    Cargo = null
                });
            }
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

                return JsonConvert.SerializeObject(new SuccessDataTableBlock
                {
                    Error = null,
                    Cargo = table
                });
            }
        }

        string IKit.GetTicks()
        {
            return JsonConvert.SerializeObject(new SuccessLongBlock
            {
                Error = null,
                Cargo = DateTime.UtcNow.Ticks
            });
        }

        string IKit.GetUnixTimestamp()
        {
            var ticks = DateTime.UtcNow;
            var unixTimestamp = (Int64)ticks.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            return JsonConvert.SerializeObject(new SuccessLongBlock
            {
                Error = null,
                Cargo = unixTimestamp
            });
        }
    }
}

