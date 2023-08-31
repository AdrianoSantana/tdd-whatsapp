using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Domain.Service;
public class HandleWebHookService : IHandleWebhookService
{
  private readonly ITicketService _ticketService;

  public HandleWebHookService(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }
  public async Task<TicketMessage> Execute(Message message)
  {
    var lastTicket = await _ticketService.GetByUserPhone(message.From);
    if (lastTicket is null)
    {
      await _ticketService.CreateTicket(new TicketMessage { });
    }
    return new TicketMessage { };
  }
}
