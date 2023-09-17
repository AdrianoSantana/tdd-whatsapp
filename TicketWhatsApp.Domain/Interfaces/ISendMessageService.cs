using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Domain.Interfaces;

public interface ISendMessageService
{
    Task<bool> Send(Message message);
}