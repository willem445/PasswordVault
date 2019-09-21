using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* TODO - 8 - Add new form for adding password with validation, password strength etc
 * TODO - 1 - Generate very strong random key from passphase to encrypt data with (Makes it easier to change passwords) (https://security.stackexchange.com/questions/30193/encrypting-user-data-using-password-and-forgot-my-password)
 * https://security.stackexchange.com/questions/157422/store-encrypted-user-data-in-database
 * TODO - 9 - Create INI file to store database configuration etc.
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

    public enum ErrorLevel
    {
        Error,
        Warning,
        Ok,
        Neutral,
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
        private const int EMPTY_DGV = 1;

        private const int COLOR_ERROR_RED = 0xFF0000;
        private const int COLOR_WARNING_YELLOW = 0xFFD633;
        private const int COLOR_OK_GREEN = 0x00CC44;
        private const int COLOR_FONT_DEFAULT_WHITE = 0xF2F2F2;

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
        public event Action<string, string, string, string> CopyUserNameEvent;
        public event Action<string, string, string, string> CopyPasswordEvent;
        public event Action<string, string, string, string> ShowPasswordEvent;
        public event Action<string, string, string, string> NavigateToWebsiteEvent;

        /*PRIVATE*****************************************************************************************/
        private ILoginView _loginView;

        private AdvancedContextMenuStrip passwordContextMenuStrip;                      // Context menu for right clicking on datagridview row
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
            _dgvPasswordList = new BindingList<Password>();
            InitializeComponent();

            #region UI
            // TODO - 9 - Create custom controls with default custom properties

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

            // Configure menu strip
            menuStrip.BackColor = DarkBackground();
            menuStrip.ForeColor = WhiteText();
            menuStrip.Font = UIFont(STANDARD_UI_FONT_SIZE);
            menuStrip.MenuItemSelectedColor = ControlHighlight();
            menuStrip.MenuItemBackgroundColor = ControlBackground();
            loginToolStripMenuItem.BackColor = ControlBackground();
            loginToolStripMenuItem.ForeColor = WhiteText();
            loginToolStripMenuItem.Font = UIFont(STANDARD_UI_FONT_SIZE);

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
            moveUpButton.Enabled = false;
            moveUpButton.Visible = false;

            moveDownButton.BackColor = ControlBackground();
            moveDownButton.ForeColor = WhiteText();
            moveDownButton.FlatStyle = FlatStyle.Flat;
            moveDownButton.Font = UIFont(BUTTON_FONT_SIZE);
            moveDownButton.FlatAppearance.BorderColor = DarkBackground();
            moveDownButton.FlatAppearance.BorderSize = 1;
            moveDownButton.Enabled = false;
            moveDownButton.Visible = false;

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
            passwordDataGridView.BorderStyle = BorderStyle.None;
            passwordDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            passwordDataGridView.MultiSelect = false;
            passwordDataGridView.ReadOnly = true;
            passwordDataGridView.BackgroundColor = DarkBackground();
            passwordDataGridView.RowsDefaultCellStyle.Font = UIFont(STANDARD_UI_FONT_SIZE);
            passwordDataGridView.RowsDefaultCellStyle.ForeColor = WhiteText();
            passwordDataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(65, 65, 65);
            passwordDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(85, 85, 85);
            passwordDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
            passwordDataGridView.AllowUserToOrderColumns = true;
            passwordDataGridView.DefaultCellStyle.SelectionBackColor = Color.DarkGray;
            passwordDataGridView.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            passwordDataGridView.EnableHeadersVisualStyles = false;
            passwordDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            passwordDataGridView.ColumnHeadersDefaultCellStyle.Font = UIFont(STANDARD_UI_FONT_SIZE);
            passwordDataGridView.ColumnHeadersDefaultCellStyle.BackColor = DarkBackground();
            passwordDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = WhiteText();
            passwordDataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = DarkBackground();
            passwordDataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = WhiteText();
            passwordDataGridView.ScrollBars = ScrollBars.None;
            passwordDataGridView.MouseWheel += PasswordDataGridView_MouseWheel;

            // Configure context menu
            passwordContextMenuStrip = new AdvancedContextMenuStrip();
            var copyUsernameToolStripItem = new ToolStripMenuItem("Copy Username");
            copyUsernameToolStripItem.Font = UIFont(8.0f);
            copyUsernameToolStripItem.BackColor = ControlBackground();
            copyUsernameToolStripItem.ForeColor = WhiteText();
            copyUsernameToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-copy-48.png"));
            passwordContextMenuStrip.Items.Add(copyUsernameToolStripItem);

            var copyPasswordToolStripItem = new ToolStripMenuItem("Copy Password");
            copyPasswordToolStripItem.Font = UIFont(8.0f);
            copyPasswordToolStripItem.BackColor = ControlBackground();
            copyPasswordToolStripItem.ForeColor = WhiteText();
            copyPasswordToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-copy-48.png"));
            passwordContextMenuStrip.Items.Add(copyPasswordToolStripItem);

            var websiteToolStripItem = new ToolStripMenuItem("Visit Website");
            websiteToolStripItem.Font = UIFont(8.0f);
            websiteToolStripItem.BackColor = ControlBackground();
            websiteToolStripItem.ForeColor = WhiteText();
            websiteToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-link-100.png"));
            passwordContextMenuStrip.Items.Add(websiteToolStripItem);

            var showPasswordToolStripItem = new ToolStripMenuItem("Show Password");
            showPasswordToolStripItem.Font = UIFont(8.0f);
            showPasswordToolStripItem.BackColor = ControlBackground();
            showPasswordToolStripItem.ForeColor = WhiteText();
            showPasswordToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-show-property-60.png"));
            passwordContextMenuStrip.Items.Add(showPasswordToolStripItem);

            var editToolStripItem = new ToolStripMenuItem("Edit");
            editToolStripItem.Font = UIFont(8.0f);
            editToolStripItem.BackColor = ControlBackground();
            editToolStripItem.ForeColor = WhiteText();
            editToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-edit-48.png"));
            passwordContextMenuStrip.Items.Add(editToolStripItem);

            var deleteToolStripItem = new ToolStripMenuItem("Delete");
            deleteToolStripItem.Font = UIFont(8.0f);
            deleteToolStripItem.BackColor = ControlBackground();
            deleteToolStripItem.ForeColor = WhiteText();
            deleteToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-delete-60.png"));
            passwordContextMenuStrip.Items.Add(deleteToolStripItem);

            passwordContextMenuStrip.Items[0].Click += CopyUser_Click;
            passwordContextMenuStrip.Items[1].Click += CopyPass_Click;
            passwordContextMenuStrip.Items[2].Click += Website_Click;
            passwordContextMenuStrip.Items[3].Click += ShowPassword_Click;
            passwordContextMenuStrip.Items[4].Click += EditButton_Click;
            passwordContextMenuStrip.Items[5].Click += DeleteButton_Click;
            #endregion

            userStatusLabel.Text = "Not logged in.";       
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void DisplayPasswords(BindingList<Password> passwordList)
        {
            _dgvPasswordList = new BindingList<Password>(passwordList);
            passwordDataGridView.DataSource = _dgvPasswordList;
            passwordDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            passwordDataGridView.RowHeadersVisible = false;
        }

        /*************************************************************************************************/
        public void DisplayUserID(string userID)
        {
            UpdateStatus(string.Format("Welcome: {0}", userID), ErrorLevel.Neutral);
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
        public void DisplayAddEditPasswordResult(AddPasswordResult result)
        {
            switch(result)
            {
                case AddPasswordResult.DuplicatePassword:
                    UpdateStatus("Duplicate password.", ErrorLevel.Error);
                    break;

                case AddPasswordResult.Failed:
                    UpdateStatus("Modify password failed.", ErrorLevel.Error);
                    break;

                case AddPasswordResult.UsernameError:
                    UpdateStatus("Issue with username field!", ErrorLevel.Error);
                    break;

                case AddPasswordResult.ApplicationError:
                    UpdateStatus("Issue with application field!", ErrorLevel.Error);
                    break;

                case AddPasswordResult.PassphraseError:
                    UpdateStatus("Issue with passphrase field!", ErrorLevel.Error);
                    break;

                case AddPasswordResult.Success:
                    addButton.Text = "Add";
                    _editMode = false;
                    editCancelButton.Enabled = false;
                    editCancelButton.Visible = false;
                    applicationTextBox.Text = "";
                    usernameTextBox.Text = "";
                    descriptionTextBox.Text = "";
                    websiteTextBox.Text = "";
                    passphraseTextBox.Text = "";
                    UpdateStatus("Password modified.", ErrorLevel.Ok);
                    this.Refresh();
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayLogOutResult(LogOutResult result)
        {
            switch (result)
            {
                case LogOutResult.Failed:
                    UpdateStatus("Log out failed!", ErrorLevel.Error);
                    break;

                case LogOutResult.Success:
                    _loggedIn = false;
                    passwordDataGridView.DataSource = null;
                    passwordDataGridView.Rows.Clear();
                    userStatusLabel.Text = "";
                    applicationTextBox.Enabled = false;
                    descriptionTextBox.Enabled = false;
                    websiteTextBox.Enabled = false;
                    passphraseTextBox.Enabled = false;
                    usernameTextBox.Enabled = false;
                    addButton.Enabled = false;
                    //moveUpButton.Enabled = false;
                    //moveDownButton.Enabled = false;
                    deleteButton.Enabled = false;
                    editButton.Enabled = false;
                    filterTextBox.Enabled = false;
                    loginToolStripMenuItem.Text = "Login";
                    UpdateStatus("Logged off.", ErrorLevel.Neutral);
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayPassword(string password)
        {
            MessageBox.Show(password);
        }

        /*************************************************************************************************/
        public void DisplayAddPasswordResult(AddPasswordResult result)
        {
            switch(result)
            {
                case AddPasswordResult.Failed:
                    UpdateStatus("Add password failed.", ErrorLevel.Error);
                    break;

                case AddPasswordResult.DuplicatePassword:
                    UpdateStatus("Duplicate password.", ErrorLevel.Error);
                    break;

                case AddPasswordResult.UsernameError:
                    UpdateStatus("Issue with username field!", ErrorLevel.Error);
                    break;

                case AddPasswordResult.ApplicationError:
                    UpdateStatus("Issue with application field!", ErrorLevel.Error);
                    break;

                case AddPasswordResult.PassphraseError:
                    UpdateStatus("Issue with passphrase field!", ErrorLevel.Error);
                    break;

                case AddPasswordResult.Success:
                    UpdateStatus("Success.", ErrorLevel.Ok);
                    applicationTextBox.Text = "";
                    descriptionTextBox.Text = "";
                    websiteTextBox.Text = "";
                    passphraseTextBox.Text = "";
                    usernameTextBox.Text = "";
                    break;
            }
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void UpdateStatus(string message, ErrorLevel errorLevel)
        {
            userStatusLabel.Text = message;

            switch (errorLevel)
            {
                case ErrorLevel.Error:
                    userStatusLabel.ForeColor = Color.FromArgb(COLOR_ERROR_RED);
                    break;

                case ErrorLevel.Warning:
                    userStatusLabel.ForeColor = Color.FromArgb(COLOR_WARNING_YELLOW);
                    break;

                case ErrorLevel.Ok:
                    userStatusLabel.ForeColor = Color.FromArgb(COLOR_OK_GREEN);
                    break;

                case ErrorLevel.Neutral:
                    userStatusLabel.ForeColor = Color.FromArgb(COLOR_FONT_DEFAULT_WHITE);
                    break;
            }
        }

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
            //moveUpButton.Enabled = true;
            //moveDownButton.Enabled = true;
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
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                editCancelButton.Enabled = true;
                editCancelButton.Visible = true;
                _editMode = true;
                addButton.Text = "Ok";

                RaiseEditPasswordEvent(passwordDataGridView.SelectedCells[(int)DgvColumns.Application].Value.ToString(),
                                       passwordDataGridView.SelectedCells[(int)DgvColumns.Username].Value.ToString(),
                                       passwordDataGridView.SelectedCells[(int)DgvColumns.Description].Value.ToString(),
                                       passwordDataGridView.SelectedCells[(int)DgvColumns.Website].Value.ToString());
            }             
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                RaiseDeletePasswordEvent(passwordDataGridView.SelectedCells[(int)DgvColumns.Application].Value.ToString(),
                                         passwordDataGridView.SelectedCells[(int)DgvColumns.Username].Value.ToString(),
                                         passwordDataGridView.SelectedCells[(int)DgvColumns.Description].Value.ToString(),
                                         passwordDataGridView.SelectedCells[(int)DgvColumns.Website].Value.ToString());
            }
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
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                RaiseCopyPasswordEvent(passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Application].Value.ToString(),
                                       passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Username].Value.ToString(),
                                       passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Description].Value.ToString(),
                                       passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Website].Value.ToString());
            }             
        }

        /*************************************************************************************************/
        private void RaiseCopyPasswordEvent(string application, string username, string description, string website)
        {
            if (CopyPasswordEvent != null)
            {
                CopyPasswordEvent(application, username, description, website);
            }
        }

        /*************************************************************************************************/
        private void CopyUser_Click(object sender, EventArgs e)
        {
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                RaiseCopyUserEvent(passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Application].Value.ToString(),
                                   passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Username].Value.ToString(),
                                   passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Description].Value.ToString(),
                                   passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Website].Value.ToString());
            }             
        }

        /*************************************************************************************************/
        private void RaiseCopyUserEvent(string application, string username, string description, string website)
        {
            if (CopyUserNameEvent != null)
            {
                CopyUserNameEvent(application, username, description, website);
            }
        }

        /*************************************************************************************************/
        private void Website_Click(object sender, EventArgs e)
        {
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                RaiseNavigateToWebsiteEvent(passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Application].Value.ToString(),
                                            passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Username].Value.ToString(),
                                            passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Description].Value.ToString(),
                                            passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Website].Value.ToString());
            }           
        }

        /*************************************************************************************************/
        private void RaiseNavigateToWebsiteEvent(string application, string username, string description, string website)
        {
            if (NavigateToWebsiteEvent != null)
            {
                NavigateToWebsiteEvent(application, username, description, website);
            }
        }

        /*************************************************************************************************/
        private void ShowPassword_Click(object sender, EventArgs e)
        {
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                RaiseShowPasswordEvent(passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Application].Value.ToString(),
                                       passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Username].Value.ToString(),
                                       passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Description].Value.ToString(),
                                       passwordDataGridView.Rows[_rowIndexCopy].Cells[(int)DgvColumns.Website].Value.ToString());
            }            
        }

        /*************************************************************************************************/
        private void RaiseShowPasswordEvent(string application, string username, string description, string website)
        {
            if (ShowPasswordEvent != null)
            {
                ShowPasswordEvent(application, username, description, website);
            }
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
                hitTestInfo = passwordDataGridView.HitTest(e.X, e.Y);
                // If column is first column
                if (hitTestInfo.Type == DataGridViewHitTestType.Cell)
                {
                    passwordContextMenuStrip.Show(passwordDataGridView, new Point(e.X, e.Y));
                    _rowIndexCopy = hitTestInfo.RowIndex;
                }                
            }
        }

        /*************************************************************************************************/
        private void PasswordDataGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_dgvPasswordList.Count > 0)
            {
                if (e.Delta > 0 && passwordDataGridView.FirstDisplayedScrollingRowIndex > 0)
                {
                    passwordDataGridView.FirstDisplayedScrollingRowIndex--;
                }
                else if (e.Delta < 0)
                {
                    passwordDataGridView.FirstDisplayedScrollingRowIndex++;
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
            if(passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                _selectedDgvIndex = passwordDataGridView.CurrentCell.RowIndex;
            }      
        }

        /*************************************************************************************************/
        private void AboutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //AboutView about = new AboutView();
            //about.ShowDialog();

            MessageBox.Show("This application is still under development.\nUse at your own risk!\n\nThis application utilizes icons from Icons8. (https://icons8.com.)", "Version 0.0.1", MessageBoxButtons.OK);
        }

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

    } // MainForm CLASS
} // MainForm NAMESPACE
