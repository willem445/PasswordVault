using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Desktop.Winforms
{
    public interface IAddPasswordView
    {
        event Action<Password> AddPasswordEvent;
        event Action<string> PasswordChangedEvent;
        event Action GenerateNewPasswordEvent;
        event Action<AddPasswordResult> AddPasswordResultEvent;

        void DisplayGeneratePasswordResult(string generatedPassword);
        void DisplayAddPasswordResult(AddPasswordResult result);
        void DisplayPasswordComplexity(PasswordComplexityLevel complexity);
        void ShowMenu();
    }
}
