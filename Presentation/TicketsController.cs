using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController(IServiceManager serviceManager) : ControllerBase
    {
        private readonly IServiceManager _serviceManager = serviceManager;


        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
        {
            var ticketId = await _serviceManager.TicketService.CreateTicketAsync(dto);
            return CreatedAtAction(nameof(GetTicketById), new { id = ticketId }, ticketId);
        }



        [HttpPost("{id:int}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] string content)
        {
            await _serviceManager.TicketService.AddCommentAsync(id, content);
            return Ok();
        }


        [HttpPost("{id:int}/attachments")]
        public async Task<IActionResult> AddAttachment(int id, [FromBody] CreateAttachmentDto dto)
        {
            await _serviceManager.TicketService.AddAttachmentAsync(id, dto);
            return Ok();
        }


        [HttpPut("{id:int}/assign")]
        public async Task<IActionResult> AssignEngineer(int id, [FromQuery] int engineerId)
        {
            await _serviceManager.TicketService.AssignEngineerAsync(id, engineerId);
            return NoContent();
        }
  
        
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> ChangeStatus(int id,[FromQuery] TicketStatus status)
        {
            await _serviceManager.TicketService.ChangeStatusAsync(id, status);
            return NoContent();
        }
 
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var ticket = await _serviceManager.TicketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }
 
        
        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _serviceManager.TicketService.GetAllTicketsAsync();
            return Ok(tickets);
        }
    }
}