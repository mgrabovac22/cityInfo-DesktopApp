using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    //Lucija Polak
    public class PostRepository : Repository<Post>
    {
        public PostRepository() : base(new CityInfoModel())
        {
        }
        public void AddPost(Post post)
        {
            Add(post);
        }
        public IQueryable<Post> GetAllSuggestions()
        {
            return GetAll();
        }
        public IQueryable<Post> GetUrgentPost()
        {
            var query = from p in Entities
                        where p.isUrgent == 1
                        orderby p.DateCreated descending
                        select p;
            return query;
        }

        public IQueryable<Post> GetRegularPost()
        {
            var query = from p in Entities
                        where p.isUrgent == 0
                        orderby p.DateCreated descending
                        select p;
            return query;
        }

        public void DeleteOldPosts()
        {
            var allPosts = Entities.ToList();

            var oldUrgentPosts = allPosts.Where(p => p.isUrgent == 1 && p.DateCreated < DateTime.Now.AddDays(-14)).ToList();
            foreach (var post in oldUrgentPosts)
            {
                Remove(post);
            }

            var oldRegularPosts = allPosts.Where(p => p.isUrgent == 0 && p.DateCreated < DateTime.Now.AddDays(-30)).ToList();
            foreach (var post in oldRegularPosts)
            {
                Remove(post);
            }

        }


    }
}
