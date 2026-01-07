using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
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
    internal class TicketServices : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateTicketAsync(CreateTicketDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Title))
                errors.Add("Ticket title is required");

            if (string.IsNullOrWhiteSpace(dto.Description))
                errors.Add("Ticket description is required");

            if (!Enum.IsDefined(typeof(TicketPriority), dto.Priority))
                errors.Add("Invalid ticket priority");

            if (errors.Any())
                throw new DomainValidationException(errors);

            var ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = TicketStatus.New,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Ticket, int>().AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            return ticket.Id;
        }


        public async Task AssignEngineerAsync(int ticketId, int engineerId)
        {
            var ticket = await GetTicketEntity(ticketId);

            if (ticket.Status == TicketStatus.Closed)
                throw new InvalidTicketStatusException(
                    "Cannot change engineer after ticket is closed.");

            ticket.AssignedEngineerId = engineerId;

            _unitOfWork.GetRepository<Ticket, int>().Update(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(int ticketId, TicketStatus newStatus)
        {
            var ticket = await GetTicketEntity(ticketId);

            if (newStatus < ticket.Status)
                throw new InvalidTicketStatusException("Invalid status transition.");

            if (newStatus == TicketStatus.Closed &&
           ticket.Status != TicketStatus.Resolved)
                throw new InvalidTicketStatusException(
                    "Ticket must be resolved before closing.");

            ticket.Status = newStatus;

            switch (newStatus)
            {
                case TicketStatus.InProgress:
                    ticket.InProgressAt = DateTime.UtcNow;
                    break;

                case TicketStatus.Resolved:
                    ticket.ResolvedAt = DateTime.UtcNow;
                    break;

                case TicketStatus.Closed:
                    ticket.ClosedAt = DateTime.UtcNow;
                    break;
            }

            _unitOfWork.GetRepository<Ticket, int>().Update(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddCommentAsync(int ticketId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new DomainValidationException(
                    new[] { "Comment content is required" });

            var ticket = await GetTicketEntity(ticketId);

            if (ticket.Status == TicketStatus.Closed)
                throw new InvalidTicketStatusException(
                    "Cannot add comment to a closed ticket");

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
        private async Task<Ticket> GetTicketEntity(int id)
        {
            var ticket = await _unitOfWork.GetRepository<Ticket, int>().GetAsync(id);
            if (ticket == null)
                throw new TicketNotFoundException(id);
            return ticket;
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


    }




}
