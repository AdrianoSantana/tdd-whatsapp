using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Domain.Interfaces;
public interface IMessageRepository
{
  Task<TicketMessage> Save(Message message, Ticket ticket);
}
