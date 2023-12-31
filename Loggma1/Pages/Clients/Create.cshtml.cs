using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Loggma1.Models;

namespace Loggma1.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {

        }

        public void OnPost(ClientInfo clientInfo)
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];
            clientInfo.IdentityNumber = Request.Form["IdentityNumber"];

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
            // TC kimlik numaras�n�n format�n� kontrol et
            if (!IsValidTcNumber(clientInfo.IdentityNumber))
            {
                errorMessage = "Invalid Identity Number";
                return;
            }
            try
            {
                if (IsIdentityNumberAlreadyExist(clientInfo.IdentityNumber))
                {
                    errorMessage = "This Identity Number already exists in the database.";
                    return;
                }
                string connectionString = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO clients " +
                        "(name, email, phone, address, IdentityNumber) VALUES" +
                        "(@name, @email, @phone, @address, @IdentityNumber);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@IdentityNumber", clientInfo.IdentityNumber);

                        command.ExecuteNonQuery();
                    }
                }

                successMessage = "New Client Added Correctly";
                Response.Redirect("/Clients/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
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
        private bool IsIdentityNumberAlreadyExist(string IdentityNumber)
        {
            try
            {

                string connectionString = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT COUNT(*) FROM clients WHERE IdentityNumber = @IdentityNumber";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IdentityNumber", IdentityNumber);
                        int count = (int)command.ExecuteScalar();

                        return count > 0; // E�er count 0'dan b�y�kse, ayn� kimlik numaras�na sahip m��teri bulunuyor demektir.
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumlar�n� ele almak i�in uygun bir i�lem yap�labilir, �rne�in loglama veya iste�e ba�l� hata mesaj�.
                return false; // Hata durumunda varsay�lan olarak false d�nebiliriz.
            }
        }

    }


}
