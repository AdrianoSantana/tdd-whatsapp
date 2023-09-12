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

  public async Task<Ticket?> GetById(Guid id)
  {
    return await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);
  }

  public async Task<List<Ticket>> GetByUserPhone(string phone)
  {
    return await _context.Tickets.Where(x => x.ConsumerPhone == phone).ToListAsync();
  }

  public async Task<Ticket> Save(Ticket ticket)
  {
    _context.Tickets.Add(ticket);
    await _context.SaveChangesAsync();
    return ticket;
  }

  public async Task UpdateLastMessage(Guid id, string text)
  {
    var result = await _context.Tickets.SingleOrDefaultAsync(x => x.Id.Equals(id));
    if (result == null)
    {
      return;
    }

    result.UpdatedAt = DateTime.UtcNow;
    result.LastConsumerMessage = text;

    _context.Entry(result).CurrentValues.SetValues(result);
    await _context.SaveChangesAsync();
  }
}
