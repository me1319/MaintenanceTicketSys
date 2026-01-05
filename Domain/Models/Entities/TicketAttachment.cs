using Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class TicketAttachment : BaseEntity<int>
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }

        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }

        public DateTime CreatedAt { get; set; }
        public Ticket Ticket { get; set; }
    }
}
