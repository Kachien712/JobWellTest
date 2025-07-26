using JobWellTest.Models;

namespace JobWellTest.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(User user);
    }
}
