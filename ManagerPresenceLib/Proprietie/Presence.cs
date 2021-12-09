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
   public class Presence: IPresence
    {
        private string _idEntete;
        private string _idDetail;
        private int _nombrePresence;
        private DateTime _dateDebut;
        private DateTime _dateSortie;

        public string IdEntete
        {
            get
            {
                return _idEntete;
            }

            set
            {
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
                _idDetail = value;
            }
        }

        public int NombrePresence
        {
            get
            {
                return _nombrePresence;
            }

            set
            {
                _nombrePresence = value;
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
      

        public int Nouveau()
        {
            int id = 0;

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "select max(id) as lastId from TabPresence";

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

        public void Enregistrer(IPresence presence)
        {
            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_insert_Presence";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@idEntete", 50, DbType.String, _idEntete));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@IdDetail", 50, DbType.String, _idDetail));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@ombrePresence", 50, DbType.String, _nombrePresence));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@DateDebut", 50, DbType.DateTime, _dateDebut));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@DateSortie", 50, DbType.DateTime, _dateSortie));
                cmd.ExecuteNonQuery();
            }
        }

        public void Supprimer(string id)
        {
            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_delete_presence";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@idEntete", 20, DbType.String, _idEntete));
                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@idDetail", 20, DbType.String, _idDetail));

                int record = cmd.ExecuteNonQuery();

                if (record == 0)
                    throw new InvalidOperationException("That id does not exist !!!");
            }
        }

        public List<IPresence> Presences()
        {
            List<IPresence> lst = new List<IPresence>();

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_select_comptes";
                cmd.CommandType = CommandType.StoredProcedure;

                IDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    lst.Add(GetPresence(rd));
                }

                rd.Dispose();
            }

            return lst;
        }
        private IPresence GetPresence(IDataReader rd)
        {
            IPresence presence = new Presence();
            presence.IdEntete = rd["IdEntete"].ToString();
            presence.IdDetail = rd["IdDetail"].ToString();
            presence.NombrePresence = int.Parse(rd["NombrePresence"].ToString());
            presence.DateDebut = DateTime.Parse(rd["DateDebut"].ToString());
            presence.DateSortie = DateTime.Parse(rd["DateSortie"].ToString());          
            return presence;
        }
        public IPresence OnePresence(string id)
        {
            IPresence presence = new Presence();

            if (ImplementeConnexion.Instance.Conn.State == ConnectionState.Closed)
                ImplementeConnexion.Instance.Conn.Open();

            using (IDbCommand cmd = ImplementeConnexion.Instance.Conn.CreateCommand())
            {
                cmd.CommandText = "sp_select_Presence";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(ClsParametres.Instance.AjouterParametre(cmd, "@id", 50, DbType.String, id));

                IDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    presence = GetPresence(rd);
                }

                rd.Dispose();
            }

            return presence;
        }
    }
}
