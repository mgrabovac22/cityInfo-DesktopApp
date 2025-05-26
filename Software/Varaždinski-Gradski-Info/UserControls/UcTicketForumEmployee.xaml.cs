using BusinessLogicLayer;
using EntitiesLayer;
using PresentationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcTicketForumEmployee : UserControl
    {
        public UcTicketForumEmployee()
        {
            InitializeComponent();
            InitializePlaceholder();
            LoadTickets();
           
        }

        private void RespondToTicket_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedProblem = button?.DataContext as ProblemWrapper;

            if (selectedProblem?.OriginalProblem != null)
            {
                GuiManager.OpenContent(new UcRespondToTicket(selectedProblem.OriginalProblem));
            }
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

         
            public string Status => OriginalProblem?.Status ?? "Nepoznato";
            public string Username => OriginalProblem?.User?.Username ?? "Nepoznato";


            public Visibility ButtonVisibility =>
                (OriginalProblem.Status == "Riješen") ? Visibility.Collapsed : Visibility.Visible;
        }


        private void InitializePlaceholder()
        {
            txtboxPretrazi.Text = "Pretražite problem";
            txtboxPretrazi.Foreground = Brushes.Gray;
        }

      
       

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            LoadTickets();
            txtboxPretrazi.Clear();
            categoryComboBox.SelectedIndex = -1;
            statusComboBox.SelectedIndex = -1;
            lblNoResults.Visibility = Visibility.Collapsed;
          

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

            string selectedStatus = null;
            if (statusComboBox.SelectedItem is ComboBoxItem statusItem && statusItem.Content != null)
            {
                selectedStatus = statusItem.Content.ToString();
                if (selectedStatus == "Stanje problema")
                {
                    selectedStatus = null; 
                }
            }

            if (searchText == null && selectedCategoryId == null && sortOrder == null && selectedStatus == null)
            {
             
                return;
            }

            var filteredProblems = service.SearchProblems(
                string.IsNullOrWhiteSpace(searchText) ? null : searchText,
                selectedCategoryId,
                sortOrder);

            if (filteredProblems != null && filteredProblems.Count > 0)
            {
              
                if (!string.IsNullOrWhiteSpace(selectedStatus))
                {
                    filteredProblems = filteredProblems.Where(p =>
                        (p.Status == null && selectedStatus == "Novi problem") ||
                        (p.Status != null && p.Status == selectedStatus)).ToList();
                }

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





    }
}
