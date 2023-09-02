using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Domain.Interfaces;
public interface IMessageRepository
{
  Task<TicketMessage> Save(Message message, Ticket ticket);
}
