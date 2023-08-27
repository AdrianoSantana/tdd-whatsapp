using Microsoft.AspNetCore.Mvc;
using TicketWhatsApp.Api.DTOs;

namespace TicketWhatsApp.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class WhatsAppController : ControllerBase
{
  [HttpPost]
  public async Task<IActionResult> HandleWebhook(PositusRequest request)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(new HandleWebhookResponse("Verifique os parâmetros enviados."));
    }
    return Ok(new HandleWebhookResponse("Mensagem recebida com sucesso."));
  }
}
