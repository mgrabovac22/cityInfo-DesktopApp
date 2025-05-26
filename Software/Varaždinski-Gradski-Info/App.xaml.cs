using BusinessLogicLayer;
using PresentationLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Varaždinski_Gradski_Info.Properties;

namespace Varaždinski_Gradski_Info
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var service = new UsersService();
            base.OnStartup(e);

            if (!string.IsNullOrEmpty(Settings.Default.username))
            {
                OpenMainWindow(service.ReturnDecryptedRole(Settings.Default.role));
            }
            else
            {
                var loginWindow = new Login();
                loginWindow.ShowDialog();
            }
        }

        private void OpenMainWindow(string role)
        {
            Window mainWindow;
            switch (role)
            {
                case "Admin":
                    mainWindow = new AdminMainWindow(role);
                    break;
                case "User":
                    mainWindow = new UsersMainWindow(role);
                    break;
                case "Employee":
                    mainWindow = new EmployeeMainWindow(role);
                    break;
                default:
                    throw new Exception("Nepoznata uloga!");
            }

            GuiManager.SetWindow(mainWindow);
            mainWindow.ShowDialog();
        }

    }

}
