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
        public static Customer GetCustomerFromCVR(string cvr) {
            Customer _customer = new Customer();

            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetCustomerFromCVR", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CVR", cvr));
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()){
                    _customer = new Models.Customer((int)reader["ID"], (string)reader["CompanyName"], (string)reader["CVR"], (string)reader["Address"], (string)reader["Email"], (string)reader["Phonenumber"], (string)reader["ContactPerson"]);
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
            return _customer;
        }

        public static Customer GetCustomerFromID(int ID) {
            Customer _customer = new Customer();

            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetCustomerFromID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID", ID));
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    _customer = new Customer((int)reader["ID"], (string)reader["CompanyName"], (string)reader["CVR"], (string)reader["Address"], (string)reader["Email"], (string)reader["Phonenumber"], (string)reader["ContactPerson"]);
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
            return _customer;
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

        public static void UpdateCustomer(Customer customer) {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UpdateCustomer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID", customer.ID));
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

        public static List<Models.Customer> GetCustomers() {
            SqlConnection conn = new SqlConnection(ConnectionString);
            List<Models.Customer> _customerList = new List<Models.Customer>();
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetCustomers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    _customerList.Add(new Models.Customer((int)reader["ID"], (string)reader["CompanyName"], (string)reader["CVR"], (string)reader["Address"], (string)reader["Email"], (string)reader["Phonenumber"], (string)reader["ContactPerson"]));
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
            return _customerList;
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
                    string customerID = reader["CustomerID"].ToString();
                    // If product has a customer
                    if (customerID != "") {
                        Customer customer = DatabaseFacade.GetCustomerFromID(int.Parse(customerID));
                        StorageItemsList.Add(new Product(
                                                int.Parse(reader["ID"].ToString()),
                                                reader["Name"].ToString(),
                                                int.Parse(reader["InStock"].ToString()),
                                                reader["Type"].ToString(),
                                                int.Parse(reader["Tolerance"].ToString()),
                                                int.Parse(reader["Reserved"].ToString()),
                                                customer
                                            ));
                    }else {
                        StorageItemsList.Add(new Product(
                                                int.Parse(reader["ID"].ToString()),
                                                reader["Name"].ToString(),
                                                int.Parse(reader["InStock"].ToString()),
                                                reader["Type"].ToString(),
                                                int.Parse(reader["Tolerance"].ToString()),
                                                int.Parse(reader["Reserved"].ToString())
                                            ));
                    }
                    
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
                cmd.Parameters.Add(new SqlParameter("Type", product.Type));
                cmd.Parameters.Add(new SqlParameter("Tolerance", product.Tolerance));
                cmd.Parameters.Add(new SqlParameter("Reserved", product.Reserved));
                if (product.Customer != null)
                    cmd.Parameters.Add(new SqlParameter("CustomerID", product.Customer.ID));
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

        internal static void AddStorageItem(Product product)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddStorageItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("Name", product.Name));
                cmd.Parameters.Add(new SqlParameter("InStock", product.InStock));
                cmd.Parameters.Add(new SqlParameter("Type", product.Type));
                cmd.Parameters.Add(new SqlParameter("Tolerance", product.Tolerance));
                cmd.Parameters.Add(new SqlParameter("Reserved", product.Reserved));
                if (product.Customer != null)
                    cmd.Parameters.Add(new SqlParameter("CustomerID", product.Customer.ID));
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        internal static List<Delivery> GetStorageItemOrders(int productID) {
            SqlConnection conn = new SqlConnection(ConnectionString);
            List<Delivery> deliveries = new List<Delivery>();
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetStorageItemOrders", conn);
                cmd.Parameters.Add(new SqlParameter("ProductID", productID));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    deliveries.Add(new Delivery(int.Parse(reader["ID"].ToString()), DateTime.Parse(reader["DeliveryDate"].ToString()), int.Parse(reader["Quantity"].ToString())));
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
            return deliveries;
        }
    }
}
