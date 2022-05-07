using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CabManagementSystem.Models
{
    public class UserModel
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Authenticated { get; set; } = false;
    }
}
