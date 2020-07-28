using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class ClientCommunicationMethod
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CommunicationMethodId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Clients Client { get; set; }
        public virtual CommunicationMethods CommunicationMethod { get; set; }
    }
}
