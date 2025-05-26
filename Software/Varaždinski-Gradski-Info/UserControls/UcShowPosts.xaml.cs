using BusinessLogicLayer;
using EntitiesLayer;
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
    public partial class UcShowPosts : UserControl
    {
        public UcShowPosts()
        {
            InitializeComponent();
            LoadPostsAsync();
        }
        private async void LoadPostsAsync()
        {

            var service = new PostService();
            var urgentPosts = await service.GetUrgentPost();
            var regularPosts = await service.GetRegularPost();

            var headerUrgentPost = new TextBlock
            {
                Text = "Hitne obavijesti",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Margin = new Thickness(0, 10, 0, 10)
            };
            urgentPostPanel.Children.Insert(0, headerUrgentPost);

            var headerRegularPost = new TextBlock
            {
                Text = "Ostale obavijesti",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Margin = new Thickness(0, 10, 0, 10)
            };
            regularPostPanel.Children.Insert(0, headerRegularPost);

            DisplayPosts(urgentPosts, urgentPostPanel, true);
            DisplayPosts(regularPosts, regularPostPanel, false);
        }
        private void DisplayPosts(List<Post> posts, StackPanel panel, bool isUrgent)
        {
            panel.Children.Clear();

            var headerContainer = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 10, 0, 10)
            };

            var header = new TextBlock
            {
                Text = isUrgent ? "Hitne obavijesti" : "Ostale obavijesti",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Margin = new Thickness(0, 10, 0, 10)
            };

            Button subscribeButton = null;
            if (isUrgent)
            {
                var service = new UsersService();
                int currentUserId = int.Parse(service.ReturnDecryptedRole(Settings.Default.userID));
                bool isSubscribed = service.GetSubscriptionStatus(currentUserId);
                subscribeButton = new Button
                {
                    Content = isSubscribed ? "Ukloni pretplatu" : "Pretplati me",
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3E3FD")),
                    Foreground = Brushes.Black,
                    Padding = new Thickness(10, 5, 10, 5),
                    Margin = new Thickness(20, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    Tag = isSubscribed
                };
                subscribeButton.Click += SubscribeButton_Click;
            }

            headerContainer.Children.Add(header);
            if (subscribeButton != null)
            {
                headerContainer.Children.Add(subscribeButton);
            }

            panel.Children.Add(headerContainer);

            foreach (var post in posts)
            {
                var border = new Border
                {
                    BorderBrush = isUrgent ? Brushes.LightCoral : Brushes.LightBlue,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(5),
                    Background = isUrgent ? Brushes.LightCoral : Brushes.LightSteelBlue,
                    Padding = new Thickness(10),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                var stack = new StackPanel();

                var title = new TextBlock
                {
                    Text = post.PostName,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 00)
                };
                TextBlock date = new TextBlock
                {
                    Text = $"Objavljeno: {post.DateCreated:dd-MM-yyyy HH:mm}",
                    FontSize = 12,
                    Foreground = isUrgent ? Brushes.White : Brushes.Gray,
                    Margin = new Thickness(0, 0, 0, 0)
                };

                var content = new TextBlock
                {
                    Text = post.PostContent,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                stack.Children.Add(title);
                stack.Children.Add(date);
                stack.Children.Add(content);
                border.Child = stack;

                panel.Children.Add(border);
            }
        }
        private void SubscribeButton_Click(object sender, RoutedEventArgs e)
        {
            var service = new UsersService();
            int currentUserId = int.Parse(service.ReturnDecryptedRole(Settings.Default.userID));

            var button = sender as Button;
            bool isSubscribed = (bool)button.Tag;

            short newStatus = (short)(isSubscribed ? 0 : 1);

            bool updateResult = service.UpdateSubscribedStatus(currentUserId, newStatus);

            if (updateResult)
            {
                button.Content = newStatus == 1 ? "Ukloni pretplatu" : "Pretplati me";
                button.Tag = newStatus == 1;
            }
        }
    }
}
