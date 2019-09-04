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

            _mainView.FilterChangedEvent += FilterChanged;
            _mainView.RequestPasswordsEvent += UpdatePasswordsUI;
            _mainView.AddPasswordEvent += AddPassword;
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
            List<Password> temp = _passwordService.GetPasswords();

            if (filterText != "")
            {
                switch (passwordFilterOption)
                {
                    case PasswordFilterOptions.Application:
                        result = (from Password password in temp
                                  where password.Application.Contains(filterText)
                                  select password).ToList<Password>();
                        break;

                    case PasswordFilterOptions.Description:
                        result = (from Password password in temp
                                  where password.Description.Contains(filterText)
                                  select password).ToList<Password>();
                        break;

                    case PasswordFilterOptions.Website:
                        result = (from Password password in temp
                                  where password.Website.Contains(filterText)
                                  select password).ToList<Password>();
                        break;
                }

                BindingList<Password> passwordList = new BindingList<Password>(result);
                _mainView.DisplayPasswords(passwordList);
            }
            else
            {
                BindingList<Password> passwordList = new BindingList<Password>(temp);
                _mainView.DisplayPasswords(passwordList);
            }
        }

        /*************************************************************************************************/
        private void UpdatePasswordsUI()
        {
            List<Password> temp = _passwordService.GetPasswords();

            BindingList<Password> passwordList = new BindingList<Password>(temp);

            _mainView.DisplayPasswords(passwordList);
        }

        /*************************************************************************************************/
        private void AddPassword(string application, string username, string description, string website, string passphrase)
        {
            _passwordService.AddPassword(new Password(application, username, description, website, passphrase));
            UpdatePasswordsUI();
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // MainFormPresenter CLASS
} // PasswordVault NAMESPACE
