using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Enums
{
    public static class TaskStatus
    {

        public const string SENT = "SENT";
        public const string INBOX = "INBOX";
        public const string COMPLETED = "COMPLETED";
        public const string ARCHIVED = "ARCHIVED";
        public const string CANCELLED = "CANCELLED";
        
    }
}
