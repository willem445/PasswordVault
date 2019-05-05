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
    public class CSV : IStorage
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
        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public CSV()
        {

        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public void AddUser(User user)
        {
            var write = File.AppendText(USERS_CSV_PATH);
            write.WriteLine(string.Format("{0},{1},{2},", user.UserID, user.Salt, user.Hash));
            write.Close();
        }

        public void ModifyUser(User user, User modifiedUser)
        {

        }

        public void DeleteUser(User user)
        {

        }

        public User GetUser(string username)
        {
            User user = new User();

            List<User> users = GetUsers();
            user = users.FirstOrDefault(x => x.UserID == username);

            return user;
        }

        public List<User> GetUsers()
        {
            List<User> list = new List<User>();

            using (var reader = new StreamReader(USERS_CSV_PATH))
            {
                while(!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(',');
                    list.Add(new User(line[0], line[1], line[2]));
                }
            }

            return list;
        }

        public void SetPasswordFileName(string name)
        {
            _passwordFileName = name;
        }

        public void AddPassword(Password password)
        {
            string path = string.Format("{0}{1}.csv", BASE_PATH, _passwordFileName);

            if (_passwordFileName != "")
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                }

                var write = File.AppendText(path);
                write.WriteLine(string.Format("{0},{1},{2},{3},{4}", password.Application, password.Username, password.Description,  password.Website, password.Passphrase));
                write.Close();

            }
            
        }

        public List<Password> GetPasswords()
        {
            List<Password> passwords = new List<Password>();
            string path = string.Format("{0}{1}.csv", BASE_PATH, _passwordFileName);

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(',');
                    passwords.Add(new Password(line[0], line[1], line[2], line[3], line[4]));
                }
            }

            return passwords;
        }

        public void ModifyPassword(Password password, Password modifiedPassword)
        {

        }

        public void DeletePassword(Password password)
        {

        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // CSV CLASS
} // PasswordHashTest NAMESPACE
