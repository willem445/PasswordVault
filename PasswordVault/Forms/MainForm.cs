using System;
using System.Drawing;
using System.Windows.Forms;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* TODO - Add new form for adding password with validation, password strength etc
 * TODO - Filter for passwords
 * TODO - Edit passwords
 * TODO - Sort _password binding list by application name
 * TODO - Add category option to password object
 * TODO - Generate very strong random key from passphase to encrypt data with (Makes it easier to change passwords) (https://security.stackexchange.com/questions/30193/encrypting-user-data-using-password-and-forgot-my-password)
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
        private const float STANDARD_UI_FONT_SIZE = 9.0f;
        private const float CLOSE_BUTTON_FONT_SIZE = 12.0f;
        private const float TEXTBOX_FONT_SIZE = 7.0f;

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private ContextMenu _cm;                      // Context menu for right clicking on datagridview row
        private int _rowIndexCopy = 0;                // Index of row being right clicked on
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
        public MainForm()
        {
            InitializeComponent();

            #region UI
            // Configure form UI
            BackColor = DarkBackground();
            FormBorderStyle = FormBorderStyle.None;

            // Configure labels
            label1.Font = UIFont(STANDARD_UI_FONT_SIZE);
            label1.ForeColor = WhiteText();

            label2.Font = UIFont(STANDARD_UI_FONT_SIZE);
            label2.ForeColor = WhiteText();

            label3.Font = UIFont(STANDARD_UI_FONT_SIZE);
            label3.ForeColor = WhiteText();

            label4.Font = UIFont(STANDARD_UI_FONT_SIZE);
            label4.ForeColor = WhiteText();

            label5.Font = UIFont(STANDARD_UI_FONT_SIZE);
            label5.ForeColor = WhiteText();

            filterLabel.Font = UIFont(STANDARD_UI_FONT_SIZE);
            filterLabel.ForeColor = WhiteText();

            // Configure menu stip
            menuStrip.BackColor = DarkBackground();
            menuStrip.ForeColor = WhiteText();
            menuStrip.Font = UIFont(STANDARD_UI_FONT_SIZE);

            // Configure buttons
            addButton.BackColor = ControlBackground();
            addButton.ForeColor = WhiteText();
            addButton.FlatStyle = FlatStyle.Flat;

            moveUpButton.BackColor = ControlBackground();
            moveUpButton.ForeColor = WhiteText();
            moveUpButton.FlatStyle = FlatStyle.Flat;

            moveDownButton.BackColor = ControlBackground();
            moveDownButton.ForeColor = WhiteText();
            moveDownButton.FlatStyle = FlatStyle.Flat;

            deleteButton.BackColor = ControlBackground();
            deleteButton.ForeColor = WhiteText();
            deleteButton.FlatStyle = FlatStyle.Flat;

            editButton.BackColor = ControlBackground();
            editButton.ForeColor = WhiteText();
            editButton.FlatStyle = FlatStyle.Flat;

            closeButton.BackColor = ControlBackground();
            closeButton.ForeColor = WhiteText();
            closeButton.Font = UIFont(CLOSE_BUTTON_FONT_SIZE);

            // Configure textbox
            applicationTextBox.BackColor = ControlBackground();
            applicationTextBox.ForeColor = WhiteText();
            applicationTextBox.BorderStyle = BorderStyle.FixedSingle;
            applicationTextBox.Font = UIFont(TEXTBOX_FONT_SIZE);

            usernameTextBox.BackColor = ControlBackground();
            usernameTextBox.ForeColor = WhiteText();
            usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            usernameTextBox.Font = UIFont(TEXTBOX_FONT_SIZE);

            descriptionTextBox.BackColor = ControlBackground();
            descriptionTextBox.ForeColor = WhiteText();
            descriptionTextBox.BorderStyle = BorderStyle.FixedSingle;
            descriptionTextBox.Font = UIFont(TEXTBOX_FONT_SIZE);

            passphraseTextBox.BackColor = ControlBackground();
            passphraseTextBox.ForeColor = WhiteText();
            passphraseTextBox.BorderStyle = BorderStyle.FixedSingle;
            passphraseTextBox.Font = UIFont(TEXTBOX_FONT_SIZE);

            websiteTextBox.BackColor = ControlBackground();
            websiteTextBox.ForeColor = WhiteText();
            websiteTextBox.BorderStyle = BorderStyle.FixedSingle;
            websiteTextBox.Font = UIFont(TEXTBOX_FONT_SIZE);

            filterTextBox.BackColor = ControlBackground();
            filterTextBox.ForeColor = WhiteText();
            filterTextBox.BorderStyle = BorderStyle.FixedSingle;
            filterTextBox.Font = UIFont(TEXTBOX_FONT_SIZE);

            // Configure combo box
            filterComboBox.BackColor = ControlBackground();
            filterComboBox.ForeColor = WhiteText();
            filterComboBox.Font = UIFont(TEXTBOX_FONT_SIZE);
            filterComboBox.Items.Add("Name");
            filterComboBox.Items.Add("Description");

            // Configure status strip
            statusStrip1.BackColor = DarkBackground();
            statusStrip1.ForeColor = WhiteText();

            // Confgiure data grid view
            PasswordDataGridView.BorderStyle = BorderStyle.None;
            PasswordDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            PasswordDataGridView.MultiSelect = false;
            PasswordDataGridView.ReadOnly = true;
            PasswordDataGridView.BackgroundColor = DarkBackground();
            PasswordDataGridView.RowsDefaultCellStyle.Font = UIFont(STANDARD_UI_FONT_SIZE);
            PasswordDataGridView.RowsDefaultCellStyle.ForeColor = WhiteText();
            PasswordDataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(65, 65, 65);
            PasswordDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(85, 85, 85);
            PasswordDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
            PasswordDataGridView.AllowUserToOrderColumns = true;
            PasswordDataGridView.DefaultCellStyle.SelectionBackColor = Color.DarkGray;
            PasswordDataGridView.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            PasswordDataGridView.EnableHeadersVisualStyles = false;
            PasswordDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            PasswordDataGridView.ColumnHeadersDefaultCellStyle.Font = UIFont(STANDARD_UI_FONT_SIZE);
            PasswordDataGridView.ColumnHeadersDefaultCellStyle.BackColor = DarkBackground();
            PasswordDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = WhiteText();
            PasswordDataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = DarkBackground();
            PasswordDataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = WhiteText();
            #endregion

            // Set storage to use CSV data

            //_user = new User();
            //userStatusLabel.Text = "";

            //_passwordList = new BindableList<Password>();
            //_passwordList.CreateBinding(PasswordDataGridView);

            _cm = new ContextMenu();
            _cm.MenuItems.Add("Copy Username", new EventHandler(CopyUser_Click));
            _cm.MenuItems.Add("Copy Password", new EventHandler(CopyPass_Click));
            _cm.MenuItems.Add("Visit Website", new EventHandler(Website_Click));
            _cm.MenuItems.Add("View Password", new EventHandler(ShowPassword_Click));
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
            //if (!_user.ValidKey)
            //{
            //    LoginForm loginForm = new LoginForm();
            //    if (loginForm.ShowDialog() == DialogResult.OK)
            //    {
            //        _user = loginForm.GetUser();
            //        userStatusLabel.Text = string.Format("Welcome: {0}", _user.UserID);
            //        applicationTextBox.Enabled = true;
            //        descriptionTextBox.Enabled = true;
            //        websiteTextBox.Enabled = true;
            //        passphraseTextBox.Enabled = true;
            //        usernameTextBox.Enabled = true;
            //        addButton.Enabled = true;
            //        moveUpButton.Enabled = true;
            //        moveDownButton.Enabled = true;
            //        deleteButton.Enabled = true;
            //        editButton.Enabled = true;
            //        filterComboBox.Enabled = true;
            //        filterTextBox.Enabled = true;

            //        if (_storage.GetType() == typeof(CSV))
            //        {
            //            ((CSV)_storage).SetCSVFileName(_user.UserID);
            //        }

            //        _passwordList.Clear();
            //        _passwordList.Clear();
            //        _passwordList.Clear();
            //        _passwordList.Clear();
            //        foreach (var item in _storage.GetPasswords())
            //        {
            //            Password password = new Password();
            //            password.Application = EncryptDecrypt.Decrypt(item.Application, _user.Key);
            //            password.Username = EncryptDecrypt.Decrypt(item.Username, _user.Key);
            //            password.Description = EncryptDecrypt.Decrypt(item.Description, _user.Key);
            //            password.Website = EncryptDecrypt.Decrypt(item.Website, _user.Key);
            //            password.Passphrase = item.Passphrase; // Leave pass phase encrypted until it is needed
            //            _passwordList.Add(password);
            //        }

            //        loginToolStripMenuItem.Text = "Logoff";
            //    }
            //}
            //else
            //{
            //    _user.ValidKey = false;
            //    _user.UserID = null;
            //    _user.Salt = null;
            //    _user.Hash = null;
            //    _user.Key = null;

            //    userStatusLabel.Text = "";
            //    applicationTextBox.Enabled = false;
            //    descriptionTextBox.Enabled = false;
            //    websiteTextBox.Enabled = false;
            //    passphraseTextBox.Enabled = false;
            //    usernameTextBox.Enabled = false;
            //    addButton.Enabled = false;
            //    moveUpButton.Enabled = false;
            //    moveDownButton.Enabled = false;
            //    deleteButton.Enabled = false;
            //    editButton.Enabled = false;
            //    filterComboBox.Enabled = false;
            //    filterTextBox.Enabled = false;

            //    if (_storage.GetType() == typeof(CSV))
            //    {
            //        ((CSV)_storage).SetUserTableName("");
            //    }

            //    _passwordList.Clear();

            //    loginToolStripMenuItem.Text = "Login";
            //}
        }

        /*************************************************************************************************/
        private void AddButton_Click(object sender, EventArgs e)
        {
            //Password newPassword = new Password();
            //Password newPassword2 = new Password(); // Make a new password object so we dont modify _passwordList object when adding to storage list

            //newPassword.Application = applicationTextBox.Text;
            //newPassword.Description = descriptionTextBox.Text;
            //newPassword.Website =     websiteTextBox.Text    ;
            //newPassword.Username =    usernameTextBox.Text   ;
            //newPassword.Passphrase =  EncryptDecrypt.Encrypt(passphraseTextBox.Text, _user.Key);
            //_passwordList.Add(newPassword);

            //newPassword2.Application = EncryptDecrypt.Encrypt(applicationTextBox.Text, _user.Key);
            //newPassword2.Description = EncryptDecrypt.Encrypt(descriptionTextBox.Text, _user.Key);
            //newPassword2.Website =     EncryptDecrypt.Encrypt(websiteTextBox.Text,     _user.Key);
            //newPassword2.Username =    EncryptDecrypt.Encrypt(usernameTextBox.Text,    _user.Key);
            //newPassword2.Passphrase =  EncryptDecrypt.Encrypt(passphraseTextBox.Text,  _user.Key);
            //_storage.AddPassword(newPassword2);

            //applicationTextBox.Text = "";
            //descriptionTextBox.Text = "";
            //websiteTextBox.Text     = "";
            //usernameTextBox.Text    = "";
            //passphraseTextBox.Text  = "";
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
            //if (_passwordList.Count != 0)
            //{
            //    Clipboard.SetText(EncryptDecrypt.Decrypt(_passwordList.GetList()[_rowIndexCopy].Passphrase, _user.Key));
            //}          
        }

        /*************************************************************************************************/
        private void CopyUser_Click(object sender, EventArgs e)
        {
            //if (_passwordList.Count != 0)
            //{
            //    Clipboard.SetText(_passwordList.GetList()[_rowIndexCopy].Username);
            //}
        }

        /*************************************************************************************************/
        private void Website_Click(object sender, EventArgs e)
        {
            //if (_passwordList.Count != 0)
            //{
            //    string website = _passwordList.GetList()[_rowIndexCopy].Website;

            //    if (UriUtilities.IsValidUri(website))
            //    {
            //        UriUtilities.OpenUri(website);
            //    }
            //}
        }

        /*************************************************************************************************/
        private void ShowPassword_Click(object sender, EventArgs e)
        {
            //if (_passwordList.Count != 0)
            //{
            //    MessageBox.Show(EncryptDecrypt.Decrypt(_passwordList.GetList()[_rowIndexCopy].Passphrase, _user.Key));
            //}
        }

        /*************************************************************************************************/
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

        /*************************************************************************************************/
        private void CloseButton_MouseClick(object sender, MouseEventArgs e)
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
        private Color WhiteText()
        {
            return Color.FromArgb(242, 242, 242);
        }

        /*************************************************************************************************/
        private Color DarkBackground()
        {
            return Color.FromArgb(35, 35, 35);
        }

        /*************************************************************************************************/
        private Color ControlBackground()
        {
            return Color.FromArgb(63, 63, 63);
        }

        /*************************************************************************************************/
        private Font UIFont(float fontSize)
        {
            return new Font("Segoe UI", fontSize, FontStyle.Bold);
        }

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

    } // MainForm CLASS
} // MainForm NAMESPACE
