using PasswordVault.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault.Desktop.Winforms
{
    public interface IConfirmDeleteUserView
    {
        event Action<string> ConfirmPasswordEvent; // Notify presenter to verify password
        event Action DeleteAccountEvent; // Notify presenter to delete password
        event Action FormClosingEvent;
        
        event Action ConfirmPasswordSuccessEvent; // Notify mainform to logout of account
        event Action DeleteSuccessEvent; // Notify mainform of result

        void DisplayConfirmPasswordResult(bool result);
        void DisplayDeleteAccountResult(DeleteUserResult result);

        void ShowView();
    }
}
