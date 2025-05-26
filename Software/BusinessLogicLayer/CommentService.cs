using DataAccessLayer;
using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessLogicLayer
{
    //Lucija Polak
    public class CommentService
    {
        public void AddComment(string commentContent, int idUser, int suggestionID)
        {

            Comment comment = new Comment
            {
                CommentDate = DateTime.Now,
                CommentContent = commentContent,
                UserID_User = idUser,
                SuggestionID_Suggestion = suggestionID
            };

            using (var repo = new CommentRepository())
            {
                repo.AddComment(comment);
            }
        }
        public List<Comment> GetCommentsForSuggestion(int suggestionID)
        {
            using (var repo = new CommentRepository())
            {
                return repo.GetCommentsForSuggestion(suggestionID);
            }
        }
    }
}
