using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    public interface ILoginView
    {
        event Action<string, string> LoginEvent;
        void DisplayLoginResult(LoginResult result);
        void ShowLoginMenu();
    }
}
