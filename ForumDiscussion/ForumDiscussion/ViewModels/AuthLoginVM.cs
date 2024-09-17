using ForumDiscussion;
using System.ComponentModel.DataAnnotations;

namespace ForumDiscussion.ViewModels;

    public class AuthLoginVM
    {
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(20)]
        public string Username { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(20)]
        public string Password { get; set; } = string.Empty;
        

        ////Ce constructeur c'est pour des authentification directe après création/modification de comptes
        //public AuthLoginVM(string username, string pwd)
        //{
        //    Username = username;
        //    Password = pwd;
        //}
    
    }







