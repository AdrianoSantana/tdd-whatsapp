namespace TicketWhatsApp.Domain.Models.Core;

public class MessageMongo: Message
{
    public string Id { get; set; }

    public MessageMongo(string from, string to, string text, string name) : base(from, to, text, name)
    {
        Id = "mongoId";
    }
}