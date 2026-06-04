using Npgsql;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paul.classes
{     
        public class Commande : ICrud<Commande>, INotifyPropertyChanged
        {
            // 1. Attributs privés (comme dans Chien.cs / Box.cs)
            private int commandeId;
            private DateTime dateCreation;
            private DateTime dateRetrait;
            private decimal acompte;
            private bool estPrete;
            private bool estRecuperee;
            private decimal total;
            private DateTime? dateEvenement;
            private int? nbPersonne;
            private int clientId;
            private int? categorieEvenementId;

            // Attribut supplémentaire pour le nom du client (récupéré via la jointure lors du FindAll)
            private string clientNom;

            // Événement requis pour le Binding dynamique (INotifyPropertyChanged)
            public event PropertyChangedEventHandler? PropertyChanged;

            // 2. Constructeurs
            public Commande()
            {
            }

            public Commande(int commandeId, DateTime dateCreation, DateTime dateRetrait, decimal acompte, bool estPrete, bool estRecuperee, decimal total, DateTime? dateEvenement, int? nbPersonne, int clientId, int? categorieEvenementId)
            {
                this.CommandeId = commandeId;
                this.DateCreation = dateCreation;
                this.DateRetrait = dateRetrait;
                this.Acompte = acompte;
                this.EstPrete = estPrete;
                this.EstRecuperee = estRecuperee;
                this.Total = total;
                this.DateEvenement = dateEvenement;
                this.NbPersonne = nbPersonne;
                this.ClientId = clientId;
                this.CategorieEvenementId = categorieEvenementId;
            }

            // 3. Propriétés publiques avec déclenchement du PropertyChanged
            public int CommandeId
            {
                get { return this.commandeId; }
                set { this.commandeId = value; NotifyPropertyChanged(nameof(CommandeId)); }
            }

            public DateTime DateCreation
            {
                get { return this.dateCreation; }
                set { this.dateCreation = value; NotifyPropertyChanged(nameof(DateCreation)); }
            }

            public DateTime DateRetrait
            {
                get { return this.dateRetrait; }
                set { this.dateRetrait = value; NotifyPropertyChanged(nameof(DateRetrait)); }
            }

            public decimal Acompte
            {
                get { return this.acompte; }
                set { this.acompte = value; NotifyPropertyChanged(nameof(Acompte)); }
            }

            public bool EstPrete
            {
                get { return this.estPrete; }
                set { this.estPrete = value; NotifyPropertyChanged(nameof(EstPrete)); }
            }

            public bool EstRecuperee
            {
                get { return this.estRecuperee; }
                set { this.estRecuperee = value; NotifyPropertyChanged(nameof(EstRecuperee)); }
            }

            public decimal Total
            {
                get { return this.total; }
                set { this.total = value; NotifyPropertyChanged(nameof(Total)); }
            }

            public DateTime? DateEvenement
            {
                get { return this.dateEvenement; }
                set { this.dateEvenement = value; NotifyPropertyChanged(nameof(DateEvenement)); }
            }

            public int? NbPersonne
            {
                get { return this.nbPersonne; }
                set { this.nbPersonne = value; NotifyPropertyChanged(nameof(NbPersonne)); }
            }

            public int ClientId
            {
                get { return this.clientId; }
                set { this.clientId = value; NotifyPropertyChanged(nameof(ClientId)); }
            }

            public int? CategorieEvenementId
            {
                get { return this.categorieEvenementId; }
                set { this.categorieEvenementId = value; NotifyPropertyChanged(nameof(CategorieEvenementId)); }
            }

            // Propriété spécifique pour stocker le Nom du client associé
            public string ClientNom
            {
                get { return this.clientNom; }
                set { this.clientNom = value; NotifyPropertyChanged(nameof(ClientNom)); }
            }

            // Propriété calculée lue par la DataGrid pour le texte du Statut
            public string StatutAfficher
            {
                get
                {
                    if (EstRecuperee) return "Restituée";
                    if (EstPrete) return "À restituer";
                    return "En préparation";
                }
            }

            // Méthode d'aide pour notifier WPF du changement d'une propriété
            private void NotifyPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            // ==============================================================
            // IMPLEMENTATION DE ICRUD (Comme dans Chien.cs / Box.cs)
            // ==============================================================

            public List<Commande> FindAll()
            {
                List<Commande> lesCommandes = new List<Commande>();

            // Requête SQL avec INNER JOIN pour récupérer également le NOM du client
            // Filtré sur CURRENT_DATE pour avoir les commandes à retirer aujourd'hui
                        string sql = @"
                SELECT c.COMMANDE_ID, c.DATE_CREATION, c.DATE_RETRAIT, c.ACOMPTE, 
                       c.EST_PRETE, c.EST_RECUPEREE, c.TOTAL, c.DATE_EVENEMENT, 
                       c.NB_PERSONNE, c.CLIENT_ID, c.CATEGORIE_EVENEMENT_ID, cl.NOM as client_nom
                FROM COMMANDE c
                INNER JOIN CLIENT cl ON c.CLIENT_ID = cl.CLIENT_ID
                ORDER BY c.DATE_RETRAIT ASC;";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
                {
                     DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                    

                    foreach (DataRow dr in dt.Rows)
                    {
                    Commande c = new Commande();

                        // Valeurs numériques et booléennes
                        c.CommandeId = Convert.ToInt32(dr["commande_id"]);
                        c.Acompte = Convert.ToDecimal(dr["acompte"]);
                        c.EstPrete = Convert.ToBoolean(dr["est_prete"]);
                        c.EstRecuperee = Convert.ToBoolean(dr["est_recuperee"]);
                        c.Total = Convert.ToDecimal(dr["total"]);
                        c.ClientId = Convert.ToInt32(dr["client_id"]);

                        // TEXTE (String)
                        c.ClientNom = dr["client_nom"].ToString();

                        // DATES : Utilisation de la syntaxe DateOnly -> ToDateTime
                        c.DateCreation = ((DateOnly)dr["date_creation"]).ToDateTime(TimeOnly.MinValue);
                        c.DateRetrait = ((DateOnly)dr["date_retrait"]).ToDateTime(TimeOnly.MinValue);

                        // DATES NULLABLES : On vérifie d'abord DBNull.Value
                        c.DateEvenement = dr["date_evenement"] == DBNull.Value ? (DateTime?)null : ((DateOnly)dr["date_evenement"]).ToDateTime(TimeOnly.MinValue);

                        // Autres valeurs nullables
                        c.NbPersonne = dr["nb_personne"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["nb_personne"]);
                        c.CategorieEvenementId = dr["categorie_evenement_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["categorie_evenement_id"]);

                        lesCommandes.Add(c);
                }
                }
                return lesCommandes;
            }

            public int Create()
            {
                throw new NotImplementedException();
            }

            public void Read()
            {
                throw new NotImplementedException();
            }

            public int Update()
            {
                throw new NotImplementedException();
            }

            public int Delete()
            {
                throw new NotImplementedException();
            }

            public List<Commande> FindBySelection(string criteres)
            {
                throw new NotImplementedException();
            }
        }
}