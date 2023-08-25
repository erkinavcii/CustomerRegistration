using Loggma1.Pages.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Loggma1.Models;

namespace Loggma1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ClientsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetClients()
        {
            List<ClientInfo> clients = new List<ClientInfo>();

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    email = reader.GetString(2),
                                    phone = reader.GetString(3),
                                    address = reader.GetString(4),
                                    IdentityNumber = reader.GetString(5)
                                };
                                clients.Add(clientInfo);
                            }
                        }
                    }
                }

                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetClient(int id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    email = reader.GetString(2),
                                    phone = reader.GetString(3),
                                    address = reader.GetString(4),
                                    IdentityNumber=reader.GetString(5)
                                };
                                return Ok(clientInfo);
                            }
                        }
                    }
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ClientInfo clientInfo)
        {
            if (string.IsNullOrEmpty(clientInfo.name) || string.IsNullOrEmpty(clientInfo.email)
                 || string.IsNullOrEmpty(clientInfo.phone) || string.IsNullOrEmpty(clientInfo.address)||string.IsNullOrEmpty(clientInfo.IdentityNumber))
            {
                return BadRequest("All fields are required");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsValidTcIdentityNumber(clientInfo.IdentityNumber))
            {
                ModelState.AddModelError("IdentityNumber", "Invalid T.C. Identity Number format");
                return BadRequest(ModelState);
            }
            // IdentityNumber'ı daha önce kullanılmış mı kontrol et
            if (IsIdentityNumberAlreadyExist(clientInfo.IdentityNumber))
            {
                return BadRequest("This IdentityNumber already exists in the database");
            }

            try
            {
                String connectionString = "Data Source=localhost\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO clients " +
                                "(name,email,phone,address, IdentityNumber) VALUES" +
                                "(@name,@email,@phone,@address, @IdentityNumber)";

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

                return Ok("New Client Added Correctly");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateClient(int id, ClientInfo updatedClientInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsValidTcIdentityNumber(updatedClientInfo.IdentityNumber))
            {
                ModelState.AddModelError("IdentityNumber", "Invalid T.C. Identity Number format");
                return BadRequest(ModelState);
            }
            if (IsIdentityNumberAlreadyExist(updatedClientInfo.IdentityNumber, id))
            {
                return BadRequest("This IdentityNumber already exists in the database");
            }
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE clients " +
                                "SET name = @name, email = @email, phone = @phone, address = @address, IdentityNumber = @IdentityNumber " +
                                "WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", updatedClientInfo.name);
                        command.Parameters.AddWithValue("@email", updatedClientInfo.email);
                        command.Parameters.AddWithValue("@phone", updatedClientInfo.phone);
                        command.Parameters.AddWithValue("@address", updatedClientInfo.address);
                        command.Parameters.AddWithValue("@IdentityNumber", updatedClientInfo.IdentityNumber); 
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

                return Ok("Client Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Postmanda Update işlemi yapma adımları
        /// put seçilecek sonrasında http://localhost:32387/api/Clients/1 (burada 1, güncellenecek istemcinin id'si)
        /// Headers bölümüne geçin ve Authorization başlığını ekleyin. Değeri olarak "Bearer" ile başlayan ve önceki adımlarda aldığınız JWT token'ını ekleyin.
        /// Body sekmesine geçin ve "raw" seçeneğini seçin. Aşağıda verilen gibi güncel istemci verilerini JSON formatında ekleyin:
        //{
        //    "name": "Updated name",
        //    "email": "updated@example.com",
        //    "phone": "1234567890",
        //    "address": "Updated address",
        //    "identityNumber": "12345678901"
        //}
        //şeklinde send yapıldıgında seçili id varsa ve isterler geçerliyse 200 döndürmeli eğer bulamazsa 404 döndürmeli , eğer bearer yanlışsa 401 döndürmeli



    [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteClient(int id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM clients WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

                return Ok("Client Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Kullanıcı adı ve şifre kontrolü burada yapılır.Kullanıcı bu post isteğini kullanarak kendine token üretebilir ve bu tokeni diğer istekleri(update,create,get) gerçekleştirirken authorization kısmına bearer olarak ekler.
        [HttpPost("Login")]
        public IActionResult Login([FromQuery] string username, [FromQuery] string password)
        {
            if (IsValidUser(username, password))
            {
                var token = GenerateJwtToken(username);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        [HttpPost("CheckToken")]
        public IActionResult CheckToken([FromBody] string token)
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

                return Ok("Tebrikler, token doğrulandı!");
            }
            catch (Exception ex)
            {
                return Unauthorized("Token doğrulanamadı.");
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
        private bool IsValidUser(string username, string password)
        {
            // Bu örnekte, varsayılan olarak kullanıcı adı ve şifreleri bir koleksiyonda saklıyoruz.
            // Veritabanından alınacak olan verileri kullanarak gerçek bir kullanıcı doğrulama işlemi yapmak gerek.
            var users = new List<User>
    {
        new User { Username = "admin", Password = "admin" },
        new User { Username = "erkin", Password = "erkin" }
    };

            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            return user != null;
        }
        private bool IsValidTcIdentityNumber(string identityNumber)
        {
            // Türkiye Cumhuriyeti T.C. kimlik numarası için uygun bir regular expression kullanabilirsiniz
            // Örnek olarak: 11 haneli ve sadece rakamlardan oluşan bir T.C. kimlik numarasını kontrol ediyoruz
            string tcIdentityNumberRegex = @"^(?!0{11}|1{11}|2{11}|3{11}|4{11}|5{11}|6{11}|7{11}|8{11}|9{11})([1-9]{1}[0-9]{9}[02468]{1})$";
            return Regex.IsMatch(identityNumber, tcIdentityNumberRegex);
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, username)
        // İhtiyaca göre daha fazla claim ekleyebilirsiniz
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("y9yPvg+2q3eFruhT6rGyTqApFp5PwWkD"));///32 bit uzunluğunda secretkey
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "your_issuer_here",
                audience: "your_audience_here",
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token süresi burada ayarlanabilir
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool IsIdentityNumberAlreadyExist(string identityNumber, int? clientId = null)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT COUNT(*) FROM clients WHERE IdentityNumber = @IdentityNumber";

                // Eğer güncelleme işlemi yapılıyorsa, mevcut clientId'yi filtrele
                if (clientId.HasValue)
                {
                    sql += " AND id != @ClientId";
                }

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdentityNumber", identityNumber);
                    if (clientId.HasValue)
                    {
                        command.Parameters.AddWithValue("@ClientId", clientId.Value);
                    }

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }


    }


    // Kullanıcı sınıfı
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    //public class ClientInfo
    //{
    //    public int id { get; set; }

    //    [Required(ErrorMessage = "name is required")]
    //    public string name { get; set; }

    //    [Required(ErrorMessage = "email is required")]
    //    [emailaddress(ErrorMessage = "Invalid email format")]
    //    public string email { get; set; }

    //    [Required(ErrorMessage = "phone is required")]
    //    [phone(ErrorMessage = "Invalid phone number format")]
    //    public string phone { get; set; }

    //    [Required(ErrorMessage = "address is required")]
    //    public string address { get; set; }

    //    [Required(ErrorMessage = "Identity number is required")]
    //    public string IdentityNumber { get; set; }
    //}

}
