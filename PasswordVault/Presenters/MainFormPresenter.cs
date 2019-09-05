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
    class MainFormPresenter
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

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public MainFormPresenter(IMainView mainView, IPasswordService passwordService)
        {
            _mainView = mainView;
            _passwordService = passwordService;

            _mainView.FilterChangedEvent += FilterChanged;
            _mainView.RequestPasswordsOnLoginEvent += UpdatePasswordsUI;
            _mainView.RequestPasswordsOnLoginEvent += UpdateUsernameWelcomeUI;
            _mainView.AddPasswordEvent += AddPassword;
            _mainView.DeletePasswordEvent += DeletePassword;
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
        private void AddPassword(string application, string username, string description, string website, string passphrase)
        {
            Password uiPassword = new Password(application, username, description, website, passphrase);
            _passwordService.AddPassword(uiPassword);
            UpdatePasswordsUI();
        }

        /*************************************************************************************************/
        private void DeletePassword(string application, string username, string description, string website)
        {
            List<Password> passwords = _passwordService.GetPasswords();

            Password result = (from Password password in passwords
                               where password.Application.Contains(application)
                               where password.Username.Contains(username)
                               where password.Description.Contains(description)
                               where password.Website.Contains(website)
                               select password).First();

            _passwordService.RemovePassword(result);
            UpdatePasswordsUI();
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // MainFormPresenter CLASS
} // PasswordVault NAMESPACE
