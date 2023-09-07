using Microsoft.EntityFrameworkCore;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Persistence.Repositories;
using Shouldly;
using TicketWhatsApp.Domain.Enums;

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
    await context.AddAsync(new Ticket(myGuid.ToString(), "81995029086", "Hello", DateTime.Now, DateTime.Now, TicketStatusId.Openned));
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

  [Fact]
  public async void Should_return_list_of_tickets_that_has_user_phone()
  {
    var dbOptions = new DbContextOptionsBuilder<TicketWhatsAppDbContext>().UseInMemoryDatabase("ticketTest").Options;
    using var context = new TicketWhatsAppDbContext(dbOptions);

    await context.AddAsync(new Ticket(Guid.NewGuid().ToString(), "551126264234", "Hello", DateTime.Now, DateTime.Now, TicketStatusId.Openned));
    await context.AddAsync(new Ticket(Guid.NewGuid().ToString(), "551126264234", "Hello", DateTime.Now, DateTime.Now, TicketStatusId.Openned));
    await context.AddAsync(new Ticket(Guid.NewGuid().ToString(), "another_phone", "Hello", DateTime.Now, DateTime.Now, TicketStatusId.Openned));
    await context.AddAsync(new Ticket(Guid.NewGuid().ToString(), "other_phone", "Hello", DateTime.Now, DateTime.Now, TicketStatusId.Openned));


    await context.SaveChangesAsync();

    var sut = new TicketRepository(context);
    var result = await sut.GetByUserPhone("551126264234");

    result.ShouldNotBeNull();
    result.ShouldNotBeEmpty();
    result.Count.ShouldBe(2);
  }

  [Fact]
  public async void Should_return_an_empty_list_of_tickets_if_no_user_phone()
  {
    var dbOptions = new DbContextOptionsBuilder<TicketWhatsAppDbContext>().UseInMemoryDatabase("ticketTest").Options;
    using var context = new TicketWhatsAppDbContext(dbOptions);

    await context.AddAsync(new Ticket(Guid.NewGuid().ToString(), "551126264234", "Hello", DateTime.Now, DateTime.Now, TicketStatusId.Openned));
    await context.AddAsync(new Ticket(Guid.NewGuid().ToString(), "551126264234", "Hello", DateTime.Now, DateTime.Now, TicketStatusId.Openned));
    await context.AddAsync(new Ticket(Guid.NewGuid().ToString(), "another_phone", "Hello", DateTime.Now, DateTime.Now, TicketStatusId.Openned));
    await context.AddAsync(new Ticket(Guid.NewGuid().ToString(), "other_phone", "Hello", DateTime.Now, DateTime.Now, TicketStatusId.Openned));


    await context.SaveChangesAsync();

    var sut = new TicketRepository(context);
    var result = await sut.GetByUserPhone("invalid_phone");

    result.ShouldNotBeNull();
    result.ShouldBeEmpty();
    result.Count.ShouldBe(0);
  }
}
