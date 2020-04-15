using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using PasswordVault.Models;
using PasswordVault.Services;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Desktop.Winforms
{
    public partial class AddPasswordView : Form, IAddPasswordView
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
        public event Action<Password> AddPasswordEvent;
        public event Action<string> PasswordChangedEvent;
        public event Action GenerateNewPasswordEvent;
        public event Action<AddPasswordResult> AddPasswordResultEvent;

        /*PRIVATE*****************************************************************************************/
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to
        private GhostTextBoxHelper ghostApplicationTextBox;
        private GhostTextBoxHelper ghostUsernameTextBox;
        private GhostTextBoxHelper ghostEmailTextBox;
        private GhostTextBoxHelper ghostDescriptionTextBox;
        private GhostTextBoxHelper ghostWebsiteTextBox;
        private GhostTextBoxHelper ghostPasswordTextBox;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public AddPasswordView()
        {
            InitializeComponent();

            #region UI
            // Configure form UI
            BackColor = UIHelper.GetColorFromCode(UIColors.SecondaryFromBackgroundColor);
            FormBorderStyle = FormBorderStyle.None;   

            // Configure buttons
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            closeButton.Font = UIHelper.GetFont(UIFontSizes.CloseButtonFontSize);

            generatePasswordButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            generatePasswordButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            generatePasswordButton.FlatStyle = FlatStyle.Flat;
            generatePasswordButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            generatePasswordButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            generatePasswordButton.FlatAppearance.BorderSize = 1;

            addButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            addButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            addButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            addButton.FlatAppearance.BorderSize = 1;

            cancelButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            cancelButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            cancelButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            cancelButton.FlatAppearance.BorderSize = 1;

            // Configure textbox
            applicationTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            applicationTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            applicationTextbox.BorderStyle = BorderStyle.FixedSingle;
            applicationTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            ghostApplicationTextBox = new GhostTextBoxHelper(applicationTextbox, "Application");

            usernameTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            usernameTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            usernameTextbox.BorderStyle = BorderStyle.FixedSingle;
            usernameTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            ghostUsernameTextBox = new GhostTextBoxHelper(usernameTextbox, "Username");

            emailTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            emailTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            emailTextbox.BorderStyle = BorderStyle.FixedSingle;
            emailTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            ghostEmailTextBox = new GhostTextBoxHelper(emailTextbox, "Email");

            descriptionTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            descriptionTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            descriptionTextbox.BorderStyle = BorderStyle.FixedSingle;
            descriptionTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            ghostDescriptionTextBox = new GhostTextBoxHelper(descriptionTextbox, "Description");

            websiteTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            websiteTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            websiteTextbox.BorderStyle = BorderStyle.FixedSingle;
            websiteTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            ghostWebsiteTextBox = new GhostTextBoxHelper(websiteTextbox, "Username");

            passwordTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            passwordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            passwordTextbox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            ghostPasswordTextBox = new GhostTextBoxHelper(passwordTextbox, "Username");

            // Configure labels

            //loginResultLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            //loginResultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
            //loginResultLabel.Visible = false;

            // Configure groupbox
            groupBox2.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            groupBox2.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            #endregion            
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void DisplayAddPasswordResult(AddPasswordResult result)
        {
            throw new NotImplementedException();
        }

        /*************************************************************************************************/
        public void DisplayGeneratePasswordResult(string generatedPassword)
        {

        }

        /*************************************************************************************************/
        public void DisplayPasswordComplexity(PasswordComplexityLevel complexity)
        {
            //switch (complexity)
            //{
            //    case PasswordComplexityLevel.Weak:
            //        createPasswordTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
            //        break;

            //    case PasswordComplexityLevel.Mediocre:
            //        createPasswordTextBox.ForeColor = Color.Orange;
            //        break;

            //    case PasswordComplexityLevel.Ok:
            //        createPasswordTextBox.ForeColor = Color.YellowGreen;
            //        break;

            //    case PasswordComplexityLevel.Great:
            //        createPasswordTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusGreenColor);
            //        break;

            //    default:
            //        createPasswordTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            //        break;
            //}
        }

        /*************************************************************************************************/
        public void ShowMenu()
        {
            this.Show();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        /*************************************************************************************************/

        /*************************************************************************************************/
        private void GeneratePasswordButton_Click(object sender, EventArgs e)
        {
            RaiseGeneratePasswordEvent();
        }

        /*************************************************************************************************/
        private void RaiseGeneratePasswordEvent()
        {

        }

        /*************************************************************************************************/
        private void MoveWindowPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _draggingWindow = true;
            _start_point = new Point(e.X, e.Y);
        }

        /*************************************************************************************************/
        private void MoveWindowPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _draggingWindow = false;
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
        private void CloseButton_Click(object sender, EventArgs e)
        {
            ClearView();
            this.Close();
            DialogResult = DialogResult.Cancel;
        }

        /*************************************************************************************************/
        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.CloseButtonColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void LoginView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        /*************************************************************************************************/
        private void ClearView()
        {

        }

        /*************************************************************************************************/
        private void passwordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // submit password
            {
                //if (!_editMode)
                //{
                //    RaiseAddPasswordEvent(applicationTextBox.Text,
                //                          usernameTextBox.Text,
                //                          emailTextBox.Text,
                //                          descriptionTextBox.Text,
                //                          websiteTextBox.Text,
                //                          passphraseTextBox.Text);
                //}
                //else
                //{
                //    RaiseEditOkayEvent(applicationTextBox.Text,
                //                       usernameTextBox.Text,
                //                       emailTextBox.Text,
                //                       descriptionTextBox.Text,
                //                       websiteTextBox.Text,
                //                       passphraseTextBox.Text);
                //}

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }



        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/


    } // LoginForm CLASS
} // LoginForm NAMESPACE
