using TicketWhatsApp.Domain.Interfaces;

namespace TicketWhatsApp.Domain.Service;

public class MessageAnswerService: IMessageAnswerService
{
    public async Task<string> Generate(string text, bool isFirstMessage)
    {
        if (isFirstMessage) return Phrases.GREETINGS_TO_THE_CONSUMER;
        return Phrases.DEFAULT_ERROR_MESSAGE;
    }
}