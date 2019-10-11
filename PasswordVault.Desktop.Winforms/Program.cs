using System;
using System.Windows.Forms;
using Ninject;
using System.Reflection;
using PasswordVault.Services;

namespace PasswordVault.Desktop.Winforms
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

            var loginPresenter = new LoginPresenter(loginView, passwordService);
            var mainViewPresenter = new MainPresenter(mainView, passwordService);
            var changePasswordPresenter = new ChangePasswordPresenter(changePasswordView, passwordService);
            var editUserPresenter = new EditUserPresenter(editUserView, passwordService);

            Application.Run((Form)mainView);

            kernal.Dispose();
        }
    }
}
