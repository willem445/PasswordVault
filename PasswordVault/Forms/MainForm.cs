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
    /*=================================================================================================
    ENUMERATIONS
    *================================================================================================*/

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/


    public partial class MainForm : Form
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
        private User _user;
        private BindableList<Password> _passwordList;
        private ContextMenu _cm;
        private int _rowIndexCopy = 0;
        IStorage storage;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public MainForm()
        {
            InitializeComponent();

            storage = new CSVFactory().Get();

            _user = new User();
            welcomeLabel.Text = "";

            _passwordList = new BindableList<Password>();
            _passwordList.CreateBinding(PasswordDataGridView);

            _cm = new ContextMenu();
            _cm.MenuItems.Add("Copy Username", new EventHandler(CopyUser_Click));
            _cm.MenuItems.Add("Copy Password", new EventHandler(CopyPass_Click));
            _cm.MenuItems.Add("Visit Website", new EventHandler(Website_Click));
            
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_user.ValidKey)
            {
                LoginForm loginForm = new LoginForm();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    _user = loginForm.GetUser();
                    welcomeLabel.Text = string.Format("Welcome: {0}", _user.UserID);
                    applicationTextBox.Enabled = true;
                    descriptionTextBox.Enabled = true;
                    websiteTextBox.Enabled = true;
                    passphraseTextBox.Enabled = true;
                    usernameTextBox.Enabled = true;
                    addButton.Enabled = true;
                    moveUpButton.Enabled = true;
                    moveDownButton.Enabled = true;
                    deleteButton.Enabled = true;
                    editButton.Enabled = true;

                    if (storage.GetType() == typeof(CSV))
                    {
                        ((CSV)storage).SetPasswordFileName(_user.UserID);
                    }

                    _passwordList.Clear();
                    foreach (var item in storage.GetPasswords())
                    {
                        Password password = new Password();
                        password.Application = EncryptDecrypt.Decrypt(item.Application, _user.Key);
                        password.Username = EncryptDecrypt.Decrypt(item.Username, _user.Key);
                        password.Description = EncryptDecrypt.Decrypt(item.Description, _user.Key);
                        password.Website = EncryptDecrypt.Decrypt(item.Website, _user.Key);
                        password.Passphrase = item.Passphrase;
                        _passwordList.Add(password);
                    }

                    loginToolStripMenuItem.Text = "Logoff";
                }
            }
            else
            {
                _user.ValidKey = false;
                _user.UserID = null;
                _user.Salt = null;
                _user.Hash = null;
                _user.Key = null;

                welcomeLabel.Text = "";
                applicationTextBox.Enabled = false;
                descriptionTextBox.Enabled = false;
                websiteTextBox.Enabled = false;
                passphraseTextBox.Enabled = false;
                usernameTextBox.Enabled = false;
                addButton.Enabled = false;
                moveUpButton.Enabled = false;
                moveDownButton.Enabled = false;
                deleteButton.Enabled = false;
                editButton.Enabled = false;

                if (storage.GetType() == typeof(CSV))
                {
                    ((CSV)storage).SetPasswordFileName("");
                }

                _passwordList.Clear();

                loginToolStripMenuItem.Text = "Login";
            }

        }

        /*************************************************************************************************/
        private void AddButton_Click(object sender, EventArgs e)
        {
            Password newPassword = new Password();
            Password newPassword2 = new Password(); // Make a new password object so we dont modify _passwordList object when adding to storage list

            newPassword.Application = applicationTextBox.Text;
            newPassword.Description = descriptionTextBox.Text;
            newPassword.Website =     websiteTextBox.Text    ;
            newPassword.Username =    usernameTextBox.Text   ;
            newPassword.Passphrase =  EncryptDecrypt.Encrypt(passphraseTextBox.Text, _user.Key);
            _passwordList.Add(newPassword);

            newPassword2.Application = EncryptDecrypt.Encrypt(applicationTextBox.Text, _user.Key);
            newPassword2.Description = EncryptDecrypt.Encrypt(descriptionTextBox.Text, _user.Key);
            newPassword2.Website =     EncryptDecrypt.Encrypt(websiteTextBox.Text,     _user.Key);
            newPassword2.Username =    EncryptDecrypt.Encrypt(usernameTextBox.Text,    _user.Key);
            newPassword2.Passphrase =  EncryptDecrypt.Encrypt(passphraseTextBox.Text,  _user.Key);
            storage.AddPassword(newPassword2);

            applicationTextBox.Text = "";
            descriptionTextBox.Text = "";
            websiteTextBox.Text     = "";
            usernameTextBox.Text    = "";
            passphraseTextBox.Text  = "";
        }

        /*************************************************************************************************/
        private void MoveUpButton_Click(object sender, EventArgs e)
        {

        }

        /*************************************************************************************************/
        private void MoveDownButton_Click(object sender, EventArgs e)
        {

        }

        /*************************************************************************************************/
        private void EditButton_Click(object sender, EventArgs e)
        {

        }

        /*************************************************************************************************/
        private void CopyPass_Click(object sender, EventArgs e)
        {
            if (_passwordList.Count != 0)
            {
                Clipboard.SetText(EncryptDecrypt.Decrypt(_passwordList.GetList()[_rowIndexCopy].Passphrase, _user.Key));
            }          
        }

        /*************************************************************************************************/
        private void CopyUser_Click(object sender, EventArgs e)
        {
            if (_passwordList.Count != 0)
            {
                Clipboard.SetText(_passwordList.GetList()[_rowIndexCopy].Username);
            }
        }

        /*************************************************************************************************/
        private void Website_Click(object sender, EventArgs e)
        {
            if (_passwordList.Count != 0)
            {
                string website = _passwordList.GetList()[_rowIndexCopy].Website;

                if (UriUtilities.IsValidUri(website))
                {
                    UriUtilities.OpenUri(website);
                }
            }
        }

        private void PasswordDataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            // Load context menu on right mouse click
            DataGridView.HitTestInfo hitTestInfo;
            if (e.Button == MouseButtons.Right)
            {
                hitTestInfo = PasswordDataGridView.HitTest(e.X, e.Y);
                // If column is first column
                if (hitTestInfo.Type == DataGridViewHitTestType.Cell)
                {
                    _cm.Show(PasswordDataGridView, new Point(e.X, e.Y));
                    _rowIndexCopy = hitTestInfo.RowIndex;
                }
                    
            }
        }

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

    } // MainForm CLASS
} // MainForm NAMESPACE
