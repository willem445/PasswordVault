using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
 * https://security.stackexchange.com/questions/157422/store-encrypted-user-data-in-database
 * TODO - Create INI file to store database configuration etc.
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault
{
    /*=================================================================================================
    ENUMERATIONS
    *================================================================================================*/
    public enum DgvColumns
    {
        Application,
        Username, 
        Description, 
        Website
    }

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/


    public partial class MainView : Form, IMainView
    {
        /*=================================================================================================
        CONSTANTS
        *================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private const float STANDARD_UI_FONT_SIZE = 9.0f;
        private const float CLOSE_BUTTON_FONT_SIZE = 12.0f;
        private const float TEXTBOX_FONT_SIZE = 8.0f;
        private const float BUTTON_FONT_SIZE = 8.0f;

        private const int INVALID_INDEX = -1;

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
        public event Action<string, PasswordFilterOptions> FilterChangedEvent;
        public event Action RequestPasswordsOnLoginEvent;
        public event Action<string, string, string, string, string> AddPasswordEvent;
        public event Action<int> MovePasswordUpEvent;
        public event Action<int> MovePasswordDownEvent;
        public event Action<string, string, string, string> EditPasswordEvent;
        public event Action<string, string, string, string, string> EditOkayEvent;
        public event Action EditCancelEvent;
        public event Action<string, string, string, string> DeletePasswordEvent;
        public event Action LogoutEvent;

        /*PRIVATE*****************************************************************************************/
        private ILoginView _loginView;

        private ContextMenu _cm;                      // Context menu for right clicking on datagridview row
        private int _rowIndexCopy = 0;                // Index of row being right clicked on
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to

        private bool _loggedIn = false;
        private int _selectedDgvIndex = INVALID_INDEX;
        private bool _editMode = false;

        private BindingList<Password> _dgvPasswordList;


        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        protected override CreateParams CreateParams // This helps to avoid flickering with custom controls
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;      // WS_EX_COMPOSITED
                return handleParam;
            }
        }

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public MainView(ILoginView loginView)
        {
            _loginView = loginView;
            _loginView.LoginSuccessfulEvent += DisplayLoginSuccessful;

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
            addButton.Font = UIFont(BUTTON_FONT_SIZE);
            addButton.FlatAppearance.BorderColor = DarkBackground();
            addButton.FlatAppearance.BorderSize = 1;

            moveUpButton.BackColor = ControlBackground();
            moveUpButton.ForeColor = WhiteText();
            moveUpButton.FlatStyle = FlatStyle.Flat;
            moveUpButton.Font = UIFont(BUTTON_FONT_SIZE);
            moveUpButton.FlatAppearance.BorderColor = DarkBackground();
            moveUpButton.FlatAppearance.BorderSize = 1;

            moveDownButton.BackColor = ControlBackground();
            moveDownButton.ForeColor = WhiteText();
            moveDownButton.FlatStyle = FlatStyle.Flat;
            moveDownButton.Font = UIFont(BUTTON_FONT_SIZE);
            moveDownButton.FlatAppearance.BorderColor = DarkBackground();
            moveDownButton.FlatAppearance.BorderSize = 1;

            deleteButton.BackColor = ControlBackground();
            deleteButton.ForeColor = WhiteText();
            deleteButton.FlatStyle = FlatStyle.Flat;
            deleteButton.Font = UIFont(BUTTON_FONT_SIZE);
            deleteButton.FlatAppearance.BorderColor = DarkBackground();
            deleteButton.FlatAppearance.BorderSize = 1;

            editButton.BackColor = ControlBackground();
            editButton.ForeColor = WhiteText();
            editButton.FlatStyle = FlatStyle.Flat;
            editButton.Font = UIFont(BUTTON_FONT_SIZE);
            editButton.FlatAppearance.BorderColor = DarkBackground();
            editButton.FlatAppearance.BorderSize = 1;
          
            editCancelButton.BackColor = ControlBackground();
            editCancelButton.ForeColor = WhiteText();
            editCancelButton.FlatStyle = FlatStyle.Flat;
            editCancelButton.Font = UIFont(BUTTON_FONT_SIZE);
            editCancelButton.FlatAppearance.BorderColor = DarkBackground();
            editCancelButton.FlatAppearance.BorderSize = 1;
            editCancelButton.Enabled = false;
            editCancelButton.Visible = false;

            closeButton.BackColor = ControlBackground();
            closeButton.ForeColor = WhiteText();
            closeButton.Font = UIFont(BUTTON_FONT_SIZE);

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
            filterComboBox.DataSource = Enum.GetValues(typeof(PasswordFilterOptions));
            filterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            filterComboBox.HighlightColor = ControlHighlight();
            filterComboBox.BorderColor = ControlBackground();

            // Configure status strip
            statusStrip1.BackColor = DarkBackground();
            statusStrip1.ForeColor = WhiteText();
            statusStrip1.Font = UIFont(TEXTBOX_FONT_SIZE);

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

            userStatusLabel.Text = "Not logged in.";

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
        public void DisplayPasswords(BindingList<Password> passwordList)
        {
            _dgvPasswordList = new BindingList<Password>(passwordList);
            PasswordDataGridView.DataSource = _dgvPasswordList;
            PasswordDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            PasswordDataGridView.RowHeadersVisible = false;
        }

        /*************************************************************************************************/
        public void DisplayUserID(string userID)
        {
            userStatusLabel.Text = string.Format("Welcome: {0}", userID);
        }

        /*************************************************************************************************/
        public void DisplayPasswordToEdit(Password password)
        {
            applicationTextBox.Text = password.Application;
            usernameTextBox.Text = password.Username;
            descriptionTextBox.Text = password.Description;
            websiteTextBox.Text = password.Website;
            passphraseTextBox.Text = password.Passphrase;
        }

        /*************************************************************************************************/
        public void DisplayAddEditPasswordResult(AddModifiedPasswordResult result)
        {
            switch(result)
            {
                case AddModifiedPasswordResult.DuplicatePassword:
                    userStatusLabel.Text = "Duplicate password.";
                    break;

                case AddModifiedPasswordResult.Failed:
                    userStatusLabel.Text = "Modify password failed.";
                    break;

                case AddModifiedPasswordResult.Success:
                    applicationTextBox.Text = "";
                    usernameTextBox.Text = "";
                    descriptionTextBox.Text = "";
                    websiteTextBox.Text = "";
                    passphraseTextBox.Text = "";
                    userStatusLabel.Text = "Password modified.";
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayLogOutResult(LogOutResult result)
        {
            switch (result)
            {
                case LogOutResult.Failed:

                    break;

                case LogOutResult.Success:
                    _loggedIn = false;
                    PasswordDataGridView.DataSource = null;
                    PasswordDataGridView.Rows.Clear();
                    userStatusLabel.Text = "";
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
                    filterTextBox.Enabled = false;
                    loginToolStripMenuItem.Text = "Login";
                    userStatusLabel.Text = "Logged off.";
                    break;
            }
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_loggedIn) // login
            {
                _loginView.ShowLoginMenu();
            }
            else // logout
            {
                RaiseLogoutEvent();
            }
        }

        /*************************************************************************************************/
        private void DisplayLoginSuccessful()
        {
            _loggedIn = true;

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
            filterComboBox.Enabled = true;
            filterTextBox.Enabled = true;
            loginToolStripMenuItem.Text = "Logoff";

            RaiseRequestPasswordsOnLoginEvent();
        }

        /*************************************************************************************************/
        private void RaiseLogoutEvent()
        {
            if (LogoutEvent != null)
            {
                LogoutEvent();
            }
        }

        /*************************************************************************************************/
        private void RaiseRequestPasswordsOnLoginEvent()
        {
            if (RequestPasswordsOnLoginEvent != null)
            {
                RequestPasswordsOnLoginEvent();
            }
        }

        /*************************************************************************************************/
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (!_editMode)
            {
                RaiseAddPasswordEvent(applicationTextBox.Text, 
                                      usernameTextBox.Text, 
                                      descriptionTextBox.Text, 
                                      websiteTextBox.Text, 
                                      passphraseTextBox.Text);
            }
            else
            {
                RaiseEditOkayEvent(applicationTextBox.Text,
                                   usernameTextBox.Text,
                                   descriptionTextBox.Text,
                                   websiteTextBox.Text,
                                   passphraseTextBox.Text);
            }        
        }

        /*************************************************************************************************/
        private void RaiseAddPasswordEvent(string application, string username, string description, string website, string passphrase)
        {
            if (AddPasswordEvent != null)
            {
                AddPasswordEvent(application, username, description, website, passphrase);
            }
        }

        /*************************************************************************************************/
        private void EditButton_Click(object sender, EventArgs e)
        {
            editCancelButton.Enabled = true;
            editCancelButton.Visible = true;
            _editMode = true;
            addButton.Text = "Ok";

            RaiseEditPasswordEvent(PasswordDataGridView.SelectedCells[(int)DgvColumns.Application].Value.ToString(),
                                   PasswordDataGridView.SelectedCells[(int)DgvColumns.Username].Value.ToString(),
                                   PasswordDataGridView.SelectedCells[(int)DgvColumns.Description].Value.ToString(),
                                   PasswordDataGridView.SelectedCells[(int)DgvColumns.Website].Value.ToString());
        }

        /*************************************************************************************************/
        private void EditCancelButton_Click(object sender, EventArgs e)
        {
            addButton.Text = "Add";
            _editMode = false;
            editCancelButton.Enabled = false;
            editCancelButton.Visible = false;

            applicationTextBox.Text = "";
            usernameTextBox.Text = "";
            descriptionTextBox.Text = "";
            websiteTextBox.Text = "";
            passphraseTextBox.Text = "";

            RaiseEditCancelEvent();
        }

        /*************************************************************************************************/
        private void RaiseEditPasswordEvent(string application, string username, string description, string website)
        {
            if (EditPasswordEvent != null)
            {
                EditPasswordEvent(application, username, description, website);
            }
        }

        /*************************************************************************************************/
        private void RaiseEditOkayEvent(string application, string username, string description, string website, string passphrase)
        {
            if (EditOkayEvent != null)
            {
                EditOkayEvent(application, username, description, website, passphrase);
            }
        }

        /*************************************************************************************************/
        private void RaiseEditCancelEvent()
        {
            if (EditCancelEvent != null)
            {
                EditCancelEvent();
            }
        }

        /*************************************************************************************************/
        private void MoveUpButton_Click(object sender, EventArgs e)
        {

        }

        /*************************************************************************************************/
        private void RaiseMovePasswordUpEvent(int index)
        {
            if (MovePasswordUpEvent != null)
            {
                MovePasswordUpEvent(index);
            }
        }

        /*************************************************************************************************/
        private void MoveDownButton_Click(object sender, EventArgs e)
        {

        }

        /*************************************************************************************************/
        private void RaiseMovePasswordDownEvent(int index)
        {
            if (MovePasswordDownEvent != null)
            {
                MovePasswordDownEvent(index);
            }
        }

        /*************************************************************************************************/
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            RaiseDeletePasswordEvent(PasswordDataGridView.SelectedCells[(int)DgvColumns.Application].Value.ToString(),
                                     PasswordDataGridView.SelectedCells[(int)DgvColumns.Username].Value.ToString(),
                                     PasswordDataGridView.SelectedCells[(int)DgvColumns.Description].Value.ToString(),
                                     PasswordDataGridView.SelectedCells[(int)DgvColumns.Website].Value.ToString());
        }

        /*************************************************************************************************/
        private void RaiseDeletePasswordEvent(string application, string username, string description, string website)
        {
            if (DeletePasswordEvent != null)
            {
                DeletePasswordEvent(application, username, description, website);
            }
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
        private void filterChanged(object sender, EventArgs e)
        {
            PasswordFilterOptions filterOption = (PasswordFilterOptions)filterComboBox.SelectedValue;
            RaiseNewFilterEvent(filterTextBox.Text, filterOption); 
        }

        /*************************************************************************************************/
        private void RaiseNewFilterEvent(string filterText, PasswordFilterOptions passwordFilterOption)
        {
            if (FilterChangedEvent != null)
            {
                FilterChangedEvent(filterText, passwordFilterOption);
            }
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
        private Color ControlHighlight()
        {
            return Color.FromArgb(0x80, 0x80, 0x80);
        }

        /*************************************************************************************************/
        private Font UIFont(float fontSize)
        {
            return new Font("Segoe UI", fontSize, FontStyle.Bold);
        }

        /*************************************************************************************************/
        private void PasswordDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            _selectedDgvIndex = PasswordDataGridView.CurrentCell.RowIndex;
        }

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

    } // MainForm CLASS
} // MainForm NAMESPACE
