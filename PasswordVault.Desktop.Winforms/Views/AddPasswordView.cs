﻿using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using PasswordVault.Models;
using PasswordVault.Services;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Desktop.Winforms
{
    public partial class AddPasswordView : Form, IAddPasswordView
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
        public event Action<Password> AddPasswordEvent;
        public event Action<string> PasswordChangedEvent;
        public event Action GenerateNewPasswordEvent;

        /*PRIVATE*****************************************************************************************/
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to
        private GhostTextBoxHelper ghostApplicationTextBox;
        private GhostTextBoxHelper ghostUsernameTextBox;
        private GhostTextBoxHelper ghostEmailTextBox;
        private GhostTextBoxHelper ghostDescriptionTextBox;
        private GhostTextBoxHelper ghostWebsiteTextBox;
        private GhostTextBoxHelper ghostPasswordTextBox;
        string _applicationTextboxDefault = "Application";
        string _usernameTextboxDefault = "Username";
        string _emailTextboxDefault = "Email (optional)";
        string _descriptionTextboxDefault = "Description (optional)";
        string _websiteTextboxDefault = "Website (optional)";
        string _passwordTextboxDefault = "Password";
        string _categoryTextboxDefault = "Category (optional)";

        bool _editMode = false;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public AddPasswordView()
        {
            InitializeComponent();

            #region UI
            // Configure form UI
            BackColor = UIHelper.GetColorFromCode(UIColors.SecondaryFromBackgroundColor);
            FormBorderStyle = FormBorderStyle.None;

            // configure labels
            windowLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            windowLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            windowLabel.Visible = true;

            statusLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            statusLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            statusLabel.Visible = true;
            statusLabel.Text = "";

            // Configure buttons
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            closeButton.Font = UIHelper.GetFont(UIFontSizes.CloseButtonFontSize);

            generatePasswordButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            generatePasswordButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            generatePasswordButton.FlatStyle = FlatStyle.Flat;
            generatePasswordButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            generatePasswordButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            generatePasswordButton.FlatAppearance.BorderSize = 1;

            addButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            addButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            addButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            addButton.FlatAppearance.BorderSize = 1;

            cancelButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            cancelButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            cancelButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            cancelButton.FlatAppearance.BorderSize = 1;

            // Configure textbox
            applicationTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            applicationTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            applicationTextbox.BorderStyle = BorderStyle.None;
            applicationTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            applicationTextbox.AutoSize = false;
            applicationTextbox.Font = UIHelper.GetFont(9.0f);
            applicationTextbox.Size = new System.Drawing.Size(198, 22);
            ghostApplicationTextBox = new GhostTextBoxHelper(applicationTextbox, _applicationTextboxDefault);

            usernameTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            usernameTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            usernameTextbox.BorderStyle = BorderStyle.None;
            usernameTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            usernameTextbox.AutoSize = false;
            usernameTextbox.Font = UIHelper.GetFont(9.0f);
            usernameTextbox.Size = new System.Drawing.Size(198, 22);
            ghostUsernameTextBox = new GhostTextBoxHelper(usernameTextbox, _usernameTextboxDefault);

            emailTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            emailTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            emailTextbox.BorderStyle = BorderStyle.None;
            emailTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            emailTextbox.AutoSize = false;
            emailTextbox.Font = UIHelper.GetFont(9.0f);
            emailTextbox.Size = new System.Drawing.Size(198, 22);
            ghostEmailTextBox = new GhostTextBoxHelper(emailTextbox, _emailTextboxDefault);

            descriptionTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            descriptionTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            descriptionTextbox.BorderStyle = BorderStyle.None;
            descriptionTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            descriptionTextbox.AutoSize = false;
            descriptionTextbox.Font = UIHelper.GetFont(9.0f);
            descriptionTextbox.Size = new System.Drawing.Size(198, 73);
            ghostDescriptionTextBox = new GhostTextBoxHelper(descriptionTextbox, _descriptionTextboxDefault);

            websiteTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            websiteTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            websiteTextbox.BorderStyle = BorderStyle.None;
            websiteTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            websiteTextbox.AutoSize = false;
            websiteTextbox.Font = UIHelper.GetFont(9.0f);
            websiteTextbox.Size = new System.Drawing.Size(198, 22);
            ghostWebsiteTextBox = new GhostTextBoxHelper(websiteTextbox, _websiteTextboxDefault);

            passwordTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            passwordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            passwordTextbox.BorderStyle = BorderStyle.None;
            passwordTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            passwordTextbox.AutoSize = false;
            passwordTextbox.Font = UIHelper.GetFont(9.0f);
            passwordTextbox.Size = new System.Drawing.Size(198, 22);
            ghostPasswordTextBox = new GhostTextBoxHelper(passwordTextbox, _passwordTextboxDefault);

            // configure combobox
            categoryCombobox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            categoryCombobox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            categoryCombobox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            //categoryCombobox.DataSource = Enum.GetValues(typeof(PasswordFilterOption));
            categoryCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            categoryCombobox.HighlightColor = UIHelper.GetColorFromCode(UIColors.ControlHighLightColor);
            categoryCombobox.BorderColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);

            categoryCombobox.Items.Add(_categoryTextboxDefault);
            categoryCombobox.Items.Add("Banking");
            categoryCombobox.Items.Add("Game");
            categoryCombobox.Items.Add("Email");
            categoryCombobox.Items.Add("Social");
            categoryCombobox.Items.Add("Store");
            categoryCombobox.SelectedIndex = 0;
            categoryCombobox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);

            #endregion            
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void DisplayAddPasswordResult(ValidatePassword result)
        {
            switch (result)
            {
                case ValidatePassword.ApplicationError:
                    UIHelper.UpdateStatusLabel("Invalid application.", statusLabel, ErrorLevel.Error);
                    break;
                case ValidatePassword.UsernameError:
                    UIHelper.UpdateStatusLabel("Invalid username.", statusLabel, ErrorLevel.Error);
                    break;
                case ValidatePassword.EmailError:
                    UIHelper.UpdateStatusLabel("Invalid email.", statusLabel, ErrorLevel.Error);
                    break;
                case ValidatePassword.DescriptionError:
                    UIHelper.UpdateStatusLabel("Invalid description.", statusLabel, ErrorLevel.Error);
                    break;
                case ValidatePassword.WebsiteError:
                    UIHelper.UpdateStatusLabel("Invalid website.", statusLabel, ErrorLevel.Error);
                    break;
                case ValidatePassword.PassphraseError:
                    UIHelper.UpdateStatusLabel("Invalid password.", statusLabel, ErrorLevel.Error);
                    break;
                case ValidatePassword.DuplicatePassword:
                    UIHelper.UpdateStatusLabel("Duplicate password.", statusLabel, ErrorLevel.Error);
                    break;
                case ValidatePassword.Failed:
                    UIHelper.UpdateStatusLabel("Add password failed.", statusLabel, ErrorLevel.Error);
                    break;
                case ValidatePassword.Success:
                    UIHelper.UpdateStatusLabel("Success.", statusLabel, ErrorLevel.Error);
                    this.CloseView();
                    break;
                default:
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayGeneratePasswordResult(string generatedPassword)
        {
            passwordTextbox.Text = generatedPassword;
        }

        /*************************************************************************************************/
        public void DisplayPasswordComplexity(PasswordComplexityLevel complexity)
        {
            switch (complexity)
            {
                case PasswordComplexityLevel.Weak:
                    passwordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
                    break;

                case PasswordComplexityLevel.Mediocre:
                    passwordTextbox.ForeColor = Color.Orange;
                    break;

                case PasswordComplexityLevel.Ok:
                    passwordTextbox.ForeColor = Color.YellowGreen;
                    break;

                case PasswordComplexityLevel.Great:
                    passwordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusGreenColor);
                    break;

                default:
                    passwordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayPassword(Password password)
        {
            if (password != null)
            {
                applicationTextbox.Text = password.Application;
                usernameTextbox.Text = password.Username;
                emailTextbox.Text = password.Email;
                websiteTextbox.Text = password.Website;
                categoryCombobox.Text = password.Category;
                descriptionTextbox.Text = descriptionTextbox.Text;
                passwordTextbox.Text = password.Passphrase;
            }         
        }

        /*************************************************************************************************/
        public void ShowMenu()
        {
            this.Show();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        /*************************************************************************************************/

        /*************************************************************************************************/

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
            this.CloseView();   
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
        private void ClearView()
        {

        }

        private void CloseView()
        {
            ClearView();
            this.Close();
            DialogResult = DialogResult.Cancel;
        }

        /*************************************************************************************************/
        private void passwordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // submit password
            {         
                e.Handled = true;
                e.SuppressKeyPress = true;
                AddPassword();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddPassword();
        }

        private void AddPassword()
        {
            string email = "";
            string description = "";
            string website = "";
            string category = "";

            // if required fields are still default, set to red and return
            if (applicationTextbox.Text == _applicationTextboxDefault)
            {
                applicationTextbox.ForeColor = Color.Red;
                return;
            }

            if (usernameTextbox.Text == _usernameTextboxDefault)
            {
                usernameTextbox.ForeColor = Color.Red;
                return;
            }

            if (passwordTextbox.Text == _passwordTextboxDefault)
            {
                passwordTextbox.ForeColor = Color.Red;
                return;
            }

            // Set optional fields to "" if still default values when submitted.
            if (emailTextbox.Text != _emailTextboxDefault)
            {
                email = emailTextbox.Text;
            }

            if (descriptionTextbox.Text != _descriptionTextboxDefault)
            {
                description = descriptionTextbox.Text;
            }

            if (websiteTextbox.Text != _websiteTextboxDefault)
            {
                website = websiteTextbox.Text;
            }

            if (categoryCombobox.Text != _categoryTextboxDefault)
            {
                category = categoryCombobox.Text;
            }

            Password password = new Password(
                applicationTextbox.Text,
                usernameTextbox.Text,
                email,
                description,
                website,
                passwordTextbox.Text,
                category
            );
            AddPasswordEvent?.Invoke(password);
        }

        private void categoryCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryCombobox.SelectedIndex == 0)
            {
                categoryCombobox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
            }
            else
            {
                categoryCombobox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            }
        }

        private void generatePasswordButton_Click(object sender, EventArgs e)
        {
            GenerateNewPasswordEvent?.Invoke();
        }

        private void passwordTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordTextbox.Text) && passwordTextbox.Text != _passwordTextboxDefault)
            {
                PasswordChangedEvent?.Invoke(passwordTextbox.Text);
            }       
        }



        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/


    } // LoginForm CLASS
} // LoginForm NAMESPACE
