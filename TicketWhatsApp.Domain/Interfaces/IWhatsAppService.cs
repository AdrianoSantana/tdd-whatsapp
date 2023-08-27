using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketWhatsApp.Domain.Models;

namespace TicketWhatsApp.Domain.Interfaces
{
  public interface IWhatsAppService
  {
    TicketMessage HandleMessage(WhatsAppRequest request);
  }
}