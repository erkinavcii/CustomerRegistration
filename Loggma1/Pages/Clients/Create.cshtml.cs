using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

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

        public void OnPost()
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
    }
}
