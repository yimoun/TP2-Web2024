using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ForumDiscussion.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public DateTime DatePublication { get; set; }

        [Display(Name = "Contenu du message")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Un contenu pour le message est requis.")]
        public string Contenu { get; set; }
        public int NbLike { get; set; }
        public int NbDislike { get; set; }
        public int NbView { get; set; }
       

        [ForeignKey("Membre")]
        public int AuteurId { get; set; }
        public Membre? Auteur { get; set; }
        public int SujetId { get; set; }
        public Sujet? Sujet { get; set; }

    }
}
