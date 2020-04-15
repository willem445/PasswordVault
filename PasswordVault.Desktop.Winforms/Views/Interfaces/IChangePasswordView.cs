using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Services;
using PasswordVault.Models;

namespace PasswordVault.Desktop.Winforms
{
    public interface IChangePasswordView
    {
        event Action<string, string, string> ChangePasswordEvent;
        event Action<string> PasswordTextChangedEvent;
        event Action GenerateNewPasswordEvent;

        void ShowChangePassword();
        void DisplayGeneratedPassword(string generatedPassword);
        void DisplayChangePasswordResult(ValidateUserPasswordResult result);
        void DisplayPasswordComplexity(PasswordComplexityLevel complexity);
    }
}
