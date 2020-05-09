using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace Cooking_TDF_Eq14
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetDate1()
        {
            // Test first day of the Week (monday)
            string date = Program.GetDate();
            Assert.AreEqual("4-5-20", date);
        }

        [TestMethod]
        public void TestGetDate2()
        {
            // Test first day of the Week (monday)
            string date = Program.GetDate();
            Assert.AreNotEqual("04-05-20", date); // with zero
        }

        [TestMethod]
        public void TestIsCdR1()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=cookingmama;PASSWORD=coco;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            bool isCdR = Program.IsCdR(connection, "C0664"); // Is a CdR
            Assert.AreEqual(true, isCdR);
        }

        [TestMethod]
        public void TestIsCdR2()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=cookingmama;PASSWORD=coco;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            bool isCdR = Program.IsCdR(connection, "C0662"); // Is not a CdR
            Assert.AreEqual(false, isCdR);
        }

        [TestMethod]
        public void TestCodeMeal1()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=cookingmama;PASSWORD=coco;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string codeM = Program.CodeMeal(connection); // last code meal should be 'R0017'
            Assert.AreEqual("R0018", codeM);
        }
    }
}
