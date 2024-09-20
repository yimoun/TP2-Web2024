using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class SectionListVM
    {
        public Section Section { get; }
        public int NbSujets { get; }
        public int NbMessages { get; }
        public MessageModel DernierMessage { get; }
        public SectionListVM(Section section, int nbSujets, int nbMessages, MessageModel dernierMessage)
        {
            Section = section;
            NbSujets = nbSujets;
            NbMessages = nbMessages;
            DernierMessage = dernierMessage;
        }
    }
}
