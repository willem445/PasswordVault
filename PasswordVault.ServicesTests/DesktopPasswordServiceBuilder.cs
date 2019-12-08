using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Desktop.Winforms;
using PasswordVault.Data;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.ServicesTests
{
    public static class DesktopPasswordServiceBuilder
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/

        /*PUBLIC METHODS***************************************************/

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/
        public static IDesktopServiceWrapper BuildDesktopServiceWrapper()
        {
            IDesktopServiceWrapper desktopServiceWrapper;

            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            ITokenService tokenService = new TokenService();
            IMasterPassword masterPassword = new MasterPassword();
            IEncryptionService encryptionService = new AesEncryption();
            IAuthenticationService auth = new AuthenticationService(tokenService, masterPassword, encryptionService, db);
            IExportPasswords export = new ExportPasswords();

            desktopServiceWrapper = new DesktopServiceWrapper(auth, new PasswordService(db, encryptionService), new UserService(db, masterPassword, encryptionService, auth), export);

            return desktopServiceWrapper;
        }

        public static IDesktopServiceWrapper BuildDesktopServiceWrapper(IDatabase db)
        {
            IDesktopServiceWrapper desktopServiceWrapper;

            ITokenService tokenService = new TokenService();
            IMasterPassword masterPassword = new MasterPassword();
            IEncryptionService encryptionService = new AesEncryption();
            IAuthenticationService auth = new AuthenticationService(tokenService, masterPassword, encryptionService, db);
            IExportPasswords export = new ExportPasswords();

            desktopServiceWrapper = new DesktopServiceWrapper(auth, new PasswordService(db, encryptionService), new UserService(db, masterPassword, encryptionService, auth), export);

            return desktopServiceWrapper;
        }

    } // DesktopPasswordServiceBuilder CLASS
}
