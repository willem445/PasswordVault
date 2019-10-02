using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PasswordVault.Models;
using PasswordVault.Service;

namespace PasswordVault.Desktop.Winforms
{
    interface IMainView
    {
        event Action<string, PasswordFilterOptions> FilterChangedEvent;
        event Action RequestPasswordsOnLoginEvent;
        event Action LogoutEvent;
        event Action DeleteAccountEvent;

        event Action<string, string, string, string, string, string> AddPasswordEvent;
        event Action<int> MovePasswordUpEvent;
        event Action<int> MovePasswordDownEvent;
        event Action<DataGridViewRow> EditPasswordEvent;
        event Action<string, string, string, string, string, string> EditOkayEvent;
        event Action EditCancelEvent;

        event Action<DataGridViewRow> DeletePasswordEvent;     
        event Action<DataGridViewRow> CopyUserNameEvent;
        event Action<DataGridViewRow> CopyPasswordEvent;
        event Action<DataGridViewRow> ShowPasswordEvent;
        event Action<DataGridViewRow> NavigateToWebsiteEvent;

        void DisplayPasswords(BindingList<Password> passwordList);
        void DisplayUserID(string userID);
        void DisplayPasswordToEdit(Password password);
        void DisplayAddEditPasswordResult(AddPasswordResult result);
        void DisplayLogOutResult(LogOutResult result);
        void DisplayPassword(string password);
        void DisplayAddPasswordResult(AddPasswordResult result);
        void DisplayDeletePasswordResult(DeletePasswordResult result);
    }
}
