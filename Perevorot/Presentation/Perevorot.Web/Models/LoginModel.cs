using System.ComponentModel.DataAnnotations;

namespace Perevorot.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}