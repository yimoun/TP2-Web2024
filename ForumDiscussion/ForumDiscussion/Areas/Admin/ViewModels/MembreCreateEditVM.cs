using ForumDiscussion.Models;

namespace ForumDiscussion.Areas.Admin.ViewModels
{
    public class MembreCreateEditVM
    {
        /* Puisque lors de l'édition d'un membre, il va falloir proposer une liste déroulantre de choix de role, 
         * cette nouvelle ViewModel est pour concilier cela !
         */

        public Membre Membre { get; set; } = new Membre();
        public List<string> LesRoles { get; set; }

        public MembreCreateEditVM() { }     //Constructeur vide requis pour la désérialisation

        public MembreCreateEditVM(Membre membre, List<string> lesRoles)
        {
            Membre = membre;
            LesRoles = lesRoles;
        }
    }
}
