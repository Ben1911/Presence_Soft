using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUtilitiesLib
{
   public class ClsParametres
    {
        private ClsParametres()
        {
        }

        private static ClsParametres _instance = null;

        public static ClsParametres Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClsParametres();
                return _instance;
            }
        }

        public IDbDataParameter AjouterParametre(IDbCommand command, string nomParametre, int taille, DbType type, object valeur)
        {
            IDbDataParameter param = command.CreateParameter();
            param.ParameterName = nomParametre;
            param.DbType = type;
            param.Size = taille;
            param.Value = valeur;

            return param;
        }
    }
}
