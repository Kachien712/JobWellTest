using JobWellTest2.Models;

namespace JobWellTest2.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(User user);
    }
}
