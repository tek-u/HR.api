using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ADO
{
	public class DBAccess
	{
		public SqlConnection conn;
		protected internal SqlCommand cmd;
		protected internal String connStr;
		protected internal SqlTransaction trans;
		protected internal string trxName;

		public DBAccess(string StrConn, bool IsTransaction = false, string transName = "trx")
		{
			connStr = StrConn;

			if (conn != null && conn.State == ConnectionState.Open)
				conn.Close();

			conn = new SqlConnection(connStr);
			cmd = new SqlCommand();
			cmd.Connection = conn;

			if (IsTransaction)
			{
				this.trxName = transName;
				conn.Open();
				cmd = conn.CreateCommand();
				trans = conn.BeginTransaction(IsolationLevel.ReadCommitted, transName);
				cmd.Transaction = trans;
			}
		}
		public void CloseTrx()
		{
			if (conn != null && conn.State == ConnectionState.Open && trans != null)
			{
				trans.Dispose();
				conn.Close();
				conn.Dispose();
			}
		}

		public Object ExecuteNonQuery(String Type, String spSQL)
		{
			cmd.CommandText = spSQL;
			cmd.CommandType = (Type == CmdT.sproc ? CommandType.StoredProcedure : CommandType.Text);
			try
			{
				conn.Open();
				return cmd.ExecuteNonQuery();
			}
			catch (Exception ex) { return ex.Message; }
			finally
			{
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
		}

		public Object ExecuteScalar(String Type, String spSQL)
		{
			cmd.CommandText = spSQL;
			cmd.CommandType = (Type == CmdT.sproc ? CommandType.StoredProcedure : CommandType.Text);
			try
			{
				conn.Open();
				return cmd.ExecuteScalar();
			}
			catch (Exception ex) { return ex.Message; }
			finally
			{
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
		}

		public DataTable Table(String Type, String spSQL)
		{
			SqlDataReader DR;
			DataTable DT = new DataTable();
			cmd.CommandText = spSQL;
			cmd.CommandType = (Type == CmdT.sproc ? CommandType.StoredProcedure : CommandType.Text);
			try
			{
				conn.Open();
				DR = cmd.ExecuteReader();
				DT.Load(DR);
				DR.Close();
				return DT;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
			finally
			{
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
		}

		public DataSet TableSet(String Type, String spSQL)
		{
			DataSet DS = new DataSet();
			SqlDataAdapter DA;
			cmd.CommandText = spSQL;
			cmd.CommandType = (Type == CmdT.sproc ? CommandType.StoredProcedure : CommandType.Text);
			try
			{
				conn.Open();
				DA = new SqlDataAdapter(cmd);
				DA.Fill(DS);
				DA.Dispose();
				return DS;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
			finally
			{
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
		}

		public void BulkCopy(DataTable DT, string tableName)
		{
			try
			{
				conn.Open();
				using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
				{
					bulkCopy.DestinationTableName = tableName;
					//bulkCopy.ColumnMappings.Add("ID", "ID");
					//bulkCopy.ColumnMappings.Add("NAME", "NAME");
					bulkCopy.WriteToServer(DT);
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}

		//public bool Transaction(string transName = "dbAccess_trx")
		//{
		//    bool succeed = false;            

		//    conn.Open();                  // <-- Open connection
		//    cmd = conn.CreateCommand();   // <-- Create a command           
		//    SqlTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted, transName);
		//    cmd.Connection = conn;
		//    cmd.Transaction = trans;
		//    try
		//    {

		//        trans.Commit();
		//        succeed = true;               
		//    }
		//    catch (Exception ex)
		//    {
		//        try { trans.Rollback(transName); succeed = false; }
		//        catch (Exception rbEx) { succeed = false; }
		//    }
		//    finally
		//    {
		//        trans.Dispose();
		//        conn.Close();
		//        conn.Dispose();
		//    }
		//    return succeed;
		//}
	}
}
