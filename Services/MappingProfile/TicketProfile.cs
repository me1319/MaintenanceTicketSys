using AutoMapper;
using Domain.Models.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
  
    public class TicketProfile: Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketResultDto>()
                .ForMember(s => s.Comments, o => o.MapFrom(s => s.Comments))
                .ForMember(s => s.Attachments, o => o.MapFrom(s => s.Attachments));
     
            CreateMap<TicketComment,CommentResultDto>();
            CreateMap<TicketAttachment, AttachmentResultDto>();
        }

    }
}
