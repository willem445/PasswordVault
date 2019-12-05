﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Services;
using PasswordVault.Data;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Desktop.Winforms
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    public class NinjectBindings : Ninject.Modules.NinjectModule
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public override void Load()
        {
            Bind<IDatabase>().To<SQLiteDatabase>().InSingletonScope();

            Bind<IEncryptionService>().To<AesEncryption>();
            Bind<IMasterPassword>().To<MasterPassword>();
            Bind<IPasswordService>().To<PasswordService>().InSingletonScope();
            Bind<IUserService>().To<UserService>().InSingletonScope();
            Bind<IAuthenticationService>().To<AuthenticationService>();
            Bind<ITokenService>().To<TokenService>();
            Bind<IExportPasswords>().To<ExportPasswords>();

            Bind<IDesktopServiceWrapper>().To<DesktopServiceWrapper>().InSingletonScope();        
            Bind<ILoginView>().To<LoginView>().InSingletonScope();
            Bind<IMainView>().To<MainView>().InSingletonScope();
            Bind<IChangePasswordView>().To<ChangePasswordView>().InSingletonScope();
            Bind<IEditUserView>().To<EditUserView>().InSingletonScope();
            Bind<IConfirmDeleteUserView>().To<ConfirmDeleteUserView>().InSingletonScope();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // NinjectBindings CLASS
} // PasswordVault NAMESPACE
