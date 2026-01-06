using Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{

    public class TicketComment : BaseEntity<int>
    {

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }


    }

}
