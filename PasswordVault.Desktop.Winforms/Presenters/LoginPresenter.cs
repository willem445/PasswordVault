using PasswordVault.Services;
using PasswordVault.Models;

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
    class LoginPresenter
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
        private ILoginView _loginView;
        private IDesktopServiceWrapper _serviceWrapper;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public LoginPresenter(ILoginView loginView, IDesktopServiceWrapper serviceWrapper)
        {
            _loginView = loginView;
            _serviceWrapper = serviceWrapper;

            _loginView.LoginEvent += Login;
            _loginView.CreateNewUserEvent += CreateNewUser;
            _loginView.PasswordChangedEvent += CalculatePasswordComplexity;
            _loginView.GenerateNewPasswordEvent += GeneratePassword;
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void Login(string username, string password)
        {
            AuthenticateResult result = AuthenticateResult.Failed;

            result = _serviceWrapper.Login(username, password);

            _loginView.DisplayLoginResult(result);
        }

        /********************************************************************************** ***************/
        private void CreateNewUser(string username, string password, string firstName, string lastName, string phoneNumber, string email)
        {
            AddUserResult result = AddUserResult.Failed;

            User user = new User(username, password, firstName, lastName, phoneNumber, email);

            result = _serviceWrapper.CreateNewUser(user);

            _loginView.DisplayCreateNewUserResult(result, _serviceWrapper.GetMinimumPasswordLength());
        }

        /*************************************************************************************************/
        private void GeneratePassword()
        {
            string generatedPassword = "";

            generatedPassword = _serviceWrapper.GeneratePasswordKey();

            _loginView.DisplayGeneratePasswordResult(generatedPassword);
        }

        /*************************************************************************************************/
        private void CalculatePasswordComplexity(string password)
        {
            PasswordComplexityLevel passwordComplexityLevel = PasswordComplexityLevel.Weak;

            passwordComplexityLevel = PasswordComplexity.checkEffectiveBitSize(password.Length, password);

            _loginView.DisplayPasswordComplexity(passwordComplexityLevel);
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // LoginPresenter CLASS
} // PasswordVault NAMESPACE