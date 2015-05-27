using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ojm.Models;
using ojm.Controllers;
using System.Data.SqlClient;
using System.Data;

namespace UnitTests {
    [TestClass]
    public class UnitTest1 {
        string ConnectionString = "Server=ealdb1.eal.local;" + "Database=ejl26_db;" + "User Id=ejl26_usr;" + "Password=Baz1nga26";
        [TestMethod]
        public void AddProductOrderTest() {
            // Create ProductOrder
            ProductOrder p = new ProductOrder("Test", "Tester", new Customer() { ID=4}, new List<ProductOrderMaterialUsage>());
            ojm.Controllers.DatabaseFacade.AddProductOrder(p);
            // Check if it exist
            string Name = "";
            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT TOP 1 ID, Name FROM ProductOrders ORDER BY ID DESC";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    p.ID = int.Parse(reader["ID"].ToString());
                    Name = reader["Name"].ToString();
                }

                if (p.Name != Name) {
                    Assert.Fail();
                }

                reader.Close();
                reader.Dispose();

                p.Name = "Test2";
                ojm.Controllers.DatabaseFacade.UpdateProductOrder(p);

                cmd = new SqlCommand();

                cmd.CommandText = "SELECT TOP 1 Name FROM ProductOrders ORDER BY ID DESC";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Name = reader["Name"].ToString();
                }

                reader.Close();
                reader.Dispose();

            }catch (Exception) {
                Assert.Fail();
            }
            // To Delete Test if worked
            if (p.Name == Name) {
                Assert.AreEqual(p.Name, Name);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = " DELETE FROM ProductOrders WHERE Name='Test2'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            else {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void AddCustomerTest() {
            Customer c = new Customer("Test", "4334", "test", "test", "test", "test");
            ojm.Controllers.DatabaseFacade.AddCustomer(c);
            // Check if it exist
            string Name = "";
            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT TOP 1 CompanyName FROM Customers ORDER BY ID DESC";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Name = reader["CompanyName"].ToString();
                }
                reader.Close();
                reader.Dispose();
            }
            catch (Exception) {
                Assert.Fail();
            }
            // To Delete Test if worked
            if (c.CompanyName == Name) {
                Assert.AreEqual(c.CompanyName, Name);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = " DELETE FROM Customers WHERE CompanyName='Test'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            else {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void AddMaterialTest() {
            Material m = new Material(0, "Test", 5, "Test", 5, 5);
            ojm.Controllers.DatabaseFacade.AddMaterial(m);
            // Check if it exist
            string Name = "";
            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT TOP 1 Name FROM Materials ORDER BY ID DESC";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Name = reader["Name"].ToString();
                }
                reader.Close();
                reader.Dispose();
            }
            catch (Exception) {
                Assert.Fail();
            }
            // To Delete Test if worked
            if (m.Name == Name) {
                Assert.AreEqual(m.Name, Name);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = " DELETE FROM Materials WHERE Name='Test'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            else {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AddProductionTest() {
            ProductOrder p = new ProductOrder(4);
            ojm.Controllers.DatabaseFacade.AddProduction(p, 1337, DateTime.Now);
            // Check if it exist
            int Amount = 0;
            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT TOP 1 Amount FROM Productions ORDER BY ID DESC";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Amount = int.Parse(reader["Amount"].ToString());
                }
                reader.Close();
                reader.Dispose();
            }
            catch (Exception) {
                Assert.Fail();
            }
            // To Delete Test if worked
            if (1337 == Amount) {
                Assert.AreEqual(1337, Amount);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "DELETE FROM Productions WHERE Amount='1337'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            else {
                Assert.Fail();
            }
        }

    }
}
