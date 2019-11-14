using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Desktop.Winforms
{
    public class ConfirmDeleteUserPresenter
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private IDesktopServiceWrapper _serviceWrapper;
        private IConfirmDeleteUserView _confirmDeleteUserView;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public ConfirmDeleteUserPresenter(IConfirmDeleteUserView confirmDeleteUserView, IDesktopServiceWrapper serviceWrapper)
        {
            _confirmDeleteUserView = _confirmDeleteUserView ?? throw new ArgumentNullException(nameof(confirmDeleteUserView));
            _serviceWrapper = _serviceWrapper ?? throw new ArgumentNullException(nameof(serviceWrapper));

            _confirmDeleteUserView.ConfirmPasswordEvent += VerifyPassword;
            _confirmDeleteUserView.DeleteAccountEvent += DeleteAccount;

            //User user = _serviceWrapper.GetCurrentUser();
            //LogOutResult result = LogoutUser();

            //if (result == LogOutResult.Success)
            //{
            //    _serviceWrapper.DeleteUser(user);
            //}
        }

        /*PUBLIC METHODS***************************************************/
        public void VerifyPassword(string password)
        {
            bool result = _serviceWrapper.VerifyCurrentUserPassword(password);
            _confirmDeleteUserView.DisplayConfirmPasswordResult(result);

        }

        public void DeleteAccount()
        {
            User user = _serviceWrapper.GetCurrentUser();
            DeleteUserResult result = _serviceWrapper.DeleteUser(user);
            _confirmDeleteUserView.DisplayDeleteAccountResult(result);
        }

        /*PRIVATE METHODS**************************************************/
        //private LogOutResult LogoutUser()
        //{
            //LogOutResult result = _serviceWrapper.Logout();

            //if (result == LogOutResult.Success)
            //{
            //    _mainView.RequestPasswordsOnLoginEvent += UpdateUsernameWelcomeUI;
            //}

            //_mainView.DisplayLogOutResult(result);

            //return result;
        //}

        /*STATIC METHODS***************************************************/

    } // ConfirmDeleteUserPresenter CLASS
}
