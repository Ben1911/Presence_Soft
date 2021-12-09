using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerPresenceLib.Interfaces
{
   public  interface IMouvement
    {

        string IdEntete { get; set; }
        string IdDetail { get; set; }
        string Motif { get; set; }
        DateTime DateSortie { get; set; }
        DateTime DateRentree { get; set; }

        int Nouveau();
        void Enregistrer(IMouvement mouvement);
        void Supprimer(string id);
        List<IMouvement> Mouvements();
        IMouvement OneMouvement(string id);
    }
}
