namespace ForumDiscussion.Models
{
    public class Membre
    {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public string Courriel { get; set; }
        public string Username { get; set; }
        public string MotDePasse { get; set; }
        public string Role { get; set; }
        public string Profil { get; set; }

        public List<Message>? Messages { get; set; }
        public List<Sujet>? Sujets { get; set; }
    }
}
