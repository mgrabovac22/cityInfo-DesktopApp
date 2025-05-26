using DataAccessLayer;
using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BusinessLogicLayer
{
    //Lucija Polak
    public class ForumService
    {

        public void AddSuggestion(string suggestionName, string suggestionContent, int idUser)
        {

            Suggestion suggestion = new Suggestion
            {
                SuggestionName = suggestionName,
                SuggestionContent = suggestionContent,
                SuggestionLikes = 0,
                SuggestionDateCreated = DateTime.Now,
                UserID_User = idUser
            };

            using (var repo = new ForumRepository())
            {
                repo.AddSuggestion(suggestion);
            }
        }

        public async Task<List<Suggestion>> GetSuggestions()
        {
            using (var repo = new ForumRepository())
            {
                var suggestionsQuery = repo.GetAllSuggestions()
                    .Include(s => s.User);  

                var suggestions = await suggestionsQuery.ToListAsync();
                return suggestions;
            }
        }

        public bool AddLike(int suggestionID, int userID)
        {
            using (var repo = new ForumRepository())
            {
                return repo.AddLike(suggestionID, userID);
            }
        }
        public bool RemoveLike(int suggestionID, int userID)
        {
            using (var repo = new ForumRepository())
            {
                return repo.RemoveLike(suggestionID, userID);
            }
        }
        
        public bool IsSuggestionLiked(int suggestionID, int userID)
        {
            using (var repo = new ForumRepository())
            {
                return repo.IsSuggestionLiked(suggestionID, userID);
            }
        }
        public async Task<List<Suggestion>> GetSuggestionsByPage(int pageNumber, int itemsPerPage)
        {
            using (var repo = new ForumRepository())
            {
                var suggestionsQuery = repo.GetSuggestionsByPage(pageNumber, itemsPerPage)
                                           .Include(s => s.User);

                return await suggestionsQuery.ToListAsync();
            }
        }

        public async Task<int> GetTotalSuggestionsCount()
        {
            using (var repo = new ForumRepository())
            {
                return await Task.FromResult(repo.GetTotalSuggestionsCount());
            }
        }

    }
}
