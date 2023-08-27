using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketWhatsApp.Api.DTOs;
public class HandleWebhookResponse
{
  public HandleWebhookResponse(string message)
  {
    Message = message;
  }
  public string Message { get; set; }
}
