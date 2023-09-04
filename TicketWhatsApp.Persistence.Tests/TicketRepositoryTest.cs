using Microsoft.EntityFrameworkCore;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Persistence.Repositories;
using Shouldly;

namespace TicketWhatsApp.Persistence.Tests;

public class TicketRepositoryTest
{
  [Fact]
  public async void Should_Return_Ticket_By_Id()
  {
    // Arrange
    var dbOptions = new DbContextOptionsBuilder<TicketWhatsAppDbContext>().UseInMemoryDatabase("ticketTest").Options;

    using var context = new TicketWhatsAppDbContext(dbOptions);
    var myGuid = new Guid();
    await context.AddAsync(new Ticket(myGuid.ToString(), "81995029086", "Hello", DateTime.Now, DateTime.Now));
    await context.SaveChangesAsync();

    var sut = new TicketRepository(context);

    var ticket = await sut.GetById(myGuid.ToString());

    ticket.ShouldNotBeNull();
    ticket.Id.ShouldBe(myGuid.ToString());
    ticket.ConsumerPhone.ShouldBe("81995029086");
    ticket.LastConsumerMessage.ShouldBe("Hello");
  }

  [Fact]
  public async void Should_return_null_if_no_ticket_found_it()
  {
    var dbOptions = new DbContextOptionsBuilder<TicketWhatsAppDbContext>().UseInMemoryDatabase("ticketTest").Options;
    using var context = new TicketWhatsAppDbContext(dbOptions);
    var myGuid = new Guid();

    var sut = new TicketRepository(context);

    var ticket = await sut.GetById(myGuid.ToString());

    ticket.ShouldBeNull();
  }
}
