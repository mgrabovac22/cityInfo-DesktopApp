using BusinessLogicLayer;
using EntitiesLayer;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcClinicsOnDuty : UserControl
    {
        private InstitutionService _institutionService;

        public UcClinicsOnDuty()
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
    }
}
