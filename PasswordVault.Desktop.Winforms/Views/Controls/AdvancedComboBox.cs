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
    class AdvancedComboBox : ComboBox
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
        private const int WM_PAINT = 0xF;
        private Color _borderColor = Color.Black;
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
        new public System.Windows.Forms.DrawMode DrawMode { get; set; }
        public Color HighlightColor { get; set; }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate(); // causes control to be redrawn
            }
        }

        [Category("Appearance")]
        public ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }

        //[Browsable(true)]
        //[Category("Appearance")]
        //[DefaultValue(typeof(Color), "DimGray")]
        //public Color BorderColor { get; set; }

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public AdvancedComboBox()
        {
            base.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.HighlightColor = Color.Gray;
            this.DrawItem += new DrawItemEventHandler(AdvancedComboBox_DrawItem);

            //BorderColor = Color.DimGray;
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        void AdvancedComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ComboBox combo = sender as ComboBox;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                var brush = new SolidBrush(HighlightColor);
                e.Graphics.FillRectangle(brush, e.Bounds);
                brush.Dispose();
            }          
            else
            {
                var brush = new SolidBrush(combo.BackColor);
                e.Graphics.FillRectangle(brush, e.Bounds);
                brush.Dispose();
            }

            var brush2 = new SolidBrush(combo.ForeColor);
            e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                  brush2,
                                  new Point(e.Bounds.X, e.Bounds.Y));
            brush2.Dispose();

            e.DrawFocusRectangle();
        }

        /*************************************************************************************************/
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                Rectangle bounds = new Rectangle(0, 0, Width, Height);
                ControlPaint.DrawBorder(g, bounds, _borderColor, _borderStyle);
            }
        }


        /*=================================================================================================
        STATIC METHODS
        *================================================================================================
        /*************************************************************************************************/

    } // AdvancedComboBox CLASS
} // PasswordVault NAMESPACE
