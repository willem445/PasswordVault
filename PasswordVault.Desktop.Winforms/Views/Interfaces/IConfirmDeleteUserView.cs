using PasswordVault.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault.Desktop.Winforms
{
    /// <summary>
    /// Interface for a form that confirms that a user wants to delete account.
    /// </summary>
    public interface IConfirmDeleteUserView
    {
        /// <summary>
        /// Notify presenter to verify the users password.
        /// </summary>
        event Action<string> ConfirmPasswordEvent; 

        /// <summary>
        /// Notify presenter to delete the users account.
        /// </summary>
        event Action DeleteAccountEvent; 

        /// <summary>
        /// Notify presenter to perform any cleanup on close.
        /// </summary>
        event Action FormClosingEvent;
        
        /// <summary>
        /// Notify the calling form that the password was confirmed successfully.
        /// </summary>
        event Action ConfirmPasswordSuccessEvent; 

        /// <summary>
        /// Notify the calling form that the delete action was successful.
        /// </summary>
        event Action DeleteSuccessEvent; 

        /// <summary>
        /// Display the result of authenticating the users password.
        /// </summary>
        /// <param name="result">True if successful.</param>
        void DisplayConfirmPasswordResult(bool result);

        /// <summary>
        /// Display the result of deleting the user's account.
        /// </summary>
        /// <param name="result">Result of deleting account.</param>
        void DisplayDeleteAccountResult(DeleteUserResult result);

        /// <summary>
        /// Shows the confirm delete user form.
        /// </summary>
        void ShowView();
        void CloseView();
    }
}
