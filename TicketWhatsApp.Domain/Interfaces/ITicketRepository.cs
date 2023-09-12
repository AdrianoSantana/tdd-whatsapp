using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Domain.Interfaces;
public interface ITicketRepository
{
  Task<Ticket> Save(Ticket ticket);
  Task<Ticket?> GetById(Guid id);
  Task<List<Ticket>> GetByUserPhone(string phone);
}
