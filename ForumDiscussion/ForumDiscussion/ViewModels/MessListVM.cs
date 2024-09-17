using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class messListVM
    {
        public List<MessageModel> Messages { get; set; }
        public messListVM(List<MessageModel> messages)
        {
            Messages = messages;
        }
    }
}
