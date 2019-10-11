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
        private IPasswordService _passwordService;

        private Password _editPassword;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public MainPresenter(IMainView mainView, IPasswordService passwordService)
        {
            _mainView = mainView;
            _passwordService = passwordService;

            _mainView.FilterChangedEvent += FilterChanged;
            _mainView.RequestPasswordsOnLoginEvent += UpdatePasswordsUI;
            _mainView.RequestPasswordsOnLoginEvent += UpdateUsernameWelcomeUI;
            _mainView.AddPasswordEvent += AddPassword;
            _mainView.DeletePasswordEvent += DeletePassword;
            _mainView.EditPasswordEvent += EditPasswordInit;
            _mainView.EditOkayEvent += EditPasswordExecute;
            _mainView.LogoutEvent += Logout;
            _mainView.CopyPasswordEvent += CopyPassword;
            _mainView.CopyUserNameEvent += CopyUsername;
            _mainView.NavigateToWebsiteEvent += NavigateToWebsite;
            _mainView.ShowPasswordEvent += ViewPassword;
            _mainView.DeleteAccountEvent += DeleteAccount;
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
        private void DeleteAccount()
        {
            User user = _passwordService.GetCurrentUser();
            LogOutResult result = LogoutUser();

            if (result == LogOutResult.Success)
            {
                _passwordService.DeleteUser(user);
            }         
        }

        /*************************************************************************************************/
        private void FilterChanged(string filterText, PasswordFilterOption passwordFilterOption)
        {
            List<Password> result = new List<Password>();
            List<Password> passwords = _passwordService.GetPasswords();

            if (!string.IsNullOrEmpty(filterText))
            {
                switch (passwordFilterOption)
                {
                    case PasswordFilterOption.Application:
                        result = (from Password password in passwords
                                  where password.Application.Contains(filterText)
                                  select password).ToList<Password>();
                        break;

                    case PasswordFilterOption.Description:
                        result = (from Password password in passwords
                                  where password.Description.Contains(filterText)
                                  select password).ToList<Password>();
                        break;

                    case PasswordFilterOption.Website:
                        result = (from Password password in passwords
                                  where password.Website.Contains(filterText)
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

        /*************************************************************************************************/
        private void UpdatePasswordsUI()
        {
            // Get encrypted passwords from the password service and modify to display in UI
            List<Password> passwords = _passwordService.GetPasswords();

            BindingList<Password> uiBindingList = new BindingList<Password>(passwords);
            _mainView.DisplayPasswords(uiBindingList);
        }

        /*************************************************************************************************/
        private void UpdateUsernameWelcomeUI()
        {
            _mainView.DisplayUserID(_passwordService.GetCurrentUsername());
            _mainView.RequestPasswordsOnLoginEvent -= UpdateUsernameWelcomeUI;
        }

        /*************************************************************************************************/
        private void AddPassword(string application, string username, string email, string description, string website, string passphrase)
        {
            Password uiPassword = new Password(application, username, email, description, website, passphrase);
            AddPasswordResult result = _passwordService.AddPassword(uiPassword);

            if (result == AddPasswordResult.Success)
            {
                UpdatePasswordsUI();
            }

            _mainView.DisplayAddPasswordResult(result);    
        }

        /*************************************************************************************************/
        private void DeletePassword(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);

            if (result != null)
            {
                DeletePasswordResult deleteResult = _passwordService.DeletePassword(result);
                UpdatePasswordsUI();
                _mainView.DisplayDeletePasswordResult(deleteResult);
            }        
        }

        /*************************************************************************************************/
        private void EditPasswordInit(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);

            if (result != null)
            {
                _editPassword = result;
                _mainView.DisplayPasswordToEdit(_passwordService.DecryptPassword(_editPassword));
            }
        }

        /*************************************************************************************************/
        private void EditPasswordExecute(string application, string username, string email, string description, string website, string passphrase)
        {
            Password modifiedPassword = new Password(_editPassword.UniqueID, application, username, email, description, website, passphrase);

            AddPasswordResult result = _passwordService.ModifyPassword(_editPassword, modifiedPassword);

            if (result == AddPasswordResult.Success)
            {
                _editPassword = null;
            }

            _mainView.DisplayAddEditPasswordResult(result);
        }

        /*************************************************************************************************/
        private void Logout()
        {
            LogoutUser();
        }

        /*************************************************************************************************/
        private LogOutResult LogoutUser()
        {
            LogOutResult result = _passwordService.Logout();

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

            string passphrase = _passwordService.DecryptPassword(result).Passphrase;

            if (!string.IsNullOrEmpty(passphrase))
            {
                System.Windows.Forms.Clipboard.SetText(passphrase);
            }          
        }

        /*************************************************************************************************/
        private void ViewPassword(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);

            string passphrase = _passwordService.DecryptPassword(result).Passphrase;

            _mainView.DisplayPassword(passphrase);
        }

        /*************************************************************************************************/
        private void NavigateToWebsite(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);

            string uri = result.Website;

            if (UriUtilities.IsValidUri(uri))
            {
                UriUtilities.OpenUri(uri);
            }
        }

        /*************************************************************************************************/
        private void CopyUsername(DataGridViewRow dgvrow)
        {
            Password password = ConvertDgvRowToPassword(dgvrow);
            Password result = QueryForFirstPassword(password);

            System.Windows.Forms.Clipboard.SetText(result.Username);
        }

        /*************************************************************************************************/
        private Password QueryForFirstPassword(string application, string username, string email, string description, string website)
        {
            List<Password> passwords = _passwordService.GetPasswords();

            Password result = (from Password password in passwords
                               where password.Application == application
                               where password.Username == username
                               where password.Email == email
                               where password.Description == description
                               where password.Website == website
                               select password).FirstOrDefault();

            return result;
        }

        /*************************************************************************************************/
        private Password QueryForFirstPassword(Password password)
        {
            List<Password> passwords = _passwordService.GetPasswords();

            Password result = (from Password queryPassword in passwords
                               where queryPassword.Application == password.Application
                               where queryPassword.Username == password.Username
                               where queryPassword.Email == password.Email
                               where queryPassword.Description == password.Description
                               where queryPassword.Website == password.Website
                               select queryPassword).FirstOrDefault();

            return result;
        }

        /*************************************************************************************************/
        private int FindPasswordIndex(string application, string username, string email, string description, string website)
        {
            List<Password> passwords = _passwordService.GetPasswords();

            int index = passwords.FindIndex(x => (x.Application == application) && (x.Username == username) &&  (x.Email == email) && (x.Description == description) && (x.Website == website));

            return index;
        }

        /*************************************************************************************************/
        private static Password ConvertDgvRowToPassword(DataGridViewRow dgvrow)
        {
            Password p = new Password
            (
                dgvrow.Cells[0].Value.ToString(),
                dgvrow.Cells[1].Value.ToString(),
                dgvrow.Cells[2].Value.ToString(),
                dgvrow.Cells[3].Value.ToString(),
                dgvrow.Cells[4].Value.ToString(),
                null
            );

            return p;
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // MainFormPresenter CLASS
} // PasswordVault NAMESPACE
