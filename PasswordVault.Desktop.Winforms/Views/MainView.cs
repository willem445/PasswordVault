using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PasswordVault.Services;
using PasswordVault.Models;
using System.Globalization;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* TODO - 8 - Add new form for adding password with validation, password strength etc
 * TODO - 1 - Generate very strong random key from passphase to encrypt data with (Makes it easier to change passwords) (https://security.stackexchange.com/questions/30193/encrypting-user-data-using-password-and-forgot-my-password)
 * https://security.stackexchange.com/questions/157422/store-encrypted-user-data-in-database
 * TODO - 9 - Create INI file to store database configuration etc.
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Desktop.Winforms
{
    /*=================================================================================================
    ENUMERATIONS
    *================================================================================================*/
    public enum DgvColumn
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
        private const int EMPTY_DGV = 0;

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
        public event Action<string, PasswordFilterOption> FilterChangedEvent;
        public event Action RequestPasswordsOnLoginEvent;
        public event Action LogoutEvent;
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
        public event Action GeneratePasswordEvent;

        /*PRIVATE*****************************************************************************************/
        private ILoginView _loginView;
        private IChangePasswordView _changePasswordView;
        private IEditUserView _editUserView;
        private IConfirmDeleteUserView _confirmDeleteUserView;

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
        public MainView(ILoginView loginView, IChangePasswordView changePasswordView, IEditUserView editUserView, IConfirmDeleteUserView confirmDeleteUserView)
        {
            _loginView = loginView ?? throw new ArgumentNullException(nameof(loginView));
            _changePasswordView = changePasswordView ?? throw new ArgumentNullException(nameof(changePasswordView));
            _editUserView = editUserView ?? throw new ArgumentNullException(nameof(editUserView));
            _confirmDeleteUserView = confirmDeleteUserView ?? throw new ArgumentNullException(nameof(confirmDeleteUserView));

            _loginView.LoginSuccessfulEvent += DisplayLoginSuccessful;
            _confirmDeleteUserView.ConfirmPasswordSuccessEvent += DeleteAccountConfirmPasswordSuccess;
            _confirmDeleteUserView.DeleteSuccessEvent += DeleteAccountSuccess;

            _dgvPasswordList = new BindingList<Password>();
            InitializeComponent();

            #region UI
            // Configure form UI
            BackColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            FormBorderStyle = FormBorderStyle.None;

            // Configure labels
            label1.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label1.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            label2.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label2.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            label3.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label3.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            label4.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label4.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            label5.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label5.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            label6.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label6.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            filterLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            filterLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            // Configure menu strip
            menuStrip.BackColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            menuStrip.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            menuStrip.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            menuStrip.MenuItemSelectedColor = UIHelper.GetColorFromCode(UIColors.ControlHighLightColor);
            menuStrip.MenuItemBackgroundColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            loginToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            loginToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            loginToolStripMenuItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            accountToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            accountToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            accountToolStripMenuItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            editToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            editToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            editToolStripMenuItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            editToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            deleteToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            deleteToolStripMenuItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            deleteToolStripMenuItem.Enabled = false;
            changePasswordToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            changePasswordToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            changePasswordToolStripMenuItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            changePasswordToolStripMenuItem.Enabled = false;

            // Configure buttons
            addButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            addButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            addButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            addButton.FlatAppearance.BorderSize = 1;

            deleteButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            deleteButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            deleteButton.FlatStyle = FlatStyle.Flat;
            deleteButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            deleteButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            deleteButton.FlatAppearance.BorderSize = 1;

            editButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            editButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            editButton.FlatStyle = FlatStyle.Flat;
            editButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            editButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            editButton.FlatAppearance.BorderSize = 1;

            editCancelButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            editCancelButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            editCancelButton.FlatStyle = FlatStyle.Flat;
            editCancelButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            editCancelButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            editCancelButton.FlatAppearance.BorderSize = 1;
            editCancelButton.Enabled = false;
            editCancelButton.Visible = false;

            clearFilterButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            clearFilterButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            clearFilterButton.FlatStyle = FlatStyle.Flat;
            clearFilterButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            clearFilterButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            clearFilterButton.FlatAppearance.BorderSize = 1;

            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            closeButton.Font = UIHelper.GetFont(UIFontSizes.CloseButtonFontSize);

            minimizeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            minimizeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            minimizeButton.Font = UIHelper.GetFont(UIFontSizes.CloseButtonFontSize);

            // Configure textbox
            applicationTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            applicationTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            applicationTextBox.BorderStyle = BorderStyle.FixedSingle;
            applicationTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            usernameTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            usernameTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            usernameTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            emailTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            emailTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            emailTextBox.BorderStyle = BorderStyle.FixedSingle;
            emailTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            descriptionTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            descriptionTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            descriptionTextBox.BorderStyle = BorderStyle.FixedSingle;
            descriptionTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            passphraseTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            passphraseTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            passphraseTextBox.BorderStyle = BorderStyle.FixedSingle;
            passphraseTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            websiteTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            websiteTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            websiteTextBox.BorderStyle = BorderStyle.FixedSingle;
            websiteTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            filterTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            filterTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            filterTextBox.BorderStyle = BorderStyle.FixedSingle;
            filterTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            // Configure combo box
            filterComboBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            filterComboBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            filterComboBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            filterComboBox.DataSource = Enum.GetValues(typeof(PasswordFilterOption));
            filterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            filterComboBox.HighlightColor = UIHelper.GetColorFromCode(UIColors.ControlHighLightColor);
            filterComboBox.BorderColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);

            // Configure status strip
            statusStrip1.BackColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            statusStrip1.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            statusStrip1.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            // Confgiure data grid view
            passwordDataGridView.BorderStyle = BorderStyle.None;
            passwordDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            passwordDataGridView.MultiSelect = false;
            passwordDataGridView.ReadOnly = true;
            passwordDataGridView.BackgroundColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            passwordDataGridView.RowsDefaultCellStyle.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            passwordDataGridView.RowsDefaultCellStyle.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            passwordDataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(65, 65, 65);
            passwordDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(85, 85, 85);
            passwordDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
            passwordDataGridView.AllowUserToOrderColumns = true;
            passwordDataGridView.AllowUserToAddRows = false;
            passwordDataGridView.DefaultCellStyle.SelectionBackColor = Color.DarkGray;
            passwordDataGridView.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            passwordDataGridView.EnableHeadersVisualStyles = false;
            passwordDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            passwordDataGridView.ColumnHeadersDefaultCellStyle.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            passwordDataGridView.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            passwordDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            passwordDataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            passwordDataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            passwordDataGridView.ScrollBars = ScrollBars.None;
            passwordDataGridView.MouseWheel += PasswordDataGridView_MouseWheel;

            // Configure context menu
            passwordContextMenuStrip = new AdvancedContextMenuStrip();
            var copyUsernameToolStripItem = new ToolStripMenuItem("Copy Username");
            copyUsernameToolStripItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            copyUsernameToolStripItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            copyUsernameToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            copyUsernameToolStripItem.Image = Bitmap.FromFile(Path.Combine(Environment.CurrentDirectory, @"Resources\icons8-copy-48.png"));
            passwordContextMenuStrip.Items.Add(copyUsernameToolStripItem);

            var copyPasswordToolStripItem = new ToolStripMenuItem("Copy Password");
            copyPasswordToolStripItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            copyPasswordToolStripItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            copyPasswordToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            copyPasswordToolStripItem.Image = Bitmap.FromFile(Path.Combine(Environment.CurrentDirectory, @"Resources\icons8-copy-48.png"));
            passwordContextMenuStrip.Items.Add(copyPasswordToolStripItem);

            var websiteToolStripItem = new ToolStripMenuItem("Visit Website");
            websiteToolStripItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            websiteToolStripItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            websiteToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            websiteToolStripItem.Image = Bitmap.FromFile(Path.Combine(Environment.CurrentDirectory, @"Resources\icons8-link-100.png"));
            passwordContextMenuStrip.Items.Add(websiteToolStripItem);

            var showPasswordToolStripItem = new ToolStripMenuItem("Show Password");
            showPasswordToolStripItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            showPasswordToolStripItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            showPasswordToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            showPasswordToolStripItem.Image = Bitmap.FromFile(Path.Combine(Environment.CurrentDirectory, @"Resources\icons8-show-property-60.png"));
            passwordContextMenuStrip.Items.Add(showPasswordToolStripItem);

            var editToolStripItem = new ToolStripMenuItem("Edit");
            editToolStripItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            editToolStripItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            editToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            editToolStripItem.Image = Bitmap.FromFile(Path.Combine(Environment.CurrentDirectory, @"Resources\icons8-edit-48.png"));
            passwordContextMenuStrip.Items.Add(editToolStripItem);

            var deleteToolStripItem = new ToolStripMenuItem("Delete");
            deleteToolStripItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            deleteToolStripItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            deleteToolStripItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            deleteToolStripItem.Image = Bitmap.FromFile(Path.Combine(Environment.CurrentDirectory, @"Resources\icons8-delete-60.png"));
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
            UIHelper.UpdateStatusLabel(string.Format(CultureInfo.CurrentCulture, "Welcome: {0}", userID), userStatusLabel, ErrorLevel.Neutral);
        }

        /*************************************************************************************************/
        public void DisplayPasswordToEdit(Password password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            applicationTextBox.Text = password.Application;
            usernameTextBox.Text = password.Username;
            emailTextBox.Text = password.Email;
            descriptionTextBox.Text = password.Description;
            websiteTextBox.Text = password.Website;
            passphraseTextBox.Text = password.Passphrase;
        }

        /*************************************************************************************************/
        public void DisplayAddEditPasswordResult(AddModifyPasswordResult result)
        {
            switch(result)
            {
                case AddModifyPasswordResult.DuplicatePassword:
                    UIHelper.UpdateStatusLabel("Duplicate password.", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.Failed:
                    UIHelper.UpdateStatusLabel("Modify password failed.", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.UsernameError:
                    UIHelper.UpdateStatusLabel("Issue with username field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.ApplicationError:
                    UIHelper.UpdateStatusLabel("Issue with application field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.PassphraseError:
                    UIHelper.UpdateStatusLabel("Issue with passphrase field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.EmailError:
                    UIHelper.UpdateStatusLabel("Invalid email!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.DescriptionError:
                    UIHelper.UpdateStatusLabel("Invalid description!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.WebsiteError:
                    UIHelper.UpdateStatusLabel("Invalid website!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.Success:
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
                    UIHelper.UpdateStatusLabel("Password modified.", userStatusLabel, ErrorLevel.Ok);
                    UpdateDataGridViewAfterEdit();
                    break;

                default:

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
                    filterTextBox.Enabled = false;
                    applicationTextBox.Text = "";
                    descriptionTextBox.Text = "";
                    websiteTextBox.Text = "";
                    passphraseTextBox.Text = "";
                    usernameTextBox.Text = "";
                    emailTextBox.Text = "";
                    filterTextBox.Text = "";
                    addButton.Enabled = false;
                    deleteButton.Enabled = false;
                    editButton.Enabled = false;
                    clearFilterButton.Enabled = false;
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
            if (!string.IsNullOrEmpty(password))
            {
                ShowPasswordView showPasswordView = new ShowPasswordView(password);
                showPasswordView.ShowDialog();
                showPasswordView.Dispose();
            }       
        }

        /*************************************************************************************************/
        public void DisplayAddPasswordResult(AddModifyPasswordResult result)
        {
            switch(result)
            {
                case AddModifyPasswordResult.Failed:
                    UIHelper.UpdateStatusLabel("Add password failed.", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.DuplicatePassword:
                    UIHelper.UpdateStatusLabel("Duplicate password.", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.UsernameError:
                    UIHelper.UpdateStatusLabel("Issue with username field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.ApplicationError:
                    UIHelper.UpdateStatusLabel("Issue with application field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.PassphraseError:
                    UIHelper.UpdateStatusLabel("Issue with passphrase field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.EmailError:
                    UIHelper.UpdateStatusLabel("Invalid email!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.WebsiteError:
                    UIHelper.UpdateStatusLabel("Invalid website!", userStatusLabel, ErrorLevel.Error);
                    break;

                case AddModifyPasswordResult.Success:
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

                    UpdateDataGridViewAfterDelete();

                    break;
            }   
        }

        public void DisplayGeneratePasswordResult(string generatedPassword)
        {
            passphraseTextBox.Text = generatedPassword;
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
            clearFilterButton.Enabled = true;
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
        /// <summary>
        /// Show the confirm delete view to confirm that the user would like to delete the account.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _confirmDeleteUserView.ShowView();
        }

        /*************************************************************************************************/
        /// <summary>
        /// If authentication was successfull, we need to first log out user before deleting the account.
        /// </summary>
        private void DeleteAccountConfirmPasswordSuccess()
        {
            RaiseLogoutEvent();
        }

        /*************************************************************************************************/
        /// <summary>
        /// User account was successfully deleted.
        /// </summary>
        private void DeleteAccountSuccess()
        {
            UIHelper.UpdateStatusLabel("Account deleted.", userStatusLabel, ErrorLevel.Ok);
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
            PasswordFilterOption filterOption = (PasswordFilterOption)filterComboBox.SelectedValue;
            RaiseNewFilterEvent(filterTextBox.Text, filterOption); 
        }

        /*************************************************************************************************/
        private void RaiseNewFilterEvent(string filterText, PasswordFilterOption passwordFilterOption)
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
                    
                    passwordDataGridView.Rows[_rowIndexCopy].Cells[0].Selected = true;

                    // PasswordDataGridView_SelectionChanged uses the old selected cell value for some reason after 
                    // manually setting the selected cell. Manually set the _selectedDgvIndex here to update the 
                    // selected cell to the correct index.
                    _selectedDgvIndex = _rowIndexCopy;
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
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.CloseButtonColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /*************************************************************************************************/
        private void MinimizeButton_MouseEnter(object sender, EventArgs e)
        {
            minimizeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlHighLightColor);
            minimizeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void MinimizeButton_MouseLeave(object sender, EventArgs e)
        {
            minimizeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            minimizeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
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
            about.Dispose();
        }

        /*************************************************************************************************/
        private void PassphraseTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // submit password
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

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /*************************************************************************************************/
        private void passphraseTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.G) // generate password
            {
                RaiseAddPasswordEvent();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /*************************************************************************************************/
        private void RaiseAddPasswordEvent()
        {
            if (GeneratePasswordEvent != null)
            {
                GeneratePasswordEvent();
            }
        }

        /*************************************************************************************************/
        private void UpdateDataGridViewAfterDelete()
        {
            int newDgvIndex = _selectedDgvIndex - 1;

            // Fix for #37, update filter when we delete a password
            PasswordFilterOption filterOption = (PasswordFilterOption)filterComboBox.SelectedValue;
            RaiseNewFilterEvent(filterTextBox.Text, filterOption);

            if (newDgvIndex >= 0)
            {
                passwordDataGridView.Rows[newDgvIndex].Selected = true;
                passwordDataGridView.Rows[newDgvIndex].Cells[0].Selected = true;
            }
        }

        /*************************************************************************************************/
        private void UpdateDataGridViewAfterEdit()
        {
            int dgvIndex = _selectedDgvIndex; // Need to store current index before updating filter since index will reset to 0

            // Fix for #37, update filter when we delete a password
            PasswordFilterOption filterOption = (PasswordFilterOption)filterComboBox.SelectedValue;
            RaiseNewFilterEvent(filterTextBox.Text, filterOption);

            passwordDataGridView.Rows[dgvIndex].Selected = true;
            passwordDataGridView.Rows[dgvIndex].Cells[0].Selected = true;
        }

        /*************************************************************************************************/
        private void clearFilterButton_Click(object sender, EventArgs e)
        {
            filterTextBox.Text = "";
        }

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

    } // MainForm CLASS
} // MainForm NAMESPACE
