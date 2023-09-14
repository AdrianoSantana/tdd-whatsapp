using Moq;
using Shouldly;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Service;

namespace TicketWhatsApp.Domain.Tests;

public class MessageAnswerServiceTest
{
    private readonly Mock<IGetInfoService> _getInfoService = new Mock<IGetInfoService>();
    private readonly IMessageAnswerService _sut;

    public MessageAnswerServiceTest()
    {
        _getInfoService.Setup(x => x.Execute(It.IsAny<string>()))
            .ReturnsAsync("result_for_search");
        
        _sut = new MessageAnswerService(_getInfoService.Object);
    }

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

    [Fact]
    public async void Should_call_Get_Info_Service_With_Correct_Params()
    {
        string? topic = null;
        _getInfoService.Setup(x => x.Execute(It.IsAny<string>()))
            .Callback<string>(t =>
            {
                topic = t;
            });

        await _sut.Generate("search_message", false);

        topic.ShouldNotBeNull();
        topic.ShouldBe("search_message");
        _getInfoService.Verify(x => x.Execute(It.IsAny<string>()), Times.Once());

    }

    [Fact]
    public async void Should_return_a_default_message_error_if_get_info_service_throws()
    {
        _getInfoService.Setup(x => x.Execute(It.IsAny<string>()))
            .ThrowsAsync(new Exception());

        var result = await _sut.Generate("search_message", false);
        
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe(Phrases.DEFAULT_ERROR_MESSAGE);
    }
    
    [Fact]
    public async void Should_return_correct_message_if_get_info_service_returns_a_message()
    {
        _getInfoService.Setup(x => x.Execute(It.IsAny<string>())).ReturnsAsync("answer_search");

        var result = await _sut.Generate("search_message", false);

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("answer_search");
    }
}