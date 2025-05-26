using BusinessLogicLayer;
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
using WpfAnimatedGif;
using System.Collections.Generic;
using DataAccessLayer;
using EntitiesLayer;

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcAddNewPost : UserControl
    {
        private short valueOfButton;
        public UcAddNewPost()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
            GuiManager.OpenContent(new UcEmergencies());
        }

        private async void btnPost_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Length >= 255)
            {
                lblError.Content = "Naslov je predugačak (255 znakova moguće)";
            }
            else if (txtDesc.Text.Length >= 1000)
            {
                lblError.Content = "";
                lblError.Content = "Opis je predugačak (1000 znakova moguće)";
            }
            else if (txtDesc.Text.Trim() == "" || txtName.Text.Trim() == "" || (btnYes.IsChecked==false && btnNo.IsChecked==false))
            {
                lblError.Content = "";
                lblError.Content = "Niste unijeli sve podatke";
            }
            else
            {
                btnPost.IsEnabled = true;
                ImageBehavior.SetAnimatedSource(loadingGif, new BitmapImage(new Uri("Assets/loadingGif.gif", UriKind.Relative)));

                loadingGif.Visibility = Visibility.Visible;
                txtPost.Visibility = Visibility.Collapsed;
                if (btnYes.IsChecked==true)
                {
                    valueOfButton = 1;
                }
                else valueOfButton = 0;
                var service = new PostService();
                var serviceUser = new UsersService();

                service.AddPost(txtName.Text, txtDesc.Text, int.Parse(await Task.Run(() => serviceUser.ReturnDecryptedRole(Settings.Default.userID))), valueOfButton);

                List<User> subscribedUsers = serviceUser.GetSubsribedUsers();

                if (subscribedUsers.Count > 0 && btnYes.IsChecked==true)
                {
                    var emailService = new EmailService();
                    List<string> emailAddresses = subscribedUsers.Select(u => u.Email).ToList();

                    string subject = "Nova Hitna Obavijest: " + txtName.Text;
                    string body = $"<h2>Hitna Obavijest</h2><p>{txtDesc.Text}</p>";

                    await emailService.SendEmailAsync(emailAddresses, subject, body);
                }

                loadingGif.Visibility = Visibility.Collapsed;
                txtPost.Visibility = Visibility.Visible;
                btnPost.IsEnabled = true;

                GuiManager.CloseContent();
                GuiManager.OpenContent(new UcEmergencies());
            }

        }
    }
}
