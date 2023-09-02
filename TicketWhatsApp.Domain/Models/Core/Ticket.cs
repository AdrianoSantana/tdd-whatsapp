namespace TicketWhatsApp.Domain.Models.Core;

public class Ticket
{
  public Ticket(string id, string consumerPhone, string lastConsumerMessage, DateTime createdAt, DateTime updatedAt)
  {
    Id = id;
    ConsumerPhone = consumerPhone;
    LastConsumerMessage = lastConsumerMessage;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
  }

  public string Id { get; set; }
  public string ConsumerPhone { get; set; }
  public string LastConsumerMessage { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

}
