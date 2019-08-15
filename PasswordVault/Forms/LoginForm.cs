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
    public partial class LoginForm : Form
    {
        /*=================================================================================================
        CONSTANTS
        *================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        const int DEFAULT_PASSWORD_LENGTH = 15;
        const int MINIMUM_PASSWORD_LENGTH = 8;

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private User _user;
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
        public LoginForm()
        {
            InitializeComponent();

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

            // Configure groupbox
            groupBox1.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            groupBox1.ForeColor = Color.FromArgb(242, 242, 242);

            groupBox2.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            groupBox2.ForeColor = Color.FromArgb(242, 242, 242);

            _user = new User(null, null, null);
            _user.ValidKey = false;
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public User GetUser()
        {
            return _user;
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void LoginButton_Click(object sender, EventArgs e)
        {
            var storage = new CsvDatabaseFactory().Get();

            string user = loginUsernameTextBox.Text;
            string pass = loginPasswordTextBox.Text;

            User userInfo = storage.GetUser(user);

            if (userInfo == null)
            {
                MessageBox.Show("Invalid username.");
                return;
            }

            MasterPassword hash = new MasterPassword();
            bool valid = hash.VerifyPassword(pass, userInfo.Salt, userInfo.Hash);

            if (!valid)
            {
                MessageBox.Show("Password is incorrect!");
                return;
            }

            //_user.UserID = user;
            //_user.Key = pass;
            //_user.ValidKey = true;

            DialogResult = DialogResult.OK;
            this.Close();
        }

        /*************************************************************************************************/
        private void GeneratePasswordButton_Click(object sender, EventArgs e)
        {
            //createPasswordTextBox.Text = EncryptDecrypt.CreateKey(DEFAULT_PASSWORD_LENGTH);
        }

        /*************************************************************************************************/
        private void CreateLoginButton_Click(object sender, EventArgs e)
        {        
            var csv = new CsvDatabaseFactory().Get();
            List<User> users = csv.GetUsers();

            string newUser = createUsernameTextBox.Text;
            string newPass = createPasswordTextBox.Text;

            // Verify username doesnt exist
            bool exists = users.Any(user => user.UserID == newUser);

            if (exists)
            {
                MessageBox.Show("Username already exists.");
                return;
            }

            // Verify password length
            if (newPass.Length < MINIMUM_PASSWORD_LENGTH)
            {
                MessageBox.Show("Password requires at least 8 characters.");
                return;
            }

            // Hash password 
            MasterPassword hash = new MasterPassword();
            CryptData_S data = hash.HashPassword(newPass);
     
            //csv.AddUser(new User(newUser, data.Salt, data.Hash));

            DialogResult = DialogResult.OK;
            this.Close();
        }

        /*************************************************************************************************/
        private void CreatePasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            string complexity = PasswordComplexity.checkEffectiveBitSize(((TextBox)sender).Text.Length, ((TextBox)sender).Text);

            switch(complexity)
            {
                case "WEAK":
                    ((TextBox)sender).ForeColor = Color.Red;
                    break;

                case "MEDIOCRE":
                    ((TextBox)sender).ForeColor = Color.Orange;
                    break;

                case "OK":
                    ((TextBox)sender).ForeColor = Color.YellowGreen;
                    break;

                case "GREAT":
                    ((TextBox)sender).ForeColor = Color.Green;
                    break;

                default:
                    ((TextBox)sender).ForeColor = Color.FromArgb(242, 242, 242); 
                    break;
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

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/


    } // LoginForm CLASS
} // LoginForm NAMESPACE
