using TicketWhatsApp.Domain.Interfaces;

namespace TicketWhatsApp.Domain.Service;

public class MessageAnswerService: IMessageAnswerService
{
    private readonly IGetInfoService _getInfoService;
    public MessageAnswerService(IGetInfoService getInfoService)
    {
        _getInfoService = getInfoService;
    }
    public async Task<string> Generate(string text, bool isFirstMessage)
    {
        try
        {
            if (isFirstMessage) 
                return Phrases.GREETINGS_TO_THE_CONSUMER;

            var answer = await _getInfoService.Execute(text);
            return answer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Phrases.DEFAULT_ERROR_MESSAGE;
        }
    }
}