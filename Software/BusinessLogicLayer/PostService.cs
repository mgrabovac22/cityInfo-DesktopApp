using DataAccessLayer;
using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class PostService
    {
        public void AddPost(string postName, string postContent, int idUser, short urgent)
        {

            Post post = new Post
            {
                PostName = postName,
                PostContent = postContent,
                ID_User = idUser,
                isUrgent = urgent,
                DateCreated = DateTime.Now
            };

            using (var repo = new PostRepository())
            {
                repo.AddPost(post);
            }
        }
        public async Task<List<Post>> GetSuggestionsAsync()
        {
            using (var repo = new PostRepository())
            {
                var postsQuery = repo.GetAllSuggestions()
                    .Include(s => s.User);

                var posts = await postsQuery.ToListAsync();
                return posts;
            }
        }
        public async Task<List<Post>> GetUrgentPost()
        {
            using (var repo = new PostRepository())
            {
                repo.DeleteOldPosts();
                var postsQuery = repo.GetUrgentPost()
                    .Include(s => s.User);

                var posts = await postsQuery.ToListAsync();
                return posts;
            }
        }
        public async Task<List<Post>> GetRegularPost()
        {
            using (var repo = new PostRepository())
            {
                repo.DeleteOldPosts();
                var postsQuery = repo.GetRegularPost()
                    .Include(s => s.User);

                var posts = await postsQuery.ToListAsync();
                return posts;
            }
        }
    }
}
