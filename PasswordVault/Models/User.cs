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
    public class User
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

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
        public string PlainTextRandomKey { get; }
        public string PlainTextPassword { get; }
        public bool ValidUser { get; set; }

        // Properties stored in database
        public string UniqueID { get; } // unique Id assigned to each user, this unique id is the PK for password table
        public string EncryptedKey { get; } // encrypted version of randomly generated key, encrypted using the plaintext user password  
        public string Username { get; } 
        public string Iterations { get; }
        public string Salt { get; }
        public string Hash { get; }
        public string FirstName { get; } // use randomly generated key to hash and store
        public string LastName { get; } // use randomly generated key to hash and store
        public string PhoneNumber { get; } // use randomly generated key to hash and store
        public string Email { get; } // use randomly generated key to hash and store



        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public User(string uniqueID, string encryptedKey, string username, string iterations, string salt, string hash, string firstName, string lastName, string phoneNumber, string email, bool validUser = false)
        {
            UniqueID = uniqueID;
            EncryptedKey = encryptedKey;
            Username = username;
            Iterations = iterations;
            Salt = salt;
            Hash = hash;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;

            ValidUser = validUser;
        }

        public User(string username, string salt, string hash, string key, bool validKey = false)
        {
            Username = username;
            Salt = salt;
            Hash = hash;
            PlainTextRandomKey = key;
            ValidUser = validKey;
        }

        public User(string username, string salt, string hash, bool validKey = false)
        {
            Username = username;
            Salt = salt;
            Hash = hash;
            ValidUser = validKey;
        }

        public User(bool validKey = false)
        {
            ValidUser = validKey;
        }

        public User(string username)
        {
            Username = username;
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public string GetUserString()
        {
            string userString = "";

            userString = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{8}", UniqueID, EncryptedKey, Username, Iterations, Salt, Hash, FirstName, LastName, PhoneNumber, Email);

            return userString;
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // User CLASS
} // PasswordHashTest NAMESPACE
