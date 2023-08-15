using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Loggma1.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo =new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
           
        }
        public void OnPost() {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0
                 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "All Fields are required";
                return;

            }//save the data to database now 
            try
            {
                String connectionString = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients " + 
                        "(name,email,phone,address) VALUES" +
                        "(@name,@email,@phone,@address);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                       
                        command.ExecuteNonQuery();
                    }



                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                throw;
            }
            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = "";
            successMessage = "New Client Added Correctly";
            Response.Redirect("/Clients/Index");
        }
    }
}
/*using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Loggma1.Pages.Clients
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ClientInfo clientInfo { get; set; } = new ClientInfo();

        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            // Sayfayý görüntülerken yapýlacak iþlemler burada olabilir.
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "All Fields are required";
                return;
            }

            // Verileri veritabanýna kaydetme iþlemi burada yapýlabilir.

            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";

            successMessage = "New Client Added Correctly";
        }
    }
}*/

