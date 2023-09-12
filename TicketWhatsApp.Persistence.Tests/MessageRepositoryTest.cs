using MongoDB.Driver;
using Shouldly;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Persistence.Repositories;

namespace TicketWhatsApp.Persistence.Tests;

public class MessageRepositoryTest
{
  private readonly IMongoClient _testMongoClient;
  private readonly IDbSettings _testDbSettings;
  public MessageRepositoryTest()
  {
    _testDbSettings = new MessageDbSettings(
        "message",
        "message_test",
        "mongodb://root:messageDB@localhost:27017/message_test?authSource=admin"
    );
    _testMongoClient = new MongoClient(_testDbSettings.ConnectionString);
  }
  [Fact]
  public async void Should_Save_Message_In_Mongo()
  {
    var sut = new MessageRepository(_testDbSettings, _testMongoClient);
    var myGuid = Guid.NewGuid();
    var mockTicketMessage = new Message(
      "consumer_phone",
      "company_phone",
      "any_message",
      "consumer_name"
    );
    var mockTicket = new Ticket(myGuid, "consumer_phone", "last_message", DateTime.Now, DateTime.Now);
    mockTicketMessage.TicketId = mockTicket.Id.ToString();

    var result = await sut.Save(mockTicketMessage, mockTicket);
    result.ShouldNotBeNull();
    result.From.ShouldBe("consumer_phone");
    result.To.ShouldBe("company_phone");
    result.Text.ShouldBe("any_message");
    result.Name.ShouldBe("consumer_name");
  }
}