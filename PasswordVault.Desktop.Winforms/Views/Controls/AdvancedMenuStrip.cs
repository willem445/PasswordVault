using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    class AdvancedMenuStrip : MenuStrip
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
        private MyColorTable _myColorTable;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
        [Category("Appearance")]
        public Color MenuItemSelectedColor
        {
            get { return _myColorTable.MenuItemSelectedColor; }
            set
            {
                _myColorTable.MenuItemSelectedColor = value;
                Invalidate(); // causes control to be redrawn
            }
        }

        [Category("Appearance")]
        public Color MenuItemBackgroundColor
        {
            get { return _myColorTable.MenuItemBackgroundColor; }
            set
            {
                _myColorTable.MenuItemBackgroundColor = value;
                Invalidate();
            }
        }

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public AdvancedMenuStrip()
        {
            _myColorTable = new MyColorTable();

            this.Renderer = new ToolStripProfessionalRenderer(_myColorTable); 
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*************************************************************************************************/


        /*=================================================================================================
        STATIC METHODS
        *================================================================================================
        /*************************************************************************************************/

    } // AdvancedComboBox CLASS

    public class MyColorTable : ProfessionalColorTable
    {
        public Color MenuItemSelectedColor { get; set; } = Color.FromArgb(0x80, 0x80, 0x80);
        public Color MenuItemBackgroundColor { get; set; } = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);

        public override Color ToolStripDropDownBackground
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color ImageMarginGradientBegin
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color ImageMarginGradientEnd
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color MenuBorder
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color MenuItemSelected
        {
            get
            {
                return MenuItemSelectedColor; 
            }
        }

        public override Color MenuStripGradientBegin
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color MenuStripGradientEnd
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get
            {
                return MenuItemBackgroundColor;
            }
        }
    }
} // PasswordVault NAMESPACE
