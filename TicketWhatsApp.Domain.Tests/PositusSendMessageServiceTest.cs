using System.Net;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Domain.Models.Positus;
using TicketWhatsApp.Domain.Service;

namespace TicketWhatsApp.Domain.Tests;
public class PositusSendMessageServiceTest
{
  private readonly Message _message;
  public PositusSendMessageServiceTest()
  {
    _message = new Message("user_phone", "to_phone", "message", "user_name");
  }

  [Fact]
  public void should_create_correct_body_to_positus_send_message_service()
  {
    var httpClient = new HttpClient(new MockHttpHandler(System.Net.HttpStatusCode.OK));
    var sut = new PositusSendMessageService(httpClient);
    var jsonString = sut.CreatePositusBodyResponseJson(_message);

    var result = JsonConvert.DeserializeObject<PositusResponse>(jsonString);

    result.ShouldNotBeNull();
    result.To.ShouldBe(_message.To);
    result.Type.ShouldBe("text");
    result.Text?.Body.ShouldBe(_message.Text);
  }

  [Fact]
  public async void Should_return_true_if_receive_success_status_code()
  {

    var httpClient = new HttpClient(new MockHttpHandler(System.Net.HttpStatusCode.OK));
    var sut = new PositusSendMessageService(httpClient);
    var result = await sut.Send(_message);
    result.ShouldBe(true);
  }

  [Theory]
  [InlineData(HttpStatusCode.BadRequest)]
  [InlineData(HttpStatusCode.InternalServerError)]
  [InlineData(HttpStatusCode.Forbidden)]
  [InlineData(HttpStatusCode.BadGateway)]


  public async void Should_return_false_if_receive_another_status_code_thats_not_success(HttpStatusCode code)
  {

    var httpClient = new HttpClient(new MockHttpHandler(code));
    var sut = new PositusSendMessageService(httpClient);
    var result = await sut.Send(_message);
    result.ShouldBe(false);
  }
}

public class MockHttpHandler : HttpMessageHandler
{
  private readonly HttpStatusCode _code;

  protected override Task<HttpResponseMessage> SendAsync(
    HttpRequestMessage request,
    CancellationToken cancellationToken
)
  {
    return Task.FromResult(
        new HttpResponseMessage()
        {
          StatusCode = _code,
        }
    );
  }

  public MockHttpHandler(HttpStatusCode code)
  {
    _code = code;
  }
}
