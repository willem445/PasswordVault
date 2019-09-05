using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    interface IMainView
    {
        event Action<string, PasswordFilterOptions> FilterChangedEvent;
        event Action RequestPasswordsOnLoginEvent;
        event Action<string, string, string, string, string> AddPasswordEvent;
        event Action<int> MovePasswordUpEvent;
        event Action<int> MovePasswordDownEvent;
        event Action<int> EditPasswordEvent;
        event Action <string, string, string, string> DeletePasswordEvent;
        void DisplayPasswords(BindingList<Password> passwordList);
        void DisplayUserID(string userID);
    }
}
