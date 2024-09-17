using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class SectionListVM
    {
        public List<Section> Sections { get; }
        public List<MessageModel> Messages { get; }
        public int count { get; }
        public SectionListVM(List<Section> sections, int count, List<MessageModel> messages)
        {
            Sections = sections;
            this.count = count;
            Messages = messages;
        }
    }
}
