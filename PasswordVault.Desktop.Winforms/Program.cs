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
#if (!DEBUG)
            try
            {
#endif
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var kernal = new StandardKernel();

                kernal.Load(Assembly.GetExecutingAssembly());

                var loginView = kernal.Get<ILoginView>();
                var mainView = kernal.Get<IMainView>();
                var changePasswordView = kernal.Get<IChangePasswordView>();
                var confirmDeleteUserView = kernal.Get<IConfirmDeleteUserView>();
                var editUserView = kernal.Get<IEditUserView>();
                var exportView = kernal.Get<IExportView>();
                var importView = kernal.Get<IImportView>();
                var serviceWrapper = kernal.Get<IDesktopServiceWrapper>();

                var loginPresenter = new LoginPresenter(loginView, serviceWrapper);
                var mainViewPresenter = new MainPresenter(mainView, serviceWrapper);
                var changePasswordPresenter = new ChangePasswordPresenter(changePasswordView, serviceWrapper);
                var editUserPresenter = new EditUserPresenter(editUserView, serviceWrapper);
                var exportPresenter = new ExportPresenter(exportView, serviceWrapper);
                var importPresenter = new ImportPresenter(importView, serviceWrapper);
            
                var confirmDeleteUserPresenter = new ConfirmDeleteUserPresenter(confirmDeleteUserView, serviceWrapper);

                Application.Run((Form)mainView);

                kernal.Dispose();
#if (!DEBUG)
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show(e.StackTrace);
            }
#endif
        }
    }
}
