using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketWhatsApp.Domain.Models.Positus;

namespace TicketWhatsApp.Api.DTOs;
public class PositusRequest
{
  public List<PositusContact> Contacts { get; set; }
  public List<PositusMessage> Messages { get; set; }
}