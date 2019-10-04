using System;
using System.Drawing;
using System.Windows.Forms;
using PasswordVault.Services;
using PasswordVault.Models;

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

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    public partial class EditUserView : Form, IEditUserView
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
        public event Action<string, string, string, string> ModifyAccountEvent;
        public event Action RequestUserEvent;

        /*PRIVATE*****************************************************************************************/
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
        public EditUserView()
        {
            InitializeComponent();

            // Configure form UI
            BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            FormBorderStyle = FormBorderStyle.None;

            // Configure labels
            statusLabel.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            statusLabel.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            statusLabel.Text = "";

            label2.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            label2.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            label3.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            label3.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            label4.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            label4.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            label5.Font = UIHelper.GetFont(UIHelper.UIFontSizes.DefaultFontSize);
            label5.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);

            // Configure buttons
            modifyButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            modifyButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            modifyButton.FlatStyle = FlatStyle.Flat;
            modifyButton.Font = UIHelper.GetFont(UIHelper.UIFontSizes.ButtonFontSize);
            modifyButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultBackgroundColor);
            modifyButton.FlatAppearance.BorderSize = 1;
            modifyButton.Enabled = false;

            closeButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            closeButton.Font = UIHelper.GetFont(UIHelper.UIFontSizes.CloseButtonFontSize);

            // Configure textbox
            firstNameTextbox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            firstNameTextbox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            firstNameTextbox.BorderStyle = BorderStyle.FixedSingle;
            firstNameTextbox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            lastNameTextbox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            lastNameTextbox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            lastNameTextbox.BorderStyle = BorderStyle.FixedSingle;
            lastNameTextbox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            emailTextBox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            emailTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            emailTextBox.BorderStyle = BorderStyle.FixedSingle;
            emailTextBox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);

            phoneNumberTextbox.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            phoneNumberTextbox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            phoneNumberTextbox.BorderStyle = BorderStyle.FixedSingle;
            phoneNumberTextbox.Font = UIHelper.GetFont(UIHelper.UIFontSizes.TextBoxFontSize);
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void ShowEditUserMenu()
        {
            this.Show();
            RaiseRequestUserEvent();
        }

        /*************************************************************************************************/
        public void DisplayModifyResult(UserInformationResult result)
        {
            switch(result)
            {
                case UserInformationResult.Failed:
                    UIHelper.UpdateStatusLabel("Failed to modify information!", statusLabel, ErrorLevel.Error);
                    break;

                case UserInformationResult.InvalidEmail:
                    UIHelper.UpdateStatusLabel("Invalid email!", statusLabel, ErrorLevel.Error);
                    break;

                case UserInformationResult.InvalidFirstName:
                    UIHelper.UpdateStatusLabel("Invalid first name!", statusLabel, ErrorLevel.Error);
                    break;

                case UserInformationResult.InvalidLastName:
                    UIHelper.UpdateStatusLabel("Invalid last name!", statusLabel, ErrorLevel.Error);
                    break;

                case UserInformationResult.InvalidPhoneNumber:
                    UIHelper.UpdateStatusLabel("Invalid phone number!", statusLabel, ErrorLevel.Error);
                    break;

                case UserInformationResult.Success:
                    UIHelper.UpdateStatusLabel("Success", statusLabel, ErrorLevel.Ok);
                    ClearChangePasswordView();
                    this.Close();
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayUser(User user)
        {
            firstNameTextbox.Text = user.FirstName;
            lastNameTextbox.Text = user.LastName;
            emailTextBox.Text = user.Email;
            phoneNumberTextbox.Text = user.PhoneNumber;
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void ClearChangePasswordView()
        {
            modifyButton.Enabled = false;
            firstNameTextbox.Text = "";
            lastNameTextbox.Text = "";
            emailTextBox.Text = "";
            phoneNumberTextbox.Text = "";
            statusLabel.Text = "";
        }
      
        /*************************************************************************************************/
        private void EditUserView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        /*************************************************************************************************/
        private void moveWindowPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _draggingWindow = true;
            _start_point = new Point(e.X, e.Y);
        }

        /*************************************************************************************************/
        private void moveWindowPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingWindow)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

        /*************************************************************************************************/
        private void moveWindowPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _draggingWindow = false;
        }

        /*************************************************************************************************/
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.Cancel;
        }

        /*************************************************************************************************/
        private void closeButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.CloseButtonColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void closeButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.BackColor = UIHelper.GetColorFromCode(UIHelper.UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            modifyButton.Enabled = true;

            if (((TextBox)sender).Name == phoneNumberTextbox.Name)
            {
                phoneNumberTextbox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            }
            else if (((TextBox)sender).Name == emailTextBox.Name)
            {
                emailTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            }
        }

        /*************************************************************************************************/
        private void modifyButton_Click(object sender, EventArgs e)
        {
            RaiseModifyAccountEvent(firstNameTextbox.Text, lastNameTextbox.Text, emailTextBox.Text, phoneNumberTextbox.Text);
        }

        /*************************************************************************************************/
        private void RaiseModifyAccountEvent(string firstName, string lastName, string email, string phoneNumber)
        {
            ModifyAccountEvent?.Invoke(firstName, lastName, email, phoneNumber);
        }

        /*************************************************************************************************/
        private void RaiseRequestUserEvent()
        {
            RequestUserEvent?.Invoke();
        }

        /*************************************************************************************************/
        private void phoneNumberTextbox_Leave(object sender, EventArgs e)
        {
            if (phoneNumberTextbox.Text == "")
            {
                phoneNumberTextbox.Text = "xxx-xxx-xxxx";
                phoneNumberTextbox.ForeColor = Color.FromArgb(0x6B, 0x6B, 0x6B);
            }
        }

        /*************************************************************************************************/
        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            if (emailTextBox.Text == "")
            {
                emailTextBox.Text = "example@provider.com";
                emailTextBox.ForeColor = Color.FromArgb(0x6B, 0x6B, 0x6B);
            }
        }

        /*************************************************************************************************/
        private void phoneNumberTextbox_Enter(object sender, EventArgs e)
        {
            if (phoneNumberTextbox.Text == "xxx-xxx-xxxx")
            {
                phoneNumberTextbox.Text = "";
                phoneNumberTextbox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            }
        }

        /*************************************************************************************************/
        private void emailTextBox_Enter(object sender, EventArgs e)
        {
            if (emailTextBox.Text == "example@provider.com")
            {
                emailTextBox.Text = "";
                emailTextBox.ForeColor = UIHelper.GetColorFromCode(UIHelper.UIColors.DefaultFontColor);
            }
        }

        /*=================================================================================================
        STATIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/

    } // EditUserView CLASS
} // PasswordVault NAMESPACE
