using System.ComponentModel.DataAnnotations;

namespace Loggma1.Models
{
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
