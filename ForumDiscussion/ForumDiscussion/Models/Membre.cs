using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ForumDiscussion.Models
{
    public class Membre
    {
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_STANDARD = "Standard";

        public int Id { get; set; }

        [Display(Name = "Pseudo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Le nom d'affichage (pseudo) est requis.")]
        [MaxLength(20, ErrorMessage = "Le pseudo ne doit pas contenir 20 caractères")]
        public string Pseudo { get; set; } = String.Empty;

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage= "Un membre doit avoir une adresse courriel")]
        public string Courriel { get; set; }

        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Le nom d'utilisateur est requis")]
        [MaxLength(20, ErrorMessage = "Le nom d'utilisateur ne doit pas contenir 20 caractères")]
        // [Remote("MemberUsernameNotExists", "Member", "Admin", AdditionalFields = "Id", ErrorMessageResourceName = "UsernameAlreadyExists", ErrorMessageResourceType = typeof(Resource))]
        public string Username { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Un mot de passe est requis.")]
        [MaxLength(20, ErrorMessage = "Le mot de passe ne doit pas contenir 20 caractères")]
        public string MotDePasse { get; set; }
       // public bool IsActive { get; set; } = true;
       // public Guid ActivationCode { get; set; } = Guid.Empty;
        public string Role { get; set; } = ROLE_STANDARD;

        [Display(Name = "Image de profil")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Une image de profil est requise")]
        public string Profil { get; set; } = string.Empty;

        public List<MessageModel>? Messages { get; set; }
        public List<Sujet>? Sujets { get; set; }

        // Constructeur vide requis pour la désérialisation
        public Membre() { }

        public Membre(int id, string pseudo, string courriel, string username, string motDePasse, string role, string profil)
        {
            Id = id;
            Pseudo = pseudo;
            Courriel = courriel;
            Username = username;
            MotDePasse = motDePasse;
            Role = role;
            Profil = profil;
        }
    }
}
