using EntitiesLayer;
using PresentationLayer;
using System.Windows.Controls;
using BusinessLogicLayer;
using System.Windows;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcProblemResponse : UserControl
    {
        private Problem currentProblem;

        public UcProblemResponse(Problem problem)
        {
            InitializeComponent();
            currentProblem = problem;
            LoadProblemDetails();
        }

        private void LoadProblemDetails()
        {
            if (currentProblem != null)
            {
                txtProblemId.Text = $"Problem ID: #{currentProblem.ID_Problem}";
                txtReportDate.Text = $"Datum prijave problema: {currentProblem.ReportDate:dd-MM-yyyy HH:mm}";
                txtProblemName.Text = $"Naslov: {currentProblem.ProblemName}";
                txtProblemDescription.Text = $"Opis: {currentProblem.Description}";
                txtDateOccured.Text = $"Datum nastanka problema: {currentProblem.DateOccured:dd-MM-yyyy HH:mm}";
                txtProblemUser.Text = $"Prijavio korisnik sa ID: {currentProblem.ID_User.ToString() ?? "Nepoznato"}";
                txtProblemCategory.Text = $"Kategorija: {currentProblem.ID_ProblemCategory}";
                txtReply.Text = $"{currentProblem.ProblemReply ?? "Nema odgovora"}";
                txtEmployee.Text = $"Odgovorio: {currentProblem.User.Username?? "Nepoznato"}";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcTicketForum());
        }
    }
}
