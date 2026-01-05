using Domain.Models.Entities;
using Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class TicketResultDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        public TicketStatus Status { get; set; } = TicketStatus.New;

        [Required]
        public TicketPriority Priority { get; set; }

        public int? AssignedEngineerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? InProgressAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public List<CommentResultDto> Comments { get; set; }
        public List<AttachmentResultDto> Attachments { get; set; }


    }
}
