namespace ForumDiscussion.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public List<Sujet>? Sujets { get; set; }
    }
}
