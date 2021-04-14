using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using PasswordVault.Services;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Desktop.Winforms
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
        public event Action<string, string> LoginEvent;
        public event Action<string, string, string, string, string, string> CreateNewUserEvent;
        public event Action GenerateNewPasswordEvent;
        public event Action<string> PasswordChangedEvent;
        public event Action LoginSuccessfulEvent;
        public event Action AuthenticationSuccessfulEvent;
        public event Action DisplayPasswordRequirementsEvent;

        /*PRIVATE*****************************************************************************************/
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to
        private Size SmallSize = new Size(279, 222);
        private Size LargeSize = new Size(549, 320);

        private GhostTextBoxHelper ghostLoginUsername;
        private GhostTextBoxHelper ghostLoginPassword;
        private GhostTextBoxHelper ghostNewUsername;
        private GhostTextBoxHelper ghostNewPassword;
        private GhostTextBoxHelper ghostNewFirstName;
        private GhostTextBoxHelper ghostNewLastName;
        private GhostTextBoxHelper ghostNewPhoneNumber;
        private GhostTextBoxHelper ghostNewEmail;

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
            DisableCreateUserForm();
            BackColor = UIHelper.GetColorFromCode(UIColors.SecondaryFromBackgroundColor);
            FormBorderStyle = FormBorderStyle.None;

            // Configure tooltip
            toolTip.OwnerDraw = true;
            toolTip.Draw += new DrawToolTipEventHandler(toolTip_Draw);     

            // Configure buttons
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            closeButton.Font = UIHelper.GetFont(UIFontSizes.CloseButtonFontSize);

            loginButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            loginButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            loginButton.FlatStyle = FlatStyle.Flat;
            loginButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            loginButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            loginButton.FlatAppearance.BorderSize = 1;

            newUserButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            newUserButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            newUserButton.FlatStyle = FlatStyle.Flat;
            newUserButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            newUserButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            newUserButton.FlatAppearance.BorderSize = 1;

            generatePasswordButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            generatePasswordButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            generatePasswordButton.FlatStyle = FlatStyle.Flat;
            generatePasswordButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            generatePasswordButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            generatePasswordButton.FlatAppearance.BorderSize = 1;

            createLoginButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createLoginButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createLoginButton.FlatStyle = FlatStyle.Flat;
            createLoginButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            createLoginButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            createLoginButton.FlatAppearance.BorderSize = 1;

            // Configure textbox
            loginUsernameTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            loginUsernameTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            loginUsernameTextBox.BorderStyle = BorderStyle.None;
            loginUsernameTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            loginUsernameTextBox.AutoSize = false;
            loginUsernameTextBox.Font = UIHelper.GetFont(9.0f);
            loginUsernameTextBox.Size = new System.Drawing.Size(218, 22);
            ghostLoginUsername = new GhostTextBoxHelper(loginUsernameTextBox, "Username");

            loginPasswordTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            loginPasswordTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            loginPasswordTextBox.BorderStyle = BorderStyle.None;
            loginPasswordTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            loginPasswordTextBox.AutoSize = false;
            loginPasswordTextBox.Font = UIHelper.GetFont(9.0f);
            loginPasswordTextBox.Size = new System.Drawing.Size(218, 22);
            ghostLoginPassword = new GhostTextBoxHelper(loginPasswordTextBox, "Password", true);
                    
            createUsernameTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createUsernameTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createUsernameTextBox.BorderStyle = BorderStyle.None;
            createUsernameTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            createUsernameTextBox.AutoSize = false;
            createUsernameTextBox.Font = UIHelper.GetFont(9.0f);
            createUsernameTextBox.Size = new System.Drawing.Size(218, 22);
            ghostNewUsername = new GhostTextBoxHelper(createUsernameTextBox, "Username");

            createPasswordTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createPasswordTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createPasswordTextBox.BorderStyle = BorderStyle.None;
            createPasswordTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            createPasswordTextBox.AutoSize = false;
            createPasswordTextBox.Font = UIHelper.GetFont(9.0f, true);
            createPasswordTextBox.Size = new System.Drawing.Size(218, 22);
            ghostNewPassword = new GhostTextBoxHelper(createPasswordTextBox, "Password");

            createFirstNameTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createFirstNameTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createFirstNameTextBox.BorderStyle = BorderStyle.None;
            createFirstNameTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            createFirstNameTextBox.AutoSize = false;
            createFirstNameTextBox.Font = UIHelper.GetFont(9.0f);
            createFirstNameTextBox.Size = new System.Drawing.Size(218, 22);
            ghostNewFirstName = new GhostTextBoxHelper(createFirstNameTextBox, "First Name");

            createLastNameTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createLastNameTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createLastNameTextBox.BorderStyle = BorderStyle.None;
            createLastNameTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            createLastNameTextBox.AutoSize = false;
            createLastNameTextBox.Font = UIHelper.GetFont(9.0f);
            createLastNameTextBox.Size = new System.Drawing.Size(218, 22);
            ghostNewLastName = new GhostTextBoxHelper(createLastNameTextBox, "Last Name");

            createEmailTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createEmailTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createEmailTextBox.BorderStyle = BorderStyle.None;
            createEmailTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            createEmailTextBox.Text = "example@provider.com";
            createEmailTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
            createEmailTextBox.AutoSize = false;
            createEmailTextBox.Font = UIHelper.GetFont(9.0f);
            createEmailTextBox.Size = new System.Drawing.Size(218, 22);
            ghostNewEmail = new GhostTextBoxHelper(createEmailTextBox, "Email (example@provider.com)");

            createPhoneNumberTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createPhoneNumberTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createPhoneNumberTextBox.BorderStyle = BorderStyle.None;
            createPhoneNumberTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            createPhoneNumberTextBox.Text = "xxx-xxx-xxxx";
            createPhoneNumberTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
            createPhoneNumberTextBox.AutoSize = false;
            createPhoneNumberTextBox.Font = UIHelper.GetFont(9.0f);
            createPhoneNumberTextBox.Size = new System.Drawing.Size(218, 22);
            ghostNewPhoneNumber = new GhostTextBoxHelper(createPhoneNumberTextBox, "Phone Number (xxx-xxx-xxxx)");

            // Configure labels

            loginResultLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            loginResultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
            loginResultLabel.Visible = false;

            createNewUserResultLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            createNewUserResultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
            createNewUserResultLabel.Visible = false;

            newPasswordHelpLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            newPasswordHelpLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            newPasswordHelpLabel.Visible = true;
            newPasswordHelpLabel.Text = "It is strongly encouraged to use the generate password feature " +
                                        "when creating a new master password. Your master password should " +
                                        "be long and random. This password should be memorized or kept safe " +
                                        "in a secure location.";

            // Configure groupbox
            groupBox1.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            groupBox1.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            groupBox2.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            groupBox2.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            #endregion            
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void DisplayLoginResult(AuthenticateResult result)
        {
            this.BeginInvoke((Action)(() =>
            {
                switch (result)
                {
                    case AuthenticateResult.PasswordIncorrect:
                        loginResultLabel.Visible = true;
                        UIHelper.UpdateStatusLabel("Password incorrect.", loginResultLabel, ErrorLevel.Error);
                        break;

                    case AuthenticateResult.UsernameDoesNotExist:
                        loginResultLabel.Visible = true;
                        UIHelper.UpdateStatusLabel("Username doesn't exist.", loginResultLabel, ErrorLevel.Error);
                        break;

                    case AuthenticateResult.Failed:
                        loginResultLabel.Visible = true;
                        loginResultLabel.Text = "Login failed.";
                        UIHelper.UpdateStatusLabel("Username doesn't exist.", loginResultLabel, ErrorLevel.Error);
                        break;

                    case AuthenticateResult.Successful:
                        AuthenticationSuccessfulEvent?.Invoke();
                        DisableCreateUserForm();
                        ClearLoginView();
                        DialogResult = DialogResult.OK;                 
                        this.Close();
                        break;

                    default:
                        loginResultLabel.Visible = true;
                        UIHelper.UpdateStatusLabel("Login failed.", loginResultLabel, ErrorLevel.Error);
                        break;
                }
            }));
        }

        /*************************************************************************************************/
        public void PasswordLoadingDone()
        {
            RaiseLoginSuccessfulEvent();
        }

        /*************************************************************************************************/
        public void DisplayGeneratePasswordResult(string generatedPassword)
        {
            createPasswordTextBox.Text = generatedPassword;
        }

        /*************************************************************************************************/
        public void DisplayCreateNewUserResult(AddUserResult result, int minimumPasswordLength)
        {
            this.BeginInvoke((Action)(() =>
            {
                switch (result)
                {
                    case AddUserResult.UsernameTaken:
                        UIHelper.UpdateStatusLabel("Username taken!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.Failed:
                        UIHelper.UpdateStatusLabel("Unsuccessful!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.PasswordNotValid:
                        UIHelper.UpdateStatusLabel("Password does not meet requirements!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.UsernameNotValid:
                        UIHelper.UpdateStatusLabel("Invalid username!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.FirstNameNotValid:
                        UIHelper.UpdateStatusLabel("First name not valid!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.LastNameNotValid:
                        UIHelper.UpdateStatusLabel("Last name not valid!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.PhoneNumberNotValid:
                        UIHelper.UpdateStatusLabel("Phone number not valid!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.EmailNotValid:
                        UIHelper.UpdateStatusLabel("Email not valid!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.NoLowerCaseCharacter:
                        UIHelper.UpdateStatusLabel("Password must have lower case!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.LengthRequirementNotMet:
                        UIHelper.UpdateStatusLabel(string.Format(CultureInfo.CurrentCulture, "Password must have length: {0}!", minimumPasswordLength), createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.NoNumber:
                        UIHelper.UpdateStatusLabel("Password must have number!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.NoSpecialCharacter:
                        UIHelper.UpdateStatusLabel("Password must have special character!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.NoUpperCaseCharacter:
                        UIHelper.UpdateStatusLabel("Password must have upper case!", createNewUserResultLabel, ErrorLevel.Error);
                        break;

                    case AddUserResult.Successful:
                        UIHelper.UpdateStatusLabel("Success. Please log in.", createNewUserResultLabel, ErrorLevel.Ok);
                        createUsernameTextBox.Text = "";
                        createPasswordTextBox.Text = "";
                        createFirstNameTextBox.Text = "";
                        createLastNameTextBox.Text = "";
                        createPhoneNumberTextBox.Text = "";
                        createEmailTextBox.Text = "";

                        ghostNewUsername.Reset();
                        ghostNewPassword.Reset();
                        ghostNewFirstName.Reset();
                        ghostNewLastName.Reset();
                        ghostNewPhoneNumber.Reset();
                        ghostNewEmail.Reset();

                        break;

                    default:
                        createNewUserResultLabel.Text = "Unsuccessful.";
                        createNewUserResultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
                        break;
                }

                createNewUserResultLabel.Visible = true;
            }));
        }

        /*************************************************************************************************/
        public void DisplayPasswordComplexity(PasswordComplexityLevel complexity)
        {
            switch (complexity)
            {
                case PasswordComplexityLevel.Weak:
                    createPasswordTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
                    break;

                case PasswordComplexityLevel.Mediocre:
                    createPasswordTextBox.ForeColor = Color.Orange;
                    break;

                case PasswordComplexityLevel.Ok:
                    createPasswordTextBox.ForeColor = Color.YellowGreen;
                    break;

                case PasswordComplexityLevel.Great:
                    createPasswordTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusGreenColor);
                    break;

                default:
                    createPasswordTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayPasswordRequirements(int passwordLength)
        {
            string message = $"Password Requirements:\r\n" +
                             $"Contains {passwordLength} characters\r\n" +
                             $"Contains 1 Uppercase\r\n" +
                             $"Contains 1 Lowercase\r\n" +
                             $"Contains 1 Digit\r\n" +
                             $"Contains 1 symbol (!@#$%^&*()_+=[]{{}};:<>|./?,-)";

            toolTip.InitialDelay = 0;
            toolTip.Show(string.Empty, createPasswordTextBox);
            toolTip.Show(message, createPasswordTextBox, 10000);
        }

        /*************************************************************************************************/
        public void ShowLoginMenu()
        {
            this.Show();
            loginUsernameTextBox.Focus();
            loginUsernameTextBox.SelectionLength = 0;
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void LoginButton_Click(object sender, EventArgs e)
        {
            loginResultLabel.Visible = true;
            UIHelper.UpdateStatusLabel("Authenticating..", loginResultLabel, ErrorLevel.Neutral);
            RaiseLoginEvent(loginUsernameTextBox.Text, loginPasswordTextBox.Text);
        }

        /*************************************************************************************************/
        private void RaiseLoginEvent(string username, string password)
        {
            LoginEvent?.Invoke(username, password);
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
            RaiseCreateNewUserEvent(createUsernameTextBox.Text, createPasswordTextBox.Text, createFirstNameTextBox.Text, createLastNameTextBox.Text, createPhoneNumberTextBox.Text, createEmailTextBox.Text);
        }

        /*************************************************************************************************/
        private void RaiseCreateNewUserEvent(string user, string password, string firstName, string lastName, string phoneNumber, string email)
        {
            if (CreateNewUserEvent != null)
            {
                CreateNewUserEvent(user, password, firstName, lastName, phoneNumber, email);
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
            DisableCreateUserForm();
            ClearLoginView();
            this.Close();
            DialogResult = DialogResult.Cancel;
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
        private void LoginView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        /*************************************************************************************************/
        private void ClearLoginView()
        {
            this.BeginInvoke((Action)(() => loginResultLabel.Visible = false));
            this.BeginInvoke((Action)(() => loginResultLabel.Text = ""));
            this.BeginInvoke((Action)(() => loginUsernameTextBox.Text = ""));
            this.BeginInvoke((Action)(() => loginPasswordTextBox.Text = ""));
            this.BeginInvoke((Action)(() => createNewUserResultLabel.Visible = false));
            this.BeginInvoke((Action)(() => createNewUserResultLabel.Text = ""));
            this.BeginInvoke((Action)(() => createUsernameTextBox.Text = ""));
            this.BeginInvoke((Action)(() => createPasswordTextBox.Text = ""));
            this.BeginInvoke((Action)(() => createPhoneNumberTextBox.Text = ""));
            this.BeginInvoke((Action)(() => createEmailTextBox.Text = ""));

            this.BeginInvoke((Action)(() => ghostLoginUsername.Reset()));
            this.BeginInvoke((Action)(() => ghostLoginPassword.Reset()));
            this.BeginInvoke((Action)(() => ghostNewUsername.Reset()));
            this.BeginInvoke((Action)(() => ghostNewPassword.Reset()));
            this.BeginInvoke((Action)(() => ghostNewFirstName.Reset()));
            this.BeginInvoke((Action)(() => ghostNewLastName.Reset()));
            this.BeginInvoke((Action)(() => ghostNewPhoneNumber.Reset()));
            this.BeginInvoke((Action)(() => ghostNewEmail.Reset()));
            this.BeginInvoke((Action)(() => loginUsernameTextBox.Focus()));
        }

        /*************************************************************************************************/
        private void createPhoneNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            createPhoneNumberTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void createPhoneNumberTextBox_Leave(object sender, EventArgs e)
        {

        }

        /*************************************************************************************************/
        private void createEmailTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        /*************************************************************************************************/
        private void createEmailTextBox_Leave(object sender, EventArgs e)
        {

        }

        /*************************************************************************************************/
        private void createPhoneNumberTextBox_Enter(object sender, EventArgs e)
        {

        }

        /*************************************************************************************************/
        private void createEmailTextBox_Enter(object sender, EventArgs e)
        {

        }

        private void newUserButton_Click(object sender, EventArgs e)
        {
            EnableCreateUserForm();       
        }

        private void createPasswordTextBox_MouseEnter(object sender, EventArgs e)
        {
            //RaiseShowPasswordRequirements();
        }

        private void createPasswordTextBox_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(createPasswordTextBox);
        }

        private void createPasswordTextBox_Enter(object sender, EventArgs e)
        {
            RaiseShowPasswordRequirements();            
        }

        private void createPasswordTextBox_Leave(object sender, EventArgs e)
        {
            toolTip.Hide(createPasswordTextBox);
        }

        void toolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            Font f = UIHelper.GetFont(8.0f);
            toolTip.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            toolTip.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            e.DrawBackground();
            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, f, Brushes.White, new PointF(2, 2));
            f.Dispose();
        }

        private void RaiseShowPasswordRequirements()
        {
            DisplayPasswordRequirementsEvent?.Invoke();
        }

        private void EnableCreateUserForm()
        {
            this.Size = LargeSize;
            moveWindowPanel.Size = new Size(516, 27);
            closeButton.Location = new Point(524, 9);
            createUsernameTextBox.Enabled = true;
            createPasswordTextBox.Enabled = true;
            createFirstNameTextBox.Enabled = true;
            createLastNameTextBox.Enabled = true;
            createEmailTextBox.Enabled = true;
            createPhoneNumberTextBox.Enabled = true;
            generatePasswordButton.Enabled = true;
            createLoginButton.Enabled = true;
        }

        private void DisableCreateUserForm()
        {
            this.Size = SmallSize;
            moveWindowPanel.Size = new Size(234, 27);
            closeButton.Location = new Point(254, 9);
            createUsernameTextBox.Enabled = false;
            createPasswordTextBox.Enabled = false;
            createFirstNameTextBox.Enabled = false;
            createLastNameTextBox.Enabled = false;
            createEmailTextBox.Enabled = false;
            createPhoneNumberTextBox.Enabled = false;
            generatePasswordButton.Enabled = false;
            createLoginButton.Enabled = false;
        }

        private void loginPasswordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                loginButton.PerformClick();
                e.Handled = true;
            }       
        }

        private void createEmailTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                createLoginButton.PerformClick();
                e.Handled = true;
            }
        }

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/


    } // LoginForm CLASS
} // LoginForm NAMESPACE
