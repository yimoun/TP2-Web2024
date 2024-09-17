using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class SujetCreateVM
    {
        public Sujet sujet { get; set; }
        public MessageModel message { get; set; }

        SujetCreateVM(Sujet sujet, MessageModel message)
        {
            this.sujet = sujet;
            this.message = message;
        }
    }
}
