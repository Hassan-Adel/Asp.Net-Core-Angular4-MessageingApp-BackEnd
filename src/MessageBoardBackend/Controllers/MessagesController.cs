using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MessageBoardBackend.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MessageBoardBackend.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private ApiContext context;

        public MessagesController(ApiContext _context) {
            this.context = _context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Message> Get()
        {
            return context.Messages;
        }

        // GET api/values/5
        [HttpGet("{name}")]
        public IEnumerable<Message> Get(string name)
        {
            return context.Messages.Where(message => message.Owner == name);
        }

        /*
         *  We first need to get our backend to respond with the new message that was just added. 
         *  The reason for doing this is that in most cases where you use a database you will generate
         *  a new ID for each item you add and so you pass back the item you just added which contains
         *  that new ID.
         */
        // POST api/values
        [HttpPost]
        public Message Post([FromBody]Message message)
        {
            // Will contain the added Id
            var dbMessage = context.Messages.Add(message).Entity;
            context.SaveChanges();
            return dbMessage;
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
