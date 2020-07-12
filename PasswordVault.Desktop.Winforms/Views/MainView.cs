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
/* 
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
        public event Action RequestPasswordsEvent;
        public event Action LogoutEvent;
        public event Action<string, string, string, string, string, string, string> AddPasswordEvent;
        public event Action<DataGridViewRow> EditPasswordEvent;
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
        private IExportView _exportView;
        private IImportView _importView;
        private IAddPasswordView _addPasswordView;

        private AdvancedContextMenuStrip passwordContextMenuStrip;                      // Context menu for right clicking on datagridview row
        private int _rowIndexCopy = 0;                // Index of row being right clicked on
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to

        private bool _loggedIn = false;
        private int _selectedDgvIndex = INVALID_INDEX;
        private int _selectedDgvIndexPriorToPasswordListModification = INVALID_INDEX;
        private bool _editMode = false;

        private BindingList<Password> _dgvPasswordList;

        private readonly GhostTextBoxHelper _filterGhost;
        private string _filterGhostText = "Filter";

        private System.Timers.Timer logoutTimeoutTimer;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public MainView(
            ILoginView loginView, 
            IChangePasswordView changePasswordView, 
            IEditUserView editUserView, 
            IConfirmDeleteUserView confirmDeleteUserView, 
            IExportView exportView, 
            IImportView importView,
            IAddPasswordView addPasswordView)
        {
            _loginView = loginView ?? throw new ArgumentNullException(nameof(loginView));
            _changePasswordView = changePasswordView ?? throw new ArgumentNullException(nameof(changePasswordView));
            _editUserView = editUserView ?? throw new ArgumentNullException(nameof(editUserView));
            _confirmDeleteUserView = confirmDeleteUserView ?? throw new ArgumentNullException(nameof(confirmDeleteUserView));
            _exportView = exportView ?? throw new ArgumentNullException(nameof(exportView));
            _importView = importView ?? throw new ArgumentNullException(nameof(importView));
            _addPasswordView = addPasswordView ?? throw new ArgumentNullException(nameof(addPasswordView));

            _loginView.LoginSuccessfulEvent += DisplayLoginSuccessful;
            _loginView.AuthenticationSuccessfulEvent += AuthenticationSuccessful;
            _confirmDeleteUserView.ConfirmPasswordSuccessEvent += DeleteAccountConfirmPasswordSuccess;
            _confirmDeleteUserView.DeleteSuccessEvent += DeleteAccountSuccess;
            _importView.ImportPasswordsDoneEvent += DisplayImportResult;

            _dgvPasswordList = new BindingList<Password>();
            InitializeComponent();

            #region UI
            this.Icon = new Icon(@"Resources\081vault_101519.ico");

            // Configure form UI
            BackColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            FormBorderStyle = FormBorderStyle.None;

            // Configure labels
            label7.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label7.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            label7.Visible = false;

            passwordCountLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            passwordCountLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);         
            passwordCountLabel.Visible = false;

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
            exportPasswordsToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            exportPasswordsToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            exportPasswordsToolStripMenuItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            exportPasswordsToolStripMenuItem.Enabled = false;
            importPasswordsToolStripMenuItem.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            importPasswordsToolStripMenuItem.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            importPasswordsToolStripMenuItem.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            importPasswordsToolStripMenuItem.Enabled = false;

            // Configure buttons
            addButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            addButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            addButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            addButton.FlatAppearance.BorderSize = 1;

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
            filterTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            filterTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            filterTextBox.BorderStyle = BorderStyle.None;
            filterTextBox.AutoSize = false;
            filterTextBox.Font = UIHelper.GetFont(9.0f);
            filterTextBox.Size = new System.Drawing.Size(264, 22);
            _filterGhost = new GhostTextBoxHelper(filterTextBox, _filterGhostText);

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
            passwordDataGridView.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            passwordDataGridView.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
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
            passwordCountLabel.Text = "0";

            logoutTimeoutTimer = new System.Timers.Timer();
            logoutTimeoutTimer.Interval = 900000; // 15 minutes
            logoutTimeoutTimer.Enabled = false;
            logoutTimeoutTimer.Elapsed += TimerExpired;
            logoutTimeoutTimer.AutoReset = false;
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void SetTimeoutTime(int minutes)
        {
            int milliseconds = minutes * 60 * 1000;
            logoutTimeoutTimer.Interval = milliseconds;
        }

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

            throw new NotImplementedException();
        }

        /*************************************************************************************************/
        public void DisplayAddEditPasswordResult(ValidatePassword result)
        {
            switch(result)
            {
                case ValidatePassword.DuplicatePassword:
                    UIHelper.UpdateStatusLabel("Duplicate password.", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.Failed:
                    UIHelper.UpdateStatusLabel("Modify password failed.", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.UsernameError:
                    UIHelper.UpdateStatusLabel("Issue with username field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.ApplicationError:
                    UIHelper.UpdateStatusLabel("Issue with application field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.PassphraseError:
                    UIHelper.UpdateStatusLabel("Issue with passphrase field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.EmailError:
                    UIHelper.UpdateStatusLabel("Invalid email!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.DescriptionError:
                    UIHelper.UpdateStatusLabel("Invalid description!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.WebsiteError:
                    UIHelper.UpdateStatusLabel("Invalid website!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.Success:
                    addButton.Text = "Add";
                    _editMode = false;

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
                    this.BeginInvoke((Action)(() => UIHelper.UpdateStatusLabel("Log out failed!", userStatusLabel, ErrorLevel.Error)));
                    break;

                case LogOutResult.Success:
                    _loggedIn = false;
                    this.BeginInvoke((Action)(() => passwordDataGridView.DataSource = null));
                    this.BeginInvoke((Action)(() => passwordDataGridView.Rows.Clear()));            
                    this.BeginInvoke((Action)(() => userStatusLabel.Text = ""));
                    this.BeginInvoke((Action)(() => filterTextBox.Enabled = false));
                    this.BeginInvoke((Action)(() => filterTextBox.Text = ""));
                    this.BeginInvoke((Action)(() => addButton.Enabled = false));
                    this.BeginInvoke((Action)(() => clearFilterButton.Enabled = false));
                    this.BeginInvoke((Action)(() => deleteToolStripMenuItem.Enabled = false));
                    this.BeginInvoke((Action)(() => changePasswordToolStripMenuItem.Enabled = false));
                    this.BeginInvoke((Action)(() => exportPasswordsToolStripMenuItem.Enabled = false));
                    this.BeginInvoke((Action)(() => importPasswordsToolStripMenuItem.Enabled = false));
                    this.BeginInvoke((Action)(() => editToolStripMenuItem.Enabled = false));
                    this.BeginInvoke((Action)(() => label7.Visible = false));
                    this.BeginInvoke((Action)(() => passwordCountLabel.Visible = false));
                    this.BeginInvoke((Action)(() => passwordCountLabel.Text = ""));
                    this.BeginInvoke((Action)(() => loginToolStripMenuItem.Text = "Login"));
                    this.BeginInvoke((Action)(() => logoutTimeoutTimer.Enabled = false));
                    this.BeginInvoke((Action)(() => UIHelper.UpdateStatusLabel("Logged off.", userStatusLabel, ErrorLevel.Neutral)));
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
        public void DisplayAddPasswordResult(ValidatePassword result)
        {
            switch(result)
            {
                case ValidatePassword.Failed:
                    UIHelper.UpdateStatusLabel("Add password failed.", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.DuplicatePassword:
                    UIHelper.UpdateStatusLabel("Duplicate password.", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.UsernameError:
                    UIHelper.UpdateStatusLabel("Issue with username field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.ApplicationError:
                    UIHelper.UpdateStatusLabel("Issue with application field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.PassphraseError:
                    UIHelper.UpdateStatusLabel("Issue with passphrase field!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.EmailError:
                    UIHelper.UpdateStatusLabel("Invalid email!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.WebsiteError:
                    UIHelper.UpdateStatusLabel("Invalid website!", userStatusLabel, ErrorLevel.Error);
                    break;

                case ValidatePassword.Success:
                    UIHelper.UpdateStatusLabel("Success.", userStatusLabel, ErrorLevel.Neutral);
                    UpdateDataGridViewAfterEdit();
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

        public void DisplayPasswordCount(int count)
        {
            passwordCountLabel.Text = count.ToString(CultureInfo.CurrentCulture);
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

            addButton.Enabled = true;
            clearFilterButton.Enabled = true;
            filterComboBox.Enabled = true;
            filterTextBox.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            changePasswordToolStripMenuItem.Enabled = true;
            exportPasswordsToolStripMenuItem.Enabled = true;
            importPasswordsToolStripMenuItem.Enabled = true;
            editToolStripMenuItem.Enabled = true;
            label7.Visible = true;
            passwordCountLabel.Visible = true;
            loginToolStripMenuItem.Text = "Logoff";
            Cursor = Cursors.Arrow;
            logoutTimeoutTimer.Enabled = true;

            RaiseRequestPasswordsOnLoginEvent();
        }

        /*************************************************************************************************/
        private void AuthenticationSuccessful()
        {
            Cursor = Cursors.WaitCursor;
            UIHelper.UpdateStatusLabel("Loading passwords...", userStatusLabel, ErrorLevel.Neutral);
        }

        /*************************************************************************************************/
        private void DisplayImportResult(ImportExportResult result)
        {
            switch (result)
            {
                case ImportExportResult.Success:
                    UIHelper.UpdateStatusLabel("Success", userStatusLabel, ErrorLevel.Neutral);
                    break;
                case ImportExportResult.Fail:
                default:
                    UIHelper.UpdateStatusLabel("Import failed!", userStatusLabel, ErrorLevel.Error);
                    break;
            }

            RaiseRequestPasswordsEvent();
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
        private void RaiseRequestPasswordsEvent()
        {
            RequestPasswordsEvent?.Invoke();
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
            _addPasswordView.ShowMenu();
        }

        /*************************************************************************************************/
        private void EditButton_Click(object sender, EventArgs e)
        {
            if (passwordDataGridView.Rows.Count > EMPTY_DGV)
            {
                _editMode = true;

                // Save DGV index prior to reloading password list etc. which changes _selectedDgvIndex.
                _selectedDgvIndexPriorToPasswordListModification = _selectedDgvIndex;

                DataGridViewRow row = passwordDataGridView.Rows[_selectedDgvIndex];
                RaiseEditPasswordEvent(row);
            }             
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
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var confirmDelete = new ConfirmDeletePasswordView();
            if (confirmDelete.ShowDialog() == DialogResult.OK)
            {
                if (passwordDataGridView.Rows.Count > EMPTY_DGV)
                {
                    // Save DGV index prior to reloading password list etc. which changes _selectedDgvIndex.
                    _selectedDgvIndexPriorToPasswordListModification = _selectedDgvIndex;

                    DataGridViewRow row = passwordDataGridView.Rows[_selectedDgvIndex];
                    RaiseDeletePasswordEvent(row);
                }
            }
            confirmDelete.Dispose();
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
            if(filterTextBox.Text != _filterGhostText)
            {
                PasswordFilterOption filterOption = (PasswordFilterOption)filterComboBox.SelectedValue;
                RaiseNewFilterEvent(filterTextBox.Text, filterOption);
            }    
        }

        /*************************************************************************************************/
        private void RaiseNewFilterEvent(string filterText, PasswordFilterOption passwordFilterOption)
        {
            if (FilterChangedEvent != null && filterText != _filterGhostText)
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
        private void UpdateDataGridViewAfterDelete()
        {
            // Use _selectedDgvIndexPriorToPasswordListModification instead of _selectedDgvIndex because _selectedDgvIndex gets
            // modified frequently when reloading the password list etc. _selectedDgvIndexPriorToPasswordListModification only gets
            // updated when edit or delete buttons are clicked.
            int newDgvIndex = _selectedDgvIndexPriorToPasswordListModification - 1;

            // Fix for #37, update filter when we delete a password
            if (filterTextBox.Text != _filterGhostText)
            {
                PasswordFilterOption filterOption = (PasswordFilterOption)filterComboBox.SelectedValue;
                RaiseNewFilterEvent(filterTextBox.Text, filterOption);
            }
            else
            {
                RaiseRequestPasswordsEvent();
            }                   

            if (newDgvIndex >= 0 && passwordDataGridView.Rows.Count > newDgvIndex)
            {
                passwordDataGridView.Rows[newDgvIndex].Selected = true;
                passwordDataGridView.Rows[newDgvIndex].Cells[0].Selected = true;
                _selectedDgvIndex = newDgvIndex;
            }
        }

        /*************************************************************************************************/
        private void UpdateDataGridViewAfterEdit()
        {
            int dgvIndex = _selectedDgvIndexPriorToPasswordListModification; // Need to store current index before updating filter since index will reset to 0

            // Fix for #37, update filter when we delete a password
            if (filterTextBox.Text != _filterGhostText)
            {
                PasswordFilterOption filterOption = (PasswordFilterOption)filterComboBox.SelectedValue;
                RaiseNewFilterEvent(filterTextBox.Text, filterOption);
            }
            else
            {
                RaiseRequestPasswordsEvent();
            }

            if (dgvIndex >= 0 && passwordDataGridView.Rows.Count > dgvIndex)
            {
                passwordDataGridView.Rows[dgvIndex].Selected = true;
                passwordDataGridView.Rows[dgvIndex].Cells[0].Selected = true;
                _selectedDgvIndex = dgvIndex;
            }        
        }

        /*************************************************************************************************/
        private void clearFilterButton_Click(object sender, EventArgs e)
        {
            filterTextBox.Text = "";
            _filterGhost.Reset();
        }

        /*************************************************************************************************/
        private void exportPasswordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _exportView.ShowExportView();
        }

        /*************************************************************************************************/
        private void importPasswordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _importView.ShowImportView();
        }

        /*************************************************************************************************/
        private void MainView_SizeChanged(object sender, EventArgs e)
        {
            // This should help with redrawing the form when minimizing
            this.Refresh();
        }

        private void passwordDataGridView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var confirmDelete = new ConfirmDeletePasswordView();
                if (confirmDelete.ShowDialog() == DialogResult.OK)
                {
                    if (passwordDataGridView.Rows.Count > EMPTY_DGV)
                    {
                        // Save DGV index prior to reloading password list etc. which changes _selectedDgvIndex.
                        _selectedDgvIndexPriorToPasswordListModification = _selectedDgvIndex;

                        DataGridViewRow row = passwordDataGridView.Rows[_selectedDgvIndex];
                        RaiseDeletePasswordEvent(row);
                    }
                }
                confirmDelete.Dispose();
            }
        }

        private void MainView_MouseMove(object sender, MouseEventArgs e)
        {   
            if (_loggedIn)
            {
                logoutTimeoutTimer.Stop();
                logoutTimeoutTimer.Start();
            }  
        }

        private void TimerExpired(Object source, System.Timers.ElapsedEventArgs e)
        {
            RaiseLogoutEvent();
        }

        private void passwordDataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((Control.ModifierKeys & Keys.Control) == Keys.Control) && (e.KeyChar == 1)) // 7 is char for 'g'
            {
                e.Handled = true;
                _addPasswordView.ShowMenu();
            }
        }

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

    } // MainForm CLASS
} // MainForm NAMESPACE
