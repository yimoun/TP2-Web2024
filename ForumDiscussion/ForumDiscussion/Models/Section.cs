using System.ComponentModel.DataAnnotations;

namespace ForumDiscussion.Models
{
    public class Section
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le titre est requis.")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "La description est requise.")]
        public string Description { get; set; }
        public List<Sujet> Sujets { get; set; } = new List<Sujet>();    //Une section doit pouvoir être créee sans sujet.


        public Section() { }    //Pour creer une instance par défaut !
        public Section(string titre, string description)
        {
            Titre = titre;
            Description = description;
        } 
    }
}
