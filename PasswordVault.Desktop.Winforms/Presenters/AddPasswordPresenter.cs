using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault.Desktop.Winforms
{
    class AddPasswordPresenter
    {
        private IAddPasswordView _addPasswordView;
        private IDesktopServiceWrapper _serviceWrapper;

        public AddPasswordPresenter(IAddPasswordView addPasswordView, IDesktopServiceWrapper serviceWrapper)
        {
            if (addPasswordView == null)
            {
                throw new ArgumentNullException(nameof(addPasswordView));
            }

            if (serviceWrapper == null)
            {
                throw new ArgumentNullException(nameof(serviceWrapper));
            }

            _addPasswordView = addPasswordView;
            _serviceWrapper = serviceWrapper;
        }
    }
}
