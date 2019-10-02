using PasswordVault.Service;

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
            ChangeUserPasswordResult result = ChangeUserPasswordResult.Failed;

            if (password == confirmPassword)
            {
                bool validPassword = _passwordService.VerifyCurrentUserPassword(originalPassword);

                if (validPassword)
                {
                    ChangeUserPasswordResult passresult = _passwordService.ChangeUserPassword(password);

                    if (passresult == ChangeUserPasswordResult.Success)
                    {
                        result = ChangeUserPasswordResult.Success;
                    }
                }              
            }
            else
            {
                result = ChangeUserPasswordResult.PasswordsDoNotMatch;
            }

            _changePasswordView.DisplayChangePasswordResult(result);
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
