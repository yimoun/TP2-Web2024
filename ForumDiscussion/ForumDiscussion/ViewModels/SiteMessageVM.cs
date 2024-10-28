namespace ForumDiscussion.ViewModels
{
    public class SiteMessageVM
    {
        public string Message { get; set; } = "";

        public SiteMessageVM(string message)
        {
            Message = message;
        }
    }
}
