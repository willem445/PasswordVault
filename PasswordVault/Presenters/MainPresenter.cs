using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault
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
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void FilterChanged(string filterText, PasswordFilterOptions passwordFilterOption)
        {
            List<Password> result = new List<Password>();
            List<Password> passwords = _passwordService.GetPasswords();

            if (filterText != "")
            {
                switch (passwordFilterOption)
                {
                    case PasswordFilterOptions.Application:
                        result = (from Password password in passwords
                                  where password.Application.Contains(filterText)
                                  select password).ToList<Password>();
                        break;

                    case PasswordFilterOptions.Description:
                        result = (from Password password in passwords
                                  where password.Description.Contains(filterText)
                                  select password).ToList<Password>();
                        break;

                    case PasswordFilterOptions.Website:
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
            _mainView.DisplayUserID(_passwordService.GetCurrentUserID());
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
        private void DeletePassword(string application, string username, string email, string description, string website)
        {
            Password result = QueryForFirstPassword(application, username, email, description, website);

            if (result != null)
            {
                DeletePasswordResult deleteResult = _passwordService.DeletePassword(result);
                UpdatePasswordsUI();
                _mainView.DisplayDeletePasswordResult(deleteResult);
            }        
        }

        /*************************************************************************************************/
        private void EditPasswordInit(string application, string username, string email, string description, string website)
        {
            Password result = QueryForFirstPassword(application, username, email, description, website);

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
            LogOutResult result = _passwordService.Logout();

            if (result == LogOutResult.Success)
            {
                _mainView.RequestPasswordsOnLoginEvent += UpdateUsernameWelcomeUI;
            }

            _mainView.DisplayLogOutResult(result);
        }

        /*************************************************************************************************/
        private void CopyPassword(string application, string username, string email, string description, string website)
        {
            Password result = QueryForFirstPassword(application, username, email, description, website);

            string passphrase = _passwordService.DecryptPassword(result).Passphrase;

            if ((passphrase != "") && (passphrase != null))
            {
                System.Windows.Forms.Clipboard.SetText(passphrase);
            }          
        }

        /*************************************************************************************************/
        private void ViewPassword(string application, string username, string email, string description, string website)
        {
            Password result = QueryForFirstPassword(application, username, email, description, website);

            string passphrase = _passwordService.DecryptPassword(result).Passphrase;

            _mainView.DisplayPassword(passphrase);
        }

        /*************************************************************************************************/
        private void NavigateToWebsite(string application, string username, string email, string description, string website)
        {
            Password result = QueryForFirstPassword(application, username, email, description, website);

            string uri = result.Website;

            if (UriUtilities.IsValidUri(uri))
            {
                UriUtilities.OpenUri(uri);
            }
        }

        /*************************************************************************************************/
        private void CopyUsername(string application, string username, string email, string description, string website)
        {
            Password result = QueryForFirstPassword(application, username, email, description, website);

            System.Windows.Forms.Clipboard.SetText(result.Username);
        }

        /*************************************************************************************************/
        private Password QueryForFirstPassword(string application, string username, string email, string description, string website)
        {
            List<Password> passwords = _passwordService.GetPasswords();

            Password result = (from Password password in passwords
                               where password.Application.Contains(application)
                               where password.Username.Contains(username)
                               where password.Email.Contains(email)
                               where password.Description.Contains(description)
                               where password.Website.Contains(website)
                               select password).FirstOrDefault();

            return result;
        }

        /*************************************************************************************************/
        private int FindPasswordIndex(string application, string username, string email, string description, string website)
        {
            List<Password> passwords = _passwordService.GetPasswords();

            int index = passwords.FindIndex(x => (x.Application == application) && (x.Username == username) &&  (x.Email == email) && (x.Description == description) && (x.Website == website));

            return index;
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // MainFormPresenter CLASS
} // PasswordVault NAMESPACE
