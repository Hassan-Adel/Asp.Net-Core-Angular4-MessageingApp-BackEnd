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
        static List<Message> messages = new 
            List<Message> {
                new Message {
                Owner ="First User",
                Text = "My_message"
                },
                new Message {
                Owner ="Hassan",
                Text = "My_message"
                }
        };
        // GET: api/values
        [HttpGet]
        public IEnumerable<Message> Get()
        {
            return messages;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Message message)
        {
            messages.Add(message);
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
