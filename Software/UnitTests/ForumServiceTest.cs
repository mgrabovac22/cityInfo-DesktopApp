using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogicLayer;
using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class ForumServiceTest
    {
        [TestMethod]
        public async Task GetSuggestions_WhenCalled_ReturnsNonEmptyList()
        {
            ForumService service = new ForumService();

            List<Suggestion> result = await service.GetSuggestions();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0, "Expected at least one suggestion in the list.");
        }

        [TestMethod]
        public void IsSuggestionLiked_ValidSuggestionAndUser_ReturnsTrueOrFalse()
        {
            ForumService service = new ForumService();
            int suggestionID = 1;
            int userID = 1; 

            bool result = service.IsSuggestionLiked(suggestionID, userID);

            Assert.IsNotNull(result);
        }
    }
}
