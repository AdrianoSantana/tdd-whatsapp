using System.ComponentModel.DataAnnotations;

namespace TicketWhatsApp.Domain.Models.Positus
{
  public class PositusContact
  {
    public PositusProfile? Profile { get; set; }

    [Required]
    public string Wa_Id { get; set; }
  }

  public class PositusProfile
  {

    [Required]
    public string Name { get; set; }
  }
}