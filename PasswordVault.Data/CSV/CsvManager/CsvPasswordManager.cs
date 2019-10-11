using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class CSVPasswordManager : ICSVPasswordManager
    {
        private enum CsvPasswordIndexes
        {
            UniqueId,
            UserId,
            Application,
            Username,
            Email,
            Description,
            Website,
            Password,
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
        private ICSVReader _reader;
        private ICSVWriter _writer;
        private List<DatabasePassword> _encryptedPasswordList;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public CSVPasswordManager(ICSVReader reader, ICSVWriter writer)
        {
            _reader = reader;
            _writer = writer;
            _encryptedPasswordList = new List<DatabasePassword>();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public List<DatabasePassword> GetEncryptedPasswords()
        {
            return _encryptedPasswordList;
        }

        /*************************************************************************************************/
        public void ParsePasswordCSVFile(string fileName)
        {
            string line;
            _reader.Initialize(fileName);

            line = _reader.ReadLine();

            while (ValidLine(line))
            {
                _encryptedPasswordList.Add(ParseLine(line));
                line = _reader.ReadLine();
            }

            _reader.Close();
        }

        /*************************************************************************************************/
        public void UpdatePasswordCSVFile(string filename, List<DatabasePassword> encryptedPasswords)
        {
            if (encryptedPasswords == null)
            {
                throw new ArgumentNullException(nameof(encryptedPasswords));
            }

            _writer.Initialize(filename);

            foreach (var password in encryptedPasswords)
            {
                _writer.WriteLine(password.GetPasswordString());
            }

            _writer.Close();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        /*************************************************************************************************/
        private DatabasePassword ParseLine(string line)
        {
            DatabasePassword pass = null;

            string[] fields = line.Split(',');

            if (fields.Length == (int)CsvPasswordIndexes.NumIndexes)
            {
                pass = new DatabasePassword(Convert.ToInt64(fields[(int)CsvPasswordIndexes.UniqueId], CultureInfo.CurrentCulture),
                                                            fields[(int)CsvPasswordIndexes.UserId],
                                                            fields[(int)CsvPasswordIndexes.Application],
                                                            fields[(int)CsvPasswordIndexes.Username],
                                                            fields[(int)CsvPasswordIndexes.Email],
                                                            fields[(int)CsvPasswordIndexes.Description],
                                                            fields[(int)CsvPasswordIndexes.Website],
                                                            fields[(int)CsvPasswordIndexes.Password]);
            }

            return pass;
        }

        /*************************************************************************************************/
        private bool ValidLine(string line)
        {
            bool result = false;

            if (line == null)
            {
                result = false;
            }
            else if (line.Split(',').Length == (int)CsvPasswordIndexes.NumIndexes)
            {
                result = true;
            }

            return result;
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // CSVPasswordManager CLASS
} // PasswordVault.Data NAMESPACE
