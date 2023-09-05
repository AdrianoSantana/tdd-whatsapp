using Moq;
using Shouldly;
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
    .ReturnsAsync(_ticket);

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
    .ReturnsAsync(createMockTicket());

    await sut.GetByUserPhone("valid_phone");

    expectedUserPhone.ShouldNotBeNull();
    expectedUserPhone.ShouldBe("valid_phone");
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
  public async void Should_return_ticket_if_ticket_was_found_it()
  {
    var result = await sut.GetByUserPhone("valid_phone");

    result.ShouldNotBeNull();
    result.ConsumerPhone.ShouldBe(_ticket.ConsumerPhone);
    result.Id.ShouldBe(_ticket.Id);
    result.LastConsumerMessage.ShouldBe(_ticket.LastConsumerMessage);
  }

  [Fact]
  public async void Should_return_null_if_no_ticket_was_found_it()
  {
    _ticketRepository.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(null as Ticket);

    var result = await sut.GetByUserPhone("invalid_phone");

    result.ShouldBeNull();
  }

  private static Ticket createMockTicket()
  {
    return new Ticket(new Guid().ToString(), "valid_phone", "Hello", DateTime.Now, DateTime.Now);
  }
}
