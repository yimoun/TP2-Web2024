using ForumDiscussion.Models;

namespace ForumDiscussion.ViewModels
{
    public class SectionListVM
    {
        public List<Section> Sections { get; }
        public SectionListVM(List<Section> sections)
        {
            Sections = sections;
        }
    }
}
