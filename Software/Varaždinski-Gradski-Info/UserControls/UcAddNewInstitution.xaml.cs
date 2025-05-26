using BusinessLogicLayer;
using EntitiesLayer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using PresentationLayer;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcAddNewInstitution : UserControl
    {
        private InstitutionService _service;
        private Institution _currentInstitution;

        public UcAddNewInstitution()
        {
            InitializeComponent();
            _service = new InstitutionService();
            LoadInstitutionTypes();
        }

        public UcAddNewInstitution(Institution institution) : this()
        {
            _currentInstitution = institution;
            PopulateFields();
        }

        private void LoadInstitutionTypes()
        {
            cmbInstitutionType.ItemsSource = _service.GetInstitutionTypes();
            cmbInstitutionType.DisplayMemberPath = "TypeName";
            cmbInstitutionType.SelectedValuePath = "ID_Type";
        }

        private void PopulateFields()
        {
            if (_currentInstitution != null)
            {
                txtName.Text = _currentInstitution.InstitutionName;
                txtDescription.Text = _currentInstitution.Description;
                txtWorkingDays.Text = _currentInstitution.DaysOnDuty.ToString();
                txtPhoneNumber.Text = _currentInstitution.PhoneNumber;
                txtAdresa.Text = _currentInstitution.Address;
                txtWorkingHours.Text = _currentInstitution.WorkingHours;
                cmbInstitutionType.SelectedValue = _currentInstitution.ID_Type;

                btnAdd.Content = "Spremi izmjene";
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcShowEmployees());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidForm())
            {
                if (_currentInstitution == null)
                {
                    Institution inst = new Institution
                    {
                        InstitutionName = txtName.Text,
                        Description = txtDescription.Text,
                        DaysOnDuty = short.Parse(txtWorkingDays.Text),
                        PhoneNumber = txtPhoneNumber.Text,
                        Address = txtAdresa.Text,
                        WorkingHours = txtWorkingHours.Text,
                        ID_Type = cmbInstitutionType.SelectedItem != null ? (int?)cmbInstitutionType.SelectedValue : null,
                    };
                    _service.AddInstitution(inst);
                }
                else
                {
                    _currentInstitution.InstitutionName = txtName.Text;
                    _currentInstitution.Description = txtDescription.Text;
                    _currentInstitution.DaysOnDuty = short.Parse(txtWorkingDays.Text);
                    _currentInstitution.PhoneNumber = txtPhoneNumber.Text;
                    _currentInstitution.Address = txtAdresa.Text;
                    _currentInstitution.WorkingHours = txtWorkingHours.Text;
                    _currentInstitution.ID_Type = cmbInstitutionType.SelectedItem != null ? (int?)cmbInstitutionType.SelectedValue : null;

                    if (cmbInstitutionType.SelectedValue != null)
                    {
                        var institutionTypeId = (int)cmbInstitutionType.SelectedValue;
                        _currentInstitution.InstitutionType = _service.GetInstitutionTypeById(institutionTypeId);
                        _currentInstitution.ID_Type = institutionTypeId;
                    }

                    _service.UpdateInstitution(_currentInstitution);
                }

                GuiManager.OpenContent(new UcShowEmployees());
            }
        }

        private bool IsValidForm()
        {
            lblError.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAdresa.Text) ||
                string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(txtWorkingDays.Text) ||
                string.IsNullOrWhiteSpace(txtWorkingHours.Text) ||
                cmbInstitutionType.SelectedItem == null)
            {
                lblError.Content = "Sva polja moraju biti popunjena!";
                lblError.Visibility = Visibility.Visible;
                return false;
            }

            if (!short.TryParse(txtWorkingDays.Text, out short workingDays) || workingDays < 1 || workingDays > 7)
            {
                lblError.Content = "Radni dani moraju biti broj između 1 i 7!";
                lblError.Visibility = Visibility.Visible;
                return false;
            }

            if (!Regex.IsMatch(txtPhoneNumber.Text, @"^\+?[\d\s\-()]{10,15}$"))
            {
                lblError.Content = "Broj telefona nije u ispravnom formatu! Format: xxx-xxx-xxx!";
                lblError.Visibility = Visibility.Visible;
                return false;
            }

            if (!Regex.IsMatch(txtWorkingHours.Text, @"^\d{1,2}:\d{2}\s*-\s*\d{1,2}:\d{2}$"))
            {
                lblError.Content = "Radni sati nisu u ispravnom formatu! (npr. 9:00 - 17:00)";
                lblError.Visibility = Visibility.Visible;
                return false;
            }

            return true;
        }
    }
}
