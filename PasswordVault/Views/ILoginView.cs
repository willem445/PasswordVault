using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Service;

namespace PasswordVault.Desktop.Winforms
{
    public interface ILoginView
    {
        event Action<string, string> LoginEvent;
        event Action<string, string, string, string, string, string> CreateNewUserEvent;
        event Action GenerateNewPasswordEvent;
        event Action<string> PasswordChangedEvent;
        event Action LoginSuccessfulEvent;
        void DisplayLoginResult(LoginResult result);
        void ShowLoginMenu();
        void DisplayCreateNewUserResult(CreateUserResult result, int minimumPasswordLength);
        void DisplayGeneratePasswordResult(string generatedPassword);
        void DisplayPasswordComplexity(PasswordComplexityLevel complexity);
    }
}
