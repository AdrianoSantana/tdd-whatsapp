namespace TicketWhatsApp.Domain.Interfaces;

public interface IMessageAnswerService
{
    Task<string> Generate(string text, bool isFirstMessage);
}