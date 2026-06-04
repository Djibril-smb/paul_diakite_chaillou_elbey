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
    /// Logique d'interaction pour ucClient.xaml
    /// </summary>
    public partial class ucClient : UserControl
    {
        public ObservableCollection<Client> LesClients { get; set; }
    
        public ucClient()
        {
            InitializeComponent();
            this.Loaded += UcClients_Loaded;
        }

        private void UcClients_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 3. On crée l'objet outil temporaire pour appeler FindAll()
                ICrud<Client> outilClient = new Client();

                // 4. On remplit l'ObservableCollection avec la liste triée de la BDD
                // (En utilisant Convert.ToInt32 dans le modèle, pas de plantage de BIGINT !)
                this.LesClients = new ObservableCollection<Client>(outilClient.FindAll());

                // 5. On définit le DataContext pour l'écoute WPF
                this.DataContext = this;

                // 6. On lie la collection à la DataGrid
                DgClients.ItemsSource = this.LesClients;
            }
            catch (Exception ex)
            {
                // En cas de problème de connexion ou de requête, on affiche l'erreur proprement
                MessageBox.Show("Erreur lors du chargement de la liste des clients : " + ex.Message,
                                "Erreur Base de données", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    }
