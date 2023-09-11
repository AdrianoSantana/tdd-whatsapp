using MongoDB.Driver;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Persistence.Repositories;

public class MessageRepository : IMessageRepository
{
  private readonly IMongoCollection<Message> _messageCollection;

  public MessageRepository(IDbSettings settings, IMongoClient mongoClient)
  {
    var database = mongoClient.GetDatabase(settings.DatabaseName);
    _messageCollection = database.GetCollection<Message>(settings.DbCollectionName);
  }
  public async Task<TicketMessage> Save(Message message, Ticket ticket)
  {
    await _messageCollection.InsertOneAsync(message);
    return new TicketMessage(ticket.Id, message.From, message.To, message.Text, message.Name);
  }
}