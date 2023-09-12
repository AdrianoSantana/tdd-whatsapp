using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TicketWhatsApp.Domain.Models.Positus;

public class PositusMessage
{
  [Required]
  public string From { get; set; }

  [Required]
  public string Id { get; set; }

  [Required]
  public MessageText text { get; set; }

  [Required]
  public string Type { get; set; }
  public string Timestamp { get; set; } = String.Empty;

}

public class MessageText
{
  public string Body { get; set; }
}
