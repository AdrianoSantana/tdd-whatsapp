using Moq;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Domain.Service;
using Shouldly;

namespace TicketWhatsApp.Domain.Tests;

public class HandleWebHookServiceTest
{
  [Fact]
  public async void Should_Call_Get_Ticket_By_User_Phone_With_Correct_Params()
  {
    var ticketService = new Mock<ITicketService>();
    string? userPhone = null;

    ticketService.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .Callback<string>((uph) =>
    {
      userPhone = uph;
    })
    .ReturnsAsync(new Ticket { });

    IHandleWebhookService sut = new HandleWebHookService(ticketService.Object);
    var message = new Message
    {
      From = "from_user",
      Name = "user_name",
      Text = "any_message",
      To = "to_user"
    };

    sut.Execute(message);

    userPhone.ShouldBe(message.From);
  }
}
