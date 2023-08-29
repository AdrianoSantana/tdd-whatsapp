using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketWhatsApp.Domain.Models.Core
{
  public class Message
  {
    public string From { get; set; }
    public string To { get; set; }
    public string Text { get; set; }
    public string Name { get; set; }
  }
}