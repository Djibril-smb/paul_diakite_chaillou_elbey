using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paul.classes
{
    public class Client : ICrud<Client>, INotifyPropertyChanged
    {
        // 1. Attributs privés (Correspondant aux colonnes de ta table CLIENT)
        private int clientId;
        private string nom;
        private string prenom;
        private string mail;
        private string telephone;

        // Événement requis pour le Binding WPF
        public event PropertyChangedEventHandler? PropertyChanged;

        // 2. Constructeurs
        public Client()
        {

        }

        public Client(int clientId, string nom, string prenom, string mail, string telephone)
        {
            this.ClientId = clientId;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Mail = mail;
            this.Telephone = telephone;
        }

        // 3. Propriétés publiques avec déclenchement de PropertyChanged
        public int ClientId
        {
            get { return this.clientId; }
            set { this.clientId = value; NotifyPropertyChanged(nameof(ClientId)); }
        }

        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; NotifyPropertyChanged(nameof(Nom)); }
        }

        public string Prenom
        {
            get { return this.prenom; }
            set { this.prenom = value; NotifyPropertyChanged(nameof(Prenom)); }
        }

        public string Mail
        {
            get { return this.mail; }
            set { this.mail = value; NotifyPropertyChanged(nameof(Mail)); }
        }

        public string Telephone
        {
            get { return this.telephone; }
            set { this.telephone = value; NotifyPropertyChanged(nameof(Telephone)); }
        }

        // Méthode d'aide pour notifier WPF
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ==============================================================
        // IMPLEMENTATION DE ICRUD<Client>
        // ==============================================================

        List<Client> ICrud<Client>.FindAll()
        {
            List<Client> lesClients = new List<Client>();

            // Requête pour récupérer tous les clients triés par Nom
            string sql = "SELECT client_id, nom, prenom, mail, telephone FROM CLIENT ORDER BY nom ASC;";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
            {
                // Appel de ton Singleton DataAccess
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);

                foreach (DataRow dr in dt.Rows)
                {
                    Client cl = new Client();
                    cl.ClientId = Convert.ToInt32(dr["client_id"]);
                    cl.Nom = dr["nom"].ToString();
                    cl.Prenom = dr["prenom"].ToString();
                    cl.Mail = dr["mail"].ToString();
                    cl.Telephone = dr["telephone"].ToString();

                    lesClients.Add(cl);
                }
            }
            return lesClients;
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

        public List<Client> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }
    }
}
