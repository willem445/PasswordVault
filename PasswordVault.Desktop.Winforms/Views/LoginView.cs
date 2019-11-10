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
        public event Action DisplayPasswordRequirementsEvent;

        /*PRIVATE*****************************************************************************************/
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to
        private ToolTip toolTip;
        private Size SmallSize = new Size(279, 258);
        private Size LargeSize = new Size(549, 400);

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
            toolTip = new ToolTip();
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
            loginUsernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            loginUsernameTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            loginPasswordTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            loginPasswordTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            loginPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            loginPasswordTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            createUsernameTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createUsernameTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createUsernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            createUsernameTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            createPasswordTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createPasswordTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            createPasswordTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            createFirstNameTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createFirstNameTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createFirstNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            createFirstNameTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            createLastNameTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createLastNameTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createLastNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            createLastNameTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            createEmailTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createEmailTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createEmailTextBox.BorderStyle = BorderStyle.FixedSingle;
            createEmailTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            createEmailTextBox.Text = "example@provider.com";
            createEmailTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);

            createPhoneNumberTextBox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            createPhoneNumberTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            createPhoneNumberTextBox.BorderStyle = BorderStyle.FixedSingle;
            createPhoneNumberTextBox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            createPhoneNumberTextBox.Text = "xxx-xxx-xxxx";
            createPhoneNumberTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);

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

            label7.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label7.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            label8.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label8.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

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
            switch(result)
            {
                case AuthenticateResult.PasswordIncorrect:
                    loginResultLabel.Visible = true;
                    loginResultLabel.Text = "Password incorrect.";
                    break;

                case AuthenticateResult.UsernameDoesNotExist:
                    loginResultLabel.Visible = true;
                    loginResultLabel.Text = "Username doesn't exist.";
                    break;

                case AuthenticateResult.Failed:
                    loginResultLabel.Visible = true;
                    loginResultLabel.Text = "Login failed.";
                    break;

                case AuthenticateResult.Successful:
                    ClearLoginView();
                    DialogResult = DialogResult.OK;
                    DisableCreateUserForm();
                    this.Close();
                    RaiseLoginSuccessfulEvent();
                    break;

                default:
                    loginResultLabel.Visible = true;
                    loginResultLabel.Text = "Login failed.";
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayGeneratePasswordResult(string generatedPassword)
        {
            createPasswordTextBox.Text = generatedPassword;
        }

        /*************************************************************************************************/
        public void DisplayCreateNewUserResult(AddUserResult result, int minimumPasswordLength)
        {
            switch(result)
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
                    createEmailTextBox.Text = "";
                    createPhoneNumberTextBox.Text = "";
                    break;

                default:       
                    createNewUserResultLabel.Text = "Unsuccessful.";
                    createNewUserResultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
                    break;
            }

            createNewUserResultLabel.Visible = true;
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
            toolTip.Show(message, createPasswordTextBox, 0);
        }

        /*************************************************************************************************/
        public void ShowLoginMenu()
        {
            this.Show();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void LoginButton_Click(object sender, EventArgs e)
        {
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
            loginResultLabel.Visible = false;
            loginResultLabel.Text = "";
            loginUsernameTextBox.Text = "";
            loginPasswordTextBox.Text = "";
            createNewUserResultLabel.Visible = false;
            createNewUserResultLabel.Text = "";
            createUsernameTextBox.Text = "";
            createPasswordTextBox.Text = "";
        }

        /*************************************************************************************************/
        private void createPhoneNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            createPhoneNumberTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void createPhoneNumberTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(createPhoneNumberTextBox.Text))
            {
                createPhoneNumberTextBox.Text = "xxx-xxx-xxxx";
                createPhoneNumberTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
            }
        }

        /*************************************************************************************************/
        private void createEmailTextBox_TextChanged(object sender, EventArgs e)
        {
            createEmailTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void createEmailTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(createEmailTextBox.Text))
            {
                createEmailTextBox.Text = "example@provider.com";
                createEmailTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
            }
        }

        /*************************************************************************************************/
        private void createPhoneNumberTextBox_Enter(object sender, EventArgs e)
        {
            if (createPhoneNumberTextBox.Text == "xxx-xxx-xxxx")
            {
                createPhoneNumberTextBox.Text = "";
                createPhoneNumberTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            }
        }

        /*************************************************************************************************/
        private void createEmailTextBox_Enter(object sender, EventArgs e)
        {
            if (createEmailTextBox.Text == "example@provider.com")
            {
                createEmailTextBox.Text = "";
                createEmailTextBox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            }
        }

        private void newUserButton_Click(object sender, EventArgs e)
        {
            EnableCreateUserForm();       
        }

        private void createPasswordTextBox_MouseEnter(object sender, EventArgs e)
        {
            RaiseShowPasswordRequirements();
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

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/


        } // LoginForm CLASS
} // LoginForm NAMESPACE
