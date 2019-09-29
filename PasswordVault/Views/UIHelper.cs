﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
    public static class UIHelper
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
        public const float LabelFontSize = 9.0f;
        public const float CloseButtonFontSize = 12.0f;
        public const float TextBoxFontSize = 8.0f;
        public const float ButtonFontSize = 8.0f;

        private const int StatusRedColor = 0xFF0000;
        private const int StatusYellowColor = 0xFFD633;
        private const int StatusGreenColor = 0x00CC44;
        private const int DefaultWhiteFontColor = 0xF2F2F2;     
        private const int ControlBackgroundColor = 0x3F3F3F;
        private const int MainFormBackgroundColor = 0x232323;
        private const int SecondaryFromBackgroundColor = 0x2A2A2A;

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

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public static Font UIFont(float fontSize)
        {
            return new Font("Segoe UI", fontSize, FontStyle.Bold);
        }

    } // UIHelper CLASS
} // PasswordVault NAMESPACE
