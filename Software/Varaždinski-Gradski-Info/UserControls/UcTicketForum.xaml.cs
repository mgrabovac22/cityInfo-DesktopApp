using BusinessLogicLayer;
using EntitiesLayer;
using PresentationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Varaždinski_Gradski_Info.Properties;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcTicketForum : UserControl
    {
        public UcTicketForum()
        {
            InitializeComponent();
         
            InitializePlaceholder();
            LoadTickets();
           
        }

        private int currentPage = 1;
        private int pageSize = 5;
        private int totalPages = 1;

        private void LoadTickets()
        {
            var service = new ProblemService();
            var (problems, totalRecords) = service.GetPaginatedProblems(currentPage, pageSize);

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var problemWrappers = problems.Select(p => new ProblemWrapper { OriginalProblem = p }).ToList();
            TicketsList.ItemsSource = problemWrappers;
            txtboxPretrazi.Clear();

            UpdatePaginationControls();
        }

        private void UpdatePaginationControls()
        {
            prevPageButton.IsEnabled = currentPage > 1;
            nextPageButton.IsEnabled = currentPage < totalPages;
            pageLabel.Content = $"Stranica {currentPage} od {totalPages}";
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadTickets();
            }
        }

        private void PrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadTickets();
            }
        }

        public class ProblemWrapper
        {
            public Problem OriginalProblem { get; set; }
            public Visibility ButtonVisibility => string.IsNullOrWhiteSpace(OriginalProblem?.ProblemReply)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void NewTicket_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcNewProblem());
        }

        private void InitializePlaceholder()
        {
            txtboxPretrazi.Text = "Pretražite problem";
            txtboxPretrazi.Foreground = Brushes.Gray;
        }

    
        private void Button_Click(object sender, RoutedEventArgs e)
        {
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

          
            TicketsList.ItemsSource = problemWrappers;
            txtboxPretrazi.Clear();
            categoryComboBox.SelectedIndex = -1;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            lblNoResults.Visibility = Visibility.Collapsed;
            var service = new ProblemService();

            string searchText = string.IsNullOrWhiteSpace(txtboxPretrazi.Text) ? null : txtboxPretrazi.Text.Trim();


            int? selectedCategoryId = null;
            if (categoryComboBox.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Tag?.ToString(), out int categoryId))
            {
                selectedCategoryId = categoryId;
            }

            int? sortOrder = null;
            if (sortComboBox.SelectedItem is ComboBoxItem sortItem && int.TryParse(sortItem.Tag?.ToString(), out int order))
            {
                sortOrder = order;
            }

         
            if (searchText == null && selectedCategoryId == null && sortOrder == null)
            {
                LoadTickets();
                return;
            }

          
            var filteredProblems = service.SearchProblems(
                string.IsNullOrWhiteSpace(searchText) ? null : searchText,
                selectedCategoryId,
                sortOrder);

            if (filteredProblems != null && filteredProblems.Count > 0)
            {
                var problemWrappers = new List<ProblemWrapper>();
                foreach (var problem in filteredProblems)
                {
                    problemWrappers.Add(new ProblemWrapper { OriginalProblem = problem });
                }

                TicketsList.ItemsSource = problemWrappers;
            }
            else
            {
                TicketsList.ItemsSource = null;
                lblNoResults.Visibility = Visibility.Visible;
            }
        }


        private void OpenMyTickets(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcMyTickets());
        }


        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
             LoadTickets();
            lblNoResults.Visibility = Visibility.Collapsed;
            txtboxPretrazi.Clear();
            categoryComboBox.SelectedIndex = -1;
           
        }
        private void ViewResponse_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ProblemWrapper wrapper)
            {
                GuiManager.OpenContent(new UcProblemResponse(wrapper.OriginalProblem));
            }
        }



    }
}
