using BusinessLogicLayer;
using DataAccessLayer;
using EntitiesLayer;
using PresentationLayer;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcAddNewEmployee : UserControl
    {
        private UsersService _userService;
        private InstitutionService institutionService;
        private User _currentUser;

        public UcAddNewEmployee()
        {
            InitializeComponent();
            _userService = new UsersService();
            institutionService = new InstitutionService();
            LoadUserTypes();
            LoadInstitutions();
        }

        public UcAddNewEmployee(User user) : this()
        {
            _currentUser = user;
            PopulateFields();
            btnAdd.Content = "Spremi izmjene";
        }

        private void PopulateFields()
        {
            if (_currentUser != null)
            {
                txtUsername.Text = _currentUser.Username;
                txtFirstName.Text = _currentUser.FirstName;
                txtLastName.Text = _currentUser.LastName;
                txtEmail.Text = _currentUser.Email;
                txtPhoneNumber.Text = _currentUser.PhoneNumber;
                cmbUserType.SelectedValue = _currentUser.ID_UserType;
                cmbInstitution.SelectedValue = _currentUser.ID_Institution;
                txtPassword.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadUserTypes()
        {
            cmbUserType.ItemsSource = _userService.GetUserTypes();
            cmbUserType.DisplayMemberPath = "TypeName";
            cmbUserType.SelectedValuePath = "ID_UserType";
        }

        private void LoadInstitutions()
        {
            cmbInstitution.ItemsSource = institutionService.GetAllInstitutions();
            cmbInstitution.DisplayMemberPath = "InstitutionName";
            cmbInstitution.SelectedValuePath = "ID_Institution";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
            {
                lblError.Visibility = Visibility.Visible;
                return;
            }

            if (_currentUser == null)
            {
                var newUser = new User
                {
                    Username = txtUsername.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    PhoneNumber = txtPhoneNumber.Text.Trim(),
                    Password = HashData(txtPassword.Password.Trim()),
                    ID_UserType = (int)cmbUserType.SelectedValue,
                    ID_Institution = cmbInstitution.SelectedItem != null ? (int?)cmbInstitution.SelectedValue : null,
                    IsSubscribed = null
                };

                try
                {
                    _userService.AddUser(newUser);
                    GuiManager.OpenContent(new UcShowEmployees());
                }
                catch (Exception ex)
                {
                    lblError.Content = "Došlo je do greške: " + ex.Message;
                }
            }
            else
            {
                _currentUser.Username = txtUsername.Text.Trim();
                _currentUser.FirstName = txtFirstName.Text.Trim();
                _currentUser.LastName = txtLastName.Text.Trim();
                _currentUser.Email = txtEmail.Text.Trim();
                _currentUser.PhoneNumber = txtPhoneNumber.Text.Trim();

                if (cmbUserType.SelectedValue != null)
                {
                    _currentUser.UserType = _userService.GetUserTypeById((int)cmbUserType.SelectedValue);
                    _currentUser.ID_UserType = (int)cmbUserType.SelectedValue;
                }

                if (cmbInstitution.SelectedValue != null)
                {
                    var institutionId = (int)cmbInstitution.SelectedValue;
                    _currentUser.Institution = institutionService.GetInstitutionById(institutionId);
                    _currentUser.ID_Institution = institutionId;
                }


                try
                {
                    _userService.UpdateUser(_currentUser);
                    GuiManager.OpenContent(new UcShowEmployees());
                }
                catch (Exception ex)
                {
                    lblError.Content = "Došlo je do greške: " + ex.Message;
                }
            }
        }

        private bool ValidateForm()
        {
            lblError.Content = "";
            lblError.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhoneNumber.Text) || cmbUserType.SelectedItem == null)
            {
                lblError.Content = "Sva polja moraju biti popunjena!";
                return false;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                lblError.Content = "Neispravan format email adrese!";
                return false;
            }

            if (!Regex.IsMatch(txtPhoneNumber.Text, @"^0[1-9][0-9][\s.-]?[0-9]{3,}([\s.-]?[0-9]+)*$"))
            {
                lblError.Content = "Neispravan format broja telefona!";
                return false;
            }

            if (!Regex.IsMatch(txtUsername.Text, @"^(?=.*[a-zA-Z]{4,})(?=^[a-zA-Z0-9]*[\W_]{0,1}[a-zA-Z0-9]*$).{4,15}$"))
            {
                lblError.Content = "Korisničko ime mora imati najmanje 4 slova!";
                return false;
            }

            if (_currentUser == null && _userService.UsernameInUse(txtUsername.Text.Trim()))
            {
                lblError.Content = "Korisničko ime već postoji!";
                return false;
            }

            if (_currentUser == null && _userService.EmailInUse(txtEmail.Text.Trim()))
            {
                lblError.Content = "Email adresa već postoji!";
                return false;
            }

            if (_currentUser == null && !Regex.IsMatch(txtPassword.Password, @"^(?=.*[0-9])(?=.*[\W_]).{8,}$"))
            {
                lblError.Content = "Lozinka mora imati najmanje 8 znakova, 1 broj i 1 specijalni znak!";
                return false;
            }

            return true;
        }

        private string HashData(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            GuiManager.OpenContent(new UcShowEmployees());
        }

        private void ClearForm()
        {
            txtUsername.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPhoneNumber.Clear();
            txtPassword.Clear();
            cmbUserType.SelectedIndex = -1;
            cmbInstitution.SelectedIndex = -1;
        }
    }
}
