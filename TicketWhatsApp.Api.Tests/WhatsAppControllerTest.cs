using TicketWhatsApp.Api.Controllers;
using Shouldly;
using Microsoft.AspNetCore.Mvc;
using TicketWhatsApp.Api.DTOs;
using Moq;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Domain.Models.Positus;

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
      _request = new PositusRequest
      {
        Messages = new List<PositusMessage>() {
          new PositusMessage {
            From = "from_number",
            text = new MessageText { Body = "Lorem Ipsum "},
            Id = "any_id",
            Type = "text",
            Timestamp = "any_time"
          },
        },
        Contacts = new List<PositusContact>() {
          new PositusContact {
            Profile = new PositusProfile { Name = "user_name "},
            Wa_Id = "consumer_number"
          }
        }
      };
      _message = new Message("user_phone", "to_phone", "message", "user_name");
      _ticketMessage = new TicketMessage(Guid.NewGuid(), "user_phone", "to_phone", "message", "user_name");

      _handleWebhookService.Setup(x => x.Execute(_message)).ReturnsAsync(_ticketMessage);
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
      Message? receivedMessage = null;

      _handleWebhookService.Setup(x => x.Execute(It.IsAny<Message>()))
      .Callback<Message>((Message m) =>
      {
        receivedMessage = m;
      });

      await _sut.HandleWebhook(_request);

      receivedMessage.ShouldNotBeNull();

      receivedMessage.From.ShouldBe(_request.Messages[0].From);
      receivedMessage.Name.ShouldBe(_request.Contacts[0].Profile?.Name);
      receivedMessage.Text.ShouldBe(_request.Messages[0].text.Body);
      receivedMessage.To.ShouldBe(_request.Contacts[0].Wa_Id);
    }

    [Fact]
    public async void Should_Call_HandleWebHookService_With_BOT_Profile_Name_If_Profile_is_null()
    {
      Message? receivedMessage = null;

      _handleWebhookService.Setup(x => x.Execute(It.IsAny<Message>()))
      .Callback<Message>((Message m) =>
      {
        receivedMessage = m;
      });

      _request.Contacts[0].Profile = null;

      await _sut.HandleWebhook(_request);

      receivedMessage.ShouldNotBeNull();

      receivedMessage.From.ShouldBe(_request.Messages[0].From);
      receivedMessage.Name.ShouldBe("BOT");
      receivedMessage.Text.ShouldBe(_request.Messages[0].text.Body);
      receivedMessage.To.ShouldBe(_request.Contacts[0].Wa_Id);
    }
  }
}