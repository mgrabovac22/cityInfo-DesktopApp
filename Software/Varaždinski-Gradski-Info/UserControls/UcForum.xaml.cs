using BusinessLogicLayer;
using EntitiesLayer;
using PresentationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Varaždinski_Gradski_Info.Properties;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcForum : UserControl
    {
        private int currentPage = 1;
        private const int itemsPerPage = 5;
        private int totalSuggestionsCount = 0;
        private List<Suggestion> allSuggestions = new List<Suggestion>();

        private int commentsPerPage = 2;
        private Dictionary<int, int> commentDisplayCount = new Dictionary<int, int>();

        public UcForum()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcAddNewSuggestion());
        }


        private async void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            var service = new ForumService();
            var userService = new UsersService();
            int currentUserId = int.Parse(userService.ReturnDecryptedRole(Settings.Default.userID));

            totalSuggestionsCount = await service.GetTotalSuggestionsCount();

            var suggestionsForCurrentPage = await service.GetSuggestionsByPage(currentPage, itemsPerPage);

            SuggestionsList.Items.Clear();

            foreach (var suggestion in suggestionsForCurrentPage)
            {
                StackPanel suggestionPanel = new StackPanel();
                double fixedPanelWidth = this.ActualWidth - 100;
                suggestionPanel.Width = fixedPanelWidth;

                var commentService = new CommentService();
                var comments = commentService.GetCommentsForSuggestion(suggestion.ID_Suggestion);


                StackPanel userPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 10),
                };

                Button btnLoadMoreComments = new Button
                {
                    Content = "Učitaj još",
                    Width = 80,
                    Height = 20,
                    Margin = new Thickness(10, 0, 0, 0),
                    Tag = suggestion.ID_Suggestion,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#388E3C")),
                    BorderThickness = new Thickness(1),
                    VerticalAlignment = VerticalAlignment.Center
                };

                btnLoadMoreComments.Click += BtnLoadMoreComments_Click;

                Button btnHideComments = new Button
                {
                    Content = "Sakrij",
                    Width = 80,
                    Height = 20,
                    Margin = new Thickness(10, 0, 10, 0),
                    Tag = suggestion.ID_Suggestion,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F")),
                    BorderThickness = new Thickness(1),
                    VerticalAlignment = VerticalAlignment.Center
                };
                btnHideComments.Click += BtnHideComments_Click;

                userPanel.Children.Add(btnLoadMoreComments);
                userPanel.Children.Add(btnHideComments);

                TextBlock title = new TextBlock
                {
                    Text = suggestion.SuggestionName,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold
                };

                TextBlock user = new TextBlock
                {
                    Text = suggestion.User.Username,
                    FontSize = 12,
                    Foreground = Brushes.Gray,
                    Margin = new Thickness(10, 5, 0, 0)
                };

                TextBlock date = new TextBlock
                {
                    Text = $"Objavljeno: {suggestion.SuggestionDateCreated:dd-MM-yyyy HH:mm}",
                    FontSize = 12,
                    Foreground = Brushes.Gray,
                    Margin = new Thickness(0, 5, 0, 0)
                };

                TextBlock content = new TextBlock
                {
                    Text = suggestion.SuggestionContent,
                    TextWrapping = TextWrapping.Wrap,
                    Width = suggestionPanel.Width - 20,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    FontWeight = FontWeights.Bold
                };

                StringBuilder commentText = new StringBuilder();

                TextBox txtMainComment = new TextBox
                {
                    Width = 300,
                    Margin = new Thickness(0, 0, 10, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    Name = "txtMainComment"
                };

                Button btnMainComment = new Button
                {
                    Content = "➤",
                    Width = 30,
                    Height = 30,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50")),
                    Foreground = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsEnabled = false
                };

                btnMainComment.Tag = suggestion;
                btnMainComment.Click += btnMainComment_Click;

                txtMainComment.TextChanged += (s, ev) =>
                {
                    btnMainComment.IsEnabled = !string.IsNullOrWhiteSpace(txtMainComment.Text);
                };

                StackPanel buttonPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                bool isLiked = service.IsSuggestionLiked(suggestion.ID_Suggestion, currentUserId);

                Button btnLikeDislike = new Button
                {
                    Width = 40,
                    Height = 40,
                    Padding = new Thickness(5),
                    Margin = new Thickness(10, 0, 0, 0),
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0)
                };

                Image likeImage = new Image
                {
                    Source = new BitmapImage(new Uri(isLiked
                    ? "pack://application:,,,/UserControls/Assets/dislike.png"
                    : "pack://application:,,,/UserControls/Assets/like.png")),
                    Width = 30,
                    Height = 30
                };


                btnLikeDislike.Content = likeImage;

                if (isLiked)
                {
                    btnLikeDislike.Click += btnDislike_Click;
                }
                else
                {
                    btnLikeDislike.Click += btnLike_Click;
                }

                btnLikeDislike.Tag = suggestion;

                TextBlock likeCount = new TextBlock
                {
                    Text = $"Broj lajkova: {suggestion.SuggestionLikes}",
                    FontSize = 12,
                    Foreground = Brushes.Gray,
                    Margin = new Thickness(10, 10, 0, 0)
                };
                userPanel.Children.Add(title);
                userPanel.Children.Add(user);
                buttonPanel.Children.Add(txtMainComment);
                buttonPanel.Children.Add(btnMainComment);
                buttonPanel.Children.Add(btnLikeDislike);
                buttonPanel.Children.Add(likeCount);

                suggestionPanel.Children.Add(userPanel);
                suggestionPanel.Children.Add(date);
                suggestionPanel.Children.Add(content);

                if (!commentDisplayCount.ContainsKey(suggestion.ID_Suggestion))
                {
                    commentDisplayCount[suggestion.ID_Suggestion] = commentsPerPage;
                }

                var displayedComments = comments.Take(commentDisplayCount[suggestion.ID_Suggestion]).ToList();

                foreach (var comment in displayedComments)

                {
                    TextBlock commentTextBlock = new TextBlock
                    {
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(20, 10, 0, 10),
                        Padding = new Thickness(10),
                    };

                    Run dateRun = new Run
                    {
                        Text = $"{comment.User.Username} {comment.CommentDate:dd-MM-yyyy HH:mm}\n",
                        FontSize = 10,
                        Foreground = Brushes.Gray
                    };

                    Run commentRun = new Run
                    {
                        Text = comment.CommentContent + "\n",
                    };

                    commentTextBlock.Inlines.Add(dateRun);
                    commentTextBlock.Inlines.Add(commentRun);
                    Border endLine = new Border
                    {
                        Background = Brushes.Gray,
                        Height = 1,
                        Margin = new Thickness(0, 10, 0, 10)
                    };

                    suggestionPanel.Children.Add(endLine);
                    suggestionPanel.Children.Add(commentTextBlock);
                }

                suggestionPanel.Children.Add(buttonPanel);

                Border border = new Border
                {
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(5),
                    Padding = new Thickness(10),
                    Margin = new Thickness(0, 0, 0, 10),
                    Background = new SolidColorBrush(Colors.LightBlue),
                    Child = suggestionPanel
                };

                SuggestionsList.Items.Add(border);

            }

            UpdatePaginationControls();
        }

        private void UpdatePaginationControls()
        {
            pageInfo.Text = $"Stranica {currentPage} od {Math.Ceiling((double)totalSuggestionsCount / itemsPerPage)}";

            btnPrevious.IsEnabled = currentPage > 1;
            btnNext.IsEnabled = currentPage * itemsPerPage < totalSuggestionsCount;
        }

        private void btnLike_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var suggestion = button?.Tag as Suggestion;
            var userService = new UsersService();
            int currentUserId = int.Parse(userService.ReturnDecryptedRole(Settings.Default.userID));

            if (suggestion != null)
            {
                var service = new ForumService();
                bool likeAdded = service.AddLike(suggestion.ID_Suggestion, currentUserId);

                if (likeAdded)
                {
                    suggestion.SuggestionLikes += 1;
                    button.Content = "Ne sviđa mi se";
                    RefreshList();
                }
            }
        }

        private void btnDislike_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var suggestion = button?.Tag as Suggestion;
            var userService = new UsersService();
            int currentUserId = int.Parse(userService.ReturnDecryptedRole(Settings.Default.userID));

            if (suggestion != null)
            {
                var service = new ForumService();
                bool likeRemoved = service.RemoveLike(suggestion.ID_Suggestion, currentUserId);

                if (likeRemoved)
                {
                    suggestion.SuggestionLikes -= 1;
                    button.Content = "Sviđa mi se";
                    RefreshList();
                }
            }
        }

        private void RefreshList()
        {
            ScrollViewer_Loaded(null, null);
        }

        private void btnMainComment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var suggestion = button?.Tag as Suggestion;

            var stackPanel = button?.Parent as StackPanel;

            var txtMainComment = stackPanel?.Children.OfType<TextBox>()
                                                .FirstOrDefault(t => t.Name == "txtMainComment");

            if (txtMainComment != null && suggestion != null)
            {
                button.IsEnabled = true;
                string commentContent = txtMainComment.Text.Trim();

                if (string.IsNullOrEmpty(commentContent))
                {
                    button.IsEnabled = false;
                    return;
                }

                var userService = new UsersService();
                int currentUserId = int.Parse(userService.ReturnDecryptedRole(Settings.Default.userID));

                var service = new CommentService();
                service.AddComment(commentContent, currentUserId, suggestion.ID_Suggestion);

                txtMainComment.Clear();

                RefreshList();
            }
        }
        private void UcForum_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSuggestionPanelWidth();
        }

        private void UpdateSuggestionPanelWidth()
        {
            double fixedPanelWidth = this.ActualWidth - 100;
            foreach (var item in SuggestionsList.Items)
            {
                if (item is Border border && border.Child is StackPanel suggestionPanel)
                {
                    suggestionPanel.Width = fixedPanelWidth;

                    var content = suggestionPanel.Children.OfType<TextBlock>()
                                                           .FirstOrDefault(tb => tb.Name == "content");
                    if (content != null)
                    {
                        content.Width = fixedPanelWidth - 20;
                    }
                }
            }
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;

            if (scrollViewer != null)
            {
                if (e.Delta > 0)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 30);
                }
                else
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 30);
                }
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ScrollViewer_Loaded(null, null);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage * itemsPerPage < totalSuggestionsCount)
            {
                currentPage++;
                ScrollViewer_Loaded(null, null);
            }
        }
        private void BtnLoadMoreComments_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int suggestionId)
            {
                commentDisplayCount[suggestionId] += commentsPerPage;
                RefreshList();
            }
        }

        private void BtnHideComments_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int suggestionId)
            {
                commentDisplayCount[suggestionId] = commentsPerPage;
                RefreshList();
            }
        }

    }
}
