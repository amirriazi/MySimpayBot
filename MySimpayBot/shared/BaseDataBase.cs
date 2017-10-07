using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Shared.WebService
{
    public static partial class DataBase
    {
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private static QueryResult ExecuteQuery(string query, CommandType commandType, SqlParameter[] parameters)
        {
            QueryResult result;
            var dS = new DataSet();
            var sw = Stopwatch.StartNew();
            var returnCodeID = parameters.Length - 1;
            var resultTextID = parameters.Length - 2;
            var spResultCodeID = parameters.Length - 3;
            string param = "";
            if (parameters != null & parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    param += parameters[i].ParameterName + ":" + parameters[i].Value;
                }
                param = "{" + param + "}";
            }
            while (true)
            {
                try
                {
                    using (var connection = new SqlConnection(ProjectValues.dataBaseConfigure.ConnectionString))
                    {
                        using (var command = new SqlCommand(query, connection) { CommandTimeout = ProjectValues.dataBaseConfigure.CommandTimeout, CommandType = commandType })
                        {
                            using (var dataAdaptor = new SqlDataAdapter(command))
                            {
                                command.Parameters.AddRange(parameters);
                                connection.Open();
                                dataAdaptor.Fill(dS);
                            }
                            if ((int)parameters[returnCodeID].Value != 1)
                            {
                                result = new QueryResult { ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };
                                Log.Error(CreateLogMessage(result.Text, query, result.ReturnCode, parameters), sw.Elapsed.TotalMilliseconds);
                                break;
                            }
                            if ((int)parameters[spResultCodeID].Value != 1)
                            {
                                result = new QueryResult { SPCode = (int)parameters[spResultCodeID].Value, ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };
                                Log.Error(CreateLogMessage(result.Text, query, result.SPCode, parameters), sw.Elapsed.TotalMilliseconds);
                                break;
                            }
                            result = new QueryResult { DataSet = dS, SPCode = (int)parameters[spResultCodeID].Value, ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };
                            Log.Debug(CreateLogMessage(result.Text, query, 1, parameters), sw.Elapsed.TotalMilliseconds);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = new QueryResult { Text = ex.Message.ToString(), ReturnCode = ex.HResult };
                    Log.Fatal(CreateLogMessage(ex.ToString(), query, ex.HResult, parameters), sw.Elapsed.TotalMilliseconds);
                }
                break;
            }
            return result;
        }
        private static QueryResult ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters)
        {
            parameters.Add(new SqlParameter { ParameterName = "@output_status", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            parameters.Add(new SqlParameter { ParameterName = "@output_message", SqlDbType = SqlDbType.NVarChar, Size = 4000, Direction = ParameterDirection.Output });
            parameters.Add(new SqlParameter { ParameterName = "@returnvalue", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });

            return ExecuteQuery(storedProcedureName, CommandType.StoredProcedure, parameters.ToArray());
        }

        private static string CreateLogMessage(string result, string storedProcedureName, int code, SqlParameter[] parameters)
        {
            var sp = "(Stored Procedure: " + storedProcedureName + ")";
            var cd = "(Code: " + code + ")";
            var dt = "(Result: " + result + ")";
            var ap = "";

            for (int i = parameters.Length - 4; i >= 0; i--)
                if (parameters[i] != null)
                {
                    var name = parameters[i].ParameterName;
                    var temp = parameters[i].Value?.ToString();
                    //var value = ProjectValues.LogMode > 2 ? temp.Substring(0, Math.Min(temp.Length, 20)) : temp;
                    var value = "N/A";
                    if (temp != null)
                        value = temp.Substring(0, Math.Min(temp.Length, 20));


                    ap = name + "=" + value + " " + ap;
                }
            ap = "(SQL Parameters: " + ap.Trim() + ")";

            return sp + cd + dt + ap;
        }
    }
}