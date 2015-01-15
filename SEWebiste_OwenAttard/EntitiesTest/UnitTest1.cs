using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using BL;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntitiesTest
{
    [TestClass]
    public class UnitTest1
    {

        #region Add Roles
        //TestCase Null
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRoleTestCase1()
        {
            string erromsg = "";
            if (AddRole(null, ref erromsg))
                Assert.Fail("The role was added when name was null");

        }


        //TestCase Empty
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddRoleTestCase2()
        {
            string erromsg = "";
            if (AddRole("", ref erromsg))
                Assert.Fail("The role was added when name was empty");

        }

        //With 1 word
        [TestMethod]
        public void AddRoleTestCase3()
        {
            string erromsg = "The role was not added";
            if (!AddRole("a", ref erromsg))
                Assert.Fail(erromsg);

        }

        //TestCase with 50 words
        [TestMethod]
        public void AddRoleTestCase4()
        {
            string txt = "";
            for (int i = 0; i < 50; i++)
            {
                txt += "a";
            }

            string erromsg = "The role was not added";

            if (!AddRole(txt, ref erromsg))
            {
                Assert.Fail(erromsg);
            }

        }

        //51 words greater than the max number of words
        [TestMethod]
        [ExpectedException(typeof(System.Data.UpdateException))]
        public void AddRoleTestCase5()
        {
            string txt = "";
            for (int i = 0; i < 51; i++)
            {
                txt += "a";
            }

            string erromsg = "The role was added when name was greater than 50";

            if (AddRole(txt, ref erromsg))
            {
                Assert.Fail(erromsg);
            }

        }

        //Non-Ascii character 
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddRoleTestCase6()
        {
            string erromsg = "The role was added when name had non ascii char";

            if (AddRole("a¢", ref erromsg))
            {
                Assert.Fail(erromsg);
            }

        }

        public bool AddRole(string name, ref string Errormsg)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                RoleBL bl = new RoleBL();
                List<Role> PrevRoles = bl.GetAllRoles().ToList();

                int ret = bl.AddRole(name);

                if (ret == -1)
                    return false;

                Debug.WriteLine(ret);
                Role ExpectedRole = new Role()
                {
                    RoleID = ret,
                    RoleName = name
                };

                PrevRoles.Add(ExpectedRole);

                List<Role> NewRoles = bl.GetAllRoles().ToList();

                Role ActualRole = NewRoles.SingleOrDefault(x => x.RoleID == ret);
                // compare products before with products after...
                AreListEqual(NewRoles, PrevRoles);

                // compare new product with the added product...
                Assert.AreEqual<string>(ExpectedRole.RoleName, ActualRole.RoleName);

                return true;

            }
        }
