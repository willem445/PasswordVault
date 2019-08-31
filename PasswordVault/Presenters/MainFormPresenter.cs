﻿using System;
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
            _mainView.RequestPasswordsEvent += UpdatePasswords;
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

        }

        private void UpdatePasswords()
        {
            List<Password> temp = _passwordService.GetPasswords();

            BindingList<Password> passwordList = new BindingList<Password>(temp);

            _mainView.DisplayPasswords(passwordList);
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // MainFormPresenter CLASS
} // PasswordVault NAMESPACE
