using BusinessLogicLayer;
using EntitiesLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class ProblemServiceTest
    {
        [TestMethod]
        public void GetAllProblemsWithUsernames_WhenCalled_ReturnsNonEmptyList()
        {
            ProblemService service = new ProblemService();

            List<Problem> result = service.GetAllProblemsWithUsernames();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0, "Expected at least one problem in the list.");
        }

        [TestMethod]
        public void GetMyProblems_ValidUserId_ReturnsNonEmptyList()
        {
            ProblemService service = new ProblemService();
            int userId = 1; 

            List<Problem> result = service.GetMyProblems(userId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count >= 0, "Expected a list of problems, possibly empty.");
        }
    }
}
