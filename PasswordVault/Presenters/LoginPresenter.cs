using System;
using System.Collections.Generic;
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
        private IPasswordService _passwordService;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public LoginPresenter(ILoginView loginView, IPasswordService passwordService)
        {
            _loginView = loginView;
            _passwordService = passwordService;

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
            LoginResult result = LoginResult.Failed;

            result = _passwordService.Login(username, password);

            _loginView.DisplayLoginResult(result);
        }

        /********************************************************************************** ***************/
        private void CreateNewUser(string username, string password, string firstName, string lastName, string phoneNumber, string email)
        {
            CreateUserResult result = CreateUserResult.Failed;

            User user = new User(username, password, firstName, lastName, phoneNumber, email);

            result = _passwordService.CreateNewUser(user);

            _loginView.DisplayCreateNewUserResult(result, _passwordService.GetMinimumPasswordLength());
        }

        /*************************************************************************************************/
        private void GeneratePassword()
        {
            string generatedPassword = "";

            generatedPassword = _passwordService.GeneratePasswordKey();

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