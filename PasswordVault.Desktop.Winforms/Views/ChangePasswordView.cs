using System;
using System.Drawing;
using System.Windows.Forms;
using PasswordVault.Services;
using PasswordVault.Models;

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
    public partial class ChangePasswordView : Form, IChangePasswordView
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
        public event Action<string, string, string> ChangePasswordEvent;
        public event Action<string> PasswordTextChangedEvent;
        public event Action GenerateNewPasswordEvent;

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
        public ChangePasswordView()
        {
            InitializeComponent();

            // Configure form UI
            BackColor = Color.FromArgb(42, 42, 42);
            FormBorderStyle = FormBorderStyle.None;

            // Configure buttons
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            closeButton.Font = UIHelper.GetFont(UIFontSizes.CloseButtonFontSize);

            changePasswordButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            changePasswordButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            changePasswordButton.FlatStyle = FlatStyle.Flat;
            changePasswordButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            changePasswordButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            changePasswordButton.FlatAppearance.BorderSize = 1;

            generatePasswordButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            generatePasswordButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            generatePasswordButton.FlatStyle = FlatStyle.Flat;
            generatePasswordButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            generatePasswordButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            generatePasswordButton.FlatAppearance.BorderSize = 1;

            // Configure textbox
            currentPasswordTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            currentPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            currentPasswordTextbox.BorderStyle = BorderStyle.FixedSingle;
            currentPasswordTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize, true);

            passwordTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            passwordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            passwordTextbox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize, true);

            confirmPasswordTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            confirmPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            confirmPasswordTextbox.BorderStyle = BorderStyle.FixedSingle;
            confirmPasswordTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize, true);

            // Configure labels
            label1.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label1.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            label2.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label2.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            label3.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label3.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            label4.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label4.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            passwordStrengthLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            passwordStrengthLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            passwordStrengthLabel.Text = "Weak";

            statusLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            statusLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            statusLabel.Text = "";

            //Configure progressbar
            passwordStrengthProgressBar.ForeColor = Color.Red;
            passwordStrengthProgressBar.BackColor = Color.Red;
            passwordStrengthProgressBar.Value = 1;
            passwordStrengthProgressBar.Style = ProgressBarStyle.Continuous;
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void ShowChangePassword()
        {
            this.Show();
            currentPasswordTextbox.Focus();
        }

        /*************************************************************************************************/
        public void DisplayGeneratedPassword(string generatedPassword)
        {
            passwordTextbox.Text = generatedPassword;
        }

        /*************************************************************************************************/
        public void DisplayPasswordComplexity(PasswordComplexityLevel complexity)
        {
            switch (complexity)
            {
                case PasswordComplexityLevel.Weak:
                    passwordStrengthProgressBar.ForeColor = Color.Red;
                    passwordStrengthProgressBar.BackColor = Color.Red;
                    passwordStrengthProgressBar.Value = 25;
                    passwordStrengthLabel.Text = "Weak";
                    break;

                case PasswordComplexityLevel.Mediocre:
                    passwordStrengthProgressBar.ForeColor = Color.Orange;
                    passwordStrengthProgressBar.BackColor = Color.Orange;
                    passwordStrengthProgressBar.Value = 50;
                    passwordStrengthLabel.Text = "Mediocre";
                    break;

                case PasswordComplexityLevel.Ok:
                    passwordStrengthProgressBar.ForeColor = Color.YellowGreen;
                    passwordStrengthProgressBar.BackColor = Color.YellowGreen;
                    passwordStrengthProgressBar.Value = 75;
                    passwordStrengthLabel.Text = "Ok";
                    break;

                case PasswordComplexityLevel.Great:
                    passwordStrengthProgressBar.ForeColor = Color.Green;
                    passwordStrengthProgressBar.BackColor = Color.Green;
                    passwordStrengthProgressBar.Value = 100;
                    passwordStrengthLabel.Text = "Great";
                    break;

                default:
                    passwordStrengthProgressBar.ForeColor = Color.Red;
                    passwordStrengthProgressBar.BackColor = Color.Red;
                    passwordStrengthProgressBar.Value = 1;
                    passwordStrengthLabel.Text = "Weak";
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayChangePasswordResult(ValidateUserPasswordResult result)
        {
            switch(result)
            {
                case ValidateUserPasswordResult.Failed:
                    UIHelper.UpdateStatusLabel("Failed!", statusLabel, ErrorLevel.Error);
                    break;

                case ValidateUserPasswordResult.PasswordsDoNotMatch:
                    UIHelper.UpdateStatusLabel("Passwords do not match!", statusLabel, ErrorLevel.Error);
                    break;

                case ValidateUserPasswordResult.LengthRequirementNotMet:
                    UIHelper.UpdateStatusLabel("Password not long enough!", statusLabel, ErrorLevel.Error);
                    break;

                case ValidateUserPasswordResult.NoLowerCaseCharacter:
                    UIHelper.UpdateStatusLabel("Password does not contain lower case!", statusLabel, ErrorLevel.Error);
                    break;

                case ValidateUserPasswordResult.NoNumber:
                    UIHelper.UpdateStatusLabel("Password does not contain number!", statusLabel, ErrorLevel.Error);
                    break;

                case ValidateUserPasswordResult.NoSpecialCharacter:
                    UIHelper.UpdateStatusLabel("Password does not contain special char!", statusLabel, ErrorLevel.Error);
                    break;

                case ValidateUserPasswordResult.NoUpperCaseCharacter:
                    UIHelper.UpdateStatusLabel("Password does not contain upper case!", statusLabel, ErrorLevel.Error);
                    break;

                case ValidateUserPasswordResult.InvalidPassword:
                    UIHelper.UpdateStatusLabel("Invalid password!", statusLabel, ErrorLevel.Error);
                    break;

                case ValidateUserPasswordResult.Success:
                    UIHelper.UpdateStatusLabel("Success!", statusLabel, ErrorLevel.Ok);
                    ClearChangePasswordView();
                    this.Close();
                    break;

                default:
                    UIHelper.UpdateStatusLabel("Failed!", statusLabel, ErrorLevel.Error);
                    break;
            }
        }

        /*************************************************************************************************/
        public void CloseView()
        {
            ClearChangePasswordView();
            this.Hide();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void changePasswordButton_Click(object sender, EventArgs e)
        {
            RaiseChangePasswordEvent(currentPasswordTextbox.Text, passwordTextbox.Text, confirmPasswordTextbox.Text);
        }

        /*************************************************************************************************/
        private void RaiseChangePasswordEvent(string oldPassword, string password, string confirmPassword)
        {
            ChangePasswordEvent?.Invoke(oldPassword, password, confirmPassword);
        }

        /*************************************************************************************************/
        private void passwordTextbox_TextChanged(object sender, EventArgs e)
        {
            RaisePasswordTextChangedEvent(passwordTextbox.Text);
        }

        /*************************************************************************************************/
        private void RaisePasswordTextChangedEvent(string password)
        {
            PasswordTextChangedEvent?.Invoke(password);
        }

        /*************************************************************************************************/
        private void generatePasswordButton_Click(object sender, EventArgs e)
        {
            RaiseGeneratePasswordEvent();
        }

        /*************************************************************************************************/
        private void RaiseGeneratePasswordEvent()
        {
            if (GenerateNewPasswordEvent != null)
            {
                GenerateNewPasswordEvent();
            }
        }

        /*************************************************************************************************/
        private void closeButton_Click(object sender, EventArgs e)
        {
            ClearChangePasswordView();
            this.Close();
            DialogResult = DialogResult.Cancel;
        }

        /*************************************************************************************************/
        private void closeButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.CloseButtonColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void closeButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void moveWindowPanel_MouseDown_1(object sender, MouseEventArgs e)
        {
            _draggingWindow = true;
            _start_point = new Point(e.X, e.Y);
        }

        /*************************************************************************************************/
        private void moveWindowPanel_MouseUp_1(object sender, MouseEventArgs e)
        {
            _draggingWindow = false;
        }

        /*************************************************************************************************/
        private void moveWindowPanel_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (_draggingWindow)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

        /*************************************************************************************************/
        private void ClearChangePasswordView()
        {
            currentPasswordTextbox.Text = "";
            confirmPasswordTextbox.Text = "";
            passwordTextbox.Text = "";
            passwordStrengthProgressBar.ForeColor = Color.Red;
            passwordStrengthProgressBar.BackColor = Color.Red;
            passwordStrengthProgressBar.Value = 0;
            passwordStrengthLabel.Text = "";
            statusLabel.Text = "";
        }

        /*************************************************************************************************/
        private void ChangePasswordView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

    }// ChangePasswordView CLASS
} // PasswordVault NAMESPACE
