using JobWell.Data;
using JobWell.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobWell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = applicationDbContext.Users.ToList();
            if (users == null || !users.Any())
            {
                return NoContent();
            }
            else
            {
                return Ok(users);
            }
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser (int id)
        {
            var user = applicationDbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(user);
            }
        }
    }
}
