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
        event Action<string, string> CreateNewUserEvent;
        event Action GenerateNewPasswordEvent;
        event Action<string> PasswordChanged;
        void DisplayLoginResult(LoginResult result);
        void ShowLoginMenu();
        void DisplayCreateNewUserResult(CreateUserResult result);
        void DisplayGeneratePasswordResult(string generatedPassword);
        void DisplayPasswordComplexity(PasswordComplexityLevel complexity);
    }
}
