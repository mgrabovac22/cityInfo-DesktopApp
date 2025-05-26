using BusinessLogicLayer;
using EntitiesLayer;
using PresentationLayer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Varaždinski_Gradski_Info.Properties;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcMyTickets : UserControl
    {
        public UcMyTickets()
        {
            InitializeComponent();
            LoadMyTickets(); 
        }

        private void LoadMyTickets()
        {
            var service = new ProblemService();
            var userService = new UsersService();

          
            int userId = int.Parse(userService.ReturnDecryptedRole(Settings.Default.userID));

          
            var userProblems = service.GetMyProblems(userId);

          
            var problemWrappers = new List<ProblemWrapper>();
            foreach (var problem in userProblems)
            {
                problemWrappers.Add(new ProblemWrapper { OriginalProblem = problem });
            }

          
            MyTicketsList.ItemsSource = problemWrappers;
        }

        public class ProblemWrapper
        {
            public Problem OriginalProblem { get; set; }
            public Visibility ButtonVisibility => string.IsNullOrWhiteSpace(OriginalProblem?.ProblemReply)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void ViewResponse_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ProblemWrapper wrapper)
            {
                GuiManager.OpenContent(new UcProblemResponse(wrapper.OriginalProblem));
            }
        }

        private void BackToForum_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcTicketForum());
        }
    }
}
