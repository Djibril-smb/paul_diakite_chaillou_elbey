using Paul.classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Paul.uc
{
    /// <summary>
    /// Logique d'interaction pour ucCommandes.xaml
    /// </summary>
    public partial class ucCommandes : UserControl
    {
        
        public ObservableCollection<Commande> LesCommandes { get; set; }

        public ucCommandes()
        {
            InitializeComponent();

            // On s'abonne à l'événement Loaded pour charger les données de la BDD
            this.Loaded += UcCommandes_Loaded;
        }

        private void UcCommandes_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 3. On crée un objet temporaire pour appeler la méthode d'instance FindAll()
                Commande outilCommande = new Commande();

                // 4. On appelle FindAll() (qui renvoie la List<Commande>) et on la convertit proprement
                this.LesCommandes = new ObservableCollection<Commande>(outilCommande.FindAll());

                // 5. On définit le DataContext sur lui-même pour que le XAML lise "LesCommandes"
                this.DataContext = this;

                // 6. On affecte la collection comme source de données de la DataGrid
                DgCommandes.ItemsSource = this.LesCommandes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des commandes : " + ex.Message,
                                "Erreur BDD", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}