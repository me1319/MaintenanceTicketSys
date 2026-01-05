using Domain.Models.Common;
using Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class TicketStatusHistory : BaseEntity<int>
    {
        public int TicketId { get; set; }
        public TicketStatus OldStatus { get; set; }
        public TicketStatus NewStatus { get; set; }
        public DateTime ChangedOn { get; set; }

        public Ticket Ticket { get; set; }
    }
}
