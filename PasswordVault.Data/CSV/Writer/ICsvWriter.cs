using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault.Data
{
    public interface ICSVWriter
    {
        void Initialize(string fileName);
        void Close();
        void WriteLine(string line);
    }
}
