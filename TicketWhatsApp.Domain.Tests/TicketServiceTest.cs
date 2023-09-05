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
  public TicketServiceTest()
  {
    _ticketRepository = new Mock<ITicketRepository>();

    _ticketRepository.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(createMockTicket());

    sut = new TicketService(_ticketRepository.Object);
  }
  [Fact]
  public async void Should_call_ticket_repository_with_correct_params()
  {
    string? expectedUserPhone = null;

    _ticketRepository.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .Callback<string>(userPhone =>
    {
      expectedUserPhone = userPhone;
    })
    .ReturnsAsync(createMockTicket());

    await sut.GetByUserPhone("81995029086");

    expectedUserPhone.ShouldNotBeNull();
    expectedUserPhone.ShouldBe("81995029086");
    _ticketRepository.Verify(x => x.GetByUserPhone(It.IsAny<string>()), Times.Exactly(1));
  }

  private static Ticket createMockTicket()
  {
    return new Ticket(new Guid().ToString(), "81995029086", "Hello", DateTime.Now, DateTime.Now);
  }
}
