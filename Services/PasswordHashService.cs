using BCrypt.Net;

namespace JobWell.Services
{
    public class PasswordHashService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
