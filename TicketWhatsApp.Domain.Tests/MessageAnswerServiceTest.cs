using Shouldly;
using TicketWhatsApp.Domain.Service;

namespace TicketWhatsApp.Domain.Tests;

public class MessageAnswerServiceTest
{
    private readonly MessageAnswerService _sut = new();

    [Fact]
    public async void Should_return_greetings_message_if_is_first_message_for_consumer()
    {
        var result = await _sut.Generate("first_message", true);
        result.ShouldBe(Phrases.GREETINGS_TO_THE_CONSUMER);
    }
    
    [Fact]
    public async void Should_not_return_greetings_message_if_is_not_first_message_for_consumer()
    {
        var result = await _sut.Generate("first_message", false);
        result.ShouldNotBe(Phrases.GREETINGS_TO_THE_CONSUMER);
        result.ShouldNotBeNullOrEmpty();
    }
    
}