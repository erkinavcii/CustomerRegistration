using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
            clientInfo.identityNumber = Request.Form["identityNumber"];

            if (string.IsNullOrEmpty(clientInfo.name) || string.IsNullOrEmpty(clientInfo.email)
                 || string.IsNullOrEmpty(clientInfo.phone) || string.IsNullOrEmpty(clientInfo.address)
                 || string.IsNullOrEmpty(clientInfo.identityNumber))
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
            if (!IsValidTcNumber(clientInfo.identityNumber))
            {
                errorMessage = "Invalid Identity Number";
                return;
            }
            try
            {
                string connectionString = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO clients " +
                        "(name, email, phone, address, IdentityNumber) VALUES" +
                        "(@name, @email, @phone, @address, @identityNumber);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@identityNumber", clientInfo.identityNumber);

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
    }


}
