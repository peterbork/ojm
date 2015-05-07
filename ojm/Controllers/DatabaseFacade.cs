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
        public static List<Material> GetMaterials()
        {
            List<Material> MaterialsList = new List<Material>();
            
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetMaterials", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string customerID = reader["CustomerID"].ToString();
                    // If material has a customer
                    if (customerID != "") {
                        Customer customer = DatabaseFacade.GetCustomerFromID(int.Parse(customerID));
                        MaterialsList.Add(new Material(
                                                int.Parse(reader["ID"].ToString()),
                                                reader["Name"].ToString(),
                                                int.Parse(reader["InStock"].ToString()),
                                                reader["Type"].ToString(),
                                                int.Parse(reader["Tolerance"].ToString()),
                                                int.Parse(reader["Reserved"].ToString()),
                                                customer
                                            ));
                    }else {
                        MaterialsList.Add(new Material(
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

            return MaterialsList;
        }
        public static void UpdateMaterial(Material material)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UpdateMaterial", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("ID", material.ID));
                cmd.Parameters.Add(new SqlParameter("Name", material.Name));
                cmd.Parameters.Add(new SqlParameter("InStock", material.InStock));
                cmd.Parameters.Add(new SqlParameter("Type", material.Type));
                cmd.Parameters.Add(new SqlParameter("Tolerance", material.Tolerance));
                cmd.Parameters.Add(new SqlParameter("Reserved", material.Reserved));
                if (material.Customer != null)
                    cmd.Parameters.Add(new SqlParameter("CustomerID", material.Customer.ID));
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

        internal static void AddMaterial(Material material)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddMaterial", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("Name", material.Name));
                cmd.Parameters.Add(new SqlParameter("InStock", material.InStock));
                cmd.Parameters.Add(new SqlParameter("Type", material.Type));
                cmd.Parameters.Add(new SqlParameter("Tolerance", material.Tolerance));
                cmd.Parameters.Add(new SqlParameter("Reserved", material.Reserved));
                if (material.Customer != null)
                    cmd.Parameters.Add(new SqlParameter("CustomerID", material.Customer.ID));
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

        internal static List<Delivery> GetMaterialDeliveries(int materialID) {
            SqlConnection conn = new SqlConnection(ConnectionString);
            List<Delivery> deliveries = new List<Delivery>();
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetMaterialDeliveries", conn);
                cmd.Parameters.Add(new SqlParameter("MaterialID", materialID));
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

        internal static void OrderMaterial(int materialID, Delivery delivery) {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("OrderMaterial", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("MaterialID", materialID));
                cmd.Parameters.Add(new SqlParameter("DeliveryDate", delivery.DeliveryDate));
                cmd.Parameters.Add(new SqlParameter("Quantity", delivery.Quantity));
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

        internal static void UpdateMaterialDelivery(Delivery delivery) {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UpdateMaterialDelivery", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("ID", delivery.ID));
                cmd.Parameters.Add(new SqlParameter("DeliveryDate", delivery.DeliveryDate));
                cmd.Parameters.Add(new SqlParameter("Quantity", delivery.Quantity));
                cmd.Parameters.Add(new SqlParameter("Arrived", delivery.Arrived));
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



        internal static void AddProductOrder(ProductOrder productorder)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddProductOrder", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("Name", productorder.Name));
                cmd.Parameters.Add(new SqlParameter("Description", productorder.Description));
                cmd.Parameters.Add(new SqlParameter("CustomerID", productorder.Customer.ID));
                int newProdID = (int)cmd.ExecuteScalar();
                
                foreach (Material m in productorder.Materials)
                {
                    cmd = new SqlCommand("AddProductOrderMaterial", conn);
                    cmd.Parameters.Add(new SqlParameter("ProductOrderID", newProdID));
                    cmd.Parameters.Add(new SqlParameter("MaterialID", m.ID));
                    cmd.ExecuteNonQuery();
                }

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

        public static List<ProductOrder> GetProductOrders() {
            SqlConnection conn = new SqlConnection(ConnectionString);
            List<ProductOrder> productorders = new List<ProductOrder>();
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetProductOrders", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    productorders.Add(new ProductOrder(int.Parse(reader["ID"].ToString()), reader["Name"].ToString(), reader["Description"].ToString(), GetCustomerFromID(int.Parse(reader["CustomerID"].ToString()))));
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
            return productorders;
        }
        public static List<Material> GetMaterialsFromCustomerID(int customerid) {
            SqlConnection conn = new SqlConnection(ConnectionString);
            List<Material> materials = new List<Material>();
            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetMaterialsFromCustomerID", conn);
                cmd.Parameters.Add(new SqlParameter("CustomerID", customerid));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    materials.Add(new Material(int.Parse(reader["ID"].ToString()), reader["Name"].ToString(), int.Parse(reader["InStock"].ToString())));
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
            return materials;
        }
    }
}
