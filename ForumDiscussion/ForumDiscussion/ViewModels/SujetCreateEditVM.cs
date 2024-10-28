using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class SujetCreateEditVM
    {
        public Sujet sujet { get; set; }
        public MessageModel messageSujet { get; set; }

        public int IdSection { get; set; }

         public SujetCreateEditVM(Sujet sujet, MessageModel messageSujet, int idSection)
            {
                this.sujet = sujet;
                this.messageSujet = messageSujet;
                IdSection = idSection;
            }

        //Sans l'Id de section, pouur les modifs
        public SujetCreateEditVM(Sujet sujet, MessageModel messageSujet)
        {
            this.sujet = sujet;
            this.messageSujet = messageSujet;
        }

        public SujetCreateEditVM()
        {
            this.sujet = new Sujet();
            this.messageSujet = new MessageModel();
        }
    }
}
