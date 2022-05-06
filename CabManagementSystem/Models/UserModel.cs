using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CabManagementSystem.Models
{
    public class UserModel : IdentityUser
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
