using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace Loggma1.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInputModel Input { get; set; } = new LoginInputModel();

        public IActionResult OnGet()
        {
            string token = Request.Cookies["JwtToken"];
            if (IsValidToken(token) == true)
            {
                return Redirect("/Clients");

            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                string username = Input.Username;
                string password = Input.Password;

                if (IsValidUser(username, password))
                {
                    var token = GenerateJwtToken(username);
                    // Token'� kullan�c�ya g�nder veya ba�ka bir i�lem yapabilirsiniz.
                    // �rne�in, kullan�c�y� ba�ka bir sayfaya y�nlendirebilirsiniz.
                    Response.Cookies.Append("JwtToken", token, new CookieOptions
                    {
                        HttpOnly = true, // Sadece sunucu taraf�ndan eri�ilebilir
                        Expires = DateTime.Now.AddHours(1) // Cookie'nin s�resi
                    });
                    return Redirect("/Clients");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                }
            }

            return Page();
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
                // �htiyaca g�re daha fazla claim ekleyebilirsiniz
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("y9yPvg+2q3eFruhT6rGyTqApFp5PwWkD"));///32 bit uzunlu�unda secretkey
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "your_issuer_here",
                audience: "your_audience_here",
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token s�resi burada ayarlanabilir
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsValidUser(string username, string password)
        {
            var users = new List<User>
            {
                new User { Username = "admin", Password = "admin" },
                new User { Username = "erkin", Password = "erkin" }
            };

            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            return user != null;
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
        public class User
        {
            public string Username { get; set; }    
            public string Password { get; set; }
        }

        public class LoginInputModel
        {
            [Required(ErrorMessage = "Username is required")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
