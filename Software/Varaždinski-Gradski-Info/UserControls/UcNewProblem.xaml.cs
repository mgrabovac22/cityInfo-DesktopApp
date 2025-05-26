using BusinessLogicLayer;
using PresentationLayer;
using System.Globalization;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Varaždinski_Gradski_Info.Properties;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcNewProblem : UserControl
    {
        public UcNewProblem()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcTicketForum());
        }

        private async void SendTicket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblErrorProblemName.Visibility = Visibility.Collapsed;
                lblErrorDescription.Visibility = Visibility.Collapsed;
                lblErrorTime.Visibility = Visibility.Collapsed;
                lblErrorCategory.Visibility = Visibility.Collapsed;
                lblErrorDate.Visibility = Visibility.Collapsed;
                lblSuccessMessage.Visibility = Visibility.Collapsed;
                lblFailMessage.Visibility = Visibility.Collapsed;

                string problemName = txtProblemName.Text;
                string description = txtDescriptionProblem.Text;
                bool hasError = false;

                if (problemName.Length < 3 || problemName.Length > 255)
                {
                    lblErrorProblemName.Visibility = Visibility.Visible;
                    hasError = true;
                }

                if (description.Length < 10 || description.Length > 1000)
                {
                    lblErrorDescription.Visibility = Visibility.Visible;
                    hasError = true;
                }

                if (dpDateOccured.SelectedDate == null)
                {
                    lblErrorDate.Visibility = Visibility.Visible;
                    hasError = true;
                }

                if (cmbCategory.SelectedItem == null)
                {
                    lblErrorCategory.Visibility = Visibility.Visible;
                    hasError = true;
                }

                TimeSpan parsedTime = TimeSpan.Zero;

                string enteredTime = txtTimeOccured.Text.Trim();
                if (string.IsNullOrEmpty(enteredTime) || !TimeSpan.TryParseExact(enteredTime, "hh\\:mm", CultureInfo.InvariantCulture, out parsedTime))
                {
                    lblErrorTime.Visibility = Visibility.Visible;
                    hasError = true;
                }

                if (hasError) return;

                DateTime fullDateOccured = dpDateOccured.SelectedDate.Value.Date + parsedTime;
                int problemCategoryId = cmbCategory.SelectedIndex + 1;
                var servis = new UsersService();
                int userId = int.Parse(servis.ReturnDecryptedRole(Settings.Default.userID));

                string problemReply = null;
                var service = new ProblemService();
                service.AddProblem(problemName, description, fullDateOccured, problemCategoryId, userId, problemReply);

                lblSuccessMessage.Visibility = Visibility.Visible;

                await Task.Delay(2000);

              
                GuiManager.OpenContent(new UcTicketForum());
            }
            catch (Exception)
            {
                lblFailMessage.Visibility = Visibility.Visible;

            }
        }
    }
}
