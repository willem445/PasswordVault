using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordVault
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ILoginView loginView = new LoginView();
            IEncryptDecrypt encryptDecrypt = new EncryptDecrypt();
            IPasswordService passwordService = new PasswordService(new CsvDatabaseFactory().Get(), new MasterPassword(), encryptDecrypt);

            MainView mainView = new MainView(loginView);

            // Create presenters
            LoginPresenter loginPresenter = new LoginPresenter(loginView, passwordService);
            MainFormPresenter mainViewPresenter = new MainFormPresenter(mainView, passwordService);

            Application.Run(mainView);
        }
    }
}
