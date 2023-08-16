using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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

        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, ClientInfo updatedClientInfo)
        {
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
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
    }
}
