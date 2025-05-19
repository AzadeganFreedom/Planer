using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace Planer.Hashing
{
    public class BCryptHandler
    {
        public string BCryptHashing(string valueToHash)
        {
            return BCrypt.Net.BCrypt.HashPassword(valueToHash, BCrypt.Net.BCrypt.GenerateSalt(), true, BCrypt.Net.HashType.SHA256);
        }

        public bool BCryptVerifyHashing(string valueToHash, string hashedValue)
        {
            return BCrypt.Net.BCrypt.Verify(valueToHash, hashedValue, true, BCrypt.Net.HashType.SHA256);
        }
    }
}
