using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Strata_WebAPI_Exercise.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public int OrderId { get; set; }
        public string MessageBody { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateMessageSent { get; set; }

        public virtual Order Order { get; set; }
    }
}