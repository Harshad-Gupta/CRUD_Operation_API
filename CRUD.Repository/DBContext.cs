using CRUD.Contract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Repository
{
    public class DBContext : IDBContext
    {
        public SqlConnection sqlConnetion;
        public SqlCommand sqlCommand;
        public SqlTransaction sqlTransaction;

        public DBContext(string connectionString)
        {
            sqlConnetion = new SqlConnection(connectionString);
        }

        public int ExecuteNonQuery(string procName, SqlParameter[]? procParameters = null)
        {
            int rowsAffacted = 0;
            try
            {
                using (sqlConnetion)
                {
                    sqlCommand = new SqlCommand(procName, sqlConnetion);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (procParameters != null && procParameters.Length > 0)
                    {
                        sqlCommand.Parameters.AddRange(procParameters);
                    }

                    if (sqlConnetion.State == ConnectionState.Closed)
                    {
                        sqlConnetion.Open();
                    }

                    rowsAffacted = sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Common.Common.WriteToLog("Repository -> DBContext -> ExecuteNonQuery", "Error Occured -> " + ex.Message);
            }
            finally
            {
                sqlConnetion.Close();
            }

            return rowsAffacted;
        }

        public DataTable ExecuteReader(string procName, SqlParameter[]? procParameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (sqlConnetion)
                {
                    sqlCommand = new SqlCommand(procName, sqlConnetion);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (procParameters != null && procParameters.Length > 0)
                    {
                        sqlCommand.Parameters.AddRange(procParameters);
                    }

                    if (sqlConnetion.State == ConnectionState.Closed)
                    {
                        sqlConnetion.Open();
                    }

                    sqlTransaction = sqlConnetion.BeginTransaction();
                    sqlCommand.Transaction = sqlTransaction;
                    SqlDataReader dr = sqlCommand.ExecuteReader();
                    dt.Load(dr);
                    dr.Close();

                    if (Convert.ToBoolean(dt.Rows[0]["FLAG"]) == true)
                    {
                        sqlTransaction.Commit();
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                    }
                }

            }
            catch (Exception ex)
            {
                Common.Common.WriteToLog("Repository -> DBContext -> ExecuteReader", "Error Occured -> " + ex.Message);
            }
            finally
            {
                sqlConnetion.Close();
            }

            return dt;
        }

        public async Task<DataSet> GetDataSetAsync(string procName, SqlParameter[]? procParameters = null)
        {
            DataSet ds = new DataSet();
            try
            {
                using (sqlConnetion)
                {
                    sqlCommand = new SqlCommand(procName, sqlConnetion);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (sqlConnetion.State == ConnectionState.Closed)
                    {
                        sqlConnetion.Open();
                    }

                    sqlTransaction = sqlConnetion.BeginTransaction();
                    sqlCommand.Transaction = sqlTransaction;

                    if (procParameters != null && procParameters.Length > 0)
                    {
                        sqlCommand.Parameters.AddRange(procParameters);
                    }

                    using (SqlDataAdapter sda = new SqlDataAdapter(sqlCommand))
                    {
                        await Task.Run(() => sda.Fill(ds));
                    }
                    sqlTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                Common.Common.WriteToLog("Repository -> DBContext -> GetDataSet", "Error Occured -> " + ex.Message);
            }
            finally
            {
                sqlConnetion.Close();
            }

            return ds;
        }
    }
}
