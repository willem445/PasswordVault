﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* TODO - Add email field
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/
    public enum PasswordFilterOptions
    {
        Application = 0,
        Description = 1,
        Website = 2,
    }
    
    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    public class Password
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
        [Browsable(false)]
        public Int64 UniqueID { get; protected set; }

        [Browsable(true)]
        public string Application { get; protected set; }

        [Browsable(true)]
        public string Username { get; protected set; }

        [Browsable(true)]
        public string Description { get; protected set; }

        [Browsable(true)]
        public string Website { get; protected set; }

        [Browsable(false)]
        public string Passphrase { get; protected set; }

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public Password()
        {

        }

        /*************************************************************************************************/
        public Password(string application, string username, string description, string website, string passphrase)
        {
            Passphrase = passphrase;
            Application = application;
            Username = username;
            Description = description;
            Website = website;
        }

        /*************************************************************************************************/
        public Password(Int64 uniqueID, string application, string username, string description, string website, string passphrase)
        {
            UniqueID = uniqueID;
            Passphrase = passphrase;
            Application = application;
            Username = username;
            Description = description;
            Website = website;
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public virtual string GetPasswordString()
        {
            return string.Format("{0},{1},{2},{3},{4}", Application, Username, Description, Website, Passphrase);
        }

        /*************************************************************************************************/

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public static explicit operator Password(DataRow dr)
        {
            Password p = new Password();
            p.Application = dr.ItemArray[0].ToString();
            p.Username = dr.ItemArray[1].ToString();
            p.Description = dr.ItemArray[2].ToString();
            p.Website = dr.ItemArray[3].ToString();

            return p;
        }

    } // Password CLASS
} // PasswordHashTest NAMESPACE
