using TicketWhatsApp.Api.Controllers;
using Shouldly;
using Microsoft.AspNetCore.Mvc;
using TicketWhatsApp.Api.DTOs;
using Moq;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Api.Tests
{
  public class WhatsAppControllerTest
  {
    private readonly WhatsAppController _sut;
    private readonly PositusRequest _request;
    private readonly Mock<IHandleWebhookService> _handleWebhookService;

    private readonly Message _message;
    private readonly TicketMessage _ticketMessage;

    public WhatsAppControllerTest()
    {
      _handleWebhookService = new Mock<IHandleWebhookService>();
      _sut = new WhatsAppController(_handleWebhookService.Object);
      _request = new PositusRequest { };
      _message = new Message { };
      _ticketMessage = new TicketMessage { };

      _handleWebhookService.Setup(x => x.Execute(_message)).Returns(_ticketMessage);
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
      var badRequestValue = badRequestResult?.Value as HandleWebhookResponse;
      badRequestValue?.Message.ShouldBe("Verifique os parâmetros enviados.");
    }

    [Fact]
    public async void Should_Call_HandleWebHookService_One_Time()
    {
      await _sut.HandleWebhook(_request);
      _handleWebhookService.Verify(x => x.Execute(It.IsAny<Message>()), Times.Exactly(1));
    }

    [Fact]
    public async void Should_Call_HandleWebHookService_With_Correct_Params()
    {

    }
  }
}