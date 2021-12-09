using ManagerConnection;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerConnection
{   
        public class ImplementeConnexion : IConnection
        {
            private ImplementeConnexion()
            {
            }
        
            private IDbConnection _conn = null;
            private static ImplementeConnexion _instance = null;           
            public IDbConnection Conn
            {
                get
                {
                    return _conn;
                }

                set
                {
                    _conn = value;
                }
            }

            public static ImplementeConnexion Instance
            {
                get
                {
                    if (_instance == null)
                        _instance = new ImplementeConnexion();
                    return _instance;
                }
            }

            public IDbConnection Initialise(ConnexionType connexionType)
            {
            string chemin = "";
            chemin = File.ReadAllText(ClsConstante.Table.chemin);
            switch (connexionType)
                {
                    case ConnexionType.SQLServer:
               
                    if(!chemin.Equals(""))
                        _conn = new SqlConnection(chemin);
                    //_conn = new SqlConnection(string.Format("Data source={0};Initial catalog={1};User ID={2};Password={3}",
                    //  connexion.Serveur, connexion.Database, connexion.User, connexion.Password));

                    else
                        throw new InvalidOperationException("Le chemin de la base de données est vide!!!");
                    // _conn = new SqlConnection(string.Format("Data source={0};Initial catalog={1};User ID={2};Password={3}",
                    //   connexion.Serveur, connexion.Database, connexion.User, connexion.Password));
                    break;
                    case ConnexionType.MySQL:
                    if (!chemin.Equals(""))
                        _conn = new MySqlConnection(chemin);
                    else
                        throw new InvalidOperationException("Le chemin de la base de données est vide!!!");
                      //_conn = new MySqlConnection(string.Format("Server={0};Database={1};UserID={2};Password={3}",
                      // connexion.Serveur, connexion.Database, connexion.User, connexion.Password));
                    break;
                    case ConnexionType.PostGrsSQL:
                        //_conn = new NpgsqlConnection(string.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4}",
                        // connexion.Serveur, connexion.Database, connexion.User, connexion.Password,connexion.Port));
                        break;
                    case ConnexionType.Oracle:
                        return null;
                    case ConnexionType.Access:
                        return null;
                }
                return _conn;
            }

      
    }
}
