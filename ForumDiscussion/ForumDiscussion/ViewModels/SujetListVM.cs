using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class SujetListVM
    {
        public List<Sujet> Sujets { get; set; }
        public SujetListVM(List<Sujet> sujets)
        {
            Sujets = sujets;
        }
    }
}
