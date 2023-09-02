using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketWhatsApp.Domain.Models.Core
{
  public class Message
  {
    public Message(string from, string to, string text, string name)
    {
      From = from;
      To = to;
      Text = text;
      Name = name;
    }

    public string From { get; set; }
    public string To { get; set; }
    public string Text { get; set; }
    public string Name { get; set; }
  }
}