using TicketWhatsApp.Domain.Enums;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Domain.Service;

public class TicketService : ITicketService
{
  private readonly ITicketRepository _repository;

  public TicketService(ITicketRepository repository)
  {
    _repository = repository;
  }
  public async Task<Ticket> CreateTicket(TicketMessage ticketMessage)
  {
    Ticket ticket = new(Guid.NewGuid(), ticketMessage.From, ticketMessage.Text, DateTime.Now, DateTime.Now);
    return await _repository.Save(ticket);
  }

  public async Task<Ticket?> GetByUserPhone(string phone)
  {
    var tickets = await _repository.GetByUserPhone(phone);
    var notFinishedTickets = tickets.FindAll(x => x.StatusId != TicketStatusId.Finished);
    notFinishedTickets = notFinishedTickets.OrderBy(x => x.CreatedAt).ToList();
    return notFinishedTickets.LastOrDefault();
  }
}
