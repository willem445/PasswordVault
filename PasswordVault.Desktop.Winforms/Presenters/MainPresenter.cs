using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    class MainPresenter
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
        private IMainView _mainView;
        private IDesktopServiceWrapper _serviceWrapper;
        private AppSettings _settings = AppSettings.Instance;
        private Password _editPassword;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public MainPresenter(IMainView mainView, IDesktopServiceWrapper serviceWrapper)
        {
            _mainView = mainView;
            _serviceWrapper = serviceWrapper;

            _mainView.FilterChangedEvent += FilterChanged;
            _mainView.RequestPasswordsEvent += UpdatePasswordsUI;
            _mainView.RequestPasswordsOnLoginEvent += UpdatePasswordsUI;
            _mainView.RequestPasswordsOnLoginEvent += UpdateUsernameWelcomeUI;
            _mainView.AddPasswordEvent += AddPassword;
            _mainView.DeletePasswordEvent += DeletePassword;
            _mainView.LogoutEvent += Logout;
            _mainView.CopyPasswordEvent += CopyPassword;
            _mainView.CopyUserNameEvent += CopyUsername;
            _mainView.NavigateToWebsiteEvent += NavigateToWebsite;
            _mainView.ShowPasswordEvent += ViewPassword;

            _mainView.SetTimeoutTime(_settings.DefaultTimeoutMinutes);
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*************************************************************************************************/
        private void FilterChanged(string filterText, PasswordFilterOption passwordFilterOption)
        {
            List<Password> result = new List<Password>();
            List<Password> passwords = _serviceWrapper.GetPasswords();

            if (passwords != null)
            {
                if (!string.IsNullOrEmpty(filterText))
                {
                    switch (passwordFilterOption)
                    {
                        case PasswordFilterOption.Application:
                            result = (from Password password in passwords
                                      where password.Application?.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0
                                      select password).ToList<Password>();
                            break;

                        case PasswordFilterOption.Description:
                            result = (from Password password in passwords
                                      where password.Description?.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0
                                      select password).ToList<Password>();
                            break;

                        case PasswordFilterOption.Website:
                            result = (from Password password in passwords
                                      where password.Website?.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0
                                      select password).ToList<Password>();
                            break;
                    }

                    BindingList<Password> uiBindingList = new BindingList<Password>(result);
                    _mainView.DisplayPasswords(uiBindingList);
                }
                else
                {
                    BindingList<Password> uiBindingList = new BindingList<Password>(passwords);
                    _mainView.DisplayPasswords(uiBindingList);
                }
            }       
        }

        /*************************************************************************************************/
        private void UpdatePasswordsUI()
        {
            // Get encrypted passwords from the password service and modify to display in UI
            List<Password> passwords = _serviceWrapper.GetPasswords();
            int count = _serviceWrapper.GetPasswordCount();

            BindingList<Password> uiBindingList = new BindingList<Password>(passwords);
            _mainView.DisplayPasswords(uiBindingList);
            _mainView.DisplayPasswordCount(count);
        }

        /*************************************************************************************************/
        private void UpdateUsernameWelcomeUI()
        {
            _mainView.DisplayUserID(_serviceWrapper.GetCurrentUsername());
            _mainView.RequestPasswordsOnLoginEvent -= UpdateUsernameWelcomeUI;
        }

        /*************************************************************************************************/
        private void AddPassword(string application, string username, string email, string description, string website, string category, string passphrase)
        {
            Password uiPassword = new Password(application, username, email, description, website, passphrase, category);
            ValidatePassword result = _serviceWrapper.AddPassword(uiPassword);

            if (result == ValidatePassword.Success)
            {
                UpdatePasswordsUI();
            }

            _mainView.DisplayAddPasswordResult(result);
            _mainView.DisplayPasswordCount(_serviceWrapper.GetPasswordCount());
        }

        /*************************************************************************************************/
        private void DeletePassword(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);

            if (result != null)
            {
                DeletePasswordResult deleteResult = _serviceWrapper.DeletePassword(result);
                UpdatePasswordsUI();
                _mainView.DisplayDeletePasswordResult(deleteResult);
                _mainView.DisplayPasswordCount(_serviceWrapper.GetPasswordCount());
            }        
        }

        /*************************************************************************************************/
        private void Logout()
        {
            LogoutUser();
        }

        /*************************************************************************************************/
        private LogOutResult LogoutUser()
        {
            LogOutResult result = _serviceWrapper.Logout();

            if (result == LogOutResult.Success)
            {
                _mainView.RequestPasswordsOnLoginEvent += UpdateUsernameWelcomeUI;
            }

            _mainView.DisplayLogOutResult(result);

            return result;
        }

        /*************************************************************************************************/
        private void CopyPassword(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);

            if (result != null)
            {
                string passphrase = result.Passphrase;

                if (!string.IsNullOrEmpty(passphrase))
                {
                    System.Windows.Forms.Clipboard.SetText(passphrase);
                }
            }                    
        }

        /*************************************************************************************************/
        private void ViewPassword(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);
            string passphrase = "";

            if (result != null)
            {
                passphrase = result.Passphrase;
            }
            
            _mainView.DisplayPassword(passphrase);
        }

        /*************************************************************************************************/
        private void NavigateToWebsite(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);

            if (result != null)
            {
                string uri = result.Website;

                if (UriUtilities.IsValidUri(uri))
                {
                    UriUtilities.OpenUri(uri);
                }
            }        
        }

        /*************************************************************************************************/
        private void CopyUsername(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);

            if (result != null && !string.IsNullOrEmpty(result.Username))
            {
                System.Windows.Forms.Clipboard.SetText(result.Username);
            }       
        }

        /*************************************************************************************************/
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

        /*************************************************************************************************/
        private int FindPasswordIndex(string application, string username, string email, string description, string website)
        {
            List<Password> passwords = _serviceWrapper.GetPasswords();

            int index = passwords.FindIndex(x => (x.Application == application) && (x.Username == username) &&  (x.Email == email) && (x.Description == description) && (x.Website == website));

            return index;
        }

        /*************************************************************************************************/
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

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // MainFormPresenter CLASS
} // PasswordVault NAMESPACE
