using BusinessLogicLayer;
using EntitiesLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class CommentServiceTest
    {
        [TestMethod]
        public void GetCommentsForSuggestion_GetOneCommentFromSuggestion_SuggestionNotNull()
        {
            CommentService service = new CommentService();

            List<Comment> comment = service.GetCommentsForSuggestion(1);

            Assert.IsTrue(comment.Count>0);
        }
    }
}
