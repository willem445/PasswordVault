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

        private User tempUser;
        private int tempPasswordCount;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public ConfirmDeleteUserPresenter(IConfirmDeleteUserView confirmDeleteUserView, IDesktopServiceWrapper serviceWrapper)
        {
            _confirmDeleteUserView = confirmDeleteUserView ?? throw new ArgumentNullException(nameof(confirmDeleteUserView));
            _serviceWrapper = serviceWrapper ?? throw new ArgumentNullException(nameof(serviceWrapper));

            _confirmDeleteUserView.ConfirmPasswordEvent += VerifyPassword;
            _confirmDeleteUserView.DeleteAccountEvent += DeleteAccount;
            _confirmDeleteUserView.FormClosingEvent += Cleanup;
        }

        /*PUBLIC METHODS***************************************************/
        public void VerifyPassword(string password)
        {
            // The user will already be logged out when we get to DeleteAccount so we need to temporarily store
            // the User object.
            tempUser = _serviceWrapper.GetCurrentUser();
            tempPasswordCount = _serviceWrapper.GetPasswordCount();
            bool result = _serviceWrapper.VerifyCurrentUserPassword(password);
            _confirmDeleteUserView.DisplayConfirmPasswordResult(result);

        }

        public void DeleteAccount()
        {
            DeleteUserResult result = _serviceWrapper.DeleteUser(tempUser, tempPasswordCount);

            if (result == DeleteUserResult.Success)
            {
                tempUser = new User();
                tempPasswordCount = -1;
            }

            _confirmDeleteUserView.DisplayDeleteAccountResult(result);
        }

        public void Cleanup()
        {
            // If delete account form is closing, clear user from memory
            tempUser = new User();
            tempPasswordCount = -1;
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // ConfirmDeleteUserPresenter CLASS
}
