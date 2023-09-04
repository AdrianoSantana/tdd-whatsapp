using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Persistence.Interfaces;
public interface ITicketRepository
{
  Task<Ticket> Save(Ticket ticket);
  Task<Ticket?> GetById(string id);
}
