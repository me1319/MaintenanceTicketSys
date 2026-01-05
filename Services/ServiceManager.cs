using AutoMapper;
using Domain.Contracts;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager(IUnitOfWork unitOfWork,IMapper mapper) : IServiceManager
    {
        public ITicketService TicketService { get; } = new TicketServices(unitOfWork, mapper);
    }
}
