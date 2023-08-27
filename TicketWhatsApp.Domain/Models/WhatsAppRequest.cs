namespace TicketWhatsApp.Domain.Models
{
  public class WhatsAppRequest
  {
    public Contacts Contacts { get; set; }
    public List<Messages> Messages { get; set; }
  }
}