#endregion

        #region Get Role

        //TestCase Null
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRoleTestCase1()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                Role ActualRole = bl.GetRoleByName(null);

                if (ActualRole == null)
                    throw new NullReferenceException();

                Assert.Fail("The role was found with null as name");
            }
        }


        //TestCase Empty
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRoleTestCase2()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                Role ActualRole = bl.GetRoleByName("");

                if (ActualRole == null)
                    throw new NullReferenceException();

                Assert.Fail("The role was found with empty string as name");
            }
        }

        //With 1 word
        [TestMethod]
        public void GetRoleTestCase3()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                int ret = bl.AddRole("A");

                Role ExpectedRole = new Role()
                {
                    RoleID = ret,
                    RoleName = "A"
                };

                Role ActualRole = bl.GetRoleByName("A");

                if (ActualRole == null)
                    throw new NullReferenceException();

                Assert.AreEqual<string>(ExpectedRole.RoleName, ActualRole.RoleName);
                Assert.AreEqual<int>(ExpectedRole.RoleID, ActualRole.RoleID);
            }
        }

        //TestCase with 50 words
        [TestMethod]
        public void GetRoleTestCase4()
        {
            string txt = "";
            for (int i = 0; i < 50; i++)
            {
                txt += "a";
            }

            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                int ret = bl.AddRole(txt);

                Role ExpectedRole = new Role()
                {
                    RoleID = ret,
                    RoleName = txt
                };

                Role ActualRole = bl.GetRoleByName(txt);

                if (ActualRole == null)
                    throw new NullReferenceException();

                Assert.AreEqual<string>(ExpectedRole.RoleName, ActualRole.RoleName);
                Assert.AreEqual<int>(ExpectedRole.RoleID, ActualRole.RoleID);
            }
        }

        //51 words greater than the max number of words
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetRoleTestCase5()
        {
            string txt = "";
            for (int i = 0; i < 51; i++)
            {
                txt += "a";
            }

            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                Role ActualRole = bl.GetRoleByName(txt);

                if (ActualRole == null)
                    throw new NullReferenceException();

                Assert.Fail("Role was found with name greater than 50 ");
            }

        }

        //Non-Ascii character 
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRoleTestCase6()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                Role ActualRole = bl.GetRoleByName("AØ");

                if (ActualRole == null)
                    throw new NullReferenceException();

                Assert.Fail("The role was not null with non ascii characters");
            }

        }

        #endregion

        #region Update Roles
        //TestCase parameter 1 as Null
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateRoleTestCase1()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                bl.UpdateRoleByName(null, "ab");

                Assert.Fail("Exception for null was not thrown");
            }
        }


        //TestCase parameter 1 as empty
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateRoleTestCase2()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                bl.UpdateRoleByName("", "ab");

                Assert.Fail("Exception for empty was not thrown");
            }
        }

        //Testcase with valid parameter 1 and a valid parameter 2 Length 1
        [TestMethod]
        public void UpdateRoleTestCase3()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();
               
                int ret = bl.AddRole("a");

                if (ret == -1)
                    Assert.Fail("Could not add the role");

                List<Role> PrevRoles = bl.GetAllRoles().ToList();
                ret = bl.UpdateRoleByName("a", "ab");

                if (ret == -1)
                    Assert.Fail("Could not update the role");

                Role role = PrevRoles.SingleOrDefault(x => x.RoleName == "a");
                int index = PrevRoles.IndexOf(role);
                PrevRoles[index].RoleName = "ab";

                List<Role> NewRoles = bl.GetAllRoles().ToList();

                AreListEqual(NewRoles, PrevRoles);
            }
        }

        //Testcase with valid parameter 1 and a valid parameter 2 Length 50
        [TestMethod]
        public void UpdateRoleTestCase4()
        {
            string txt = "";
            for (int i = 0; i < 50; i++)
            {
                txt += "a";
            }

            string txt2 = "";
            for (int i = 0; i < 50; i++)
            {
                txt2 += "b";
            }
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                int ret = bl.AddRole(txt);

                if (ret == -1)
                    Assert.Fail("Could not add the role");

                List<Role> PrevRoles = bl.GetAllRoles().ToList();
                ret = bl.UpdateRoleByName(txt, txt2);

                if (ret == -1)
                    Assert.Fail("Could not update the role");

                Role role = PrevRoles.SingleOrDefault(x => x.RoleName == txt);
                int index = PrevRoles.IndexOf(role);
                PrevRoles[index].RoleName = txt2;

                List<Role> NewRoles = bl.GetAllRoles().ToList();

                AreListEqual(NewRoles, PrevRoles);
            }
        }

        //51 words greater than the max number of words
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateRoleTestCase5()
        {
            string txt = "";
            for (int i = 0; i < 51; i++)
            {
                txt += "a";
            }

            using (TransactionScope scope = new TransactionScope())
            {

                RoleBL bl = new RoleBL();

                int ret = bl.UpdateRoleByName(txt, "ab");

                if (ret != -1)
                    Assert.Fail("updated a row with name larger than 51");

            }

        }

        //Non-Ascii character 
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateRoleTestCase6()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                int ret = bl.UpdateRoleByName("AØ", "ab");

                if (ret != -1)
                    Assert.Fail("The role was updated with non ascii characters");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateRoleTestCase7()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                bl.UpdateRoleByName("a", null);

                Assert.Fail("Exception for null was not thrown");
            }
        }


        //TestCase parameter 1 as empty
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateRoleTestCase8()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                bl.UpdateRoleByName("a", "");

                Assert.Fail("Exception for empty was not thrown");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.Data.UpdateException))]
        public void UpdateRoleTestCase9()
        {
            string txt = "";
            for (int i = 0; i < 51; i++)
            {
                txt += "a";
            }

            using (TransactionScope scope = new TransactionScope())
            {

                RoleBL bl = new RoleBL();

                int ret = bl.AddRole("a");

                if (ret == -1)
                    Assert.Fail("Could not add the role");

                List<Role> PrevRoles = bl.GetAllRoles().ToList();
                ret = bl.UpdateRoleByName("a", txt);

                Assert.Fail("The role was updated with name longer than 51");

            }

        }

        //Non-Ascii character 
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateRoleTestCase10()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                int ret = bl.UpdateRoleByName("a", "AØ");

                if (ret != -1)
                    Assert.Fail("The role was updated with non ascii characters");
            }
        }

        #endregion

        #region Delete Roles

        //TestCase Null
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteRoleTestCase1()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                bl.DeleteRoleByName(null);

                Assert.Fail("Exception for null was not thrown");
            }
        }


        //TestCase Empty
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteRoleTestCase2()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                bl.DeleteRoleByName("");

                Assert.Fail("Exception for empty was not thrown");
            }
        }

        //With 1 word
        [TestMethod]
        public void DeleteRoleTestCase3()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                List<Role> PrevRoles = bl.GetAllRoles().ToList();

                int ret = bl.AddRole("A");

                if (ret == -1)
                    Assert.Fail("Could not add the role");

                List<Role> NewRoles = bl.GetAllRoles().ToList();

                Assert.AreNotEqual(NewRoles.Count, PrevRoles.Count);

                ret = bl.DeleteRoleByName("A");

                if (ret == -1)
                    Assert.Fail("Could not delete the role");

                NewRoles = bl.GetAllRoles().ToList();

                AreListEqual(NewRoles, PrevRoles);
            }
        }

        //TestCase with 50 words
        [TestMethod]
        public void DeleteRoleTestCase4()
        {
            string txt = "";
            for (int i = 0; i < 50; i++)
            {
                txt += "a";
            }

            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                List<Role> PrevRoles = bl.GetAllRoles().ToList();

                int ret = bl.AddRole(txt);

                if (ret == -1)
                    Assert.Fail("Could not add the role");

                List<Role> NewRoles = bl.GetAllRoles().ToList();

                Assert.AreNotEqual(NewRoles.Count, PrevRoles.Count);

                ret = bl.DeleteRoleByName(txt);

                if (ret == -1)
                    Assert.Fail("Could not delete the role");

                NewRoles = bl.GetAllRoles().ToList();

                AreListEqual(NewRoles, PrevRoles);
            }
        }

        //51 words greater than the max number of words
        [TestMethod]
        public void DeleteRoleTestCase5()
        {
            string txt = "";
            for (int i = 0; i < 51; i++)
            {
                txt += "a";
            }

            using (TransactionScope scope = new TransactionScope())
            {

                RoleBL bl = new RoleBL();

                int ret = bl.DeleteRoleByName(txt);

                if (ret != -1)
                    Assert.Fail("Deleted a row with name larger than 51");

            }

        }

        //Non-Ascii character 
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteRoleTestCase6()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                RoleBL bl = new RoleBL();

                int ret = bl.DeleteRoleByName("AØ");

                if (ret != -1)
                    Assert.Fail("The role was not deleted with non ascii characters");
            }

        }


        #endregion

        [TestMethod]
        public void ListTestCase()
        {
            try
            {

                RoleBL bl = new RoleBL();
                List<Role> PrevRoles = bl.GetAllRoles().ToList();

                Assert.IsNotNull(PrevRoles);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        public static void AreListEqual(List<Role> expected, List<Role> actual)
        {
            if (actual.Count != expected.Count)
            {
                Assert.Fail("Expected and actual lists are not of the same size");
            }

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual<string>(expected[i].RoleName, actual[i].RoleName);
                Assert.AreEqual<int>(expected[i].RoleID, actual[i].RoleID);
            }
        }


    }
}
