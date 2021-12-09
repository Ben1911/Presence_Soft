using GestionPersonneLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerPresenceLib.Interfaces
{
  public  interface ICompte
    {

        string Id { get; set; }
        string NomUser { get; set; }
        string Password { get; set; }
        int Niveau { get; set; }
        DateTime DateDebut { get; set; }
        DateTime DateFin { get; set; }

        int Nouveau();
        void Enregistrer(ICompte compte);
        void Supprimer(string id);
        List<ICompte> Comptes();
        ICompte OneCompte(string id);
    }
}
