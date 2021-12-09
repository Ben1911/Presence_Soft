
using ManagerConnection;
using System;
using System.Collections.Generic;
using System.Data;
using GestionUtilitiesLib;

namespace GestionPersonneLib
{
    public class Personne : IPersonne
    {
        public Personne()
        {
        }

        private string  _id;
        private string _nom;
        private string _postnom;
        private string _prenom;
        private Sexe _sex;
        private string _phone;
        private string _mail;
        private byte[] _profil;
        

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Nom
        {
            get
            {
                return _nom;
            }

            set
            {
                _nom = ValidateName(value);
            }
        }

        private string ValidateName(string nom)
        {
            if (!string.IsNullOrEmpty(nom))
            {
                if (!char.IsLetter(nom[0]))
                    throw new InvalidOperationException("Name must begin with letter !!!");
                else
                {
                    nom = nom.ToLower();
                    return nom[0].ToString().ToUpper() + new string(nom.ToCharArray(), 1, nom.Length - 1);
                }

            }
            else
                throw new InvalidOperationException("Name must have value !!!");
        }

        public string Postnom
        {
            get
            {
                return _postnom;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _postnom = value[0].ToString().ToUpper() + new string(value.ToCharArray(), 1, value.Length - 1);
                }
                else
                    _postnom = value;
            }
        }

        public string Prenom
        {
            get
            {
                return _prenom;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _prenom = value[0].ToString().ToUpper() + new string(value.ToCharArray(), 1, value.Length - 1);
                }
                else
                    _prenom = value;
            }
        }

        public Sexe Sex
        {
            get
            {
                return _sex;
            }

            set
            {
                _sex = value;
            }
        }

       

        public string NomComplet
        {
            get
            {
                return (_nom + " " + (string.IsNullOrEmpty(_postnom) ? "" : _postnom + " ") + _prenom).Trim();
            }
        }

        public string Phone
        {
            get
            {
                return _phone;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _phone = value[0].ToString().ToUpper() + new string(value.ToCharArray(), 1, value.Length - 1);
                }
                else
                _phone = value;
            }
        }

        public string Mail
        {
            get
            {
                return _mail;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _mail = value[0].ToString().ToUpper() + new string(value.ToCharArray(), 1, value.Length - 1);
                }
                else
                _mail = value;
            }
        }

        public byte[] Profil
        {
            get
            {
                return _profil;
            }

            set
            {
                _profil = value;
            }
        }

        public int Nouveau()
        {
            int id = 0;

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "select max(id) as lastId from TabPersonne";

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

        public void Enregistrer(IPersonne personne)
        {
            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_insert_Personne";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@id", 4, DbType.String, _id));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@nom", 50, DbType.String, _nom));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@postnom", 50, DbType.String, _postnom));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@prenom", 50, DbType.String, _prenom));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@sexe", 1, DbType.String, _sex == Sexe.Féminin ? "F" : "M"));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@Phone", 50, DbType.String, _phone));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@Mail", 50, DbType.String, _mail));
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

        public List<IPersonne> Personnes()
        {
            List<IPersonne> lst = new List<IPersonne>();

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_select_personnes";
                cmd.CommandType = CommandType.StoredProcedure;

                IDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    lst.Add(GetPersonne(rd));
                }

                rd.Dispose();
            }

            return lst;
        }
        private IPersonne GetPersonne(IDataReader rd)
        {
            IPersonne personne = new Personne();
            personne.Id = rd["id"].ToString();
            personne.Nom = rd["nom"].ToString();
            personne.Postnom = rd["postnom"].ToString();
            personne.Prenom = rd["Prenom"].ToString();
            personne.Sex = rd["Sexe"].ToString().Equals("M") ? Sexe.Masculin : Sexe.Féminin;
            personne.Phone = rd["Phone"].ToString();
            personne.Mail = rd["Email"].ToString();
            return personne;
        }
        public IPersonne OnePersonne(string id)
        {
            IPersonne personne = new Personne();

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_select_personne";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@id", 4, DbType.String, id));

                IDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    personne = GetPersonne(rd);
                }

                rd.Dispose();
            }

            return personne;
        }

        public override string ToString()
        {
            return (_nom + " " + (string.IsNullOrEmpty(_postnom) ? "" : _postnom + " ") + _prenom).Trim();
        }
    }
}
