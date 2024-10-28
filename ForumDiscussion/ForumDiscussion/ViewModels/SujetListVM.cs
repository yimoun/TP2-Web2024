using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class SujetListVM
    {
        public Sujet Sujet { get; set; } = new Sujet();
        public int NbReponses { get; set; } = 0;
        public MessageModel DernierMessage { get; set; } = new MessageModel();

        public SujetListVM() { }
        public SujetListVM(Sujet sujets, int nbReponses, MessageModel dernierMessage)
        {
            Sujet = sujets;
            NbReponses = nbReponses;
            DernierMessage = dernierMessage;
        }
    }
}
