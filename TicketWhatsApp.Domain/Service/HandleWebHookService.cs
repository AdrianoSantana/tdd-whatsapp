using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Domain.Service;
public class HandleWebHookService : IHandleWebhookService
{
  private readonly ITicketService _ticketService;
  private readonly IMessageRepository _messageRepository;
  private readonly IMessageAnswerService _messageAnswerService;
  private readonly ISendMessageService _sendMessageService;

  public HandleWebHookService(ITicketService ticketService, IMessageRepository messageRepository, IMessageAnswerService messageAnswerService, ISendMessageService sendMessageService)
  {
    _ticketService = ticketService;
    _messageRepository = messageRepository;
    _messageAnswerService = messageAnswerService;
    _sendMessageService = sendMessageService;
  }
  public async Task<TicketMessage> Execute(Message message)
  {
    var ticketMessage = new TicketMessage(null, message.From, message.To, message.Text, message.Name);
    bool isFirstMessage = false;

    Ticket? ticket = await _ticketService.GetByUserPhone(message.From);
    if (ticket is null)
    {
      ticket = await _ticketService.CreateTicket(ticketMessage);
      isFirstMessage = true;
    }
    else
      await _ticketService.UpdateLastMessage(ticket.Id, ticketMessage.Text);

    message.TicketId = ticket.Id.ToString();
    await _messageRepository.Save(message, ticket);

    string messageToConsumerText = await _messageAnswerService.Generate(ticketMessage.Text, isFirstMessage);

    var messageToConsumer = new Message(message.To, message.From, messageToConsumerText, "BOT")
    {
      TicketId = message.TicketId
    };

    await _sendMessageService.Send(messageToConsumer);
    return ticketMessage;
  }
}
