using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using Hikers_Diary.Models;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Hikers_Diary.Repository
{
    public class HashtagRepository:IHashtagRepository
    {
        private readonly MyDbContext _context;
        public HashtagRepository(MyDbContext context)
        {
            _context = context;       
        }
        public void ExtractAndInsertHashtags(PostDto post)
        {
            List<string> hashtags = ExtractHashtags(post.PostContent);

            foreach (var hashtagText in hashtags)
            {
                var hashtag = new Hashtag
                {
                    Tag_Name = hashtagText,
                    Tpost_Id = post.PostId
                };

                _context.Hashtag.Add(hashtag);
            }

            _context.SaveChanges();
        }

        public List<string> ExtractHashtags(string postContent)
        {
            List<string> hashtags = new List<string>();
            Regex regex = new Regex(@"#\w+"); 

            MatchCollection matches = regex.Matches(postContent);
            foreach (Match match in matches)
            {
                string hashtag = match.Value.TrimStart('#'); 
                hashtags.Add(hashtag);
            }

            return hashtags;
        }
    }

}

