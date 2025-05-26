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

namespace Varaždinski_Gradski_Info.UserControls
{
    /// <summary>
    /// Interaction logic for UcHelp.xaml
    /// </summary>
    public partial class UcHelp : UserControl
    {
        public UcHelp()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcFAQ());
        }
    }
}
