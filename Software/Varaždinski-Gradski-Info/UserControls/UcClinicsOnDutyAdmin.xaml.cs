using BusinessLogicLayer;
using EntitiesLayer;
using PresentationLayer;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcClinicsOnDutyAdmin : UserControl
    {
        private InstitutionService _institutionService;

        public UcClinicsOnDutyAdmin()
        {
            InitializeComponent();
            _institutionService = new InstitutionService();
            LoadInstitutions();
        }

        private void LoadInstitutions()
        {
            List<Institution> pharmacies = _institutionService.GetInstitutionsByType(1);

            InstitutionsList.ItemsSource = pharmacies;
        }

        private void EditInstitution_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int institutionId)
            {
                GuiManager.OpenContent(new UcEditPharmacy(institutionId));
            }
        }
    }
}
