using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PasswordVault.Desktop.Winforms
{
    public partial class ShowPasswordView : Form
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
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to

        /*=================================================================================================
        PROPERTIES
        *================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public ShowPasswordView(string password)
        {
            InitializeComponent();

            BackColor = Color.FromArgb(35, 35, 35);
            FormBorderStyle = FormBorderStyle.None;

            closeButton.BackColor = Color.FromArgb(63, 63, 63);
            closeButton.ForeColor = Color.FromArgb(242, 242, 242);
            closeButton.Font = new Font("Segoe UI", 12.0f, FontStyle.Bold);

            showPasswordTextbox.BackColor = Color.FromArgb(63, 63, 63);
            showPasswordTextbox.ForeColor = Color.FromArgb(242, 242, 242);
            showPasswordTextbox.BorderStyle = BorderStyle.FixedSingle;
            showPasswordTextbox.ReadOnly = true;
            showPasswordTextbox.Text = password;
            showPasswordTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            showPasswordTextbox.SelectionStart = 0;
            showPasswordTextbox.SelectionLength = 0;

            okButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            okButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            okButton.FlatStyle = FlatStyle.Flat;
            okButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            okButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            okButton.FlatAppearance.BorderSize = 1;

            copyButton.BackColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            copyButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            copyButton.FlatStyle = FlatStyle.Flat;
            copyButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            copyButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            copyButton.FlatAppearance.BorderSize = 1;
            copyButton.Image = Bitmap.FromFile(Path.Combine(Environment.CurrentDirectory, @"Resources\icons8-copy-24.png"));
            copyButton.ImageAlign = ContentAlignment.MiddleCenter;

            okButton.Focus();
            this.ActiveControl = okButton;
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*************************************************************************************************/
        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.BackColor = Color.FromArgb(107, 20, 3);
            closeButton.ForeColor = Color.FromArgb(242, 242, 242);
        }

        /*************************************************************************************************/
        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.BackColor = Color.FromArgb(63, 63, 63);
            closeButton.ForeColor = Color.FromArgb(242, 242, 242);
        }

        /*************************************************************************************************/
        private void MoveWindowPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _draggingWindow = true;
            _start_point = new Point(e.X, e.Y);
        }

        /*************************************************************************************************/
        private void MoveWindowPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingWindow)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

        /*************************************************************************************************/
        private void MoveWindowPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _draggingWindow = false;
        }

        private void copyButton_MouseEnter(object sender, EventArgs e)
        {
            copyButton.Image = Bitmap.FromFile(Path.Combine(Environment.CurrentDirectory, @"Resources\icons8-copy-24_hover.png"));
        }

        private void copyButton_MouseLeave(object sender, EventArgs e)
        {
            copyButton.Image = Bitmap.FromFile(Path.Combine(Environment.CurrentDirectory, @"Resources\icons8-copy-24.png"));
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(showPasswordTextbox.Text))
            {
                System.Windows.Forms.Clipboard.SetText(showPasswordTextbox.Text);
            }
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
    } // AboutView CLASS
} // AboutView NAMESPACE