using System.Text;
using System.Text.Json;
using System.Net;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Domain.Models.Positus;

namespace TicketWhatsApp.Domain.Service;
public class PositusSendMessageService : ISendMessageService
{
  private readonly string URI = "https://api.positus.global/v2/whatsapp/numbers/{{chave}}/messages";
  private readonly HttpClient _httpClient;

  public PositusSendMessageService(HttpClient client)
  {
    _httpClient = client;
  }

  public string CreatePositusBodyResponseJson(Message message)
  {
    var positusResponse = new PositusResponse(message.To, "text", new TextPositusTypeResponse(message.Text));
    var jsonString = JsonSerializer.Serialize(positusResponse);
    return jsonString;
  }

  public async Task<bool> Send(Message message)
  {
    var responseBody = this.CreatePositusBodyResponseJson(message);
    var data = new StringContent(responseBody, Encoding.UTF8, "application/json");
    var result = await _httpClient.PostAsync(URI, data);
    if (result.StatusCode == HttpStatusCode.OK)
      return true;
    else
      return false;
  }
}
