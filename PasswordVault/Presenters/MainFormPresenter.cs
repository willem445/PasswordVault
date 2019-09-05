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
        private IPasswordUIFormatter _passwordUIFormatter;

        private List<Password> _uiPasswordList;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public MainFormPresenter(IMainView mainView, IPasswordService passwordService, IPasswordUIFormatter passwordUIFormatter)
        {
            _mainView = mainView;
            _passwordService = passwordService;
            _passwordUIFormatter = passwordUIFormatter;
            _uiPasswordList = new List<Password>();

            _mainView.FilterChangedEvent += FilterChanged;
            _mainView.RequestPasswordsEvent += UpdatePasswordsUI;
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

            if (filterText != "")
            {
                switch (passwordFilterOption)
                {
                    case PasswordFilterOptions.Application:
                        result = (from Password password in _uiPasswordList
                                  where password.Application.Contains(filterText)
                                  select password).ToList<Password>();
                        break;

                    case PasswordFilterOptions.Description:
                        result = (from Password password in _uiPasswordList
                                  where password.Description.Contains(filterText)
                                  select password).ToList<Password>();
                        break;

                    case PasswordFilterOptions.Website:
                        result = (from Password password in _uiPasswordList
                                  where password.Website.Contains(filterText)
                                  select password).ToList<Password>();
                        break;
                }

                BindingList<Password> uiBindingList = new BindingList<Password>(result);
                _mainView.DisplayPasswords(uiBindingList);
            }
            else
            {
                BindingList<Password> uiBindingList = new BindingList<Password>(_uiPasswordList);
                _mainView.DisplayPasswords(uiBindingList);
            }
        }

        /*************************************************************************************************/
        private void UpdatePasswordsUI()
        {
            // Get encrypted passwords from the password service and modify to display in UI
            List<Password> encryptedPasswords = _passwordService.GetPasswords();
            _uiPasswordList.Clear();

            foreach (var encryptedPassword in encryptedPasswords)
            {
                Password uiPassword = _passwordUIFormatter.PasswordServiceToUI(encryptedPassword, _passwordService.GetMasterUserKey());
                _uiPasswordList.Add(uiPassword);
            }

            BindingList<Password> uiBindingList = new BindingList<Password>(_uiPasswordList);
            _mainView.DisplayPasswords(uiBindingList);
        }

        /*************************************************************************************************/
        private void AddPassword(string application, string username, string description, string website, string passphrase)
        {
            Password uiPassword = new Password(application, username, description, website, passphrase);
            _uiPasswordList.Add(uiPassword);

            Password encryptedServicePassword = _passwordUIFormatter.PasswordUIToService(uiPassword, _passwordService.GetMasterUserKey());
            _passwordService.AddPassword(encryptedServicePassword);
            UpdatePasswordsUI();
        }

        /*************************************************************************************************/
        private void DeletePassword(string application, string username, string description, string website)
        {
            Password result = (from Password password in _uiPasswordList
                               where password.Application.Contains(application)
                               where password.Username.Contains(username)
                               where password.Description.Contains(description)
                               where password.Website.Contains(website)
                               select password).First();

            Password encryptedServicePassword = _passwordUIFormatter.PasswordUIToService(result, _passwordService.GetMasterUserKey());
            _passwordService.RemovePassword(encryptedServicePassword);
            UpdatePasswordsUI();
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // MainFormPresenter CLASS
} // PasswordVault NAMESPACE
