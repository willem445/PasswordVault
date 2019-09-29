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
        Email,
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
        private const int INVALID_INDEX = -1;
        private const int EMPTY_DGV = 1;

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
        public event Action<string, PasswordFilterOptions> FilterChangedEvent;
        public event Action RequestPasswordsOnLoginEvent;
        public event Action LogoutEvent;
        public event Action DeleteAccountEvent;
        public event Action<string, string, string, string, string, string> AddPasswordEvent;
        public event Action<int> MovePasswordUpEvent;
        public event Action<int> MovePasswordDownEvent;
        public event Action<DataGridViewRow> EditPasswordEvent;
        public event Action<string, string, string, string, string, string> EditOkayEvent;
        public event Action EditCancelEvent;
        public event Action<DataGridViewRow> DeletePasswordEvent;   
        public event Action<DataGridViewRow> CopyUserNameEvent;
        public event Action<DataGridViewRow> CopyPasswordEvent;
        public event Action<DataGridViewRow> ShowPasswordEvent;
        public event Action<DataGridViewRow> NavigateToWebsiteEvent;

        /*PRIVATE*****************************************************************************************/
        private ILoginView _loginView;
        private IChangePasswordView _changePasswordView;
        private IEditUserView _editUserView;

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
        public MainView(ILoginView loginView, IChangePasswordView changePasswordView, IEditUserView editUserView)
        {
            _loginView = loginView;
            _changePasswordView = changePasswordView;
            _editUserView = editUserView;

            _loginView.LoginSuccessfulEvent += DisplayLoginSuccessful;
            _dgvPasswordList = new BindingList<Password>();
            InitializeComponent();

            #region UI
            // Configure form UI
            BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            FormBorderStyle = FormBorderStyle.None;

            // Configure labels
            label1.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            label1.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            label2.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            label2.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            label3.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            label3.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            label4.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            label4.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            label5.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            label5.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            label6.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            label6.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            filterLabel.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            filterLabel.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            // Configure menu strip
            menuStrip.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            menuStrip.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            menuStrip.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            menuStrip.MenuItemSelectedColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlHighLightColor);
            menuStrip.MenuItemBackgroundColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            loginToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            loginToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            loginToolStripMenuItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            accountToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            accountToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            accountToolStripMenuItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            editToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            editToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            editToolStripMenuItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            editToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            deleteToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            deleteToolStripMenuItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            deleteToolStripMenuItem.Enabled = false;
            changePasswordToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            changePasswordToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            changePasswordToolStripMenuItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            changePasswordToolStripMenuItem.Enabled = false;

            // Configure buttons
            addButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            addButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.Font = UIHelper.GetFont(UIHelper.UIFontSizes.ButtonFontSize);
            addButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            addButton.FlatAppearance.BorderSize = 1;

            deleteButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            deleteButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            deleteButton.FlatStyle = FlatStyle.Flat;
            deleteButton.Font = UIHelper.GetFont(UIHelper.UIFontSizes.ButtonFontSize);
            deleteButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            deleteButton.FlatAppearance.BorderSize = 1;

            editButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            editButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            editButton.FlatStyle = FlatStyle.Flat;
            editButton.Font = UIHelper.GetFont(UIHelper.UIFontSizes.ButtonFontSize);
            editButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            editButton.FlatAppearance.BorderSize = 1;
          
            editCancelButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            editCancelButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            editCancelButton.FlatStyle = FlatStyle.Flat;
            editCancelButton.Font = UIHelper.GetFont(UIHelper.UIFontSizes.ButtonFontSize);
            editCancelButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            editCancelButton.FlatAppearance.BorderSize = 1;
            editCancelButton.Enabled = false;
            editCancelButton.Visible = false;

            closeButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            closeButton.Font = UIHelper.GetFont(UIHelper.UIFontSizes.CloseButtonFontSize);

            minimizeButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            minimizeButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            minimizeButton.Font = UIHelper.GetFont(UIHelper.UIFontSizes.CloseButtonFontSize);

            // Configure textbox
            applicationTextBox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            applicationTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            applicationTextBox.BorderStyle = BorderStyle.FixedSingle;
            applicationTextBox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            usernameTextBox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            usernameTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            usernameTextBox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            emailTextBox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            emailTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            emailTextBox.BorderStyle = BorderStyle.FixedSingle;
            emailTextBox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            descriptionTextBox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            descriptionTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            descriptionTextBox.BorderStyle = BorderStyle.FixedSingle;
            descriptionTextBox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            passphraseTextBox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            passphraseTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            passphraseTextBox.BorderStyle = BorderStyle.FixedSingle;
            passphraseTextBox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            websiteTextBox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            websiteTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            websiteTextBox.BorderStyle = BorderStyle.FixedSingle;
            websiteTextBox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            filterTextBox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            filterTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            filterTextBox.BorderStyle = BorderStyle.FixedSingle;
            filterTextBox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            // Configure combo box
            filterComboBox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            filterComboBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            filterComboBox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);
            filterComboBox.DataSource = Enum.GetValues(typeof(PasswordFilterOptions));
            filterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            filterComboBox.HighlightColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlHighLightColor);
            filterComboBox.BorderColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);

            // Configure status strip
            statusStrip1.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            statusStrip1.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            statusStrip1.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            // Confgiure data grid view
            passwordDataGridView.BorderStyle = BorderStyle.None;
            passwordDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            passwordDataGridView.MultiSelect = false;
            passwordDataGridView.ReadOnly = true;
            passwordDataGridView.BackgroundColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            passwordDataGridView.RowsDefaultCellStyle.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            passwordDataGridView.RowsDefaultCellStyle.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            passwordDataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(65, 65, 65);
            passwordDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(85, 85, 85);
            passwordDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
            passwordDataGridView.AllowUserToOrderColumns = true;
            passwordDataGridView.DefaultCellStyle.SelectionBackColor = Color.DarkGray;
            passwordDataGridView.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            passwordDataGridView.EnableHeadersVisualStyles = false;
            passwordDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            passwordDataGridView.ColumnHeadersDefaultCellStyle.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            passwordDataGridView.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            passwordDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            passwordDataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            passwordDataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            passwordDataGridView.ScrollBars = ScrollBars.None;
            passwordDataGridView.MouseWheel += PasswordDataGridView_MouseWheel;

            // Configure context menu
            passwordContextMenuStrip = new AdvancedContextMenuStrip();
            var copyUsernameToolStripItem = new ToolStripMenuItem("Copy Username");
            copyUsernameToolStripItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            copyUsernameToolStripItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            copyUsernameToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            copyUsernameToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-copy-48.png"));
            passwordContextMenuStrip.Items.Add(copyUsernameToolStripItem);

            var copyPasswordToolStripItem = new ToolStripMenuItem("Copy Password");
            copyPasswordToolStripItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            copyPasswordToolStripItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            copyPasswordToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            copyPasswordToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-copy-48.png"));
            passwordContextMenuStrip.Items.Add(copyPasswordToolStripItem);

            var websiteToolStripItem = new ToolStripMenuItem("Visit Website");
            websiteToolStripItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            websiteToolStripItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            websiteToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            websiteToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-link-100.png"));
            passwordContextMenuStrip.Items.Add(websiteToolStripItem);

            var showPasswordToolStripItem = new ToolStripMenuItem("Show Password");
            showPasswordToolStripItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            showPasswordToolStripItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            showPasswordToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            showPasswordToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-show-property-60.png"));
            passwordContextMenuStrip.Items.Add(showPasswordToolStripItem);

            var editToolStripItem = new ToolStripMenuItem("Edit");
            editToolStripItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            editToolStripItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            editToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            editToolStripItem.Image = Bitmap.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\icons8-edit-48.png"));
            passwordContextMenuStrip.Items.Add(editToolStripItem);

            var deleteToolStripItem = new ToolStripMenuItem("Delete");
            deleteToolStripItem.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            deleteToolStripItem.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            deleteToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
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
            UIHelper.UpdateStatusLabel(string.Format("Welcome: {0}", userID), userStatusLabel, ErrorLevel.Neutral);
        }

        /*************************************************************************************************/
        public void DisplayPasswordToEdit(Password password)
        {
            applicationTextBox.Text = password.Application;
            usernameTextBox.Text = password.Username;
            emailTextBox.Text = password.Email;
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
                    UIHelper.UpdateStatusLabel("Duplicate password.", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.Failed:
                    UIHelper.UpdateStatusLabel("Modify password failed.", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.UsernameError:
                    UIHelper.UpdateStatusLabel("Issue with username field!", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.ApplicationError:
                    UIHelper.UpdateStatusLabel("Issue with application field!", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.PassphraseError:
                    UIHelper.UpdateStatusLabel("Issue with passphrase field!", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.EmailError:
                    UIHelper.UpdateStatusLabel("Invalid email!", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.Success:
                    addButton.Text = "Add";
                    _editMode = false;
                    editCancelButton.Enabled = false;
                    editCancelButton.Visible = false;
                    applicationTextBox.Text = "";
                    usernameTextBox.Text = "";
                    emailTextBox.Text = "";
                    descriptionTextBox.Text = "";
                    websiteTextBox.Text = "";
                    passphraseTextBox.Text = "";
                    applicationTextBox.Focus();
                    UIHelper.UpdateStatusLabel("Password modified.", userStatusLabel, ErrorLevel.Neutral);
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
                    UIHelper.UpdateStatusLabel("Log out failed!", userStatusLabel, ErrorLevel.Neutral);
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
                    emailTextBox.Enabled = false;
                    addButton.Enabled = false;
                    deleteButton.Enabled = false;
                    editButton.Enabled = false;
                    filterTextBox.Enabled = false;
                    deleteToolStripMenuItem.Enabled = false;
                    changePasswordToolStripMenuItem.Enabled = false;
                    editToolStripMenuItem.Enabled = false;
                    loginToolStripMenuItem.Text = "Login";
                    UIHelper.UpdateStatusLabel("Logged off.", userStatusLabel, ErrorLevel.Neutral);
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
                    UIHelper.UpdateStatusLabel("Add password failed.", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.DuplicatePassword:
                    UIHelper.UpdateStatusLabel("Duplicate password.", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.UsernameError:
                    UIHelper.UpdateStatusLabel("Issue with username field!", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.ApplicationError:
                    UIHelper.UpdateStatusLabel("Issue with application field!", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.PassphraseError:
                    UIHelper.UpdateStatusLabel("Issue with passphrase field!", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.EmailError:
                    UIHelper.UpdateStatusLabel("Invalid email!", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case AddPasswordResult.Success:
                    UIHelper.UpdateStatusLabel("Success.", userStatusLabel, ErrorLevel.Neutral);
                    applicationTextBox.Text = "";
                    descriptionTextBox.Text = "";
                    websiteTextBox.Text = "";
                    passphraseTextBox.Text = "";
                    usernameTextBox.Text = "";
                    emailTextBox.Text = "";
                    applicationTextBox.Focus();
                    break;
            }
        }

        public void DisplayDeletePasswordResult(DeletePasswordResult result)
        {
            switch (result)
            {
                case DeletePasswordResult.Failed:
                    UIHelper.UpdateStatusLabel("Delete password failed.", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case DeletePasswordResult.PasswordDoesNotExist:
                    UIHelper.UpdateStatusLabel("Password does not exist.", userStatusLabel, ErrorLevel.Neutral);
                    break;

                case DeletePasswordResult.Success:
                    UIHelper.UpdateStatusLabel("Password deleted.", userStatusLabel, ErrorLevel.Neutral);
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
            emailTextBox.Enabled = true;
            addButton.Enabled = true;
            deleteButton.Enabled = true;
            editButton.Enabled = true;
            filterComboBox.Enabled = true;
            filterTextBox.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            changePasswordToolStripMenuItem.Enabled = true;
            editToolStripMenuItem.Enabled = true;
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
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _editUserView.ShowEditUserMenu();
        }

        /*************************************************************************************************/
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaiseDeleteAccountEvent();
        }

        /*************************************************************************************************/
        private void RaiseDeleteAccountEvent()
        {
            if (DeleteAccountEvent != null)
            {
                DeleteAccountEvent();
            }
        }

        /*************************************************************************************************/
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _changePasswordView.ShowChangePassword();
        }

        /*************************************************************************************************/
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (!_editMode)
            {
                RaiseAddPasswordEvent(applicationTextBox.Text, 
                                      usernameTextBox.Text,
                                      emailTextBox.Text, 
                                      descriptionTextBox.Text, 
                                      websiteTextBox.Text, 
                                      passphraseTextBox.Text);
            }
            else
            {
                RaiseEditOkayEvent(applicationTextBox.Text,
                                   usernameTextBox.Text,
                                   emailTextBox.Text,
                                   descriptionTextBox.Text,
                                   websiteTextBox.Text,
                                   passphraseTextBox.Text);
            }        
        }

        /*************************************************************************************************/
        private void RaiseAddPasswordEvent(string application, string username, string email, string description, string website, string passphrase)
        {
            if (AddPasswordEvent != null)
            {
                AddPasswordEvent(application, username, email, description, website, passphrase);
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

                DataGridViewRow row = passwordDataGridView.Rows[_selectedDgvIndex];
                RaiseEditPasswordEvent(row);
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
            emailTextBox.Text = "";
            descriptionTextBox.Text = "";
            websiteTextBox.Text = "";
            passphraseTextBox.Text = "";

            RaiseEditCancelEvent();
        }

        /*************************************************************************************************/
        private void RaiseEditPasswordEvent(DataGridViewRow dgvrow)
        {
            if (EditPasswordEvent != null)
            {
                EditPasswordEvent(dgvrow);
            }
        }

        /*************************************************************************************************/
        private void RaiseEditOkayEvent(string application, string username, string email, string description, string website, string passphrase)
        {
            if (EditOkayEvent != null)
            {
                EditOkayEvent(application, username, email, description, website, passphrase);
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
                DataGridViewRow row = passwordDataGridView.Rows[_selectedDgvIndex];
                RaiseDeletePasswordEvent(row);
            }
        }

        /*************************************************************************************************/
        private void RaiseDeletePasswordEvent(DataGridViewRow dgvrow)
        {
            if (DeletePasswordEvent != null)
            {
                DeletePasswordEvent(dgvrow);
            }
        }

        /*************************************************************************************************/
        private void CopyPass_Click(object sender, EventArgs e)
        {
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                DataGridViewRow row = passwordDataGridView.Rows[_selectedDgvIndex];
                RaiseCopyPasswordEvent(row);
            }             
        }

        /*************************************************************************************************/
        private void RaiseCopyPasswordEvent(DataGridViewRow dgvrow)
        {
            if (CopyPasswordEvent != null)
            {
                CopyPasswordEvent(dgvrow);
            }
        }

        /*************************************************************************************************/
        private void CopyUser_Click(object sender, EventArgs e)
        {
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                DataGridViewRow row = passwordDataGridView.Rows[_selectedDgvIndex];
                RaiseCopyUserEvent(row);
            }             
        }

        /*************************************************************************************************/
        private void RaiseCopyUserEvent(DataGridViewRow dgvrow)
        {
            CopyUserNameEvent?.Invoke(dgvrow);
        }

        /*************************************************************************************************/
        private void Website_Click(object sender, EventArgs e)
        {
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                DataGridViewRow row = passwordDataGridView.Rows[_selectedDgvIndex];
                RaiseNavigateToWebsiteEvent(row);
            }           
        }

        /*************************************************************************************************/
        private void RaiseNavigateToWebsiteEvent(DataGridViewRow dgvrow)
        {
            if (NavigateToWebsiteEvent != null)
            {
                NavigateToWebsiteEvent(dgvrow);
            }
        }

        /*************************************************************************************************/
        private void ShowPassword_Click(object sender, EventArgs e)
        {
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                DataGridViewRow row = passwordDataGridView.Rows[_selectedDgvIndex];
                RaiseShowPasswordEvent(row);
            }            
        }

        /*************************************************************************************************/
        private void RaiseShowPasswordEvent(DataGridViewRow dgvrow)
        {
            if (ShowPasswordEvent != null)
            {
                ShowPasswordEvent(dgvrow);
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
        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /*************************************************************************************************/
        private void MinimizeButton_MouseEnter(object sender, EventArgs e)
        {
            minimizeButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlHighLightColor);
            minimizeButton.ForeColor = Color.FromArgb(242, 242, 242);
        }

        /*************************************************************************************************/
        private void MinimizeButton_MouseLeave(object sender, EventArgs e)
        {
            minimizeButton.BackColor = Color.FromArgb(63, 63, 63);
            minimizeButton.ForeColor = Color.FromArgb(242, 242, 242);
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
            AboutView about = new AboutView();
            about.ShowDialog();
        }

        private void PassphraseTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!_editMode)
                {
                    RaiseAddPasswordEvent(applicationTextBox.Text,
                                          usernameTextBox.Text,
                                          emailTextBox.Text,
                                          descriptionTextBox.Text,
                                          websiteTextBox.Text,
                                          passphraseTextBox.Text);
                }
                else
                {
                    RaiseEditOkayEvent(applicationTextBox.Text,
                                       usernameTextBox.Text,
                                       emailTextBox.Text,
                                       descriptionTextBox.Text,
                                       websiteTextBox.Text,
                                       passphraseTextBox.Text);
                }
            }
        }

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

    } // MainForm CLASS
} // MainForm NAMESPACE
