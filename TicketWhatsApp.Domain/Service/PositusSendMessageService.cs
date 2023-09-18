using System.Text;
using System.Net;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Domain.Models.Positus;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TicketWhatsApp.Domain.Service;
public class PositusSendMessageService : ISendMessageService
{
  private readonly string URI = "https://api.positus.global/v2/sandbox/whatsapp/numbers/814e1f43-af0b-4ecb-a3ee-719c3481fabf/messages";
  private readonly string TOKEN = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiZDJlNjg0YTI2MmFlMzdkMDBhNTNkYTc2Yjk0OTIxMWFiMjNiZThhYWY4MmFiMmNlN2M5OTBjZWM1MWZjYzg2Y2NhZDFjNGM5ZTZkNmIzNGEiLCJpYXQiOjE2OTUwNTA0MjkuMzE0NzA3LCJuYmYiOjE2OTUwNTA0MjkuMzE0NzI2LCJleHAiOjE3MjY2NzI4MjkuMzEyNzM2LCJzdWIiOiIzMDczMCIsInNjb3BlcyI6W119.SiijSL-ZwJCUV9gUNAl51TcWtuBn6NCyLKgN2MZs5ihRoWZIOgfbd9a6D-rYbY-Gjlf2GovFvDVyQi62vevFXtf5Kx8WZO-7ZV-RKHB92MHlOqiPCwGPXFTAKR1Y0-meqc-KF6S5aaaqaD8m43A7E76KVhsZvRubaiRLMy1BLsp2XsXQEkIa92UL7JMb8DidkbW46tL223LNVMdiwv4LdgspyaZ2fnG0UW1j5JjtioZZwfAQ4QL_roraBBqV1YFOCk_6JaKly8XUDCcOjh3YqiLbxtjIvmMTr1_k1cDu-EmhTeqz88szYxGa09luo4XebLaFeEGzLQVHRyjCxlEz4PPKPhOVrHxgm9xjIunyh7B1ZWIDrI-10dpwbPZHZQf4wjb_St9SXz-GJbngcDg11N0qdiR6X0LTgOu2_OU2Iuj3-2jdi8788R2bKV7J7V5bOJVb8Y0_8J16LzqkP9tGB2iEwRdMHx94Zv6k9_rbX1eD5F4eOArFVHjxIwHrAW5Fb4TyJnAgz1j_IvnFZJAMrXEXNdHH9d3K8uFhYBWIhi_FzpabPq6LQcg4yeu_7N44woYqYzqVSOf6b1m0zMmI9vLPvsQMgz4x7LdtxtGVEfolcnvRz6p6nExa0TfnL6E8U1rDbRMc_Fgr-2AVF1v9Y3FYjQCRILrk8jZyoyDOjK4";

  private readonly HttpClient _httpClient;

  public PositusSendMessageService(HttpClient client)
  {
    _httpClient = client;
    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
  }

  public string CreatePositusBodyResponseJson(Message message)
  {
    var positusResponse = new PositusResponse(message.To, "text", new TextPositusTypeResponse(message.Text));
    var settings = new JsonSerializerSettings();
    settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    var jsonString = JsonConvert.SerializeObject(positusResponse, settings);
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
