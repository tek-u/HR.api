using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public static class utils
    {
        public static List<user> getAllUsers()
        {
            List<user> lUsers = null;
            try
            {
                string SQL = @"SELECT * FROM personels";
                lUsers = (new user()).ToList(new DBAccess(hrDB.ConnStr).Table(CmdT.text, SQL));                
            }
            catch (Exception e) {
                //- Log into NLog if failed
                //- Nlog.write(e.Message + " [" + DateTime.Now() + "]");
                lUsers = null;
            }
            return lUsers;
        }

        public static user getUserById(int id)
        {
            try
            {
                string SQL = @"SELECT * FROM personels WHERE id = " + id.ToString();
                List<user> lUsers = (new user()).ToList(new DBAccess(hrDB.ConnStr).Table(CmdT.text, SQL));
                return lUsers[0];
            }
            catch (Exception e) {
                //- Log into NLog if failed
                //- Nlog.write(e.Message + " [" + DateTime.Now() + "]");
                return null;
            }
        }

        public static Boolean updateUser(user u)
        {
            Boolean isSucceeded = false;
            try
            {
                int result = (int)u.CRUD(Crud.U, true);
                if (result > 0)
                    isSucceeded = true;               
            }
            catch (Exception e) {
                //- Log into NLog if failed
                //- Nlog.write(e.Message + " [" + DateTime.Now() + "]");
                isSucceeded = false;
            }
            return isSucceeded;
        }

        public static int createUser(user u)
        {
            int user_id = 0;
            try
            {
                u.isActive = true;
                u.key = true;   //- set to return primary key
                user_id = (int)u.CRUD(Crud.I, true);
            }
            catch (Exception e) {
                //- Log into NLog if failed
                //- Nlog.write(e.Message + " [" + DateTime.Now() + "]");
                user_id = 0;
            }
            return user_id;
        }

        public static int deleteUser(int id)
        {
            try
            {
                user u = new user();
                u.id = id;
                int result = (int)u.CRUD(Crud.D, true);
            }
            catch (Exception e) {
                //- Log into NLog if failed
                //- Nlog.write(e.Message + " [" + DateTime.Now() + "]");
                id = 0;
            }
            return id;
        }
    }
}
