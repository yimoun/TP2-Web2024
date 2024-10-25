using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class SujetListVM
    {
        public Sujet Sujet { get; set; }
        public int NbReponses { get; set; }
        public MessageModel DernierMessage { get; set; }
        public SujetListVM(Sujet sujets, int nbReponses, MessageModel dernierMessage)
        {
            Sujet = sujets;
            NbReponses = nbReponses;
            DernierMessage = dernierMessage;
        }
    }
}
