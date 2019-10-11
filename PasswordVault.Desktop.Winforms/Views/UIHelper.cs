using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Desktop.Winforms
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/
    public enum ErrorLevel
    {
        Error,
        Warning,
        Ok,
        Neutral,
    }

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    public static class UIFontSizes

    {
        public const float DefaultFontSize = 9.0f;
        public const float CloseButtonFontSize = 12.0f;
        public const float TextBoxFontSize = 8.0f;
        public const float ButtonFontSize = 8.0f;
    }

    public static class UIColors
    {
        public const int StatusRedColor = 0xFF0000;
        public const int StatusYellowColor = 0xFFD633;
        public const int StatusGreenColor = 0x05ad0d;
        public const int DefaultFontColor = 0xF2F2F2;
        public const int ControlBackgroundColor = 0x3F3F3F;
        public const int ControlHighLightColor = 0x505050;
        public const int DefaultBackgroundColor = 0x232323;
        public const int SecondaryFromBackgroundColor = 0x2A2A2A;
        public const int CloseButtonColor = 0xb0270c;
    }

    public static class UIHelper
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
        public static Font GetFont(float fontSize)
        {
            return new Font("Segoe UI", fontSize, FontStyle.Bold);
        }

        /*************************************************************************************************/
        public static void UpdateStatusLabel(string message, Label label, ErrorLevel errorLevel)
        {
            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            int color = 0;
            label.Text = message;

            switch(errorLevel)
            {
                case ErrorLevel.Error:
                    color = UIColors.StatusRedColor;
                    break;

                case ErrorLevel.Warning:
                    color = UIColors.StatusYellowColor;
                    break;

                case ErrorLevel.Ok:
                    color = UIColors.StatusGreenColor;
                    break;

                case ErrorLevel.Neutral:
                    color = UIColors.DefaultFontColor;
                    break;
            }

            int red = ((color & 0xFF0000) >> 16);
            int green = ((color & 0x00FF00) >> 8);
            int blue = (color & 0x0000FF);

            label.ForeColor = Color.FromArgb(red, green, blue);
        }

        /*************************************************************************************************/
        public static void UpdateStatusLabel(string message, ToolStripStatusLabel label, ErrorLevel errorLevel)
        {
            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            int color = 0;
            label.Text = message;

            switch(errorLevel)
            {
                case ErrorLevel.Error:
                    color = UIColors.StatusRedColor;
                    break;

                case ErrorLevel.Warning:
                    color = UIColors.StatusYellowColor;
                    break;

                case ErrorLevel.Ok:
                    color = UIColors.StatusGreenColor;
                    break;

                case ErrorLevel.Neutral:
                    color = UIColors.DefaultFontColor;
                    break;
            }

            int red = ((color & 0xFF0000) >> 16);
            int green = ((color & 0x00FF00) >> 8);
            int blue = (color & 0x0000FF);

            label.ForeColor = Color.FromArgb(red, green, blue);
        }

        /*************************************************************************************************/
        public static Color GetColorFromCode(int code)
        {
            int red = ((code & 0xFF0000) >> 16);
            int green = ((code & 0x00FF00) >> 8);
            int blue = (code & 0x0000FF);
            return Color.FromArgb(red, green, blue);
        }

    } // UIHelper CLASS
} // PasswordVault NAMESPACE
