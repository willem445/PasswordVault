﻿using System;
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

        public bool DeletePassword(DatabasePassword password)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUserByGUID(string guid)
        {
            throw new NotImplementedException();
        }

        public User GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public List<DatabasePassword> GetUserPasswordsByGUID(string guid)
        {
            throw new NotImplementedException();
        }

        public bool ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword)
        {
            throw new NotImplementedException();
        }

        public bool ModifyUser(User user, User modifiedUser)
        {
            throw new NotImplementedException();
        }

        public bool UserExistsByGUID(string guid)
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
