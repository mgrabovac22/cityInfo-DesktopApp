using BusinessLogicLayer;
using EntitiesLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class InstitutionServiceTest
    {
        [TestMethod]
        public void GetAllInstitutions_WhenCalled_ReturnsNonEmptyList()
        {
            InstitutionService service = new InstitutionService();

            List<Institution> result = service.GetAllInstitutions();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0, "Expected at least one institution in the list.");
        }

        [TestMethod]
        public void GetInstitutionById_ValidId_ReturnsInstitution()
        {
            InstitutionService service = new InstitutionService();
            int institutionId = 1; 

            Institution result = service.GetInstitutionById(institutionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(institutionId, result.ID_Institution, "Returned institution ID does not match the requested ID.");
        }
    }
}
