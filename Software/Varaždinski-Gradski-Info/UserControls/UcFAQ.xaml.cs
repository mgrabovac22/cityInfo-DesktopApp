using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BusinessLogicLayer;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcFAQ : UserControl
    {
        private ChatGPTService _chatGPTService;
        private List<(string Question, string Answer, string Category)> _allQuestions;
        private List<(string Question, string Answer, string Category)> _filteredQuestions;
        private int _currentQuestionIndex;
        private const int QuestionsPerPage = 20;
        private string selectedCategory = "";

        public UcFAQ()
        {
            InitializeComponent();
            _chatGPTService = new ChatGPTService("ApiKey");

            _allQuestions = new List<(string Question, string Answer, string Category)>
            {
                ("1. Kako prijaviti nestanak vode?", "Nestanak vode možete prijaviti putem aplikacije ili telefonski.", "Zdravstvo"),
                ("2. Kako prijaviti problem s odvozom otpada?", "Probleme s odvozom možete prijaviti u sekciji 'Prijava problema'.", "Odvoz smeća"),
                ("3. Kako mogu dobiti obavijesti o hitnim slučajevima?", "Obavijesti možete pratiti u sekciji 'Obavijesti'.", "Zdravstvo"),
                ("4. Kako prijaviti ilegalno odlaganje otpada?", "Ilegalno odlaganje otpada možete prijaviti u aplikaciji ili pozivom na nadležnu službu.", "Odvoz smeća"),
                ("5. Kako mogu vidjeti stanje računa?", "Račun možete provjeriti u sekciji 'Računi' u svom korisničkom profilu.", "Tiketi"),
                ("6. Kada su radni sati gradske uprave?", "Gradska uprava je otvorena od 8:00 do 19:00 svaki radni dan.", "Infrastruktura"),
                ("7. Kako se pretplatiti na obavijesti?", "Pretplatu možete aktivirati u sekciji 'Obavijesti' u aplikaciji.", "Zdravstvo"),
                ("8. Kako prijaviti problem s javnim svjetlom?", "Probleme s javnim svjetlom možete prijaviti putem aplikacije ili telefonom.", "Infrastruktura"),
                ("9. Gdje se nalaze najbliže javne WC-e?", "Najbliže javne WC-e možete pronaći putem mape u aplikaciji.", "Infrastruktura"),
                ("10. Kako prijaviti nelegalnu gradnju?", "Nelegalnu gradnju možete prijaviti na službenoj stranici grada ili pozivom na gradsku upravu.", "Infrastruktura"),
                ("11. Kako mogu dobiti upute za parkiranje?", "Upute za parkiranje možete dobiti putem aplikacije 'Parking Varaždin'.", "Infrastruktura"),
                ("12. Koji su uvjeti za izdavanje dozvola za gradnju?", "Uvjeti za izdavanje dozvola mogu se pronaći na službenoj stranici grada.", "Infrastruktura"),
                ("13. Kako se mogu prijaviti za volonterski rad?", "Za volonterski rad možete se prijaviti putem aplikacije ili grada.", "Zdravstvo"),
                ("14. Kako mogu dobiti subvenciju za grijanje?", "Subvencije za grijanje možete dobiti prijavom u aplikaciji grada.", "Infrastruktura"),
                ("15. Koje su lokacije dežurnih ljekarni?", "Popis dežurnih ljekarni možete pronaći u aplikaciji ili na službenoj stranici grada.", "Zdravstvo"),
                ("16. Kako saznati radno vrijeme dežurnih ljekarni?", "Radno vrijeme dežurnih ljekarni dostupno je u sekciji 'Ljekarne' u aplikaciji.", "Zdravstvo"),
                ("17. Kako prijaviti prepun kontejner za otpad?", "Prijavu možete izvršiti putem opcije 'Prijava problema' u aplikaciji.", "Odvoz smeća"),
                ("18. Gdje mogu naći informaciju o odvozu glomaznog otpada?", "Informacije o odvozu glomaznog otpada nalaze se u sekciji 'Odvoz otpada'.", "Odvoz smeća"),
                ("19. Kada se vrši odvoz biootpada?", "Raspored odvoza biootpada dostupan je u aplikaciji.", "Odvoz smeća"),
                ("20. Kako mogu naručiti poseban odvoz otpada?", "Poseban odvoz otpada možete naručiti putem aplikacije ili telefonski.", "Odvoz smeća"),
                ("21. Što učiniti ako je ljekarna zatvorena?", "Za hitne slučajeve kontaktirajte dežurnu ljekarnu ili hitnu pomoć.", "Zdravstvo"),
                ("22. Kako mogu saznati gdje se nalaze spremnici za reciklažu?", "Lokacije spremnika za reciklažu možete pronaći na mapi u aplikaciji.", "Odvoz smeća"),
                ("23. Kako prijaviti štetu na kontejneru za otpad?", "Štetu možete prijaviti putem opcije 'Prijava problema' u aplikaciji.", "Odvoz smeća"),
                ("24. Gdje mogu naći upute za razvrstavanje otpada?", "Upute za razvrstavanje otpada nalaze se u sekciji 'Reciklaža'.", "Odvoz smeća"),
                ("25. Kako prijaviti nedostatak medicinskih zaliha?", "Prijavu možete izvršiti putem sekcije 'Medicinska pomoć' u aplikaciji.", "Zdravstvo")
            };

            _filteredQuestions = _allQuestions.ToList();
            _currentQuestionIndex = 0;

            DisplayQuestions();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = userMessageTextBox.Text;
            if (string.IsNullOrEmpty(userMessage))
            {
                errorLabel.Content = "Molimo unesite pitanje.";
                return;
            }

            if (initialMessageTextBlock != null)
            {
                chatMessagesStackPanel.Children.Remove(initialMessageTextBlock);
                initialMessageTextBlock = null;
            }

            chatMessagesStackPanel.Children.Add(new Border
            {
                Background = new SolidColorBrush(Colors.LightBlue),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10),
                Child = new TextBlock { Text = $"Korisnik: {userMessage}", TextWrapping = TextWrapping.Wrap }
            });

            userMessageTextBox.Clear();

            string response = await _chatGPTService.GetResponseAsync(userMessage);

            chatMessagesStackPanel.Children.Add(new Border
            {
                Background = new SolidColorBrush(Colors.LightGray),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10),
                Child = new TextBlock { Text = $"Asistent: {response}", TextWrapping = TextWrapping.Wrap }
            });
        }

        private void UserMessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void DisplayQuestions()
        {
            faqStackPanel.Children.Clear();

            var questionsToShow = _filteredQuestions.Skip(_currentQuestionIndex).Take(QuestionsPerPage).ToList();

            foreach (var question in questionsToShow)
            {
                var headerTextBlock = new TextBlock
                {
                    Text = question.Question,
                    TextWrapping = TextWrapping.Wrap,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16
                };

                var expander = new Expander
                {
                    Header = headerTextBlock, 
                    IsExpanded = false
                };

                expander.Content = new TextBlock
                {
                    Text = question.Answer,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10),
                    FontWeight = FontWeights.Regular,
                    FontSize = 14
                };

                faqStackPanel.Children.Add(expander);
            }

            _currentQuestionIndex += questionsToShow.Count;

            LoadMoreQuestionsButton.Visibility = _currentQuestionIndex < _filteredQuestions.Count ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadMoreQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayQuestions();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower();
            FilterQuestions();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                selectedCategory = selectedItem.Content.ToString();
                FilterQuestions();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _currentQuestionIndex = 0;

            categoryComboBox.SelectedIndex = 0; 

            selectedCategory = ""; 

            searchTextBox.Clear();

            FilterQuestions();
        }


        private void FilterQuestions()
        {
            string searchText = searchTextBox.Text.ToLower();

            _filteredQuestions = _allQuestions
                .Where(q => (string.IsNullOrEmpty(selectedCategory) || selectedCategory == "Sve" || q.Category == selectedCategory) &&
                            (string.IsNullOrEmpty(searchText) || q.Question.ToLower().Contains(searchText)))
                .ToList();

            _currentQuestionIndex = 0;
            DisplayQuestions();
        }
    }
}
