using Domain.Exceptions;
using Domain.Models.Common;
using Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{

    public class Ticket : BaseEntity<int>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }

        public Guid? AssignedEngineerId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? InProgressAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public ICollection<TicketComment> Comments { get; set; }
        public ICollection<TicketAttachment> Attachments { get; set; }
    }
}
