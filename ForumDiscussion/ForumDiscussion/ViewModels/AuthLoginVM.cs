using ForumDiscussion;
using System.ComponentModel.DataAnnotations;


namespace ForumDiscussion.ViewModels;

    public class AuthLoginVM
    {
        [Display(Name = "Nom d'utilisateur")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Le nom d'utilisateur est requis")]
        [StringLength(20)]
        public string Username { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Le mot de passe est requis")]     
        [StringLength(20)]
        public string Password { get; set; } = string.Empty;
        

        ////Ce constructeur c'est pour des authentification directe après création/modification de comptes
        //public AuthLoginVM(string username, string pwd)
        //{
        //    Username = username;
        //    Password = pwd;
        //}
    
    }







