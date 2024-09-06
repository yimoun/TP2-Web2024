using ForumDiscussion.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumDiscussion.Models;

namespace ForumDiscussion.Controllers
{
    public class SectionController : Controller
    {
        private readonly ForumContext _forumContext;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Section>>> GetSection()
        {
            return await _forumContext.Section.ToListAsync();
        }
    }
}
