using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public class PasswordService : IPasswordService
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public PasswordService()
        {

        }

        public AddModifyPasswordResult AddPassword(string userUuid, Password password)
        {
            throw new NotImplementedException();
        }

        public DeletePasswordResult DeletePassword(string userUuid, string passwordUuid)
        {
            throw new NotImplementedException();
        }

        public string GeneratePasswordKey(int length)
        {
            throw new NotImplementedException();
        }

        public List<Password> GetPasswords(string userUuid)
        {
            throw new NotImplementedException();
        }

        public AddModifyPasswordResult ModifyPassword(string userUuid, Password modifiedPassword)
        {
            throw new NotImplementedException();
        }

        /*PUBLIC METHODS***************************************************/

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // PasswordService CLASS
}
