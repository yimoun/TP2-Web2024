using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class messListVM
    {
        public List<MessageModel> Messages { get; set; }
        public MessageModel Reponse { get; set; }
        public int IdSujet { get; set; }
        public messListVM()
        {
            Messages = new List<MessageModel>();
            Reponse = new MessageModel();
        }

        public messListVM(List<MessageModel> messages, int idSujet)
        {
            Messages = messages;
            Reponse = new MessageModel() { SujetId = idSujet };
            IdSujet = idSujet;
        }
    }
}
