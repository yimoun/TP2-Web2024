using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class messListVM
    {
        public List<MessageModel> Messages { get; set; }
        public MessageModel Reponse { get; set; }
        public messListVM(List<MessageModel> messages, MessageModel reponse)
        {
            Messages = messages;
            Reponse = reponse;
        }
    }
}
