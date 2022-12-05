using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public struct CmdT
    {
        public static String text = "command text";
        public static String sproc = "store procedure";
    }

    public struct Crud
    {
        public static String S = "SELECT";
        public static String I = "INSERT";
        public static String U = "UPDATE";
        public static String D = "DELETE";
    }

    public class SQLStr
    {
        private string _DataSource { get; set; }
        private string _InitialCatalog { get; set; }
        private string _UserId { get; set; }
        private string _Password { get; set; }
        private string _ProviderName { get; set; }
        private string _Str { get; set; }

        public string DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; updateStr(); }
        }
        public string InitialCatalog
        {
            get { return _InitialCatalog; }
            set { _InitialCatalog = value; updateStr(); }
        }
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; updateStr(); }
        }
        public string Password
        {
            get { return _Password; }
            set { _Password = value; updateStr(); }
        }
        public string ProviderName
        {
            get { return _ProviderName; }
            set { _ProviderName = value; }
        }
        public string Str
        {
            get { return _Str; }
            set { _Str = value; }
        }

        public SQLStr(string ds, string ic, string uid = "", string pw = "", string pn = "System.Data.SqlClient")
        {
            this._DataSource = ds;
            this._InitialCatalog = ic;
            this._UserId = uid;
            this._Password = pw;
            this._ProviderName = pn;
            updateStr();
        }

        private void updateStr()
        {
            if (_UserId == "" && _Password == "")
                this.Str = @"Server = " + _DataSource + ";Database = " + _InitialCatalog + ";Integrated Security = true";
            else
                this.Str = "Data Source=" + this._DataSource + ";Initial Catalog=" + _InitialCatalog + ";User Id=" + _UserId + ";password=" + _Password;
        }
    }
}
