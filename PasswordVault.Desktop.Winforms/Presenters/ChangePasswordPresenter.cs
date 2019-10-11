using PasswordVault.Services;
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
        private IPasswordService _passwordService;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public ChangePasswordPresenter(IChangePasswordView changePasswordView, IPasswordService passwordService)
        {
            if (changePasswordView == null)
            {
                throw new ArgumentNullException(nameof(changePasswordView));
            }

            if (passwordService == null)
            {
                throw new ArgumentNullException(nameof(passwordService));
            }

            _changePasswordView = changePasswordView;
            _passwordService = passwordService;

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
            ChangeUserPasswordResult passresult = _passwordService.ChangeUserPassword(originalPassword, password, confirmPassword);

            _changePasswordView.DisplayChangePasswordResult(passresult);
        }

        /*************************************************************************************************/
        private void GeneratePassword()
        {
            string generatedPassword = _passwordService.GeneratePasswordKey();
            _changePasswordView.DisplayGeneratedPassword(generatedPassword);
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // ChangePasswordPresenter CLASS
} // PasswordVault NAMESPACE
