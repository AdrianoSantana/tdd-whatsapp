using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Persistence.Interfaces;

namespace TicketWhatsApp.Persistence.Repositories;

public class TicketRepository : ITicketRepository
{
  private readonly TicketWhatsAppDbContext _context;
  public TicketRepository(TicketWhatsAppDbContext _context)
  {
    this._context = _context;
  }

  public async Task<Ticket?> GetById(string id)
  {
    return await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);
  }

  public async Task<Ticket> Save(Ticket ticket)
  {
    throw new NotImplementedException();
  }
}
