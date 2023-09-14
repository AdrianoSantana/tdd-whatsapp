namespace TicketWhatsApp.Domain.Interfaces;

public interface IGetInfoService
{
    Task<string> Execute(string search);
}