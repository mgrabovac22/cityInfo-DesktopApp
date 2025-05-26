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

namespace Varaždinski_Gradski_Info.UserControls
{
    public partial class UcAddNewSuggestion : UserControl
    {
        public UcAddNewSuggestion()
        {
            InitializeComponent();
        }

        private async void btnPost_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Length >= 255 || txtDesc.Text.Length >= 1000)
            {
                lblError.Content = "Naslov je predugačak (255 znakova moguće)";
            }
            else if (txtDesc.Text.Length >= 1000)
            {
                lblError.Content = "";
                lblError.Content = "Opis je predugačak (1000 znakova moguće)";
            }
            else if (txtDesc.Text.Trim()=="" || txtName.Text.Trim()=="")
            {
                lblError.Content = "";
                lblError.Content = "Niste unijeli sve podatke";
            }
            else
            {
                lblError.Content = "";
                btnPost.IsEnabled = true;
                ImageBehavior.SetAnimatedSource(loadingGif, new BitmapImage(new Uri("Assets/loadingGif.gif", UriKind.Relative)));

                loadingGif.Visibility = Visibility.Visible;
                txtPost.Visibility = Visibility.Collapsed;
                await Task.Delay(1500);
                var service = new ForumService();
                var serviceUser = new UsersService();
                service.AddSuggestion(txtName.Text, txtDesc.Text, int.Parse(await Task.Run(() => serviceUser.ReturnDecryptedRole(Settings.Default.userID))));

                loadingGif.Visibility = Visibility.Collapsed;
                txtPost.Visibility = Visibility.Visible;
                btnPost.IsEnabled = true;

                GuiManager.CloseContent();
                GuiManager.OpenContent(new UcForum());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
