﻿using System;
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
    class AdvancedContextMenuStrip : ContextMenuStrip
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
        public AdvancedContextMenuStrip()
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

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // AdvancedContextMenuStrip CLASS
} // PasswordVault NAMESPACE