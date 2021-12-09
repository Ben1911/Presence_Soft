using GestionUtilitiesLib;
using ManagerConnection;
using ManagerPresenceLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerPresenceLib.Proprietie
{
   public  class Compte:ICompte
    {
        private string _id;
        private string _nomUser;
        private string _password;
        private int _niveau;
        private DateTime _dateDebut;
        private DateTime _dateFin;

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _id = value[0].ToString().ToUpper() + new string(value.ToCharArray(), 1, value.Length - 1);
                }
                else
                _id = value;
            }
        }

        public string NomUser
        {
            get
            {
                return _nomUser;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _nomUser = value[0].ToString().ToUpper() + new string(value.ToCharArray(), 1, value.Length - 1);
                }
                else
                _nomUser = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _password = value[0].ToString().ToUpper() + new string(value.ToCharArray(), 1, value.Length - 1);
                }
                else
                _password = value;
            }
        }

        public int Niveau
        {
            get
            {
                return _niveau;
            }

            set
            {
                
                _niveau = value;
            }
        }

        public DateTime DateDebut
        {
            get
            {
                return _dateDebut;
            }

            set
            {
                _dateDebut = value;
            }
        }

        public DateTime DateFin
        {
            get
            {
                return _dateFin;
            }

            set
            {
                _dateFin = value;
            }
        }
        public int Nouveau()
        {
            int id = 0;

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "select max(id) as lastId from TabCompte";

                IDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    if (rd["lastId"] == DBNull.Value)
                        id = 1;
                    else
                        id = Convert.ToInt32(rd["lastId"].ToString()) + 1;
                }

                rd.Dispose();
            }

            return id;
        }

        public void Enregistrer(ICompte compte)
        {
            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_insert_Personne";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@id", 4, DbType.String, _id));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@NomUser", 50, DbType.String, _nomUser));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@Password", 50, DbType.String, _password));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@Niveau", 50, DbType.Int32, _niveau));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@Datedebut", 50, DbType.DateTime, _dateDebut));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@DateFin", 50, DbType.DateTime, _dateFin));
                cmd.ExecuteNonQuery();
            }
        }

        public void Supprimer(string id)
        {
            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_delete_personne";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@id", 4, DbType.Int32, _id));

                int record = cmd.ExecuteNonQuery();

                if (record == 0)
                    throw new InvalidOperationException("That id does not exist !!!");
            }
        }

        public List<ICompte> Comptes()
        {
            List<ICompte> lst = new List<ICompte>();

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_select_comptes";
                cmd.CommandType = CommandType.StoredProcedure;

                IDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    lst.Add(GetCompte(rd));
                }

                rd.Dispose();
            }

            return lst;
        }
        private ICompte GetCompte(IDataReader rd)
        {
            ICompte compte = new Compte();
            compte.Id = rd["id"].ToString();
            compte.NomUser = rd["NomUser"].ToString();
            compte.Password = rd["Password"].ToString();
            compte.Niveau = int.Parse(rd["Niveau"].ToString());
            compte.DateDebut = DateTime.Parse(rd["DateDebut"].ToString());
            compte.DateFin = DateTime.Parse(rd["DateFin"].ToString());
            return  compte;
        }
        public ICompte OneCompte(string id)
        {
            ICompte compte = new Compte();

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_select_personne";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@id", 50, DbType.String, id));

                IDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    compte = GetCompte(rd);
                }

                rd.Dispose();
            }

            return compte;
        }
    }
}
