using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Loggma1.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                String connectionString = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.IdentityNumber = reader.GetString(5); // Ekledik
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];
            clientInfo.IdentityNumber = Request.Form["IdentityNumber"]; // Ekledik

            if (string.IsNullOrEmpty(clientInfo.name) || string.IsNullOrEmpty(clientInfo.email)
                 || string.IsNullOrEmpty(clientInfo.phone) || string.IsNullOrEmpty(clientInfo.address)
                 || string.IsNullOrEmpty(clientInfo.IdentityNumber))
            {
                errorMessage = "All fields are required";
                return;
            }
            if (!IsValidEmail(clientInfo.email))
            {
                errorMessage = "Please enter a valid email address.";
                return;
            }
            // TC kimlik numarasýnýn formatýný kontrol et
            if (!IsValidTcNumber(clientInfo.IdentityNumber))
            {
                errorMessage = "Invalid Identity Number";
                return;
            }

            try
            {
                // Güncelleme iþlemi sýrasýnda ayný kimlik numarasýna sahip baþka bir müþteri olup olmadýðýný kontrol edin
                if (IsIdentityNumberAlreadyExist(clientInfo.IdentityNumber, clientInfo.id))
                {
                    errorMessage = "This Identity Number already exists in the database.";
                    return;
                }
                String connectionString = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE clients " +
                                "SET name = @name, email = @email, phone = @phone, address = @address, IdentityNumber = @IdentityNumber " +
                                "WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@IdentityNumber", clientInfo.IdentityNumber); // Ekledik
                        command.Parameters.AddWithValue("@id", clientInfo.id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Clients/Index");

        }
        private bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
        private bool IsValidTcNumber(string tcNumber)
        {
            string regexPattern = @"^(?!0{11}|1{11}|2{11}|3{11}|4{11}|5{11}|6{11}|7{11}|8{11}|9{11})([1-9]{1}[0-9]{9}[02468]{1})$";
            return Regex.IsMatch(tcNumber, regexPattern);
        }
        private bool IsIdentityNumberAlreadyExist(string IdentityNumber, string clientId)
        {
            try
            {
                string connectionString = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT COUNT(*) FROM clients WHERE IdentityNumber = @IdentityNumber AND Id != @clientId";////update edilen müþterinin tcsini tekrar kontrol etmememesi için

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IdentityNumber", IdentityNumber);
                        command.Parameters.AddWithValue("@clientId", clientId);
                        int count = (int)command.ExecuteScalar();

                        return count > 0; // Eðer count 0'dan büyükse, ayný kimlik numarasýna sahip baþka bir müþteri bulunuyor demektir.
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumlarýný ele almak için uygun bir iþlem yapýlabilir, örneðin loglama veya isteðe baðlý hata mesajý.
                return false; // Hata durumunda varsayýlan olarak false dönebiliriz.
            }
        }

    }
}
