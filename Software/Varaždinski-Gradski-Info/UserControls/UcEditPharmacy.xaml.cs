using BusinessLogicLayer;
using EntitiesLayer;
using PresentationLayer;
using System.Windows;
using System.Windows.Controls;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcEditPharmacy : UserControl
    {
        private InstitutionService _institutionService;
        private Institution _institution;

        public UcEditPharmacy(int institutionId)
        {
            InitializeComponent();
            _institutionService = new InstitutionService();
            LoadInstitutionData(institutionId);
        }

        private void LoadInstitutionData(int institutionId)
        {
            _institution = _institutionService.GetInstitutionById(institutionId);
            if (_institution != null)
            {
                txtInstitutionName.Text = _institution.InstitutionName;
                txtAddress.Text = _institution.Address;
                txtPhoneNumber.Text = _institution.PhoneNumber;
                txtWorkingHours.SelectedItem = _institution.WorkingHours;
                txtDescription.Text = _institution.Description;
                txtDaysOnDuty.Text = _institution.DaysOnDuty.ToString();
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_institution != null)
            {
                _institution.Address = txtAddress.Text;
                _institution.PhoneNumber = txtPhoneNumber.Text;

              
                if (txtWorkingHours.SelectedItem is ComboBoxItem selectedItem)
                {
                    _institution.WorkingHours = selectedItem.Content.ToString();
                }

                _institution.Description = txtDescription.Text;

                if (short.TryParse(txtDaysOnDuty.Text, out short days))
                {
                    _institution.DaysOnDuty = days;
                }

                _institutionService.UpdateInstitution(_institution);
              
                GuiManager.OpenContent(new UcClinicsOnDutyAdmin());
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcClinicsOnDutyAdmin());
        }
    }
}
