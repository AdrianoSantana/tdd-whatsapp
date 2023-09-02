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
  private readonly TicketMessage _ticketMessage;

  private readonly Ticket _ticket;
  public HandleWebHookServiceTest()
  {
    _ticket = GenerateMockTicket();

    _ticketService = new Mock<ITicketService>();
    _ticketService.Setup(x => x.GetByUserPhone(It.IsAny<string>()))
    .ReturnsAsync(_ticket);

    _messageRepository = new Mock<IMessageRepository>();

    _ticketMessage = GenerateMockTicketMessage();

    _messageRepository.Setup(x => x.Save(It.IsAny<Message>(), It.IsAny<Ticket>()))
    .ReturnsAsync(_ticketMessage);

    _message = new Message("user_phone", "to_phone", "message", "user_name");

    _sut = new HandleWebHookService(_ticketService.Object, _messageRepository.Object);
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

    ticketSpy.ConsumerPhone.ShouldBe(_ticket.ConsumerPhone);
    ticketSpy.Id.ShouldBe(_ticket.Id);
    ticketSpy.LastConsumerMessage.ShouldBe(_ticket.LastConsumerMessage);
    ticketSpy.CreatedAt.ShouldBe(_ticket.CreatedAt);
  }

  private static Ticket GenerateMockTicket()
  {
    return new Ticket("ticket_id", "user_phone", "message", new DateTime(), new DateTime());
  }

  private static TicketMessage GenerateMockTicketMessage()
  {
    return new TicketMessage("ticket_id", "user_phone", "to_phone", "message", "user_name");
  }
}
