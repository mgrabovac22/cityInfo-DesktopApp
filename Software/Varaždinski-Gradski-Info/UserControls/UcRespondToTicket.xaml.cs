using EntitiesLayer;
using PresentationLayer;
using System.Windows.Controls;
using BusinessLogicLayer;
using System.Windows;
using System;
using Varaždinski_Gradski_Info.Properties;
using System.Windows.Media;
using System.Threading.Tasks;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcRespondToTicket : UserControl
    {
        private Problem currentProblem;

        public UcRespondToTicket(Problem problem)
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

                
                txtProblemUser.Text = $"Prijavio: {currentProblem.User?.Username ?? "Nepoznato"}";
                txtProblemCategory.Text = $"Kategorija: {currentProblem.ID_ProblemCategory}";
            }
        }

        private async void SubmitReply_Click(object sender, RoutedEventArgs e)
        {
            lblMessage.Visibility = Visibility.Collapsed; 
            lblMessage.Foreground = Brushes.Green;

            if (currentProblem == null)
            {
                ShowMessage("Nema problema za ažuriranje.", Brushes.Red);
                return;
            }

            var replyText = txtReply.Text.Trim();
            if (string.IsNullOrWhiteSpace(replyText))
            {
                ShowMessage("Molimo unesite odgovor na problem.", Brushes.Red);
                return;
            }

            if (!(cmbStatus.SelectedItem is ComboBoxItem selectedStatusItem && selectedStatusItem.Content != null))
            {
                ShowMessage("Molimo odaberite status problema.", Brushes.Red);
                return;
            }

            var status = selectedStatusItem.Content.ToString();

            try
            {
                var userService = new UsersService();
                int employeeId = int.Parse(userService.ReturnDecryptedRole(Settings.Default.userID));

                var service = new ProblemService();
                service.UpdateProblem(currentProblem.ID_Problem, replyText, status, employeeId);

                ShowMessage("Problem uspješno ažuriran.", Brushes.Green);

              
                await Task.Delay(2000);

                GuiManager.OpenContent(new UcTicketForumEmployee());
            }
            catch (Exception ex)
            {
                ShowMessage($"Došlo je do greške prilikom ažuriranja problema: {ex.Message}", Brushes.Red);
            }
        }

        private void ShowMessage(string message, Brush color)
        {
            lblMessage.Content = message;
            lblMessage.Foreground = color;
            lblMessage.Visibility = Visibility.Visible;
        }





        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
         
            GuiManager.OpenContent(new UcTicketForumEmployee());
        }
    }
}
