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
    public class TicketAttachment : BaseEntity<int>
    {
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }
        [Required]
        [MaxLength(50)]
        public string FileType { get; set; }
        [Required]
        public string FilePath { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }



    }

}
