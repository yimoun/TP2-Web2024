using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class MessPostVm
    {
        public MessageModel Reponse { get; set; }
        public MessPostVm(MessageModel reponse)
        {
            Reponse = reponse;
        }
    }
}
