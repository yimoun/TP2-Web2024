using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class messListVM
    {
        public List<MessageModel> Messages { get; set; }
        public int IdSujet { get; set; }
        public messListVM(List<MessageModel> messages, int idSujet)
        {
            Messages = messages;
            IdSujet = idSujet;
        }
    }
}
