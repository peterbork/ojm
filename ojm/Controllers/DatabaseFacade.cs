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
            List<int> _customerList = new List<int>();
            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetCustomerFromCVR", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CVR", cvr));
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read()){
                    _customerList.Add((int)reader["ID"]);
                }
                reader.Close();

            }
            catch (SqlException e) {
                MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
            if (_customerList.Count > 0) {
                return true;
            }
            else {
                return false;
            }
        }

        public static void AddCustomer(Customer customer) {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddCustomer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyName", customer.CompanyName));
                cmd.Parameters.Add(new SqlParameter("@CVR", customer.CVR));
                cmd.Parameters.Add(new SqlParameter("@Address", customer.Address));
                cmd.Parameters.Add(new SqlParameter("@Email", customer.Email));
                cmd.Parameters.Add(new SqlParameter("@Phonenumber", customer.Phonenumber));
                cmd.Parameters.Add(new SqlParameter("@ContactPerson", customer.ContactPerson));
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e) {
                MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
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
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UpdateStorageItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("ID", product.ID));
                cmd.Parameters.Add(new SqlParameter("Name", product.Name));
                cmd.Parameters.Add(new SqlParameter("InStock", product.InStock));
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e) {
                MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
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
            finally {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
