using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MessageBoardBackend.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MessageBoardBackend.Controllers
{
    public class EditProfileData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private ApiContext context;

        public UsersController(ApiContext _context)
        {
            this.context = _context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get1 ()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var user = context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult Get()
        {
            var user = GetSecureUser();
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [Authorize]
        [HttpPost("me")]
        public ActionResult Post([FromBody]EditProfileData profileData)
        {
            var user = GetSecureUser();
           if (user == null)
                return NotFound("User not found");

            // If profileData.FirstName is nul don't change 
            user.FirstName = profileData.FirstName ?? user.FirstName;
            user.LastName = profileData.LastName ?? user.LastName;
            context.SaveChanges();


            return Ok(user);
        }

        User GetSecureUser()
        {
            var id = HttpContext.User.Claims.First().Value;
            User user = context.Users.SingleOrDefault(u => u.Id == id);
            return user;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
