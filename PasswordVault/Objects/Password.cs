﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public string GetPasswordString()
        {
            return string.Format("{0},{1},{2},{3},{4}", Application, Username, Description, Website, Passphrase);
        }

        /*************************************************************************************************/
        public string GetPasswordStringWithMasterUserID(string masterUserID)
        {
            return string.Format("{0},{1},{2},{3},{4},{5}", masterUserID, Application, Username, Description, Website, Passphrase);
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // Password CLASS
} // PasswordHashTest NAMESPACE
