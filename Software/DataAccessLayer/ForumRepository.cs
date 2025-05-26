using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    //Lucija Polak
    public class ForumRepository : Repository<Suggestion>
    {
        public ForumRepository() : base(new CityInfoModel())
        {
        }
        public void AddSuggestion(Suggestion suggestion)
        {
            Add(suggestion);
        }

        public void RemovePost(Suggestion suggestion)
        {
            Remove(suggestion);
        }

        public IQueryable<Suggestion> GetAllSuggestions()
        {
            return GetAll();
        }

        public bool AddLike(int suggestionID, int userID)
        {
            using (var context = new CityInfoModel())
            {
                var existingLike = context.Like
                    .FirstOrDefault(l => l.SuggestionID_Suggestion == suggestionID && l.UserID_User == userID);

                if (existingLike != null)
                {
                    return false;
                }

                var newLike = new Like
                {
                    SuggestionID_Suggestion = suggestionID,
                    UserID_User = userID
                };

                context.Like.Add(newLike);
                context.SaveChanges();

                var suggestion = context.Suggestion.FirstOrDefault(s => s.ID_Suggestion == suggestionID);
                if (suggestion != null)
                {
                    suggestion.SuggestionLikes += 1;
                    context.SaveChanges();
                }

                return true; 
            }
        }
        public bool RemoveLike(int suggestionID, int userID)
        {
            using (var context = new CityInfoModel())
            {
                var existingLike = context.Like
                    .FirstOrDefault(l => l.SuggestionID_Suggestion == suggestionID && l.UserID_User == userID);

                if (existingLike == null)
                {
                    return false; 
                }

                context.Like.Remove(existingLike);
                context.SaveChanges();

                var suggestion = context.Suggestion.FirstOrDefault(s => s.ID_Suggestion == suggestionID);
                if (suggestion != null)
                {
                    suggestion.SuggestionLikes -= 1;
                    context.SaveChanges();
                }

                return true; 
            }
        }
        public bool IsSuggestionLiked(int suggestionID, int userID)
        {
            using (var context = new CityInfoModel())
            {
                var existingLike = context.Like
                    .FirstOrDefault(l => l.SuggestionID_Suggestion == suggestionID && l.UserID_User == userID);

                if (existingLike == null)
                {
                    return false;
                }

                return true;
            }
        }
        public IQueryable<Suggestion> GetSuggestionsByPage(int pageNumber, int itemsPerPage)
        {
            return GetAll()
                .OrderByDescending(s => s.SuggestionDateCreated)
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage);
        }

        public int GetTotalSuggestionsCount()
        {
            return GetAll().Count();
        }

    }
}
