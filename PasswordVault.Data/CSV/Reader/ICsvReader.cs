using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault.Data
{
    public interface ICSVReader
    {
        void Initialize(string fileName);
        void Close();
        string ReadLine();
    }
}
