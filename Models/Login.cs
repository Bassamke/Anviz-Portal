using System.ComponentModel.DataAnnotations;

namespace AnvizWeb.Models
{
    public class Login
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
