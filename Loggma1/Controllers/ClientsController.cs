using Loggma1.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

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
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Phone = reader.GetString(3),
                                    Address = reader.GetString(4),
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
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Phone = reader.GetString(3),
                                    Address = reader.GetString(4),
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
        public IActionResult Create(ClientInfo clientInfo)
        {
            if (string.IsNullOrEmpty(clientInfo.Name) || string.IsNullOrEmpty(clientInfo.Email)
                 || string.IsNullOrEmpty(clientInfo.Phone) || string.IsNullOrEmpty(clientInfo.Address)||string.IsNullOrEmpty(clientInfo.IdentityNumber))
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
                        command.Parameters.AddWithValue("@name", clientInfo.Name);
                        command.Parameters.AddWithValue("@email", clientInfo.Email);
                        command.Parameters.AddWithValue("@phone", clientInfo.Phone);
                        command.Parameters.AddWithValue("@address", clientInfo.Address);
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
        private bool IsValidTcIdentityNumber(string identityNumber)
        {
            // Türkiye Cumhuriyeti T.C. kimlik numarası için uygun bir regular expression kullanabilirsiniz
            // Örnek olarak: 11 haneli ve sadece rakamlardan oluşan bir T.C. kimlik numarasını kontrol ediyoruz
            string tcIdentityNumberRegex = "^[0-9]{11}$";
            return Regex.IsMatch(identityNumber, tcIdentityNumberRegex);
        }

        [HttpPut("{id}")]
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
                        command.Parameters.AddWithValue("@name", updatedClientInfo.Name);
                        command.Parameters.AddWithValue("@email", updatedClientInfo.Email);
                        command.Parameters.AddWithValue("@phone", updatedClientInfo.Phone);
                        command.Parameters.AddWithValue("@address", updatedClientInfo.Address);
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


        [HttpDelete("{id}")]
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
    }

    public class ClientInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Identity number is required")]
        public string IdentityNumber { get; set; }
    }

}
