using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Domain.Service;
public class HandleWebHookService : IHandleWebhookService
{
  private readonly ITicketService _ticketService;
  private readonly IMessageRepository _messageRepository;

  public HandleWebHookService(ITicketService ticketService, IMessageRepository messageRepository)
  {
    _ticketService = ticketService;
    _messageRepository = messageRepository;
  }
  public async Task<TicketMessage> Execute(Message message)
  {
    var ticketMessage = new TicketMessage(null, message.From, message.To, message.Text, message.Name);

    Ticket? ticket = await _ticketService.GetByUserPhone(message.From);
    if (ticket is null)
      ticket = await _ticketService.CreateTicket(ticketMessage);
    else
      await _ticketService.UpdateLastMessage(ticket.Id, ticketMessage.Text);

    message.TicketId = ticket.Id.ToString();
    await _messageRepository.Save(message, ticket);
    return ticketMessage;
  }
}
