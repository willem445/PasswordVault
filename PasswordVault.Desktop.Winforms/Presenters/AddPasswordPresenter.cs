using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
