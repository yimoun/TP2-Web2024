namespace ForumDiscussion.ViewModels
{
    public class SujetListSectionVM
    {
        public List<SujetListVM> sujetListVMs { get; set; } = new List<SujetListVM>();  

        public int IdSection { get; set; }

        public SujetListSectionVM() { }

        public SujetListSectionVM(List<SujetListVM> sujetListVMs, int idSection)
        {
            this.sujetListVMs = sujetListVMs;
            IdSection = idSection;
        }
    }
}
