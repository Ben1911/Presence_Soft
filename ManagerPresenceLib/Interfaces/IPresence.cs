using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerPresenceLib.Interfaces
{
   public  interface IPresence
    {

        string IdEntete { get; set; }
        string IdDetail { get; set; }
        int NombrePresence { get; set; }
        DateTime DateDebut { get; set; }
        DateTime DateSortie { get; set; }

        int Nouveau();
        void Enregistrer(IPresence presence);
        void Supprimer(string id);
        List<IPresence> Presences();
        IPresence OnePresence(string id);
    }
}
