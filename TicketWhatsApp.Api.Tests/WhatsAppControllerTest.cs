using TicketWhatsApp.Api.Controllers;
using Shouldly;
using Microsoft.AspNetCore.Mvc;
using TicketWhatsApp.Api.DTOs;

namespace TicketWhatsApp.Api.Tests
{
  public class WhatsAppControllerTest
  {
    private readonly WhatsAppController _sut;
    private readonly PositusRequest _request;
    public WhatsAppControllerTest()
    {
      _sut = new WhatsAppController();
      _request = new PositusRequest { };
    }

    [Fact]
    public async void Should_Return_Http_Success_With_correct_params()
    {
      var result = await _sut.HandleWebhook(_request);
      result.ShouldBeOfType(typeof(OkObjectResult));

      var OkResponseResult = result as BadRequestObjectResult;
      var okResponseValue = OkResponseResult?.Value as HandleWebhookResponse;
      okResponseValue?.Message.ShouldBe("Mensagem recebida com sucesso.");
    }

    [Fact]
    public async void Should_Return_Bad_Request_If_Incorrect_params()
    {
      _sut.ModelState.AddModelError("Key", "ErrorMessage");

      var result = await _sut.HandleWebhook(_request);
      result.ShouldBeOfType(typeof(BadRequestObjectResult));

      var badRequestResult = result as BadRequestObjectResult;
      var badRequestValue = badRequestResult.Value as HandleWebhookResponse;
      badRequestValue?.Message.ShouldBe("Verifique os parâmetros enviados.");
    }
  }
}