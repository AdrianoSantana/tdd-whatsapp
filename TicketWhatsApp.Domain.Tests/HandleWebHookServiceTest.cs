using Moq;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models.Core;
using TicketWhatsApp.Domain.Service;
using Shouldly;

namespace TicketWhatsApp.Domain.Tests;

public class HandleWebHookServiceTest
{
  private readonly Mock<ITicketService> _ticketService;

  private readonly Mock<IMessageRepository> _messageRepository;
  private readonly IHandleWebhookService _sut;
  private readonly Message _message;
  private readonly Mock<IMessageAnswerService> _messageAnswerService;

  private readonly Ticket _ticket;
  public HandleWebHookServiceTest()
  {
    _ticket = GenerateMockTicket();

    _ticketService = new Mock<ITicketService>();
    _ticketService.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(_ticket);

    _messageRepository = new Mock<IMessageRepository>();

    var ticketMessage = GenerateMockTicketMessage();

    _messageRepository.Setup(x => x.Save(It.IsAny<Message>(), It.IsAny<Ticket>()))
    .ReturnsAsync(ticketMessage);

    _message = new Message("user_phone", "to_phone", "message", "user_name");

    _messageAnswerService = new Mock<IMessageAnswerService>();

    _sut = new HandleWebHookService(_ticketService.Object, _messageRepository.Object, _messageAnswerService.Object);
  }

  [Fact]
  public async void Should_Call_Get_Ticket_By_User_Phone_With_Correct_Params()
  {
    string? userPhone = null;

    _ticketService.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .Callback<string>((uph) =>
    {
      userPhone = uph;
    })
    .ReturnsAsync(GenerateMockTicket());

    await _sut.Execute(_message);
    userPhone.ShouldBe(_message.From);
  }

  [Fact]
  public async void Should_Call_Create_Ticket_If_No_Ticket_Found_It()
  {
    _ticketService.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(null as Ticket);

    _ticketService.Setup(x => x.CreateTicket(It.IsAny<TicketMessage>()))
.ReturnsAsync(_ticket);

    await _sut.Execute(_message);

    _ticketService.Verify(x => x.CreateTicket(It.IsAny<TicketMessage>()), Times.Exactly(1));
  }

  [Fact]
  public async void Should_NOT_Call_Create_Ticket_If_Ticket_Found_It()
  {
    _ticketService.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(GenerateMockTicket());

    await _sut.Execute(_message);

    _ticketService.Verify(x => x.CreateTicket(It.IsAny<TicketMessage>()), Times.Exactly(0));
  }

  [Fact]
  public async void Should_Call_Save_Message_One_Time()
  {
    _messageRepository.Setup(x => x.Save(It.IsAny<Message>(), It.IsAny<Ticket>()))
    .ReturnsAsync(GenerateMockTicketMessage());

    await _sut.Execute(_message);

    _messageRepository.Verify(x => x.Save(It.IsAny<Message>(), It.IsAny<Ticket>()), Times.Exactly(1));
  }

  [Fact]
  public async void Should_Call_Save_With_Correct_Params()
  {
    Message? messageSpy = null;
    Ticket? ticketSpy = null;

    _messageRepository.Setup(x => x.Save(It.IsAny<Message>(), It.IsAny<Ticket>()))
    .Callback<Message, Ticket>((m, t) =>
    {
      messageSpy = m;
      ticketSpy = t;
    })
    .ReturnsAsync(GenerateMockTicketMessage());

    await _sut.Execute(_message);


    messageSpy.ShouldNotBeNull();
    ticketSpy.ShouldNotBeNull();

    messageSpy.From.ShouldBe(_message.From);
    messageSpy.Text.ShouldBe(_message.Text);
    messageSpy.To.ShouldBe(_message.To);
    messageSpy.Name.ShouldBe(_message.Name);
    messageSpy.TicketId.ShouldBe(_ticket.Id.ToString());

    ticketSpy.ConsumerPhone.ShouldBe(_ticket.ConsumerPhone);
    ticketSpy.Id.ShouldBe(_ticket.Id);
    ticketSpy.LastConsumerMessage.ShouldBe(_ticket.LastConsumerMessage);
    ticketSpy.CreatedAt.ShouldBe(_ticket.CreatedAt);

  }

  [Fact]
  public async void Should_Call_Ticket_Service_Update_Last_Message_With_Correct_Params_If_Already_ExistsTicket()
  {
    string? message = null;
    _ticketService.Setup(x => x.UpdateLastMessage(It.IsAny<Guid>(), It.IsAny<string>()))
    .Callback<Guid, string>((i, m) =>
    {
      message = m;
    });

    await _sut.Execute(_message);

    message.ShouldNotBeNullOrEmpty();
    message.ShouldBe(_message.Text);
    _ticketService.Verify(x => x.UpdateLastMessage(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
  }

  [Fact]
  public async void Should_Not_Call_Ticket_Service_Update_Last_Message_With_Correct_Params_If_No_ExistsTicket()
  {
    _ticketService.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(null as Ticket);

    _ticketService.Setup(x => x.CreateTicket(It.IsAny<TicketMessage>()))
    .ReturnsAsync(_ticket);

    await _sut.Execute(_message);

    _ticketService.Verify(x => x.UpdateLastMessage(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
  }

  [Fact]
  public async void Should_Call_Handle_Answer_Service_With_Correct_Params()
  {
    string? text = null;
    _messageAnswerService.Setup(x => x.Generate(It.IsAny<string>()))
      .Callback<string>(t =>
      {
        text = t;
      });
    
    var result = await _sut.Execute(_message);

    text.ShouldNotBeNull();
    text.ShouldBe(result.Text);
    _messageAnswerService.Verify(x => x.Generate(It.IsAny<string>()), Times.Once);
  }

  private static Ticket GenerateMockTicket()
  {
    return new Ticket(new Guid("c8d3fdf9-7ce1-49b2-ab9a-28573cda20a6"), "user_phone", "message", new DateTime(), new DateTime());
  }

  private static TicketMessage GenerateMockTicketMessage()
  {
    return new TicketMessage(new Guid("c8d3fdf9-7ce1-49b2-ab9a-28573cda20a6"), "user_phone", "to_phone", "message", "user_name");
  }
}
