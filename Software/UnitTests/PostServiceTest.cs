using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogicLayer;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntitiesLayer;

namespace UnitTests
{
    [TestClass]
    public class PostServiceTest
    {

        [TestMethod]
        public async Task GetSuggestionsAsync_ReturnsNonEmptyList()
        {
            PostService _postService = new PostService();

            var result = await _postService.GetSuggestionsAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public async Task GetUrgentPost_ReturnsNonEmptyList()
        {
            PostService _postService = new PostService();

            var result = await _postService.GetUrgentPost();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public async Task GetRegularPost_ReturnsNonEmptyList()
        {
            PostService _postService = new PostService();

            var result = await _postService.GetRegularPost();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
    }
}
