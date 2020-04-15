using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordVault.Desktop.Winforms
{
    class GhostTextBoxHelper
    {
        public string GhostText { get; set; }
        private TextBox _textbox { get; }
        private bool _passwordTextbox { get; }

        public GhostTextBoxHelper(TextBox textbox, string ghostText, bool passwordTextbox = false)
        {
            _passwordTextbox = passwordTextbox;
            GhostText = ghostText;
            _textbox = textbox;
            _textbox.Text = GhostText;
            _textbox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
            _textbox.PasswordChar = '\0';
            textbox.Enter += GhostTextbox_Enter;
            textbox.Leave += GhostTextbox_Leave;
        }

        private void GhostTextbox_Enter(object sender, EventArgs e)
        {
            if (_textbox.Text == GhostText)
            {
                if (_passwordTextbox)
                {
                    _textbox.PasswordChar = '•';
                }

                _textbox.Text = "";
                _textbox.ForeColor = UIHelper.GetColorFromCode(UIColors.DefaultFontColor);
            }
        }

        private void GhostTextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_textbox.Text))
            {
                if (_passwordTextbox)
                {
                    _textbox.PasswordChar = '\0';
                }

                _textbox.Text = GhostText;
                _textbox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
            }
        }

        public void Reset()
        {
            if (_passwordTextbox)
            {
                _textbox.PasswordChar = '\0';
            }

            _textbox.Text = GhostText;
            _textbox.ForeColor = UIHelper.GetColorFromCode(UIColors.GhostTextColor);
        }
    }
}
