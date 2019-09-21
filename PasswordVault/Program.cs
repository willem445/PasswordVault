using System;
using System.Windows.Forms;
using Ninject;
using System.Reflection;

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

            var kernal = new StandardKernel();
            kernal.Load(Assembly.GetExecutingAssembly());

            var loginView = kernal.Get<ILoginView>();
            var mainView = kernal.Get<IMainView>();
            var passwordService = kernal.Get<IPasswordService>();

            LoginPresenter loginPresenter = new LoginPresenter(loginView, passwordService);
            MainPresenter mainViewPresenter = new MainPresenter(mainView, passwordService);

            Application.Run((System.Windows.Forms.Form)mainView);
        }
    }
}
