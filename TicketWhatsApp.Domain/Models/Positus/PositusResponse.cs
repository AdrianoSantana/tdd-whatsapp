using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketWhatsApp.Domain.Models.Positus;

public class PositusResponse
{
  public PositusResponse(string to, string type, TextPositusTypeResponse? text)
  {
    To = to;
    Type = type;
    Text = text;
  }

  public string To { get; set; }
  public string Type { get; set; }
  public TextPositusTypeResponse? Text { get; set; }
}

public class TextPositusTypeResponse
{
  public TextPositusTypeResponse(string body)
  {
    Body = body;
  }

  public string Body { get; set; } = String.Empty;
}
