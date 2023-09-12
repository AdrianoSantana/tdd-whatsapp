namespace TicketWhatsApp.Domain.Interfaces;

public interface IDbSettings
{
  string DbCollectionName { get; set; }
  string ConnectionString { get; set; }
  string DatabaseName { get; set; }
}