using TicketWhatsApp.Api.Controllers;
using Shouldly;
using Microsoft.AspNetCore.Mvc;
using TicketWhatsApp.Api.DTOs;

namespace TicketWhatsApp.Api.Tests
{
  public class WhatsAppControllerTest
  {
    [Fact]
    public async void Should_Receive_Http_Post_With_correct_params()
    {
      var sut = new WhatsAppController();
      var request = new PositusRequest
      {

      };

      var result = await sut.HandleWebhook(request);
      result.ShouldBeOfType(typeof(OkObjectResult));
    }
  }
}