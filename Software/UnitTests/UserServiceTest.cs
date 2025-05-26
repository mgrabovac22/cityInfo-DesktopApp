using BusinessLogicLayer;
using EntitiesLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime;

namespace UnitTests
{
    [TestClass]
    public class UsersServiceTest
    {
        [TestMethod]
        public void GetUserRole_ValidCredentials_ReturnsCorrectRole()
        {
            UsersService service = new UsersService();
            string username = "admin123";
            string password = "admin123";

            string result = service.GetUserRole(username, password);

            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", result); 
        }

        [TestMethod]
        public void HashData_ValidPassword_ReturnsHashedString()
        {
            UsersService service = new UsersService();
            string password = "password123";

            string result = service.HashData(password);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(password, result); 
        }

        [TestMethod]
        public void EmailInUse_EmailExists_ReturnsTrue()
        {
            UsersService service = new UsersService();
            string email = "marin@gmail.com";

            bool result = service.EmailInUse(email);

            Assert.IsTrue(result); 
        }

        [TestMethod]
        public void EmailInUse_EmailNotInUse_ReturnsFalse()
        {
            UsersService service = new UsersService();
            string email = "newemail@example.com";

            bool result = service.EmailInUse(email);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UsernameInUse_UsernameExists_ReturnsTrue()
        {
            UsersService service = new UsersService();
            string username = "admin123";

            bool result = service.UsernameInUse(username);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UsernameInUse_UsernameNotInUse_ReturnsFalse()
        {
            UsersService service = new UsersService();
            string username = "newusername";

            bool result = service.UsernameInUse(username);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReturnEncryptedRole_ValidRole_ReturnsEncryptedRole()
        {
            UsersService service = new UsersService();
            string role = "Admin";

            string result = service.ReturnEncryptedRole(role);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(role, result); 
        }

        [TestMethod]
        public void ReturnDecryptedRole_EncryptedRole_ReturnsCorrectRole()
        {
            UsersService service = new UsersService();

            string role = "Admin";

            string encryptedRole = service.ReturnEncryptedRole(role);

            string result = service.ReturnDecryptedRole(encryptedRole);

            Assert.IsNotNull(result);
            Assert.AreEqual(role, result); 
        }

        [TestMethod]
        public void GetAllUsers_ReturnsNonEmptyList()
        {
            UsersService service = new UsersService();

            List<User> result = service.GetAllUsers();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0); 
        }

        [TestMethod]
        public void GetAllEmployees_ReturnsNonEmptyList()
        {
            UsersService service = new UsersService();

            List<User> result = service.GetAllEmployees();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0); 
        }

        [TestMethod]
        public void GetUserTypes_ReturnsNonEmptyList()
        {
            UsersService service = new UsersService();

            List<UserType> result = service.GetUserTypes();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0); 
        }

        [TestMethod]
        public void GetSubsribedUsers_ReturnsNonEmptyList()
        {
            UsersService service = new UsersService();

            List<User> result = service.GetSubsribedUsers();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetSubscriptionStatus_ValidUserId_ReturnsStatus()
        {
            UsersService service = new UsersService();
            int userId = 1;

            bool result = service.GetSubscriptionStatus(userId);

            Assert.IsInstanceOfType(result, typeof(bool));
        }

        [TestMethod]
        public void GetUsersByRole_ValidRole_ReturnsNonEmptyList()
        {
            UsersService service = new UsersService();
            string role = "Admin";

            List<User> result = service.GetUsersByRole(role);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

    }

}
