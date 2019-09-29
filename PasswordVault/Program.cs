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
            var changePasswordView = kernal.Get<IChangePasswordView>();
            var editUserView = kernal.Get<IEditUserView>();
            var passwordService = kernal.Get<IPasswordService>();

            LoginPresenter loginPresenter = new LoginPresenter(loginView, passwordService);
            MainPresenter mainViewPresenter = new MainPresenter(mainView, passwordService);
            ChangePasswordPresenter changePasswordPresenter = new ChangePasswordPresenter(changePasswordView, passwordService);
            EditUserPresenter editUserPresenter = new EditUserPresenter(editUserView, passwordService);

            Application.Run((System.Windows.Forms.Form)mainView);
        }
    }
}
