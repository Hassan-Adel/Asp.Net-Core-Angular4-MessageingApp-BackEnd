using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoardBackend.Models
{
    public class Message
    {
        public string Owner { get; set; }
        public string Text { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
