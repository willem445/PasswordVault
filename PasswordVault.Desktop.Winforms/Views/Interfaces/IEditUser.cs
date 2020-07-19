using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Services;
using PasswordVault.Models;

namespace PasswordVault.Desktop.Winforms
{
    public interface IEditUserView
    {
        event Action<string, string, string, string> ModifyAccountEvent;
        event Action RequestUserEvent;
        void DisplayModifyResult(UserInformationResult result);
        void DisplayUser(User user);
        void ShowEditUserMenu();
        void CloseView();
    }
}
