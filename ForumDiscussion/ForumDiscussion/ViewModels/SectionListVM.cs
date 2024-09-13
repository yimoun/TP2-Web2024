using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class SectionListVM
    {
        public List<Section> Sections { get; }
        public int count { get; }
        public SectionListVM(List<Section> sections, int count)
        {
            Sections = sections;
            this.count = count;
        }
    }
}
