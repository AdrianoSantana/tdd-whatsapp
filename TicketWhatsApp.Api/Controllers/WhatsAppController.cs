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
    return Ok(new HandleWebhookResponse { });
  }
}
