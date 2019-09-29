using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    public interface IEditUserView
    {
        event Action<string, string, string, string> ModifyAccountEvent;
        event Action RequestUserEvent;
        void DisplayModifyResult(ModifyUserResult result);
        void DisplayUser(User user);
        void ShowEditUserMenu();
    }
}
