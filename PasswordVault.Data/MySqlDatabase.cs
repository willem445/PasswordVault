using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using PasswordVault.Models;


namespace PasswordVault.Data
{
    public class MySQLDatabase : IDatabase
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public MySQLDatabase()
        {

        }

        /*PUBLIC METHODS***************************************************/
        public Int64 AddPassword(DatabasePassword password)
        {
            throw new NotImplementedException();
        }

        public bool AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DeletePassword(Int64 passwordUniqueId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(User user, int expectedNumPasswords)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUserByUuid(string uuid)
        {
            throw new NotImplementedException();
        }

        public User GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public List<DatabasePassword> GetUserPasswordsByUuid(string uuid)
        {
            throw new NotImplementedException();
        }

        public bool ModifyPassword(DatabasePassword modifiedPassword)
        {
            throw new NotImplementedException();
        }

        public bool ModifyUser(User user, User modifiedUser)
        {
            throw new NotImplementedException();
        }

        public bool UserExistsByUuid(string uuid)
        {
            throw new NotImplementedException();
        }

        public bool UserExistsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // MySQLDatabase CLASS
}
