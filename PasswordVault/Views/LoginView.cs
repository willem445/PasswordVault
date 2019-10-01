using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class LoginView : Form, ILoginView
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
        public event Action<string, string> LoginEvent;
        public event Action<string, string, string, string, string, string> CreateNewUserEvent;
        public event Action GenerateNewPasswordEvent;
        public event Action<string> PasswordChangedEvent;
        public event Action LoginSuccessfulEvent;

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
        public LoginView()
        {
            InitializeComponent();

            #region UI
            // Configure form UI
            BackColor = Color.FromArgb(42, 42, 42);
            FormBorderStyle = FormBorderStyle.None;

            // Configure buttons
            closeButton.BackColor = Color.FromArgb(63, 63, 63);
            closeButton.ForeColor = Color.FromArgb(242, 242, 242);
            closeButton.Font = new Font("Segoe UI", 12.0f, FontStyle.Bold);

            loginButton.BackColor = Color.FromArgb(63, 63, 63);
            loginButton.ForeColor = Color.FromArgb(242, 242, 242);
            loginButton.FlatStyle = FlatStyle.Flat;
            loginButton.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);
            loginButton.FlatAppearance.BorderColor = Color.FromArgb(35, 35, 35);
            loginButton.FlatAppearance.BorderSize = 1;

            generatePasswordButton.BackColor = Color.FromArgb(63, 63, 63);
            generatePasswordButton.ForeColor = Color.FromArgb(242, 242, 242);
            generatePasswordButton.FlatStyle = FlatStyle.Flat;
            generatePasswordButton.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);
            generatePasswordButton.FlatAppearance.BorderColor = Color.FromArgb(35, 35, 35);
            generatePasswordButton.FlatAppearance.BorderSize = 1;

            createLoginButton.BackColor = Color.FromArgb(63, 63, 63);
            createLoginButton.ForeColor = Color.FromArgb(242, 242, 242);
            createLoginButton.FlatStyle = FlatStyle.Flat;
            createLoginButton.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);
            createLoginButton.FlatAppearance.BorderColor = Color.FromArgb(35, 35, 35);
            createLoginButton.FlatAppearance.BorderSize = 1;

            // Configure textbox
            loginUsernameTextBox.BackColor = Color.FromArgb(63, 63, 63);
            loginUsernameTextBox.ForeColor = Color.FromArgb(242, 242, 242);
            loginUsernameTextBox.BorderStyle = BorderStyle.FixedSingle;

            loginPasswordTextBox.BackColor = Color.FromArgb(63, 63, 63);
            loginPasswordTextBox.ForeColor = Color.FromArgb(242, 242, 242);
            loginPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;

            createUsernameTextBox.BackColor = Color.FromArgb(63, 63, 63);
            createUsernameTextBox.ForeColor = Color.FromArgb(242, 242, 242);
            createUsernameTextBox.BorderStyle = BorderStyle.FixedSingle;

            createPasswordTextBox.BackColor = Color.FromArgb(63, 63, 63);
            createPasswordTextBox.ForeColor = Color.FromArgb(242, 242, 242);
            createPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;

            createFirstNameTextBox.BackColor = Color.FromArgb(63, 63, 63);
            createFirstNameTextBox.ForeColor = Color.FromArgb(242, 242, 242);
            createFirstNameTextBox.BorderStyle = BorderStyle.FixedSingle;

            createLastNameTextBox.BackColor = Color.FromArgb(63, 63, 63);
            createLastNameTextBox.ForeColor = Color.FromArgb(242, 242, 242);
            createLastNameTextBox.BorderStyle = BorderStyle.FixedSingle;

            createEmailTextBox.BackColor = Color.FromArgb(63, 63, 63);
            createEmailTextBox.ForeColor = Color.FromArgb(242, 242, 242);
            createEmailTextBox.BorderStyle = BorderStyle.FixedSingle;

            createPhoneNumberTextBox.BackColor = Color.FromArgb(63, 63, 63);
            createPhoneNumberTextBox.ForeColor = Color.FromArgb(242, 242, 242);
            createPhoneNumberTextBox.BorderStyle = BorderStyle.FixedSingle;

            // Configure labels
            label1.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(242, 242, 242);

            label2.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(242, 242, 242);

            label3.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(242, 242, 242);

            label4.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(242, 242, 242);

            label5.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label5.ForeColor = Color.FromArgb(242, 242, 242);

            label6.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label6.ForeColor = Color.FromArgb(242, 242, 242);

            label7.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label7.ForeColor = Color.FromArgb(242, 242, 242);

            label8.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label8.ForeColor = Color.FromArgb(242, 242, 242);

            loginResultLabel.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            loginResultLabel.ForeColor = Color.FromArgb(255, 0, 0);
            loginResultLabel.Visible = false;

            createNewUserResultLabel.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            createNewUserResultLabel.ForeColor = Color.FromArgb(255, 0, 0);
            createNewUserResultLabel.Visible = false;

            // Configure groupbox
            groupBox1.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            groupBox1.ForeColor = Color.FromArgb(242, 242, 242);

            groupBox2.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            groupBox2.ForeColor = Color.FromArgb(242, 242, 242);
            #endregion

            createEmailTextBox.Text = "example@provider.com";
            createEmailTextBox.ForeColor = Color.FromArgb(0x6B, 0x6B, 0x6B);

            createPhoneNumberTextBox.Text = "xxx-xxx-xxxx";
            createPhoneNumberTextBox.ForeColor = Color.FromArgb(0x6B, 0x6B, 0x6B);
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void DisplayLoginResult(LoginResult result)
        {
            switch(result)
            {
                case LoginResult.PasswordIncorrect:
                    loginResultLabel.Visible = true;
                    loginResultLabel.Text = "Password incorrect.";
                    break;

                case LoginResult.UsernameDoesNotExist:
                    loginResultLabel.Visible = true;
                    loginResultLabel.Text = "Username doesn't exist.";
                    break;

                case LoginResult.Failed:
                    loginResultLabel.Visible = true;
                    loginResultLabel.Text = "Login failed.";
                    break;

                case LoginResult.Successful:
                    ClearLoginView();
                    DialogResult = DialogResult.OK;
                    this.Close();
                    RaiseLoginSuccessfulEvent();
                    break;

                default:
                    loginResultLabel.Visible = true;
                    loginResultLabel.Text = "Login failed.";
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayGeneratePasswordResult(string generatedPassword)
        {
            createPasswordTextBox.Text = generatedPassword;
        }

        /*************************************************************************************************/
        public void DisplayCreateNewUserResult(CreateUserResult result, int minimumPasswordLength)
        {
            switch(result)
            {
                case CreateUserResult.UsernameTaken:
                    UIHelper.UpdateStatusLabel("Username taken!", createNewUserResultLabel, ErrorLevel.Error);
                    break;

                case CreateUserResult.Failed:
                    UIHelper.UpdateStatusLabel("Unsuccessful!", createNewUserResultLabel, ErrorLevel.Error);
                    break;

                case CreateUserResult.PasswordNotValid:
                    UIHelper.UpdateStatusLabel("Password does not meet requirements!", createNewUserResultLabel, ErrorLevel.Error);
                    break;

                case CreateUserResult.UsernameNotValid:
                    UIHelper.UpdateStatusLabel("Invalid username!", createNewUserResultLabel, ErrorLevel.Error);
                    break;

                case CreateUserResult.FirstNameNotValid:
                    UIHelper.UpdateStatusLabel("First name not valid!", createNewUserResultLabel, ErrorLevel.Error);
                    break;

                case CreateUserResult.LastNameNotValid:
                    UIHelper.UpdateStatusLabel("Last name not valid!", createNewUserResultLabel, ErrorLevel.Error);
                    break;

                case CreateUserResult.PhoneNumberNotValid:
                    UIHelper.UpdateStatusLabel("Phone number not valid!", createNewUserResultLabel, ErrorLevel.Error);
                    break;

                case CreateUserResult.EmailNotValid:
                    UIHelper.UpdateStatusLabel("Email not valid!", createNewUserResultLabel, ErrorLevel.Error);
                    break;

                case CreateUserResult.Successful:
                    UIHelper.UpdateStatusLabel("Success. Please log in.", createNewUserResultLabel, ErrorLevel.Ok);
                    createUsernameTextBox.Text = "";
                    createPasswordTextBox.Text = "";
                    createFirstNameTextBox.Text = "";
                    createLastNameTextBox.Text = "";
                    createEmailTextBox.Text = "";
                    createPhoneNumberTextBox.Text = "";
                    break;

                default:       
                    createNewUserResultLabel.Text = "Unsuccessful.";
                    createNewUserResultLabel.ForeColor = Color.Red;
                    break;
            }

            createNewUserResultLabel.Visible = true;
        }

        /*************************************************************************************************/
        public void DisplayPasswordComplexity(PasswordComplexityLevel complexity)
        {
            switch (complexity)
            {
                case PasswordComplexityLevel.Weak:
                    createPasswordTextBox.ForeColor = Color.Red;
                    break;

                case PasswordComplexityLevel.Mediocre:
                    createPasswordTextBox.ForeColor = Color.Orange;
                    break;

                case PasswordComplexityLevel.Ok:
                    createPasswordTextBox.ForeColor = Color.YellowGreen;
                    break;

                case PasswordComplexityLevel.Great:
                    createPasswordTextBox.ForeColor = Color.Green;
                    break;

                default:
                    createPasswordTextBox.ForeColor = Color.FromArgb(242, 242, 242);
                    break;
            }
        }

        /*************************************************************************************************/
        public void ShowLoginMenu()
        {
            this.Show();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void LoginButton_Click(object sender, EventArgs e)
        {
            RaiseLoginEvent(loginUsernameTextBox.Text, loginPasswordTextBox.Text);
        }

        /*************************************************************************************************/
        private void RaiseLoginEvent(string username, string password)
        {
            if (LoginEvent != null)
            {
                LoginEvent(username, password);
            }
        }

        /*************************************************************************************************/
        private void GeneratePasswordButton_Click(object sender, EventArgs e)
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
        private void CreateLoginButton_Click(object sender, EventArgs e)
        {
            RaiseCreateNewUserEvent(createUsernameTextBox.Text, createPasswordTextBox.Text, createFirstNameTextBox.Text, createLastNameTextBox.Text, createPhoneNumberTextBox.Text, createEmailTextBox.Text);
        }

        /*************************************************************************************************/
        private void RaiseCreateNewUserEvent(string user, string password, string firstName, string lastName, string phoneNumber, string email)
        {
            if (CreateNewUserEvent != null)
            {
                CreateNewUserEvent(user, password, firstName, lastName, phoneNumber, email);
            }
        }

        /*************************************************************************************************/
        private void CreatePasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            RaisePasswordChangedEvent(createPasswordTextBox.Text);
        }

        /*************************************************************************************************/
        private void RaisePasswordChangedEvent(string password)
        {
            if (PasswordChangedEvent != null)
            {
                PasswordChangedEvent(password);
            }
        }

        /*************************************************************************************************/
        private void RaiseLoginSuccessfulEvent()
        {
            if (LoginSuccessfulEvent != null)
            {
                LoginSuccessfulEvent();
            }
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
            ClearLoginView();
            this.Close();
            DialogResult = DialogResult.Cancel;
        }

        /*************************************************************************************************/
        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.CloseButtonColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void LoginView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        /*************************************************************************************************/
        private void ClearLoginView()
        {
            loginResultLabel.Visible = false;
            loginResultLabel.Text = "";
            loginUsernameTextBox.Text = "";
            loginPasswordTextBox.Text = "";
            createNewUserResultLabel.Visible = false;
            createNewUserResultLabel.Text = "";
            createUsernameTextBox.Text = "";
            createPasswordTextBox.Text = "";
        }

        /*************************************************************************************************/
        private void createPhoneNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            createPhoneNumberTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void createPhoneNumberTextBox_Leave(object sender, EventArgs e)
        {
            if (createPhoneNumberTextBox.Text == "")
            {
                createPhoneNumberTextBox.Text = "xxx-xxx-xxxx";
                createPhoneNumberTextBox.ForeColor = Color.FromArgb(0x6B, 0x6B, 0x6B);
            }
        }

        /*************************************************************************************************/
        private void createEmailTextBox_TextChanged(object sender, EventArgs e)
        {
            createEmailTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void createEmailTextBox_Leave(object sender, EventArgs e)
        {
            if (createEmailTextBox.Text == "")
            {
                createEmailTextBox.Text = "example@provider.com";
                createEmailTextBox.ForeColor = Color.FromArgb(0x6B, 0x6B, 0x6B);
            }
        }

        /*************************************************************************************************/
        private void createPhoneNumberTextBox_Enter(object sender, EventArgs e)
        {
            if (createPhoneNumberTextBox.Text == "xxx-xxx-xxxx")
            {
                createPhoneNumberTextBox.Text = "";
                createPhoneNumberTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            }
        }

        /*************************************************************************************************/
        private void createEmailTextBox_Enter(object sender, EventArgs e)
        {
            if (createEmailTextBox.Text == "example@provider.com")
            {
                createEmailTextBox.Text = "";
                createEmailTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            }
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/


    } // LoginForm CLASS
} // LoginForm NAMESPACE
