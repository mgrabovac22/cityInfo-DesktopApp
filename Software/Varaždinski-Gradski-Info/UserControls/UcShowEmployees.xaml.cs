using BusinessLogicLayer;
using DataAccessLayer;
using EntitiesLayer;
using PresentationLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Varaždinski_Gradski_Info.UserControls
{
    /// <summary>
    /// Interaction logic for UcShowEmployees.xaml
    /// </summary>
    public partial class UcShowEmployees : UserControl
    {
        private UsersService usersService;
        private InstitutionService institutionService;
        private ICollectionView _sluzbeniciCollectionView;
        public UcShowEmployees()
        {
            InitializeComponent();
            usersService = new UsersService();
            institutionService = new InstitutionService();
        } 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcAddNewEmployee());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayAllData();
        }

        private void DisplayAllData()
        {
            var institutions = institutionService.GetAllInstitutions();
            var filteredInstitutions = institutions.Select(x => new
            {
                x.InstitutionName,
                x.Description,
                x.PhoneNumber,
                x.WorkingHours,
                x.DaysOnDuty,
                x.Address,
                InstitutionType = x.InstitutionType?.TypeName ?? "N/A",
                InstitutionObject = x
            });

            GradskaTijelaGrid.ItemsSource = filteredInstitutions.ToList();

            var employees = usersService.GetAllEmployees();
            var filteredEmployees = employees.Select(x => new
            {
                x.Username,
                x.FirstName,
                x.LastName,
                x.Email,
                TypeName = x.UserType?.TypeName,
                InstitutionName = x.Institution?.InstitutionName,
                x.PhoneNumber,
                EmployeeObject = x
            }).ToList();

            _sluzbeniciCollectionView = CollectionViewSource.GetDefaultView(filteredEmployees);
            _sluzbeniciCollectionView.Filter = FilterUsers; 
            SluzbeniciGrid.ItemsSource = _sluzbeniciCollectionView;
        }

        private bool FilterUsers(object item)
        {
            var user = item as dynamic;
            if (user == null) return false;

            if (!string.IsNullOrEmpty(txtSearch.Text) &&
                !(user.Username.ToLower().Contains(txtSearch.Text.ToLower()) ||
                  user.FirstName.ToLower().Contains(txtSearch.Text.ToLower()) ||
                  user.LastName.ToLower().Contains(txtSearch.Text.ToLower()) ||
                  user.Email.ToLower().Contains(txtSearch.Text.ToLower())))
            {
                return false;
            }

            var selectedItem = cmbUserFilter.SelectedItem as ComboBoxItem;
            string selectedRole = selectedItem?.Tag?.ToString();
            if (!string.IsNullOrEmpty(selectedRole) && selectedRole != "All" && user.TypeName != selectedRole)
            {
                return false;
            }

            return true; 
        }


        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _sluzbeniciCollectionView.Refresh(); 
        }

        private void DisplayEmployeesForInstitution()
        {
            var selectedRow = GradskaTijelaGrid.SelectedItem;
            var selectedInstitution = (selectedRow as dynamic)?.InstitutionObject as Institution;

            if (selectedInstitution != null)
            {
                var employees = usersService.GetEmployeesForInstitution(selectedInstitution.ID_Institution);
                var filteredEmployees = employees.Select(x => new
                {
                    x.Username,
                    x.FirstName,
                    x.LastName,
                    x.Email,
                    TypeName = x.UserType?.TypeName,  
                    InstitutionName = x.Institution?.InstitutionName,
                    x.PhoneNumber,
                    EmployeeObject = x
                });

                SluzbeniciGrid.ItemsSource = filteredEmployees.ToList();
            }
        }

        private void GradskaTijelaGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GradskaTijelaGrid.SelectedItem != null)
            {
                SluzbeniciGrid.SelectedItem = null;

                btnUpdateInst.Visibility = Visibility.Visible;
                btnRemoveInst.Visibility = Visibility.Visible;
                btnUpdateEmp.Visibility = Visibility.Collapsed;
                btnRemoveEmp.Visibility = Visibility.Collapsed;


                DisplayEmployeesForInstitution();
            }
        }

        private void SluzbeniciGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SluzbeniciGrid.SelectedItem != null)
            {
                GradskaTijelaGrid.SelectedItem = null;

                btnUpdateEmp.Visibility = Visibility.Visible;
                btnRemoveEmp.Visibility = Visibility.Visible;
                btnUpdateInst.Visibility = Visibility.Collapsed;
                btnRemoveInst.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAddInst_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcAddNewInstitution());
        }

        private void btnRemoveInst_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = GradskaTijelaGrid.SelectedItem;
            var selectedInstitution = (selectedRow as dynamic)?.InstitutionObject as Institution;

            institutionService.RemoveInstitution(selectedInstitution);
            DisplayAllData();
        }

        private void btnRemoveEmp_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = SluzbeniciGrid.SelectedItem;
            var selectedUser = (selectedRow as dynamic)?.EmployeeObject as User;

            usersService.RemoveUser(selectedUser);
            DisplayAllData();
        }

        private void btnUpdateInst_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = GradskaTijelaGrid.SelectedItem;
            var selectedInstitution = (selectedRow as dynamic)?.InstitutionObject as Institution;
            GuiManager.OpenContent(new UcAddNewInstitution(selectedInstitution));

        }

        private void btnUpdateEmp_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = SluzbeniciGrid.SelectedItem;
            var selectedUser = (selectedRow as dynamic)?.EmployeeObject as User;
            GuiManager.OpenContent(new UcAddNewEmployee(selectedUser));
        }

        private void cmbUserFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = cmbUserFilter.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;

            string selectedRole = selectedItem.Tag?.ToString();

            LoadUsersByRole(selectedRole);
            _sluzbeniciCollectionView.Refresh();
        }

        private void DisplayAllUsers()
        {

            var users = usersService.GetAllUsers();


            var filteredUsers = users.Select(x => new
            {
                x.Username,
                x.FirstName,
                x.LastName,
                x.Email,
                TypeName = x.UserType?.TypeName,
                InstitutionName = x.Institution?.InstitutionName,
                x.PhoneNumber,
                EmployeeObject = x
            });

            SluzbeniciGrid.ItemsSource = filteredUsers.ToList();
        }

        private void LoadUsersByRole(string role)
        {

            if (string.IsNullOrEmpty(role) || role == "All")
            {
                var users = usersService.GetAllUsers();
                var filteredUsers = users.Select(x => new
                {
                    x.Username,
                    x.FirstName,
                    x.LastName,
                    x.Email,
                    TypeName = x.UserType?.TypeName,
                    InstitutionName = x.Institution?.InstitutionName,
                    x.PhoneNumber,
                    EmployeeObject = x
                }).ToList();

                _sluzbeniciCollectionView = CollectionViewSource.GetDefaultView(filteredUsers);
                _sluzbeniciCollectionView.Filter = FilterUsers;
                SluzbeniciGrid.ItemsSource = _sluzbeniciCollectionView;
            }
            else
            {
                var users = usersService.GetUsersByRole(role);
                var filteredUsers = users.Where(x => x.UserType?.TypeName == role)
                    .Select(x => new
                    {
                        x.Username,
                        x.FirstName,
                        x.LastName,
                        x.Email,
                        TypeName = x.UserType?.TypeName,
                        InstitutionName = x.Institution?.InstitutionName,
                        x.PhoneNumber,
                        EmployeeObject = x
                    }).ToList();

                _sluzbeniciCollectionView = CollectionViewSource.GetDefaultView(filteredUsers);
                _sluzbeniciCollectionView.Filter = FilterUsers;
                SluzbeniciGrid.ItemsSource = _sluzbeniciCollectionView;
            }
        }
    }
}
