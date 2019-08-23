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
            CsvDatabaseFactory csvDatabaseFactory = new CsvDatabaseFactory();
            IPasswordService passwordService = new PasswordService(csvDatabaseFactory.Get(), new MasterPassword(), new EncryptDecrypt());

            // Create presenters
            LoginPresenter loginPresenter = new LoginPresenter(loginView, passwordService);

            Application.Run(new MainView(loginView));
        }
    }
}
