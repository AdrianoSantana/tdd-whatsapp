using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Domain.Interfaces;
public interface ITicketService
{
  Task<Ticket?> GetByUserPhone(string phone);
  Task<Ticket> CreateTicket(TicketMessage ticketMessage);
  Task UpdateLastMessage(Guid id, string text);
}
