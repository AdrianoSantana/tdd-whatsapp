using Microsoft.AspNetCore.Mvc;
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
  public async Task<IActionResult> HandleWebhook(PositusRequest request)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(new HandleWebhookResponse("Verifique os parâmetros enviados."));
    }
    var message = new Message { };
    _handleWebhookService.Execute(message);
    return Ok(new HandleWebhookResponse("Mensagem recebida com sucesso."));
  }
}
