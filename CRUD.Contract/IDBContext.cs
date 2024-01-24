using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CRUD.Contract
{
    public interface IDBContext
    {
        public int ExecuteNonQuery(string procName, SqlParameter[]? procParameters = null);
        public Task<DataSet> GetDataSetAsync(string procName, SqlParameter[]? procParameters = null);
        public DataTable ExecuteReader(string procName, SqlParameter[]? procParameters = null);
    }
}
