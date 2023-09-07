using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Domain.Interfaces;
public interface ITicketRepository
{
  Task<Ticket> Save(Ticket ticket);
  Task<Ticket?> GetById(string id);
  Task<List<Ticket>> GetByUserPhone(string phone);
}
