
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault
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
    public sealed class CSV : IStorage
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private const string BASE_PATH = @"..\..\CSV\";
        private const string USERS_CSV_PATH = BASE_PATH + @"users.csv";

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private string _passwordFileName = "";
        private List<User> _encryptedUsers;
        private List<Password> _encryptedPasswords;
        private static CSV _instance = null;
        private static Object _mutex = new Object();
        private ICSVUserManager _csvUserManager;
        private bool _validUserPasswordTable = false;


        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        private CSV(ICSVUserManager csvUserManager)
        {
            _csvUserManager = csvUserManager;

            if (!Directory.Exists(BASE_PATH))
            {
                Directory.CreateDirectory(BASE_PATH);
            }

            if (!File.Exists(USERS_CSV_PATH))
            {
                File.Create(USERS_CSV_PATH);
            }

            _csvUserManager.ParseUsersCSVFile(USERS_CSV_PATH);
            _encryptedUsers = _csvUserManager.GetEncryptedUsers();
            _encryptedPasswords = new List<Password>();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public static CSV GetInstance(ICSVUserManager parseUsers)
        {
            if (_instance == null)
            {
                lock (_mutex) // now I can claim some form of thread safety...
                {
                    if (_instance == null)
                    {
                        _instance = new CSV(parseUsers);
                    }
                }
            }

            return _instance;
        }

        /*************************************************************************************************/
        public void AddUser(string username, string salt, string hash)
        {
            _encryptedUsers.Add(new User(username, salt, hash));
            _csvUserManager.UpdateUsersCSVFile(USERS_CSV_PATH, _encryptedUsers);

        }

        /*************************************************************************************************/
        public void ModifyUser(User user, User modifiedUser)
        {
            int index = GetIndexOfUser(user);

            if (index != -1)
            {
                _encryptedUsers[index] = modifiedUser;
                _csvUserManager.UpdateUsersCSVFile(USERS_CSV_PATH, _encryptedUsers);
            }            
        }

        /*************************************************************************************************/
        public void DeleteUser(User user)
        {
            int index = GetIndexOfUser(user);

            if (index != -1)
            {
                _encryptedUsers.Remove(user);
                _csvUserManager.UpdateUsersCSVFile(USERS_CSV_PATH, _encryptedUsers);
            }
        }

        /*************************************************************************************************/
        public User GetUser(string username)
        {
            User user;

            user = _encryptedUsers.FirstOrDefault(x => x.UserID == username);

            return user;
        }

        /*************************************************************************************************/
        public List<User> GetUsers()
        {
            return _encryptedUsers;
        }

        /*************************************************************************************************/
        public bool SetUserTableName(string name)
        {
            bool tableExists = false;
            string userTableFileName = CreateCSVFileNameFromTable(name);

            if (File.Exists(userTableFileName))
            {
                tableExists = true;
                _validUserPasswordTable = true;
                _passwordFileName = userTableFileName;
            }

            return tableExists;
        }

        /*************************************************************************************************/
        public void ClearUserTableName()
        {
            _passwordFileName = "";
            _validUserPasswordTable = false;
        }

        /*************************************************************************************************/
        void CreateUserTable(string name)
        {
            string userTableFileName = CreateCSVFileNameFromTable(name);

            File.Create(userTableFileName);
        }
            
        /*************************************************************************************************/
        public void AddPassword(Password password)
        {
            // TODO - Create password manager similar to user manager

            //string path = string.Format("{0}{1}.csv", BASE_PATH, _passwordFileName);

            //if (_passwordFileName != "")
            //{
            //    if (!File.Exists(path))
            //    {
            //        File.Create(path);
            //    }

            //    var write = File.AppendText(path);
            //    write.WriteLine(string.Format("{0},{1},{2},{3},{4}", password.Application, password.Username, password.Description,  password.Website, password.Passphrase));
            //    write.Close();
            //}
            
        }

        /*************************************************************************************************/
        public List<Password> GetPasswords()
        {
            List<Password> passwords = new List<Password>();
            //string path = string.Format("{0}{1}.csv", BASE_PATH, _passwordFileName);

            //using (var reader = new StreamReader(path))
            //{
            //    while (!reader.EndOfStream)
            //    {
            //        string[] line = reader.ReadLine().Split(',');
            //        passwords.Add(new Password(line[0], line[1], line[2], line[3], line[4]));
            //    }
            //}

            return passwords;
        }

        /*************************************************************************************************/
        public void ModifyPassword(Password password, Password modifiedPassword)
        {

        }

        /*************************************************************************************************/
        public void DeletePassword(Password password)
        {

        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private int GetIndexOfUser(User user)
        {
            int index = -1;

            index = _encryptedUsers.IndexOf(user);

            return index;
        }

        /*************************************************************************************************/
        private string CreateCSVFileNameFromTable(string name)
        {
            string userTableFileName = BASE_PATH + name + ".csv";

            return userTableFileName;
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // CSV CLASS
} // PasswordHashTest NAMESPACE
