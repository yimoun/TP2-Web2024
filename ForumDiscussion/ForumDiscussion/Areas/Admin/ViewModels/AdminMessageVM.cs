namespace ForumDiscussion.Areas.Admin.ViewModels
{
    public class AdminMessageVM
    {
        public string Message { get; }

        public AdminMessageVM(string message)
        {
            Message = message;
        }
    }
}
