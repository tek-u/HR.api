using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADO
{
	public struct hrDB
	{
		private static readonly string DBName = ConfigurationManager.AppSettings["IsProd"].ToLower() == "true" ? "prod" : "local";
		public static readonly string ConnStr = ConfigurationManager.ConnectionStrings[DBName].ToString();
	}

	public class user
	{
		public const String Table = "personels";
		public const String Nu = "System.DBNull";
		public bool key;
		public DBAccess DBA;

		#region Columns Structure
		public struct Params
		{
			public String id { get { return "@id"; } } //- int - PRIMARY_KEY 
			public String fname { get { return "@fname"; } } //- nvarchar(MAX) 
			public String lname { get { return "@lname"; } } //- nvarchar(MAX) 
			public String phone { get { return "@phone"; } } //- nvarchar(20) 
			public String email { get { return "@email"; } } //- nvarchar(30) 
			public String isActive { get { return "@isActive"; } } //- bool
			public String created { get { return "@created"; } } //- datetime 			
		}
		public struct Fields
		{
			public String id { get { return "id"; } } //- int - PRIMARY_KEY 
			public String fname { get { return "fname"; } } //- nvarchar(MAX) 
			public String lname { get { return "lname"; } } //- nvarchar(MAX) 
			public String phone { get { return "phone"; } } //- nvarchar(20) 
			public String email { get { return "email"; } } //- nvarchar(30) 
			public String isActive { get { return "isActive"; } } //- bool
			public String created { get { return "created"; } } //- datetime 	
		}
		#endregion

		#region Properties

		private int i_id; //- int - PRIMARY_KEY 
		private string i_fname; //- nvarchar(MAX) 
		private string i_lname; //- nvarchar(MAX) 
		private string i_phone; //- nvarchar(MAX) 
		private string i_email; //- nvarchar(MAX) 
		private DateTime i_created; //- datetime 
		private Boolean i_isActive; //- bit

		public bool F_id { get; set; }
		public bool F_fname { get; set; }
		public bool F_lname { get; set; }
		public bool F_phone { get; set; }
		public bool F_email { get; set; }
		public bool F_isActive { get; set; }
	
		public int id { get { return i_id; } set { i_id = value; F_id = true; } } //- int - PRIMARY_KEY 
		public string fname { get { return i_fname; } set { i_fname = value; F_fname = true; } } //- nvarchar(MAX) 
		public string lname { get { return i_lname; } set { i_lname = value; F_lname = true; } } //- nvarchar(MAX)
		public string phone { get { return i_phone; } set { i_phone = value; F_phone = true; } } //- nvarchar(MAX)
		public string email { get { return i_email; } set { i_email = value; F_email = true; } } //- nvarchar(MAX) 
		public Boolean isActive { get { return i_isActive; } set { i_isActive = value; F_isActive = true; } } //- bit 
		public DateTime created { get { return i_created; } set { i_created = value; } } //- datetime

		#endregion

		#region ToList()

		public List<user> ToList(Object Obj)
		{
			Fields f = new Fields();
			List<user> list = new List<user>();
			DataTable DT; // = new DataTable();
			user iO;
			String c, t; //- Column Name/Type

			if (Obj is System.Data.DataSet)
			{
				DataSet DS = (DataSet)Obj;
				DT = DS.Tables[0];
			}
			else { DT = (DataTable)Obj; }

			foreach (DataRow row in DT.Rows)
			{
				iO = new user();
				foreach (DataColumn col in DT.Columns)
				{
					c = col.ToString();                 //- Column Name
					t = row[col].GetType().ToString();  //- Column Type

					if (c == f.id && t != Nu) { iO.id = Convert.ToInt32(row[f.id]); } //- int - PRIMARY_KEY 
					else if (c == f.fname && t != Nu) { iO.fname = row[f.fname].ToString(); } //- nvarchar(MAX) 			
					else if (c == f.lname && t != Nu) { iO.lname = row[f.lname].ToString(); } //- nvarchar(MAX) 				
					else if (c == f.phone && t != Nu) { iO.phone = row[f.phone].ToString(); } //- nvarchar(20) 				
					else if (c == f.email && t != Nu) { iO.email = row[f.email].ToString(); } //- nvarchar(MAX) 	
					else if (c == f.created && t != Nu) { iO.created = Convert.ToDateTime(row[f.created]); } //- datetime 
					else if (c == f.isActive && t != Nu) { iO.isActive = Convert.ToBoolean(row[f.isActive]); } //- bit				
				}
				list.Add(iO);
			}
			return list;
		}

		#endregion

		#region CRUD Operations
		//- IUD short for INSERT UPDATE DELETE
		public Object CRUD(string crudType, bool inline = false)
		{
			if (crudType == Crud.I) { F_id = false; } //  <-- Disable id flag if not inserting primary key
			DBA = new DBAccess(hrDB.ConnStr);
			Object obj;

			if (!inline)
				SetParam(ref DBA.cmd);

			string sql = SQL(crudType, inline);
			if (crudType == Crud.I)
			{
				obj = (key ? DBA.ExecuteScalar(CmdT.text, sql) : DBA.ExecuteNonQuery(CmdT.text, sql));
				if (key && obj.GetType() == typeof(int)) { this.id = (int)obj; }
			}
			else
				obj = DBA.ExecuteNonQuery(CmdT.text, sql);

			return (obj.GetType() == typeof(int) ? obj : 0);
		}
		public void SetParam(ref SqlCommand cmd)
		{
			Params p = new Params();

			if (F_id) { cmd.Parameters.Add(p.id, SqlDbType.Int).Value = id; } //- int - PRIMARY_KEY 
			if (F_fname) { cmd.Parameters.Add(p.fname, SqlDbType.NVarChar, -1).Value = (fname ?? null); } //- nvarchar(MAX) 
			if (F_lname) { cmd.Parameters.Add(p.lname, SqlDbType.NVarChar, -1).Value = (lname ?? null); } //- nvarchar(MAX) 
			if (F_phone) { cmd.Parameters.Add(p.phone, SqlDbType.NVarChar, -1).Value = (phone ?? null); } //- nvarchar(MAX) 
			if (F_email) { cmd.Parameters.Add(p.email, SqlDbType.NVarChar, -1).Value = (email ?? null); } //- nvarchar(MAX) 
			if (F_isActive) { cmd.Parameters.Add(p.isActive, SqlDbType.Bit).Value = (isActive ? 1 : 0); } //- bit
		}

		public String SQL(String CRUD, bool inline)
		{
			Params p = new Params();
			Fields f = new Fields();
			List<string> lstFields;
			String SQL = "";

			if (CRUD == Crud.I)
			{
				List<String> lstColumns = new List<String>();
				List<String> lstValues = new List<String>();
				SQL = "INSERT INTO " + Table + " (";
				if (inline)
				{
					if (F_id) { lstColumns.Add(f.id); lstValues.Add(this.id.ToString()); } //- int - PRIMARY_KEY 					
					if (F_fname) { lstColumns.Add(f.fname); lstValues.Add("'" + Encode(this.fname) + "'"); } //- nvarchar(MAX) 
					if (F_lname) { lstColumns.Add(f.lname); lstValues.Add("'" + Encode(this.lname) + "'"); } //- nvarchar(MAX) 
					if (F_phone) { lstColumns.Add(f.phone); lstValues.Add("'" + Encode(this.phone) + "'"); } //- nvarchar(MAX) 
					if (F_email) { lstColumns.Add(f.email); lstValues.Add("'" + Encode(this.email) + "'"); } //- nvarchar(MAX) 
					if (F_isActive) { lstColumns.Add(f.isActive); lstValues.Add(this.isActive ? "1" : "0"); } //- bit 
				}
				else
				{
					if (F_id) { lstColumns.Add(f.id); lstValues.Add(p.id); } //- int - PRIMARY_KEY 
					if (F_fname) { lstColumns.Add(f.fname); lstValues.Add(p.fname); } //- nvarchar(MAX) 
					if (F_lname) { lstColumns.Add(f.lname); lstValues.Add(p.lname); } //- nvarchar(MAX) 
					if (F_phone) { lstColumns.Add(f.phone); lstValues.Add(p.phone); } //- nvarchar(MAX) 
					if (F_email) { lstColumns.Add(f.email); lstValues.Add(p.email); } //- nvarchar(MAX) 
					if (F_isActive) { lstColumns.Add(f.isActive); lstValues.Add(p.isActive); } //- bit 				
				}
				var Columns = String.Join(", ", lstColumns);
				var Values = String.Join(", ", lstValues);
				SQL += Columns + ") VALUES (" + Values + ")" + (key ? "; SELECT CAST(SCOPE_IDENTITY() AS INT);" : "");
			}
			else if (CRUD == Crud.U)
			{
				lstFields = new List<String>();
				SQL = "UPDATE " + Table + " SET ";
				if (inline)
				{
					if (F_id) { lstFields.Add(f.id + " = " + (this.id > 0 ? id.ToString() : "NULL")); } //- int - PRIMARY_KEY 
					if (F_fname) { lstFields.Add(f.fname + " = " + (this.fname != null ? "'" + Encode(fname) + "'" : "NULL")); } //- nvarchar(MAX) 
					if (F_lname) { lstFields.Add(f.lname + " = " + (this.lname != null ? "'" + Encode(lname) + "'" : "NULL")); } //- nvarchar(MAX) 
					if (F_phone) { lstFields.Add(f.phone + " = " + (this.phone != null ? "'" + Encode(phone) + "'" : "NULL")); } //- nvarchar(MAX) 
					if (F_email) { lstFields.Add(f.email + " = " + (this.email != null ? "'" + Encode(email) + "'" : "NULL")); } //- nvarchar(MAX) 
					if (F_isActive) { lstFields.Add(f.isActive + " = " + (this.isActive ? "1" : "0")); } //- BIT
				}
				else
				{
					if (F_id) { lstFields.Add(f.id + " = " + p.id); } //- int - PRIMARY_KEY 
					if (F_fname) { lstFields.Add(f.fname + " = " + p.fname); } //- nvarchar(MAX) 
					if (F_lname) { lstFields.Add(f.lname + " = " + p.lname); } //- nvarchar(MAX) 
					if (F_phone) { lstFields.Add(f.fname + " = " + p.fname); } //- nvarchar(MAX) 
					if (F_email) { lstFields.Add(f.fname + " = " + p.fname); } //- nvarchar(MAX) 
					if (F_isActive) { lstFields.Add(f.fname + " = " + p.fname); } //- nvarchar(MAX) 
				}
				SQL += String.Join(", ", lstFields) + (inline ? " WHERE " + f.id + " = " + this.id : " WHERE " + f.id + " = " + p.id);
			}
			else if (CRUD == Crud.D)
				SQL = "DELETE FROM " + Table + (inline ? " WHERE " + f.id + " = " + this.id : " WHERE " + f.id + " = " + p.id);

			return SQL;
		}
		public String Encode(String EncodeMe)
		{
			if (EncodeMe != null)
			{
				EncodeMe = Regex.Replace(EncodeMe, "\"", "\"");
				EncodeMe = EncodeMe.Replace("'", "''");
			}
			return EncodeMe;
		}
		#endregion
	}
}
