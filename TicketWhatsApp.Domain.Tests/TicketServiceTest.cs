using Moq;
using Shouldly;
using TicketWhatsApp.Domain.Enums;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Domain.Service;

namespace TicketWhatsApp.Domain.Tests;
public class TicketServiceTest
{
  private readonly Mock<ITicketRepository> _ticketRepository;
  private readonly ITicketService sut;
  private readonly Ticket _ticket;
  public TicketServiceTest()
  {
    _ticket = createMockTicket();
    _ticketRepository = new Mock<ITicketRepository>();

    _ticketRepository.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(new List<Ticket>() {
      _ticket,
      new Ticket(Guid.NewGuid(), "consumer_phone", "last_message", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1), TicketStatusId.Finished),
      new Ticket(Guid.NewGuid(), "consumer_phone", "last_message", DateTime.Now.AddDays(2), DateTime.Now.AddDays(3), TicketStatusId.Finished),
      new Ticket(Guid.NewGuid(), "another_consumer_phone", "last_message", DateTime.Now, DateTime.Now),
    });

    sut = new TicketService(_ticketRepository.Object);
  }

  [Fact]
  public async void Should_call_ticket_repository_with_correct_params_on_getByUser_Phone()
  {
    string? expectedUserPhone = null;

    _ticketRepository.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .Callback<string>(userPhone =>
    {
      expectedUserPhone = userPhone;
    })
    .ReturnsAsync(new List<Ticket>() { createMockTicket() });

    await sut.GetByUserPhone("consumer_phone");

    expectedUserPhone.ShouldNotBeNull();
    expectedUserPhone.ShouldBe("consumer_phone");
    _ticketRepository.Verify(x => x.GetByUserPhone(It.IsAny<string>()), Times.Exactly(1));
  }


  [Fact]
  public async void Should_call_ticket_repository_with_correct_params_on_create_ticket()
  {
    Ticket? ticket = null;

    _ticketRepository.Setup(x => x.Save(It.IsAny<Ticket>()))
    .Callback<Ticket>(t =>
    {
      ticket = t;
    })
    .ReturnsAsync(createMockTicket());

    await sut.CreateTicket(new TicketMessage(null, "from", "to", "message", "any_name"));

    ticket.ShouldNotBeNull();
    ticket.ConsumerPhone.ShouldBe("from");
    ticket.LastConsumerMessage.ShouldBe("message");
    _ticketRepository.Verify(x => x.Save(It.IsAny<Ticket>()), Times.Exactly(1));
  }

  [Fact]
  public async void Should_return_null_if_repository_returns_empty_list()
  {
    _ticketRepository.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(new List<Ticket>());

    var result = await sut.GetByUserPhone("invalid_phone");

    result.ShouldBeNull();
  }

  [Fact]
  public async void Should_get_last_and_not_finished_ticket_when_repository_returns_a_list_of_tickets()
  {
    var result = await sut.GetByUserPhone("consumer_phone");

    result.ShouldNotBeNull();
    result.LastConsumerMessage.ShouldBe("last_message");
    result.ConsumerPhone.ShouldBe("another_consumer_phone");
    result.CreatedAt.Day.ShouldBe(DateTime.Now.Day);
    result.UpdatedAt.Day.ShouldBe(DateTime.Now.Day);
  }

  private static Ticket createMockTicket()
  {
    return new Ticket(Guid.NewGuid(), "consumer_phone", "Hello", DateTime.Now, DateTime.Now);
  }
}
