using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketWhatsApp.Domain.Models.Core
{
  [BsonIgnoreExtraElements]
  public class Message
  {
    public Message(string from, string to, string text, string name)
    {
      From = from;
      To = to;
      Text = text;
      Name = name;
    }

    [BsonId]
    [BsonRepresentation((BsonType.ObjectId))]
    public string Id { get; set; } = String.Empty;

    [BsonElement("from")]
    public string From { get; set; }
    [BsonElement("to")]

    public string To { get; set; }
    [BsonElement("text")]

    public string Text { get; set; }
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("ticket_id")]
    public string TicketId { get; set; }
  }
}