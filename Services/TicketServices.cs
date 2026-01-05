using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Enums;
using Services.Abstraction;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TicketServices(IUnitOfWork unitOfWork, IMapper mapper) : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<int> CreateTicketAsync(CreateTicketDto dto)
        {
            var ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = TicketStatus.New,
               
            };

            await _unitOfWork.GetRepository<Ticket, int>().AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            return ticket.Id;
        }

        public async Task AssignEngineerAsync(int ticketId, int engineerId)
        {
            var ticket = await GetTicketEntity(ticketId);

            if (ticket.Status == TicketStatus.Closed)
                throw new InvalidOperationException("Cannot change engineer after ticket is closed.");

            ticket.Engineer_Id = engineerId;

            _unitOfWork.GetRepository<Ticket, int>().Update(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(int ticketId, TicketStatus newStatus)
        {
            var ticket = await GetTicketEntity(ticketId);

            if (newStatus < ticket.Status)
                throw new InvalidOperationException("Invalid status transition.");

            if (newStatus == TicketStatus.Closed &&
                ticket.Status != TicketStatus.Resolved)
                throw new InvalidOperationException("Ticket must be resolved before closing.");

            var oldStatus = ticket.Status;

            ticket.Status = newStatus;

            var history = new TicketStatusHistory
            {
                TicketId = ticket.Id,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                ChangedOn = DateTime.UtcNow
            };

            _unitOfWork.GetRepository<Ticket, int>().Update(ticket);
            await _unitOfWork.GetRepository<TicketStatusHistory, int>().AddAsync(history);

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task AddCommentAsync(int ticketId, string content)
        {
            await GetTicketEntity(ticketId);

            var comment = new TicketComment
            {
                TicketId = ticketId,
                Content = content
            };

            await _unitOfWork.GetRepository<TicketComment, int>().AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddAttachmentAsync(int ticketId, CreateAttachmentDto dto)
        {
            await GetTicketEntity(ticketId);

            var attachment = new TicketAttachment
            {
                TicketId = ticketId,
                FileName = dto.FileName,
                FileType = dto.FileType,
                FilePath = dto.FilePath
            };

            await _unitOfWork.GetRepository<TicketAttachment, int>().AddAsync(attachment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TicketResultDto>> GetAllTicketsAsync()
        {
            var tickets = await _unitOfWork.GetRepository<Ticket, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TicketResultDto>>(tickets);
        }

        public async Task<TicketResultDto?> GetTicketByIdAsync(int id)
        {
            var ticket = await _unitOfWork.GetRepository<Ticket, int>().GetAsync(id);
            return ticket == null ? null : _mapper.Map<TicketResultDto>(ticket);
        }

        public async Task<IEnumerable<AttachmentResultDto>> GetAllAttachmentsAsync()
        {
            var attachments = await _unitOfWork.GetRepository<TicketAttachment, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<AttachmentResultDto>>(attachments);
        }

        public async Task<IEnumerable<CommentResultDto>> GetAllCommentsAsync()
        {
            var comments = await _unitOfWork.GetRepository<TicketComment, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<CommentResultDto>>(comments);
        }
        private async Task<Ticket> GetTicketEntity(int id)
        {
            var ticket = await _unitOfWork.GetRepository<Ticket, int>().GetAsync(id);
            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");
            return ticket;
        }
    }

}
