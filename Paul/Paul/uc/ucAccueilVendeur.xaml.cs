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
using static System.Net.Mime.MediaTypeNames;

namespace Paul.uc
{
    /// <summary>
    /// Logique d'interaction pour ucAccueilVendeur.xaml
    /// </summary>
    public partial class ucAccueilVendeur : UserControl
    {
        public ucAccueilVendeur()
        {
            InitializeComponent();
        }

        private void BtnMenuCommandes_Click(object sender, RoutedEventArgs e)
        {
            ucCommandes ecranCommandes = new ucCommandes();
            EcranContenuDroite.Content = ecranCommandes;
        }

        private void BtnMenuProduits_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void BtnMenuClients_Click(object sender, RoutedEventArgs e)
        {
            
            ucClient ecranClient = new ucClient();
            EcranContenuDroite.Content = ecranClient;
        }
    }
}
