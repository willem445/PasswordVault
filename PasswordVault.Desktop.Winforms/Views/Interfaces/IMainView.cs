using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Desktop.Winforms
{
    interface IMainView
    {
        event Action<string, PasswordFilterOption> FilterChangedEvent;
        event Action RequestPasswordsOnLoginEvent;
        event Action RequestPasswordsEvent;
        event Action LogoutEvent;

        event Action<string, string, string, string, string, string, string> AddPasswordEvent;
        event Action<DataGridViewRow> EditPasswordEvent;

        event Action<DataGridViewRow> DeletePasswordEvent;     
        event Action<DataGridViewRow> CopyUserNameEvent;
        event Action<DataGridViewRow> CopyPasswordEvent;
        event Action<DataGridViewRow> ShowPasswordEvent;
        event Action<DataGridViewRow> NavigateToWebsiteEvent;

        event Action GeneratePasswordEvent;

        void SetTimeoutTime(int minutes);
        void DisplayPasswords(BindingList<Password> passwordList);
        void DisplayUserID(string userID);
        void DisplayPasswordToEdit(Password password);
        void DisplayAddEditPasswordResult(ValidatePassword result);
        void DisplayLogOutResult(LogOutResult result);
        void DisplayPassword(string password);
        void DisplayAddPasswordResult(ValidatePassword result);
        void DisplayDeletePasswordResult(DeletePasswordResult result);
        void DisplayPasswordCount(int count);
    }
}
