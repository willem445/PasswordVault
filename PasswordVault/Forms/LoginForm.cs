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
            var storage = new CSVFactory().Get();

            string user = loginUsernameTextBox.Text;
            string pass = loginPasswordTextBox.Text;

            User userInfo = storage.GetUser(user);

            if (userInfo == null)
            {
                MessageBox.Show("Invalid username.");
                return;
            }

            UserPasswordHash hash = new UserPasswordHash();
            bool valid = hash.VerifyPassword(pass, userInfo.Salt, userInfo.Hash);

            if (!valid)
            {
                MessageBox.Show("Password is incorrect!");
                return;
            }

            _user.UserID = user;
            _user.Key = pass;
            _user.ValidKey = true;

            DialogResult = DialogResult.OK;
            this.Close();
        }

        /*************************************************************************************************/
        private void GeneratePasswordButton_Click(object sender, EventArgs e)
        {
            createPasswordTextBox.Text = EncryptDecrypt.CreateKey(DEFAULT_PASSWORD_LENGTH);
        }

        /*************************************************************************************************/
        private void CreateLoginButton_Click(object sender, EventArgs e)
        {        
            var csv = new CSVFactory().Get();
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
            UserPasswordHash hash = new UserPasswordHash();
            CryptData_S data = hash.HashPassword(newPass);
     
            csv.AddUser(new User(newUser, data.Salt, data.Hash));

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
                    ((TextBox)sender).ForeColor = Color.Yellow;
                    break;

                case "OK":
                    ((TextBox)sender).ForeColor = Color.Green;
                    break;

                case "GREAT":
                    ((TextBox)sender).ForeColor = Color.Green;
                    break;

                default:
                    ((TextBox)sender).ForeColor = Color.Black;
                    break;
            }
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/


    } // LoginForm CLASS
} // LoginForm NAMESPACE
