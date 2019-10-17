using System;
using System.Drawing;
using System.Windows.Forms;
using PasswordVault.Services;

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
            closeButton.BackColor = Color.FromArgb(63, 63, 63);
            closeButton.ForeColor = Color.FromArgb(242, 242, 242);
            closeButton.Font = new Font("Segoe UI", 12.0f, FontStyle.Bold);

            changePasswordButton.BackColor = Color.FromArgb(63, 63, 63);
            changePasswordButton.ForeColor = Color.FromArgb(242, 242, 242);
            changePasswordButton.FlatStyle = FlatStyle.Flat;
            changePasswordButton.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);
            changePasswordButton.FlatAppearance.BorderColor = Color.FromArgb(35, 35, 35);
            changePasswordButton.FlatAppearance.BorderSize = 1;

            generatePasswordButton.BackColor = Color.FromArgb(63, 63, 63);
            generatePasswordButton.ForeColor = Color.FromArgb(242, 242, 242);
            generatePasswordButton.FlatStyle = FlatStyle.Flat;
            generatePasswordButton.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);
            generatePasswordButton.FlatAppearance.BorderColor = Color.FromArgb(35, 35, 35);
            generatePasswordButton.FlatAppearance.BorderSize = 1;

            // Configure textbox
            currentPasswordTextbox.BackColor = Color.FromArgb(63, 63, 63);
            currentPasswordTextbox.ForeColor = Color.FromArgb(242, 242, 242);
            currentPasswordTextbox.BorderStyle = BorderStyle.FixedSingle;
            currentPasswordTextbox.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);

            passwordTextbox.BackColor = Color.FromArgb(63, 63, 63);
            passwordTextbox.ForeColor = Color.FromArgb(242, 242, 242);
            passwordTextbox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextbox.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);

            confirmPasswordTextbox.BackColor = Color.FromArgb(63, 63, 63);
            confirmPasswordTextbox.ForeColor = Color.FromArgb(242, 242, 242);
            confirmPasswordTextbox.BorderStyle = BorderStyle.FixedSingle;
            confirmPasswordTextbox.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);

            // Configure labels
            label1.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(242, 242, 242);

            label2.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(242, 242, 242);

            label3.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(242, 242, 242);

            label4.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(242, 242, 242);

            passwordStrengthLabel.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            passwordStrengthLabel.ForeColor = Color.FromArgb(242, 242, 242);
            passwordStrengthLabel.Text = "Weak";

            statusLabel.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            statusLabel.ForeColor = Color.FromArgb(242, 242, 242);
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
        public void DisplayChangePasswordResult(ChangeUserPasswordResult result)
        {
            switch(result)
            {
                case ChangeUserPasswordResult.Failed:
                    statusLabel.Text = "Failed!";
                    break;

                case ChangeUserPasswordResult.PasswordsDoNotMatch:
                    statusLabel.Text = "Passwords do not match!";
                    statusLabel.ForeColor = Color.Red;
                    break;

                case ChangeUserPasswordResult.LengthRequirementNotMet:
                    statusLabel.Text = "Passwords do not match!";
                    statusLabel.ForeColor = Color.Red;
                    break;

                case ChangeUserPasswordResult.NoLowerCaseCharacter:
                    statusLabel.Text = "Passwords do not match!";
                    statusLabel.ForeColor = Color.Red;
                    break;

                case ChangeUserPasswordResult.NoNumber:
                    statusLabel.Text = "Passwords do not match!";
                    statusLabel.ForeColor = Color.Red;
                    break;

                case ChangeUserPasswordResult.NoSpecialCharacter:
                    statusLabel.Text = "Passwords do not match!";
                    statusLabel.ForeColor = Color.Red;
                    break;

                case ChangeUserPasswordResult.NoUpperCaseCharacter:
                    statusLabel.Text = "Passwords do not match!";
                    statusLabel.ForeColor = Color.Red;
                    break;

                case ChangeUserPasswordResult.InvalidPassword:
                    statusLabel.Text = "Password not correct!";
                    statusLabel.ForeColor = Color.Red;
                    break;

                case ChangeUserPasswordResult.Success:
                    ClearChangePasswordView();
                    this.Close();
                    break;

                default:
                    statusLabel.Text = "Failed!";
                    statusLabel.ForeColor = Color.Red;
                    break;
            }
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
            passwordStrengthProgressBar.Value = 25;
            passwordStrengthLabel.Text = "Weak";
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
