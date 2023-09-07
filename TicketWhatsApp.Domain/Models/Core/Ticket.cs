using TicketWhatsApp.Domain.Enums;

namespace TicketWhatsApp.Domain.Models.Core;

public class Ticket
{
  public Ticket()
  {

  }
  public Ticket(
    string id,
    string consumerPhone,
    string lastConsumerMessage,
    DateTime createdAt,
    DateTime updatedAt,
    TicketStatusId ticketStatus = TicketStatusId.Openned
  )
  {
    Id = id;
    ConsumerPhone = consumerPhone;
    LastConsumerMessage = lastConsumerMessage;
    StatusId = ticketStatus;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;

  }

  public string Id { get; set; }
  public string ConsumerPhone { get; set; }
  public string LastConsumerMessage { get; set; }
  public TicketStatusId StatusId { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

}
