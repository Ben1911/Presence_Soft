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
   public class Mouvement :IMouvement
    {


        private string _idEntete;
        private string _idDetail;
        private string _motif;
        private DateTime _dateSortie;
        private DateTime _dateRentree;

        public string IdEntete
        {
            get
            {
                return _idEntete;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _idEntete = value[0].ToString().ToUpper() + new string(value.ToCharArray(), 1, value.Length - 1);
                }
                else
                _idEntete = value;
            }
        }

        public string IdDetail
        {
            get
            {
                return _idDetail;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _idDetail = value[0].ToString().ToUpper() + new string(value.ToCharArray(), 1, value.Length - 1);
                }
                else
                _idDetail = value;
            }
        }

        public string Motif
        {
            get
            {
                return _motif;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _motif = value[0].ToString().ToUpper() + new string(value.ToCharArray(), 1, value.Length - 1);
                }
                else
                _motif = value;
            }
        }

        public DateTime DateSortie
        {
            get
            {
                return _dateSortie;
            }

            set
            {
                _dateSortie = value;
            }
        }

        public DateTime DateRentree
        {
            get
            {
                return _dateRentree;
            }

            set
            {
                _dateRentree = value;
            }
        }

        public int Nouveau()
        {
            int id = 0;

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "select max(id) as lastId from TabMouvement";

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

        public void Enregistrer(IMouvement mouvement)
        {
            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_insert_Mouvement";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@idEntete", 50, DbType.String, _idEntete));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@IdDetail", 50, DbType.String, _idDetail));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@Motif", 50, DbType.String, _motif ));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@DateSortie", 50, DbType.DateTime, _dateSortie));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@DateRentree", 50, DbType.DateTime, _dateRentree));
                cmd.ExecuteNonQuery();
            }
        }

        public void Supprimer(string id)
        {
            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_delete_Mouvement";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@idEntete", 20, DbType.String, _idEntete));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@idDetail", 20, DbType.String, _idDetail));

                int record = cmd.ExecuteNonQuery();

                if (record == 0)
                    throw new InvalidOperationException("That id does not exist !!!");
            }
        }

        public List<IMouvement> Mouvements()
        {
            List<IMouvement> lst = new List<IMouvement>();

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_select_Mouvement";
                cmd.CommandType = CommandType.StoredProcedure;

                IDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    lst.Add(GetMouvement(rd));
                }

                rd.Dispose();
            }

            return lst;
        }
        private IMouvement GetMouvement(IDataReader rd)
        {
            IMouvement mouvement = new Mouvement();
            mouvement.IdEntete = rd["IdEntete"].ToString();
            mouvement.IdDetail = rd["IdDetail"].ToString();
            mouvement.Motif =rd["Motif"].ToString();
            mouvement.DateSortie = DateTime.Parse(rd["DateSortie"].ToString());
            mouvement.DateRentree = DateTime.Parse(rd["DateRentree"].ToString());
            return mouvement;
        }
        public IMouvement OneMouvement(string id)
        {
            IMouvement mouvement = new Mouvement();

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_select_Mouvement";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@id", 50, DbType.String, id));

                IDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    mouvement = GetMouvement(rd);
                }

                rd.Dispose();
            }

            return mouvement;
        }
    }
}
