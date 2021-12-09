using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonneLib
{
    public interface IPersonne
    {
        string Id { get; set; }
        string Nom { get; set; }
        string Postnom { get; set; }
        string Prenom { get; set; }
        Sexe Sex { get; set; }
        string Phone { get; set; }
        string Mail { get; set; }
        byte[] Profil { get; set; }       

        int Nouveau();
        void Enregistrer(IPersonne personne);
        void Supprimer(string id);
        List<IPersonne> Personnes();
        IPersonne OnePersonne(string id);
    }
}
