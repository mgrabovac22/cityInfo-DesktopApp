using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    //Lucija Polak
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository() : base(new CityInfoModel())
        {
        }

        public void AddComment(Comment comment)
        {
            Add(comment);
        }
        public List<Comment> GetCommentsForSuggestion(int suggestionId)
        {
            using (var context = new CityInfoModel())
            {
                return Entities
                              .Where(c => c.SuggestionID_Suggestion == suggestionId)
                              .Include(c => c.User)
                              .ToList();
            }
        }

    }
}
