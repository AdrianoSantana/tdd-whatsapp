using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketWhatsApp.Domain.Interfaces;

namespace TicketWhatsApp.Domain.Service;
public class MockGetInfoService : IGetInfoService
{
  public async Task<string> Execute(string search)
  {
    return "";
  }
}
