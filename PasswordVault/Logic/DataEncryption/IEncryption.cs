using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault.Desktop.Winforms
{
    public interface IEncryption
    {
        int Iterations { get; }
        string Encrypt(string plainText, string passPhrase);
        string Decrypt(string cipherText, string passPhrase);
        string CreateKey(int keyLength);
    }
}
