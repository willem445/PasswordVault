using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    public interface IChangePasswordView
    {
        event Action<string, string, string> ChangePasswordEvent;
        event Action<string> PasswordTextChangedEvent;
        event Action GenerateNewPasswordEvent;

        void ShowChangePassword();
        void DisplayGeneratedPassword(string generatedPassword);
        void DisplayChangePasswordResult(ChangeUserPasswordResult result);
        void DisplayPasswordComplexity(PasswordComplexityLevel complexity);
    }
}
