using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketWhatsApp.Domain.Models.Core
{
  public class TicketMessage : Message
  {
    public TicketMessage(Guid? ticketId, string from, string to, string text, string name) : base(from, to, text, name)
    {
      TicketId = ticketId ?? Guid.NewGuid();
    }

    public Guid TicketId { get; set; }
  }
}