using Domain.Models.Enums;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketResultDto>> GetAllTicketsAsync();
        Task<TicketResultDto?> GetTicketByIdAsync(int id);

        Task<int> CreateTicketAsync(CreateTicketDto dto);
        Task AssignEngineerAsync(int ticketId, int engineerId);
        Task ChangeStatusAsync(int ticketId, TicketStatus newStatus);

        Task AddCommentAsync(int ticketId, string content);
        Task AddAttachmentAsync(int ticketId, CreateAttachmentDto dto);

        Task<IEnumerable<AttachmentResultDto>> GetAllAttachmentsAsync();
        Task<IEnumerable<CommentResultDto>> GetAllCommentsAsync();
    }
}
