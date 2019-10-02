using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault.Desktop.Winforms
{
    public interface ICSVWriter
    {
        void Initialize(string fileName);
        void Close();
        void WriteLine(string line);
    }
}
