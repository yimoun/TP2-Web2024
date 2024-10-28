using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ForumDiscussion.Models
{
    public class Sujet
    {
        public int Id { get; set; }

        [Display(Name = "Titre du sujet")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Un titre de sujet est requis.")]
        public string Titre { get; set; }
        public string? Auteur { get; set; }
        public DateTime DateCreation { get; set; }
        public int MembreId { get; set; }
        public Membre? Membre { get; set; }
        public int SectionId { get; set; }
        public Section? Section { get; set; }
        public List<MessageModel>? Messages { get; set; }
    }
}
