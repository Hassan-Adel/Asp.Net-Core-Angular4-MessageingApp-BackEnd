using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MessageBoardBackend.Controllers
{
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
            var id = HttpContext.User.Claims.First().Value;
            var user = context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
