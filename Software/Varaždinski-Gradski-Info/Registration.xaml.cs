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
using System.Text.RegularExpressions;
using WpfAnimatedGif;


namespace Varaždinski_Gradski_Info
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private bool isFullScreen = false;

        private void PrijavaLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var form = new Login();
            Close();
            form.ShowDialog();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            btnRegister.IsEnabled = false;
            ImageBehavior.SetAnimatedSource(loadingGif, new BitmapImage(new Uri("/UserControls/Assets/loadingGif.gif", UriKind.Relative)));
            loadingGif.Visibility = Visibility.Visible;
            txtRegister.Visibility = Visibility.Collapsed;

            var service = new UsersService();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phoneNumber = txtPhone.Text.Trim();
            string firstName = txtName.Text.Trim();
            string lastName = txtLastname.Text.Trim();
            string password = txtPassword.Password.Trim();

            lblError.Content = "";

            if (username == "" || email == "" || firstName == "" || lastName == "" || txtPassword.Password == "" || txtPasswordSec.Password == "")
            {
                lblError.Content = "Niste unijeli sve potrebne podatke";
            }
            else if (!UsernamePatternIsValid(username))
            {
                lblError.Content = "Korisničko ime mora sadržavati barem 4 slova.";
            }
            else if (!PasswordPatternIsValid(password))
            {
                lblError.Content = "Lozinka mora imati barem 8 znakova,\n uključujući 1 broj i 1 specijalni znak.";
            }
            else if (checkEmail(email))
            {
                lblError.Content = "Email se već koristi";
            }
            else if (checkUsername(username))
            {
                lblError.Content = "Korisničko ime se već koristi";
            }
            else if (!NamePatternIsValid(firstName))
            {
                lblError.Content = "Ime može sadržavati samo slova.";
            }
            else if (!NamePatternIsValid(lastName))
            {
                lblError.Content = "Prezime može sadržavati samo slova i jedan razmak ili jednu crticu.";
            }

            else if (!EmailPatternIsValid(email))
            {
                lblError.Content = "Email nema dobar format, npr. imeprezime@gmail.com";
            }
            else if (!PhonePatternIsValid(phoneNumber))
            {
                lblError.Content = "Broj telefona nema dobar format, npr. 098-123-1234";
            }
            else if (password == txtPasswordSec.Password)
            {
                if (!checkEmail(email) && !checkUsername(username))
                {
                    await Task.Run(() => service.RegisterUser(username, password, email, phoneNumber, firstName, lastName));
                    OpenMainWindow(new Login());
                }
            }
            else
            {
                lblError.Content = "Lozinke se ne podudaraju. Provjerite unos.";
            }

            loadingGif.Visibility = Visibility.Collapsed;
            txtRegister.Visibility = Visibility.Visible;
            btnRegister.IsEnabled = true;
        }


        private bool checkEmail(string email)
        {
            var service = new UsersService();
            bool inUse = service.EmailInUse(email);
            if (inUse) return true;
            return false;
        }
        private bool checkUsername(string username)
        {
            var service = new UsersService();
            bool inUse = service.UsernameInUse(username);
            if (inUse) return true;
            return false;
        }
        private void OpenMainWindow(Window window)
        {
            Close();
            GuiManager.SetWindow(window);
            window.ShowDialog();
        }

        public bool EmailPatternIsValid(string email)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }

        public bool PhonePatternIsValid(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return true;

            string pattern = @"^0[1-9][0-9][\s.-]?[0-9]{3,}([\s.-]?[0-9]+)*$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        private bool UsernamePatternIsValid(string username)
        {
            string pattern = @"^(?=.*[a-zA-Z]{4,})(?=^[a-zA-Z0-9]*[\W_]{0,1}[a-zA-Z0-9]*$).{4,15}$";
            return Regex.IsMatch(username, pattern);
        }

        private bool PasswordPatternIsValid(string password)
        {
            string pattern = @"^(?=.*[0-9])(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
        private bool NamePatternIsValid(string name)
        {
            string pattern = @"^[A-Za-z]+([ -]?[A-Za-z]+)?$";
            return Regex.IsMatch(name, pattern);
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
