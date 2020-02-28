using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using PasswordVault.Models;


namespace PasswordVault.Data
{
    public class InMemoryDatabase : IDatabase
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private List<DatabasePassword> _localPasswordDb;
        private List<User> _localUserDb;

        /*PROPERTIES*******************************************************/
        public List<DatabasePassword> LocalPasswordDbAccess
        {
            get
            {
                return _localPasswordDb;
            }
        }

        public List<User> LocalUserDbAccess
        {
            get
            {
                return _localUserDb;
            }
        }

        /*CONSTRUCTORS*****************************************************/
        public InMemoryDatabase()
        {
            _localPasswordDb = new List<DatabasePassword>();
            _localUserDb = new List<User>();
        }

        /*PUBLIC METHODS***************************************************/
        public Int64 AddPassword(DatabasePassword password)
        {
            Int64 uniqueID = 0;

            if (password != null)
            {
                if (_localPasswordDb.Count != 0)
                {
                    uniqueID = _localPasswordDb[_localPasswordDb.Count - 1].UniqueID + 1;
                }

                DatabasePassword newPassword = new DatabasePassword(
                    uniqueID,
                    password.UserUuid,
                    password.Application,
                    password.Username,
                    password.Email,
                    password.Description,
                    password.Website,
                    password.Passphrase);

                _localPasswordDb.Add(newPassword);
            }

            return uniqueID;
        }

        public bool AddUser(User user)
        {
            _localUserDb.Add(user);
            return true;
        }

        public bool DeletePassword(Int64 passwordUniqueId)
        {
            _localPasswordDb.RemoveAll(x => x.UniqueID == passwordUniqueId);
            return true;
        }
       
        public bool DeleteUser(User user, int expectedNumPasswords)
        {
            _localUserDb.RemoveAll(x => x.Uuid == user.Uuid);
            _localPasswordDb.RemoveAll(x => x.UserUuid == user.Uuid);
            return true;
        }
       
        public List<User> GetAllUsers()
        {
            return _localUserDb;
        }
       
        public User GetUserByUuid(string uuid)
        {
            return _localUserDb.Where(x => x.Uuid == uuid).FirstOrDefault();
        }
      
        public User GetUserByUsername(string username)
        {
            return _localUserDb.Where(x => x.Username == username).FirstOrDefault();
        }
        
        public List<DatabasePassword> GetUserPasswordsByUuid(string uuid)
        {
            return _localPasswordDb.Where(x => x.UserUuid == uuid).ToList();
        }
        
        public bool ModifyPassword(DatabasePassword modifiedPassword)
        {
            bool result = false;

            int index = _localPasswordDb.FindIndex(x => x.UniqueID == modifiedPassword.UniqueID);

            if (index != -1)
            {
                _localPasswordDb[index] = modifiedPassword;
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
       
        public bool ModifyUser(User user, User modifiedUser)
        {
            bool result = false;

            int index = _localUserDb.FindIndex(x => x.Uuid == user.Uuid);

            if (index != -1)
            {
                _localUserDb[index] = modifiedUser;
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
        
        public bool UserExistsByUuid(string uuid)
        {
            bool exists = _localUserDb.Exists(x => x.Uuid == uuid);
            return exists;
        }
       
        public bool UserExistsByUsername(string username)
        {
            bool exists = _localUserDb.Exists(x => x.Username == username);
            return exists;
        }     

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // InMemoryDatabase CLASS
}
