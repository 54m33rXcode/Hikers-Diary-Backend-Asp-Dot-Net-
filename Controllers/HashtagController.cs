using HikersDiary_Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hikers_Diary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashtagController : ControllerBase
    {
        private readonly MyDbContext _context;
        public HashtagController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("uniqueTags")]
        public IActionResult GetUniqueTags()
        {
            try
            {
               
                var hashtags = _context.Hashtag.ToList();

               
                var tagCounts = hashtags
                    .GroupBy(h => h.Tag_Name, StringComparer.OrdinalIgnoreCase)
                    .Select(g => new { TagName = g.Key, Count = g.Count() });

               
                var uniqueTagNames = tagCounts.Select(tc => tc.TagName).ToList();

                return Ok(uniqueTagNames);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
