using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PasswordVault.Services;

namespace PasswordVault.Desktop.Winforms
{
    public partial class ConfirmDeleteUserView : Form, IConfirmDeleteUserView
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

        /*PRIVATE*****************************************************************************************/
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to

        public event Action<string> ConfirmPasswordEvent; 
        public event Action DeleteAccountEvent;
        public event Action FormClosingEvent;
        public event Action ConfirmPasswordSuccessEvent; 
        public event Action DeleteSuccessEvent; 

        /*=================================================================================================
        PROPERTIES
        *================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public ConfirmDeleteUserView()
        {
            InitializeComponent();

            #region UI
            BackColor = UIHelper.GetColorFromCode(UIColors.SecondaryFromBackgroundColor);
            FormBorderStyle = FormBorderStyle.None;

            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            closeButton.Font = UIHelper.GetFont(UIFontSizes.CloseButtonFontSize);

            panel1.BackColor = UIHelper.GetColorFromCode(0xffb3b3);
            panel1.ForeColor = UIHelper.GetColorFromCode(0xffb3b3);

            warningLabel.ForeColor = UIHelper.GetColorFromCode(0xff1a1a);
            warningLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);

            label1.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            label1.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            label1.Text = "This action cannot be undone. Your account and all associated passwords will be permanently deleted.";

            label2.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            label2.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);

            label3.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            label3.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);

            resultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            resultLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);

            cancelButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            cancelButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            cancelButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            cancelButton.FlatAppearance.BorderSize = 1;

            deleteAccountButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            deleteAccountButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            deleteAccountButton.FlatStyle = FlatStyle.Flat;
            deleteAccountButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            deleteAccountButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            deleteAccountButton.FlatAppearance.BorderSize = 1;

            confirmPasswordTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            confirmPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            confirmPasswordTextbox.BorderStyle = BorderStyle.FixedSingle;
            confirmPasswordTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            #endregion

        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void DisplayDeleteAccountResult(DeleteUserResult result)
        {
            switch(result)
            {
                case DeleteUserResult.Failed:
                    resultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
                    resultLabel.Text = "Failed!";
                    break;

                case DeleteUserResult.Success:
                    resultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusGreenColor);
                    resultLabel.Text = "Success.";
                    DeleteSuccessEvent?.Invoke(); // Notify others that confirm password was successful   
                    this.Close();
                    break;

                default:
                    resultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
                    resultLabel.Text = "Failed!";
                    break;
            }
        }

        public void DisplayConfirmPasswordResult(bool result)
        {
            if (result == true)
            {
                ConfirmPasswordSuccessEvent?.Invoke(); // Notify others that authentication was successful
                DeleteAccountEvent?.Invoke(); // If authentication was successful, delete the account
            }
            else
            {
                resultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.StatusRedColor);
                resultLabel.Text = "Incorrect password!";
            }
        }

        public void ShowView()
        {
            this.Show();
        }

        public void CloseView()
        {
            confirmPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            confirmPasswordTextbox.Text = "";
            resultLabel.Text = "";
            resultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            FormClosingEvent?.Invoke();
            this.Hide();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*************************************************************************************************/
        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.BackColor = Color.FromArgb(107, 20, 3);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void MoveWindowPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _draggingWindow = true;
            _start_point = new Point(e.X, e.Y);
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
        private void MoveWindowPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _draggingWindow = false;
        }

        /*************************************************************************************************/
        private void cancelButton_Click(object sender, EventArgs e)
        {
            confirmPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            confirmPasswordTextbox.Text = "";
            this.Close();
        }

        /*************************************************************************************************/
        private void deleteAccountButton_Click(object sender, EventArgs e)
        {
            ConfirmPasswordEvent?.Invoke(confirmPasswordTextbox.Text);
        }

        /*************************************************************************************************/
        private void ConfirmDeleteUserView_FormClosing(object sender, FormClosingEventArgs e)
        {
            confirmPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            confirmPasswordTextbox.Text = "";
            resultLabel.Text = "";
            resultLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            FormClosingEvent?.Invoke();
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
    } // AboutView CLASS
} // AboutView NAMESPACE