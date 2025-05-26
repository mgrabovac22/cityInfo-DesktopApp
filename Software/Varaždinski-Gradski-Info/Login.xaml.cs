using BusinessLogicLayer;
using PresentationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Varaždinski_Gradski_Info.UserControls;
using WpfAnimatedGif;
using static BusinessLogicLayer.UsersService;

namespace Varaždinski_Gradski_Info
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private bool isFullScreen = false;

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = false;
            ImageBehavior.SetAnimatedSource(loadingGif, new BitmapImage(new Uri("/UserControls/Assets/loadingGif.gif", UriKind.Relative)));
            loadingGif.Visibility = Visibility.Visible;
            txtLogin.Visibility = Visibility.Collapsed;

            string username = TxtUsername.Text;
            string password = TxtPassword.Password;

            var service = new UsersService();
            string role = await Task.Run(() => service.GetUserRole(username, password));

            if (role != "Unknown")
            {
                Properties.Settings.Default.username = await Task.Run(() => service.HashData(username));
                Properties.Settings.Default.role = await Task.Run(() => service.ReturnEncryptedRole(role));
                Properties.Settings.Default.userID = await Task.Run(() => service.ReturnLoggedUser(username));
                Properties.Settings.Default.Save();
            }

            switch (role)
            {
                case "Admin":
                    OpenMainWindow(new AdminMainWindow(role));
                    break;
                case "User":
                    OpenMainWindow(new UsersMainWindow(role));
                    break;
                case "Employee":
                    OpenMainWindow(new EmployeeMainWindow(role));
                    break;
                default:
                    lblError.Content = "Provjerite unesene podatake";
                    break;
            }
            loadingGif.Visibility = Visibility.Collapsed;
            txtLogin.Visibility = Visibility.Visible;
            btnLogin.IsEnabled = true;
        }

        private void OpenMainWindow(Window window)
        {
            Close();
            GuiManager.SetWindow(window);
            window.ShowDialog(); 
        }

        private void RegistrationLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var form = new Registration();
            Close();
            form.ShowDialog();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                ToggleFullScreen();
                e.Handled = true;
            }
        }

        private void ToggleFullScreen()
        {
            if (isFullScreen)
            {
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
            }

            isFullScreen = !isFullScreen;
        }
    }
}
