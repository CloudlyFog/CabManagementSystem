using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CabManagementSystem.Models
{
    public class AuthOptions
    {
        public const string Issuer = "Sever";
        public const string Audience = "MyAuthClient";
        private const string Key = "FJKIOPSDfj)SDFJ90fj90+f0=sdfjs09fj=09j*()()&*(79F2J90=0FJ9=09fj0j90FJf8FlkljfjkljkfJKFJoJJFjklfJ;FO[-0==JfJ9=FJ90890FJ89FJWJFJ=";
        public const int Lifetime = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
