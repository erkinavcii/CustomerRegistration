using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Loggma1.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();

        public void OnGet()
        {
            try
            {
                string token = Request.Cookies["JwtToken"];

                if (string.IsNullOrEmpty(token) || !IsValidToken(token))
                {
                    // Token geçerli deðilse veya yoksa, Unauthorized hatasý göster
                    Response.Redirect("/Login"); // Unauthorized
                    return;
                }
                String connectionString = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.IdentityNumber = reader.GetString(5);
                                listClients.Add(clientInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
        private bool IsValidToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("y9yPvg+2q3eFruhT6rGyTqApFp5PwWkD"); // Secret key
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // You can configure these values according to your needs
                    ValidateAudience = false // You can configure these values according to your needs
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        // Tokeni silmek için HTTP POST iþlemi
        public IActionResult OnPostLogout()
        {
            // Cookie'deki JwtToken adlý çerezin süresini sonlandýrýn (expire)
            Response.Cookies.Delete("JwtToken");

            // Kullanýcýyý Login sayfasýna yönlendirin
            return RedirectToPage("/Login");
        }
    }

   

public class ClientInfo
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string address { get; set; }

        [Required(ErrorMessage = "IdentityNumber is required")]
        public string IdentityNumber { get; set; }
    }

}
