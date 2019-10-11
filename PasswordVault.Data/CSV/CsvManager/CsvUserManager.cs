using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Models;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

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
    public class CSVUserManager : ICSVUserManager
    {
        private enum CsvUserIndexes
        {
            UniqueId,
            EncryptedKey,
            Username,
            Iterations,
            Salt,
            Hash,
            FirstName,
            LastName,
            PhoneNumber,
            Email,
            NumIndexes
        }

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

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private ICSVReader _reader;
        private ICSVWriter _writer;
        private List<User> _encryptedUserList;

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public CSVUserManager(ICSVReader reader, ICSVWriter writer)
        {
            _reader = reader;
            _writer = writer;
            _encryptedUserList = new List<User>();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public void ParseUsersCSVFile(string file)
        {
            string line;
            _reader.Initialize(file);

            line = _reader.ReadLine();

            while (ValidLine(line))
            {
                _encryptedUserList.Add(ParseLine(line));
                line = _reader.ReadLine();
            }

            _reader.Close();
        }

        /*************************************************************************************************/
        public List<User> GetEncryptedUsers()
        {
            return _encryptedUserList;
        }

        /*************************************************************************************************/
        public void UpdateUsersCSVFile(string filename, List<User> encryptedUsers)
        {
            if (encryptedUsers == null)
            {
                throw new ArgumentNullException(nameof(encryptedUsers));
            }

            _writer.Initialize(filename);

            foreach (var user in encryptedUsers)
            {
                _writer.WriteLine(user.GetUserString());
            }

            _writer.Close();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private User ParseLine(string line)
        {
            User user = null;

            string[] fields = line.Split(',');

            if (fields.Length == (int)CsvUserIndexes.NumIndexes)
            {
                user = new User(fields[(int)CsvUserIndexes.UniqueId],
                                fields[(int)CsvUserIndexes.EncryptedKey],
                                fields[(int)CsvUserIndexes.Username],
                                fields[(int)CsvUserIndexes.Iterations],
                                fields[(int)CsvUserIndexes.Salt],
                                fields[(int)CsvUserIndexes.Hash],
                                fields[(int)CsvUserIndexes.FirstName],
                                fields[(int)CsvUserIndexes.LastName],
                                fields[(int)CsvUserIndexes.PhoneNumber],
                                fields[(int)CsvUserIndexes.Email]);
            }

            return user;
        }

        /*************************************************************************************************/
        private bool ValidLine(string line)
        {
            bool result = false;

            if (line == null)
            {
                result = false;
            }
            else if (line.Split(',').Length == (int)CsvUserIndexes.NumIndexes)
            {
                result = true;
            }

            return result;
        }

        /*************************************************************************************************/


        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // CSVUserParser CLASS
} // PasswordVault.Data NAMESPACE
