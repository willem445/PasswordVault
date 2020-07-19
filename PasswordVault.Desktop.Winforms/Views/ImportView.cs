using PasswordVault.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PasswordVault.Desktop.Winforms
{
    public partial class ImportView : Form, IImportView
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
        public event Action<ImportExportFileType, string, string> ImportPasswordsEvent;
        public event Action InitializeEvent;
        public event Action<string, string> DataValidationEvent;
        public event Action<ImportExportResult> ImportPasswordsDoneEvent;

        /*PRIVATE*****************************************************************************************/
        private bool _draggingWindow = false;         // Variable to track whether the form is being moved
        private Point _start_point = new Point(0, 0); // Varaible to track where the form should be moved to
        List<SupportedFileTypes> _fileTypes;
        private List<string> _fileFilters;

        /*=================================================================================================
        PROPERTIES
        *================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public ImportView()
        {
            InitializeComponent();

            BackColor = UIHelper.GetColorFromCode(UIColors.SecondaryFromBackgroundColor);
            FormBorderStyle = FormBorderStyle.None;

            closeButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            closeButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            closeButton.Font = UIHelper.GetFont(UIFontSizes.CloseButtonFontSize);

            windowTitleLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            windowTitleLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);

            statusLabel.Font = UIHelper.GetFont(UIFontSizes.DefaultFontSize);
            statusLabel.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            statusLabel.Text = "";

            importButton.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            importButton.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            importButton.FlatStyle = FlatStyle.Flat;
            importButton.Font = UIHelper.GetFont(UIFontSizes.ButtonFontSize);
            importButton.FlatAppearance.BorderColor = UIHelper.GetColorFromCode(UIColors.DefaultBackgroundColor);
            importButton.FlatAppearance.BorderSize = 1;

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

            importPasswordTextbox.BackColor = UIHelper.GetColorFromCode(UIColors.ControlBackgroundColor);
            importPasswordTextbox.BorderStyle = BorderStyle.FixedSingle;
            importPasswordTextbox.Font = UIHelper.GetFont(UIFontSizes.TextBoxFontSize);
            importPasswordTextbox.Text = "Enter password..";
            importPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
        }

        /*=================================================================================================
        PUBLIC METHODS
        *================================================================================================*/
        /*************************************************************************************************/
        public void DisplayImportResult(ImportExportResult result)
        {
            ImportPasswordsDoneEvent?.Invoke(result);

            switch (result)
            {
                case ImportExportResult.Success:
                    UIHelper.UpdateStatusLabel("Success.", statusLabel, ErrorLevel.Ok);
                    break;

                case ImportExportResult.PasswordInvalid:
                    UIHelper.UpdateStatusLabel("Invalid password!", statusLabel, ErrorLevel.Error);
                    break;

                case ImportExportResult.PasswordProtected:
                    UIHelper.UpdateStatusLabel("Must provide password!", statusLabel, ErrorLevel.Error);
                    break;

                case ImportExportResult.ProblemWithImportedPassword:
                    UIHelper.UpdateStatusLabel("Problem with imported password!", statusLabel, ErrorLevel.Error);
                    break;

                case ImportExportResult.InvalidPath:
                    UIHelper.UpdateStatusLabel("Invalid path!", statusLabel, ErrorLevel.Error);
                    break;

                case ImportExportResult.Fail:                                    
                default:
                    UIHelper.UpdateStatusLabel("Error occured during import.", statusLabel, ErrorLevel.Error);
                    break;
            }           
        }

        /*************************************************************************************************/
        public void DisplayValidationResult(ExportValidationResult result, ImportExportFileType fileType)
        {
            switch (result)
            {
                case ExportValidationResult.FileNotSupported:
                    UIHelper.UpdateStatusLabel("File type not supported.", statusLabel, ErrorLevel.Error);
                    break;

                case ExportValidationResult.PathDoesNotExist:
                    UIHelper.UpdateStatusLabel("Path does not exist.", statusLabel, ErrorLevel.Error);
                    break;

                case ExportValidationResult.InvalidPassword:
                    UIHelper.UpdateStatusLabel("Invalid encryption password.", statusLabel, ErrorLevel.Error);
                    break;

                case ExportValidationResult.Valid:
                    ImportPasswordsEvent?.Invoke(fileType, filePathTextbox.Text, importPasswordTextbox.Text);
                    break;              

                case ExportValidationResult.Invalid:
                default:
                    UIHelper.UpdateStatusLabel("Error occured.", statusLabel, ErrorLevel.Error);
                    break;
            }
        }

        /*************************************************************************************************/
        public void DisplayFileTypes(List<SupportedFileTypes> fileTypes)
        {
            _fileTypes = fileTypes;
            _fileFilters = new List<string>();
            _fileFilters = _fileTypes.Select(x => x.Filter).ToList();
        }

        /*************************************************************************************************/
        public void ShowImportView()
        {
            InitializeEvent?.Invoke();
            this.Show();
        }

        public void CloseView()
        {
            ResetForm();
            this.Hide();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void ResetForm()
        {
            statusLabel.Text = "";
            importPasswordTextbox.Text = "";
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

        /*************************************************************************************************/
        private void browseFoldersButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string filter = "";
            foreach (var fileFilter in _fileFilters)
            {
                filter += fileFilter + "|";
            }
            filter = filter.TrimEnd('|');


            ofd.Filter = filter;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePathTextbox.Text = ofd.FileName;
            }

            ofd.Dispose();
        }

        /*************************************************************************************************/
        private void importButton_Click(object sender, EventArgs e)
        {
            DataValidationEvent?.Invoke(filePathTextbox.Text, importPasswordTextbox.Text);
        }

        /*************************************************************************************************/
        private void ImportView_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetForm();
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        /*************************************************************************************************/
        private void importPasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            importPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
        }

        /*************************************************************************************************/
        private void importPasswordTextbox_Enter(object sender, EventArgs e)
        {
            if (importPasswordTextbox.Text == "Enter password..")
            {
                importPasswordTextbox.PasswordChar = '•';
                importPasswordTextbox.Text = "";
                importPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            }
        }

        /*************************************************************************************************/
        private void importPasswordTextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(importPasswordTextbox.Text))
            {
                importPasswordTextbox.PasswordChar = '\0';
                importPasswordTextbox.Text = "Enter password..";
                importPasswordTextbox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
            }
        }





        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
    } // AboutView CLASS
} // AboutView NAMESPACE