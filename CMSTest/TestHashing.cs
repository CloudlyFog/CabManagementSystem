using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CMSTest
{
    internal class TestHashing
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Hash()
        {
            string password = "1fasdfasd4752";

            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            Assert.That("", Is.EqualTo(hashed));
        }
    }
}
