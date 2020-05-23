using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Desktop.Winforms
{
    class AddPasswordPresenter
    {
        private IAddPasswordView _addPasswordView;
        private IDesktopServiceWrapper _serviceWrapper;
        private IMainView _mainView;

        public AddPasswordPresenter(IAddPasswordView addPasswordView, IMainView mainView, IDesktopServiceWrapper serviceWrapper)
        {
            if (addPasswordView == null)
            {
                throw new ArgumentNullException(nameof(addPasswordView));
            }

            if (serviceWrapper == null)
            {
                throw new ArgumentNullException(nameof(serviceWrapper));
            }

            if (mainView == null)
            {
                throw new ArgumentNullException(nameof(serviceWrapper));
            }

            _addPasswordView = addPasswordView;
            _serviceWrapper = serviceWrapper;
            _mainView = mainView;

            _addPasswordView.AddPasswordEvent += AddPassword;
            _addPasswordView.GenerateNewPasswordEvent += GeneratePassword;
            _addPasswordView.PasswordChangedEvent += CalculatePasswordComplexity;
            _addPasswordView.SubmitEditPasswordEvent += SubmitEditPassword;

            _mainView.EditPasswordEvent += EditPassword;
        }

        private void GeneratePassword()
        {
            string password = _serviceWrapper.GeneratePassword();
            _addPasswordView.DisplayGeneratePasswordResult(password);
        }

        private void CalculatePasswordComplexity(string password)
        {
            PasswordComplexityLevel passwordComplexityLevel = PasswordComplexityLevel.Weak;
            passwordComplexityLevel = PasswordComplexity.checkEffectiveBitSize(password.Length, password);
            _addPasswordView.DisplayPasswordComplexity(passwordComplexityLevel);
        }

        private void AddPassword(Password uiPassword)
        {
            ValidatePassword result = _serviceWrapper.AddPassword(uiPassword);

            _addPasswordView.DisplayAddPasswordResult(result);

            if (result == ValidatePassword.Success)
            {
                UpdatePasswordsUI();
                _mainView.DisplayPasswordCount(_serviceWrapper.GetPasswordCount());
            }          
        }

        private void UpdatePasswordsUI()
        {
            // Get encrypted passwords from the password service and modify to display in UI
            List<Password> passwords = _serviceWrapper.GetPasswords();
            int count = _serviceWrapper.GetPasswordCount();

            BindingList<Password> uiBindingList = new BindingList<Password>(passwords);
            _mainView.DisplayPasswords(uiBindingList);
            _mainView.DisplayPasswordCount(count);
        }

        private void EditPassword(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);

            if (result != null)
            {
                _addPasswordView.DisplayEditPassword(result);
            }
        }

        private void SubmitEditPassword(Password editPassword, Password originalPassword)
        {
            ValidatePassword result = _serviceWrapper.ModifyPassword(originalPassword, editPassword);

            _addPasswordView.DisplayEditPasswordResult(result);

            if (result == ValidatePassword.Success)
            {
                _mainView.DisplayAddEditPasswordResult(result);
            }
        }

        private static Password ConvertDgvRowToPassword(DataGridViewRow dgvrow)
        {
            Password p = new Password
            (
                dgvrow.Cells[0].Value?.ToString(),
                dgvrow.Cells[1].Value?.ToString(),
                dgvrow.Cells[2].Value?.ToString(),
                null,
                null,
                null,
                dgvrow.Cells[3].Value?.ToString()
            );

            return p;
        }

        private Password QueryForFirstPassword(Password password)
        {
            List<Password> passwords = _serviceWrapper.GetPasswords();

            Password result = (from Password queryPassword in passwords
                               where queryPassword.Application == password.Application
                               where queryPassword.Username == password.Username
                               where queryPassword.Email == password.Email
                               where queryPassword.Category == password.Category
                               select queryPassword).FirstOrDefault();

            return result;
        }
    }
}
