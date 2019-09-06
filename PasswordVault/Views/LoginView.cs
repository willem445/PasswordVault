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
/* TODO - Add Twilio for 2FA
 * https://www.twilio.com/blog/2016/04/send-an-sms-message-with-c-in-30-seconds.html
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

        /*PRIVATE*****************************************************************************************/
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to

        public event Action<string, string> LoginEvent;
        public event Action<string, string> CreateNewUserEvent;
        public event Action GenerateNewPasswordEvent;
        public event Action<string> PasswordChangedEvent;
        public event Action LoginSuccessfulEvent;

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
            BackColor = Color.FromArgb(35, 35, 35);
            FormBorderStyle = FormBorderStyle.None;

            // Configure buttons
            closeButton.BackColor = Color.FromArgb(63, 63, 63);
            closeButton.ForeColor = Color.FromArgb(242, 242, 242);
            closeButton.Font = new Font("Segoe UI", 12.0f, FontStyle.Regular);

            loginButton.BackColor = Color.FromArgb(63, 63, 63);
            loginButton.ForeColor = Color.FromArgb(242, 242, 242);
            loginButton.FlatStyle = FlatStyle.Flat;

            generatePasswordButton.BackColor = Color.FromArgb(63, 63, 63);
            generatePasswordButton.ForeColor = Color.FromArgb(242, 242, 242);
            generatePasswordButton.FlatStyle = FlatStyle.Flat;

            createLoginButton.BackColor = Color.FromArgb(63, 63, 63);
            createLoginButton.ForeColor = Color.FromArgb(242, 242, 242);
            createLoginButton.FlatStyle = FlatStyle.Flat;

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

            // Configure labels
            label1.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(242, 242, 242);

            label2.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(242, 242, 242);

            label3.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(242, 242, 242);

            label4.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(242, 242, 242);

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

                case LoginResult.UnSuccessful:
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
                    createNewUserResultLabel.Visible = true;
                    createNewUserResultLabel.Text = "Username taken.";
                    break;

                case CreateUserResult.Unsuccessful:
                    createNewUserResultLabel.Visible = true;
                    createNewUserResultLabel.Text = "Unsuccessful.";
                    break;

                case CreateUserResult.PasswordNotValid:
                    createNewUserResultLabel.Visible = true;
                    createNewUserResultLabel.Text = "Password not long enough.";
                    break;

                case CreateUserResult.UsernameNotValid:
                    createNewUserResultLabel.Visible = true;
                    createNewUserResultLabel.Text = "Invalid username.";
                    break;

                case CreateUserResult.Successful:
                    ClearLoginView();
                    DialogResult = DialogResult.OK;
                    this.Close();
                    RaiseLoginSuccessfulEvent();
                    break;

                default:
                    createNewUserResultLabel.Visible = true;
                    createNewUserResultLabel.Text = "Unsuccessful.";
                    break;
            }
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
            RaiseCreateNewUserEvent(createUsernameTextBox.Text, createPasswordTextBox.Text);
        }

        /*************************************************************************************************/
        private void RaiseCreateNewUserEvent(string user, string password)
        {
            if (CreateNewUserEvent != null)
            {
                CreateNewUserEvent(user, password);
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

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/


    } // LoginForm CLASS
} // LoginForm NAMESPACE
