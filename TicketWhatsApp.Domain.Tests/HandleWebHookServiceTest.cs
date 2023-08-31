using Moq;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Domain.Service;
using Shouldly;

namespace TicketWhatsApp.Domain.Tests;

public class HandleWebHookServiceTest
{
  private readonly Mock<ITicketService> _ticketService;
  private readonly IHandleWebhookService _sut;

  private readonly Message _message;
  public HandleWebHookServiceTest()
  {
    _ticketService = new Mock<ITicketService>();
    _ticketService.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(new Ticket { });

    _message = new Message
    {
      From = "from_user",
      Name = "user_name",
      Text = "any_message",
      To = "to_user"
    };

    _sut = new HandleWebHookService(_ticketService.Object);
  }

  [Fact]
  public async void Should_Call_Get_Ticket_By_User_Phone_With_Correct_Params()
  {
    string? userPhone = null;

    _ticketService.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .Callback<string>((uph) =>
    {
      userPhone = uph;
    })
    .ReturnsAsync(new Ticket { });

    await _sut.Execute(_message);
    userPhone.ShouldBe(_message.From);
  }

  [Fact]
  public async void Should_Call_Create_Ticket_If_No_Ticket_Found_It()
  {
    _ticketService.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(null as Ticket);

    await _sut.Execute(_message);

    _ticketService.Verify(x => x.CreateTicket(It.IsAny<TicketMessage>()), Times.Exactly(1));
  }
}
