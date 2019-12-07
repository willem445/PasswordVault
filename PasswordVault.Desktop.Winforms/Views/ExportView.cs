using PasswordVault.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PasswordVault.Desktop.Winforms
{
    public partial class ExportView : Form, IExportView
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
        public event Action<ExportFileTypes, string, string> ExportPasswordsEvent;
        public event Action InitializeEvent;

        /*PRIVATE*****************************************************************************************/
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to
        List<SupportedFileTypes> _fileTypes;
        private List<string> _fileFilters;
        private string _fileName = "";

        /*=================================================================================================
        PROPERTIES
        *================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public ExportView()
        {
            InitializeComponent();

            BackColor = UIHelper.GetColorFromCode(UIColors.SecondaryFromBackgroundColor);
            FormBorderStyle = FormBorderStyle.None;

            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            closeButton.Font = UIHelper.GetFont(UIFontSizes.CloseButtonFontSize);

            windowTitleLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            windowTitleLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            exportButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            exportButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            exportButton.FlatStyle = FlatStyle.Flat;
            exportButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            exportButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            exportButton.FlatAppearance.BorderSize = 1;

            browseFoldersButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            browseFoldersButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            browseFoldersButton.FlatStyle = FlatStyle.Flat;
            browseFoldersButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            browseFoldersButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            browseFoldersButton.FlatAppearance.BorderSize = 1;

            filePathTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            filePathTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            filePathTextbox.BorderStyle = BorderStyle.FixedSingle;
            filePathTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);

            exportPasswordTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            exportPasswordTextbox.BorderStyle = BorderStyle.FixedSingle;
            exportPasswordTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            exportPasswordTextbox.Text = "Enter encryption password";
            exportPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
            exportPasswordTextbox.Enabled = false;

            encryptionEnabledCheckbox.BackColor = UIHelper.GetColorFromCode(UIColors.SecondaryFromBackgroundColor);
            encryptionEnabledCheckbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            encryptionEnabledCheckbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void DisplayExportResult(ExportResult result)
        {
            throw new NotImplementedException();
        }

        /*************************************************************************************************/
        public void DisplayFileTypes(List<SupportedFileTypes> fileTypes)
        {
            _fileTypes = fileTypes;
            _fileFilters = new List<string>();
            _fileFilters = _fileTypes.Select(x => x.Filter).ToList();
        }

        /*************************************************************************************************/
        public void ShowExportView()
        {
            InitializeEvent?.Invoke();
            this.Show();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void ResetForm()
        {
            _fileName = "";
            filePathTextbox.Text = "";
        }

        /*************************************************************************************************/
        private void CloseButton_Click(object sender, EventArgs e)
        {
            ResetForm();
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
        private void WindowTitle_MouseDown(object sender, MouseEventArgs e)
        {
            _draggingWindow = true;
            _start_point = new Point(e.X, e.Y);
        }

        /*************************************************************************************************/
        private void WindowTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingWindow)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

        /*************************************************************************************************/
        private void WindowTitle_MouseUp(object sender, MouseEventArgs e)
        {
            _draggingWindow = false;
        }

        private void browseFoldersButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string filter = "";
            foreach (var fileFilter in _fileFilters)
            {
                filter += fileFilter + "|";
            }
            filter = filter.TrimEnd('|');


            sfd.Filter = filter;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                _fileName = sfd.FileName;
                filePathTextbox.Text = _fileName;
            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            // TODO - data validation
            ExportFileTypes fileType = _fileTypes.Where(x => x.Filter.Contains(_fileName.Split('.')[1])).FirstOrDefault().FileType;
            ExportPasswordsEvent?.Invoke(fileType, _fileName, exportPasswordTextbox.Text);
        }

        private void ExportView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        private void encryptionEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (encryptionEnabledCheckbox.Checked)
            {
                exportPasswordTextbox.Enabled = true;
            }
            else
            {
                exportPasswordTextbox.Enabled = false;
            }
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
    } // AboutView CLASS
} // AboutView NAMESPACE