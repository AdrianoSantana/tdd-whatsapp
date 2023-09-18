using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TicketWhatsApp.Api.DTOs;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class WhatsAppController : ControllerBase
{
  private readonly IHandleWebhookService _handleWebhookService;
  public WhatsAppController(IHandleWebhookService handleWebhookService)
  {
    _handleWebhookService = handleWebhookService;
  }

  [HttpPost]
  public async Task<IActionResult> HandleWebhook([FromBody] PositusRequest request)
  {
    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new HandleWebhookResponse("Verifique os parâmetros enviados."));
      }

      var message = new Message(
        request.Messages[0].From,
        request.Contacts[0].Wa_Id,
        request.Messages[0].text.Body,
        request.Contacts[0].Profile?.Name ?? "BOT"
      );


      await _handleWebhookService.Execute(message);
      return Ok(new HandleWebhookResponse("Mensagem recebida com sucesso."));
    }
    catch (System.Exception ex)
    {
      Console.WriteLine(ex);
      return BadRequest();
    }
  }
}
