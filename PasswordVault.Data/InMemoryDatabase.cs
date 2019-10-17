 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Models;

namespace PasswordVault.Data
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    public class InMemoryDatabase : IDatabase
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private List<DatabasePassword> _localPasswordDb;
        private List<User> _localUserDb;
        private Int64 _lastUniqueId = 0;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
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

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public InMemoryDatabase()
        {
            _localPasswordDb = new List<DatabasePassword>();
            _localUserDb = new List<User>();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public bool AddPassword(DatabasePassword password)
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
                    password.UserGUID,
                    password.Application,
                    password.Username,
                    password.Email,
                    password.Description,
                    password.Website,
                    password.Passphrase);

                _localPasswordDb.Add(newPassword);
            }

            _lastUniqueId = uniqueID;

            return true;
        }

        /*************************************************************************************************/
        public bool AddUser(User user)
        {
            _localUserDb.Add(user);
            return true;
        }

        /*************************************************************************************************/
        public bool DeletePassword(DatabasePassword password)
        {
            _localPasswordDb.RemoveAll(x => x.UniqueID == password.UniqueID);
            return true;
        }

        /*************************************************************************************************/
        public bool DeleteUser(User user)
        {
            _localUserDb.RemoveAll(x => x.GUID == user.GUID);
            _localPasswordDb.RemoveAll(x => x.UserGUID == user.GUID);
            return true;
        }

        /*************************************************************************************************/
        public List<User> GetAllUsers()
        {
            return _localUserDb;
        }

        /*************************************************************************************************/
        public User GetUserByGUID(string guid)
        {
            return _localUserDb.Where(x => x.GUID == guid).FirstOrDefault();
        }

        /*************************************************************************************************/
        public User GetUserByUsername(string username)
        {
            return _localUserDb.Where(x => x.Username == username).FirstOrDefault();
        }

        /*************************************************************************************************/
        public List<DatabasePassword> GetUserPasswordsByGUID(string guid)
        {
            return _localPasswordDb.Where(x => x.UserGUID == guid).ToList();
        }

        /*************************************************************************************************/
        public bool ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword)
        {
            bool result = false;

            int index = _localPasswordDb.FindIndex(x => x.UniqueID == password.UniqueID);

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

        /*************************************************************************************************/
        public bool ModifyUser(User user, User modifiedUser)
        {
            bool result = false;

            int index = _localUserDb.FindIndex(x => x.GUID == user.GUID);

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

        /*************************************************************************************************/
        public bool UserExistsByGUID(string guid)
        {
            bool exists = _localUserDb.Exists(x => x.GUID == guid);
            return exists;
        }

        /*************************************************************************************************/
        public bool UserExistsByUsername(string username)
        {
            bool exists = _localUserDb.Exists(x => x.Username == username);
            return exists;
        }

        public Int64 GetLastUniqueId()
        {
            return _lastUniqueId;
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // InMemoryDatabase CLASS
}
