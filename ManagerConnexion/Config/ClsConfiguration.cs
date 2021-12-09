using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerConnection
{
    public class ClsConfiguration
    {

        public static bool CreationDeFichierConf(Connexion con ,ConnexionType connexionType)
        {
            bool reponse = false;
            string chemin = "";
            
            switch (connexionType)
            {
                case ConnexionType.SQLServer:
                    chemin = "Data Source=" + con.Serveur + "; Initial Catalog=" + con.Database + "; User Id=" + con.User + "; Password=" + con.Password+";";
                    File.WriteAllText(ClsConstante.Table.chemin, chemin.ToString());                   
                    reponse= true;
                    break;
                case ConnexionType.MySQL:
                    chemin = "Server=" + con.Serveur + "; Database=" + con.Database + "; UserId=" + con.User + "; Password=" + con.Password + ";";
                    File.WriteAllText(ClsConstante.Table.chemin, chemin.ToString());
                    reponse = true;
                    break;
            }
            return reponse;
        }

    }
}
