using Microsoft.EntityFrameworkCore;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;

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

  public Task<Ticket?> GetByUserPhone(string phone)
  {
    throw new NotImplementedException();
  }

  public async Task<Ticket> Save(Ticket ticket)
  {
    throw new NotImplementedException();
  }
}
