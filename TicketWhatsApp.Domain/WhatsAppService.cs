using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models;

namespace TicketWhatsApp.Domain
{
  public class WhatsAppService : IWhatsAppService
  {
    private readonly IWebHookService _webHookService;
    public WhatsAppService(IWebHookService webHookService)
    {
      this._webHookService = webHookService;
    }
    public TicketMessage HandleMessage(WhatsAppRequest request)
    {
      throw new NotImplementedException();
    }
  }
}