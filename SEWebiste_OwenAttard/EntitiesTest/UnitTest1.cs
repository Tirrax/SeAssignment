using System;
using System.Linq;
using System.Web.Security;
using BL;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntitiesTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UserCRUDLTest()
        {
            CreateUser();
            ReadingUser();
            ListUsers();
            DeleteUser();
        }

        [TestMethod]
        public void CreateUser()
        {
            UsersBL bl = new UsersBL();

            User user = new User
            {
                Address = "Test Address",
                Country = "MT",
                Email = "Test123@test.com",
                FirstName = "Test",
                LastName = "Test",
                MobileNumber = "79555555",
                Password = "Test1234745847",
                Town = "Test Town",
                Username = "TestUser123",
                SellerApproved = false
            };

            var status = bl.AddNewUser(user);

            if (status != MembershipCreateStatus.Success)
                Assert.IsTrue(false);
        }


        [TestMethod]
        public void ReadingUser()
        {
            UsersBL bl = new UsersBL();

            var user = bl.GetUserByUsername("TestUser123");

            if (user == null)
            {
                Assert.IsNotNull(user);
            }

            Assert.AreEqual(user.Username, "TestUser123");
        }

        [TestMethod]
        public void UpdatingUser()
        {
            UsersBL bl = new UsersBL();

            var user = bl.GetUserByUsername("Test");

            if (user == null)
            {
                Assert.IsNotNull(user);
            }

            string oldNum = user.MobileNumber;

            user.MobileNumber = "744444";

            bl.UpdateUser(user);

            user = bl.GetUserByUsername("Test");

            Assert.AreNotEqual(user.Username, oldNum);
        }


        [TestMethod]
        public void ListUsers()
        {
            UsersBL bl = new UsersBL();

            var user = bl.GetAllUsersExcept("");

            if (user == null)
            {
                Assert.IsNotNull(user);
            }

            Assert.IsTrue(user.Any());
        }

        [TestMethod]
        public void DeleteUser()
        {
            UsersBL bl = new UsersBL();

            bool deleted = bl.DeleteUser("TestUser123");
            
            Assert.IsTrue(deleted);
        }

    }
}
