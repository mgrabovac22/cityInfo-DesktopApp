using PresentationLayer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Varaždinski_Gradski_Info.UserControls;
using static BusinessLogicLayer.UsersService;

namespace Varaždinski_Gradski_Info
{
    public partial class UsersMainWindow : Window
    {
        public string Role { get; set; }
        private bool isFullScreen = false;

        public UsersMainWindow(string role)
        {
            InitializeComponent();
            Role = role;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetActiveButton(BtnForum, new UcForum());
        }

        private void SetActiveButton(Button activeButton, UserControl content)
        {
            ResetButtonBackgrounds();

            activeButton.Background = new SolidColorBrush(Colors.LightBlue);

            GuiManager.OpenContent(content);
        }

        private void ResetButtonBackgrounds()
        {
            var defaultColor = (Color)ColorConverter.ConvertFromString("#D3E3FD");
            BtnForum.Background = new SolidColorBrush(defaultColor);
            BtnHealth.Background = new SolidColorBrush(defaultColor);
            BtnCommunalServices.Background = new SolidColorBrush(defaultColor);
            BtnEmergencies.Background = new SolidColorBrush(defaultColor);
            BtnAddProblem.Background = new SolidColorBrush(defaultColor);
            BtnFAQ.Background = new SolidColorBrush(defaultColor);
        }

        private void BtnForum_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(BtnForum, new UcForum());
        }

        private void BtnHealth_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(BtnHealth, new UcClinicsOnDuty());
        }

        private void BtnCommunalServices_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(BtnCommunalServices, new UcCommunalServices());
        }

        private void BtnEmergencies_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(BtnEmergencies, new UcShowPosts());
        }

        private void BtnAddProblem_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(BtnAddProblem, new UcTicketForum());
        }

        private void BtnFAQ_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(BtnFAQ, new UcFAQ());
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.username = null;
            Properties.Settings.Default.role = null;
            Properties.Settings.Default.userID = null;
            Properties.Settings.Default.Save();

            var form = new Login();
            Close();
            form.ShowDialog();
        }

        public void OpenContent(UserControl userControl)
        {
            contentPanel.Content = userControl;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                GuiManager.OpenContent(new UcHelp());
                e.Handled = true;
            }
            else if (e.Key == Key.F11)
            {
                ToggleFullScreen();
                e.Handled = true;
            }
        }

        private void ToggleFullScreen()
        {
            if (isFullScreen)
            {
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
            }

            isFullScreen = !isFullScreen;
        }
    }
}
