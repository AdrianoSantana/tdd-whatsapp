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
  public Task<Ticket> CreateTicket(TicketMessage ticketMessage)
  {
    throw new NotImplementedException();
  }

  public async Task<Ticket?> GetByUserPhone(string phone)
  {
    return await _repository.GetByUserPhone(phone);
  }
}
