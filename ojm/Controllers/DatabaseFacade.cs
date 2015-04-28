using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ojm.Models;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace ojm.Controllers {
    static class DatabaseFacade {
        static string ConnectionString = "Server=ealdb1.eal.local;" + "Database=ejl26_db;" + "User Id=ejl26_usr;" + "Password=Baz1nga26";

        // CUSTOMER METHODS
        public static bool IsCustomerExsisting(string cvr) {
            return true;
        }

        public static void AddCustomer(Customer customer) { 
            
        }


        // STORAGE METHODS
        public static List<Product> GetStorageItems()
        {
            List<Product> StorageItemsList = new List<Product>();
            
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetStorageItems", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product(int.Parse(reader["ID"].ToString()), reader["Name"].ToString(), int.Parse(reader["InStock"].ToString()));
                }
                reader.Close(); 
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }

            return StorageItemsList;
        }
        public static void UpdateStorageItem(Product product)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try 
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UpdateStorageItem", conn); 
                cmd.CommandType = CommandType.StoredProcedure; 
                cmd.Parameters.Add(new SqlParameter("ID", product.ID));
                cmd.Parameters.Add(new SqlParameter("Name", product.Name));
                cmd.Parameters.Add(new SqlParameter("InStock", product.InStock));
                cmd.ExecuteNonQuery(); 
            } 
            catch (SqlException e) 
            { 
                MessageBox.Show(e.Message); 
            } 
        }

        public static void RegisterDelivery(Delivery delivery)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("RegisterDelivery", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("ID", delivery.ID));
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            } 
        }
    }
}
