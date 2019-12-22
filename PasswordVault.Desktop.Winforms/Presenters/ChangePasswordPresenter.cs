using PasswordVault.Services;
using PasswordVault.Models;
using System;

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
    public class ChangePasswordPresenter
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
        private IChangePasswordView _changePasswordView;
        private IDesktopServiceWrapper _serviceWrapper;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public ChangePasswordPresenter(IChangePasswordView changePasswordView, IDesktopServiceWrapper serviceWrapper)
        {
            if (changePasswordView == null)
            {
                throw new ArgumentNullException(nameof(changePasswordView));
            }

            if (serviceWrapper == null)
            {
                throw new ArgumentNullException(nameof(serviceWrapper));
            }

            _changePasswordView = changePasswordView;
            _serviceWrapper = serviceWrapper;

            _changePasswordView.ChangePasswordEvent += ModifyPassword;
            _changePasswordView.PasswordTextChangedEvent += PasswordTextChanged;
            _changePasswordView.GenerateNewPasswordEvent += GeneratePassword;
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void PasswordTextChanged(string passwordText)
        {
            PasswordComplexityLevel passwordComplexityLevel = PasswordComplexityLevel.Weak;

            passwordComplexityLevel = PasswordComplexity.checkEffectiveBitSize(passwordText.Length, passwordText);

            _changePasswordView.DisplayPasswordComplexity(passwordComplexityLevel);
        }

        /*************************************************************************************************/
        private void ModifyPassword(string originalPassword, string password, string confirmPassword)
        {
            ValidateUserPasswordResult passresult = _serviceWrapper.ChangeUserPassword(originalPassword, password, confirmPassword);

            _changePasswordView.DisplayChangePasswordResult(passresult);
        }

        /*************************************************************************************************/
        private void GeneratePassword()
        {
            string generatedPassword = _serviceWrapper.GeneratePasswordKey();
            _changePasswordView.DisplayGeneratedPassword(generatedPassword);
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // ChangePasswordPresenter CLASS
} // PasswordVault NAMESPACE
